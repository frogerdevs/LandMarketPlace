#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/Gateway/Web.Gateway/Web.Gateway/Web.Gateway.csproj", "src/Gateway/Web.Gateway/Web.Gateway/"]
RUN dotnet restore "src/Gateway/Web.Gateway/Web.Gateway/Web.Gateway.csproj"
COPY . .
WORKDIR "/src/src/Gateway/Web.Gateway/Web.Gateway"
RUN dotnet build "Web.Gateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Web.Gateway.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Web.Gateway.dll"]