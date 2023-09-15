using Mediator;

namespace Catalog.Application.Features.Weather.Queries
{
    public sealed record GetForeCastRequest() : IQuery<IEnumerable<WeatherForecastResponse>>
    {

    }

    public sealed class ForeCastQueryHandler : IQueryHandler<GetForeCastRequest, IEnumerable<WeatherForecastResponse>>
    {
        private static readonly string[] Summaries = new[]
        {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        //private readonly ITodoItemRepository _repository;

        public ForeCastQueryHandler()
        {
        }

        public ValueTask<IEnumerable<WeatherForecastResponse>> Handle(GetForeCastRequest query, CancellationToken cancellationToken)
        {
            //var items = await _repository.GetItems(cancellationToken);
            //return items.Select(i => new TodoItemDto(i.Title, i.Text, i.Done)).ToArray();

            var dt = Enumerable.Range(1, 5).Select(index => new WeatherForecastResponse
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            });
            return new ValueTask<IEnumerable<WeatherForecastResponse>>(dt);

        }

    }
}
