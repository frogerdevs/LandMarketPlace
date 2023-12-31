﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.EventBus;
using Ordering.Application.EventBus.EventHandlings;
using Ordering.Application.EventBus.Events;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Ordering.Infrastructure.EventBus
{
    public class EventBusRabbitMQ : IEventBus, IDisposable
    {
        private readonly ILogger<EventBusRabbitMQ> _logger;
        private readonly IEventBusSubscriptionsManager _subsManager;
        private readonly IServiceProvider _services;
        readonly string BROKER_NAME;

        private IModel _consumerChannel;
        private string _queueName;
        private IConnection? _connection;
        private readonly ConnectionFactory _connectionFactory;
        private readonly bool _disposed;

        public EventBusRabbitMQ(ILogger<EventBusRabbitMQ> logger, IOptions<AppSettings> appSettings,
            IServiceProvider services,
            IEventBusSubscriptionsManager subsManager)
        {
            var _appSettings = appSettings.Value;
            _services = services;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _subsManager = subsManager ?? new EventBusSubscriptionsManager();
            BROKER_NAME = _appSettings.BrokerName ?? "ex_event_bus";
            _queueName = _appSettings.QueueName ?? "ESS";
            _connectionFactory = new ConnectionFactory
            {
                //HostName = "localhost",
                HostName = _appSettings.BrokerHostName,
                Port = _appSettings.BrokerPort,
                UserName = _appSettings.BrokerUserName,
                Password = _appSettings.BrokerPassword,
                DispatchConsumersAsync = true,
            };
            _consumerChannel = CreateConsumerChannel();
            _subsManager.OnEventRemoved += SubsManager_OnEventRemoved!;
        }
        public bool IsConnected
        {
            get
            {
                return _connection != null && _connection.IsOpen && !_disposed;
            }
        }
        public bool TryConnect()
        {
            _logger.LogInformation("RabbitMQ Client is trying to connect");
            try
            {
                _connection = _connectionFactory
                        .CreateConnection();

            }
            catch (Exception)
            {
                _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
                return false;
            }

            if (IsConnected)
            {
                _connection.ConnectionShutdown += OnConnectionShutdown!;
                _connection.CallbackException += OnCallbackException!;
                _connection.ConnectionBlocked += OnConnectionBlocked!;

                _logger.LogInformation("RabbitMQ Client acquired a persistent connection to '{HostName}' and is subscribed to failure events", _connection.Endpoint.HostName);
                return true;
            }
            else
            {
                _logger.LogCritical("FATAL ERROR: RabbitMQ connections could not be created and opened");
                return false;
            }
        }
        void OnConnectionBlocked(object sender, ConnectionBlockedEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            _logger.LogWarning("A RabbitMQ connection is shutdown. Trying to re-connect...");

            TryConnect();
        }
        void OnCallbackException(object sender, CallbackExceptionEventArgs e)
        {
            if (_disposed)
            {
                return;
            }

            _logger.LogWarning("A RabbitMQ connection throw exception. Trying to re-connect...");

            TryConnect();
        }
        void OnConnectionShutdown(object sender, ShutdownEventArgs reason)
        {
            if (_disposed)
            {
                return;
            }

            _logger.LogWarning("A RabbitMQ connection is on shutdown. Trying to re-connect...");

            TryConnect();
        }

        public IModel CreateModel()
        {
            if (!IsConnected)
            {
                throw new InvalidOperationException("No RabbitMQ connections are available to perform this action");
            }

            return _connection!.CreateModel();
        }

        private void SubsManager_OnEventRemoved(object sender, string eventName)
        {
            if (!IsConnected)
            {
                TryConnect();
            }

            using (var channel = CreateModel())
            {
                channel.QueueUnbind(queue: _queueName,
                    exchange: BROKER_NAME,
                    routingKey: eventName);

                if (_subsManager.IsEmpty)
                {
                    _queueName = string.Empty;
                    _consumerChannel.Close();
                }
            }
        }

        public void Publish(IntegrationEvent @event)
        {
            if (!IsConnected)
            {
                TryConnect();
            }

            var eventName = @event.GetType().Name;

            _logger.LogTrace("Creating RabbitMQ channel to publish event: {EventId} ({EventName})", @event.Id, eventName);

            using (var channel = CreateModel())
            {
                _logger.LogTrace("Declaring RabbitMQ exchange to publish event: {EventId}", @event.Id);

                channel.ExchangeDeclare(exchange: BROKER_NAME, type: "direct");

                var body = JsonSerializer.SerializeToUtf8Bytes(@event, @event.GetType(), new JsonSerializerOptions
                {
                    WriteIndented = true
                });

                var properties = channel.CreateBasicProperties();
                properties.DeliveryMode = 2; // persistent

                _logger.LogTrace("Publishing event to RabbitMQ: {EventId}", @event.Id);

                channel.BasicPublish(
                    exchange: BROKER_NAME,
                    routingKey: eventName,
                    mandatory: true,
                    basicProperties: properties,
                    body: body);
            }
        }

        public void Subscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();
            DoInternalSubscription(eventName);

            _logger.LogInformation("Subscribing to event {EventName} with {EventHandler}", eventName, typeof(TH));

            _subsManager.AddSubscription<T, TH>();
            StartBasicConsume();
        }

        private void DoInternalSubscription(string eventName)
        {
            var containsKey = _subsManager.HasSubscriptionsForEvent(eventName);
            if (!containsKey)
            {
                if (!IsConnected)
                {
                    TryConnect();
                }

                _consumerChannel.QueueBind(queue: _queueName,
                                    exchange: BROKER_NAME,
                                    routingKey: eventName);
            }
        }

        public void Unsubscribe<T, TH>()
            where T : IntegrationEvent
            where TH : IIntegrationEventHandler<T>
        {
            var eventName = _subsManager.GetEventKey<T>();

            _logger.LogInformation("Unsubscribing from event {EventName}", eventName);

            _subsManager.RemoveSubscription<T, TH>();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (_consumerChannel != null)
            {
                _consumerChannel.Dispose();
            }

            _subsManager.Clear();
        }
        private void StartBasicConsume()
        {
            _logger.LogTrace("Starting RabbitMQ basic consume");

            if (_consumerChannel != null)
            {
                var consumer = new AsyncEventingBasicConsumer(_consumerChannel);

                consumer.Received += Consumer_Received;

                _consumerChannel.BasicConsume(
                    queue: _queueName,
                    autoAck: false,
                    consumer: consumer);
            }
            else
            {
                _logger.LogError("StartBasicConsume can't call on _consumerChannel == null");
            }
        }

        private async Task Consumer_Received(object sender, BasicDeliverEventArgs eventArgs)
        {
            var eventName = eventArgs.RoutingKey;
            var message = Encoding.UTF8.GetString(eventArgs.Body.Span);

            try
            {
                if (message.ToLowerInvariant().Contains("throw-fake-exception"))
                {
                    throw new InvalidOperationException($"Fake exception requested: \"{message}\"");
                }

                await ProcessEvent(eventName, message);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "----- ERROR Processing message \"{Message}\"", message);
            }

            // Even on exception we take the message off the queue.
            // in a REAL WORLD app this should be handled with a Dead Letter Exchange (DLX). 
            // For more information see: https://www.rabbitmq.com/dlx.html
            _consumerChannel.BasicAck(eventArgs.DeliveryTag, multiple: false);
        }

        private IModel CreateConsumerChannel()
        {
            if (!IsConnected)
            {
                TryConnect();
            }

            _logger.LogTrace("Creating RabbitMQ consumer channel");

            var channel = CreateModel();

            channel.ExchangeDeclare(exchange: BROKER_NAME,
                                    type: "direct");

            channel.QueueDeclare(queue: _queueName,
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.CallbackException += (sender, ea) =>
            {
                _logger.LogWarning(ea.Exception, "Recreating RabbitMQ consumer channel");

                _consumerChannel.Dispose();
                _consumerChannel = CreateConsumerChannel();
                StartBasicConsume();
            };

            return channel;
        }

        private async Task ProcessEvent(string eventName, string message)
        {
            _logger.LogTrace("Processing RabbitMQ event: {EventName}", eventName);

            if (_subsManager.HasSubscriptionsForEvent(eventName))
            {

                using (var scope = _services.CreateScope())
                {
                    var subscriptions = _subsManager.GetHandlersForEvent(eventName);
                    foreach (var subscription in subscriptions)
                    {
                        try
                        {
                            if (subscription == null)
                            {
                                continue;
                            }
                            var handler = scope.ServiceProvider.GetService(subscription);
                            if (handler == null)
                            {
                                continue;
                            }

                            var options = new JsonSerializerOptions()
                            {
                                PropertyNameCaseInsensitive = false
                            };
                            //options.Converters.Add(new DateOnlyConverter());
                            //options.Converters.Add(new DateTimeConverter());
                            //options.Converters.Add(new TimeOnlyConverter());

                            var eventType = _subsManager.GetEventTypeByName(eventName);
                            var integrationEvent = JsonSerializer
                                .Deserialize(message, eventType, options);
                            var concreteType = typeof(IIntegrationEventHandler<>).MakeGenericType(eventType);


                            await Task.Yield();
                            await (Task)concreteType.GetMethod("Handle")!.Invoke(handler, new object[] { integrationEvent! })!;
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError(ex, "Error event: {EventName}", eventName);
                        }
                    }
                }
            }
            else
            {
                _logger.LogWarning("No subscription for RabbitMQ event: {EventName}", eventName);
            }
        }
    }
}
