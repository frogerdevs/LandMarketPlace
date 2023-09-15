#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Services/ordering/src/Ordering.Api/Ordering.Api.csproj", "src/Services/ordering/src/Ordering.Api/"]
COPY ["src/Services/ordering/src/Ordering.Application/Ordering.Application.csproj", "src/Services/ordering/src/Ordering.Application/"]
COPY ["src/Services/ordering/src/Ordering.Domain/Ordering.Domain.csproj", "src/Services/ordering/src/Ordering.Domain/"]
COPY ["src/Services/ordering/src/Ordering.Infrastructure/Ordering.Infrastructure.csproj", "src/Services/ordering/src/Ordering.Infrastructure/"]
RUN dotnet restore "src/Services/ordering/src/Ordering.Api/Ordering.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/ordering/src/Ordering.Api"
RUN dotnet build "Ordering.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ordering.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ordering.Api.dll"]