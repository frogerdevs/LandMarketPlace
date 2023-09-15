#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Subscription.Api/Subscription.Api.csproj", "src/Subscription.Api/"]
COPY ["src/Subscription.Application/Subscription.Application.csproj", "src/Subscription.Application/"]
COPY ["src/Subscription.Domain/Subscription.Domain.csproj", "src/Subscription.Domain/"]
COPY ["src/Subscription.Infrastructure/Subscription.Infrastructure.csproj", "src/Subscription.Infrastructure/"]
RUN dotnet restore "src/Subscription.Api/Subscription.Api.csproj"
COPY . .
WORKDIR "/src/src/Subscription.Api"
RUN dotnet build "Subscription.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Subscription.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Subscription.Api.dll"]