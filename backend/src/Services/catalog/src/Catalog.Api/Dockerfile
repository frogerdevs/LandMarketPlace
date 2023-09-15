#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/catalog/src/Catalog.Api/Catalog.Api.csproj", "src/Services/catalog/src/Catalog.Api/"]
COPY ["src/Services/catalog/src/Catalog.Application/Catalog.Application.csproj", "src/Services/catalog/src/Catalog.Application/"]
COPY ["src/Services/catalog/src/Catalog.Domain/Catalog.Domain.csproj", "src/Services/catalog/src/Catalog.Domain/"]
COPY ["src/Services/catalog/src/Catalog.Infrastructure/Catalog.Infrastructure.csproj", "src/Services/catalog/src/Catalog.Infrastructure/"]
RUN dotnet restore "src/Services/catalog/src/Catalog.Api/Catalog.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/catalog/src/Catalog.Api"
RUN dotnet build "Catalog.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Catalog.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Catalog.Api.dll"]