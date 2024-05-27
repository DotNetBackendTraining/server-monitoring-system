# Use the official .NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Use the official .NET SDK image to build the application
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ServerMonitoringSystem.csproj", "./"]
RUN dotnet restore "ServerMonitoringSystem.csproj"
COPY . .
WORKDIR "/src"
RUN dotnet build "ServerMonitoringSystem.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish the application
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ServerMonitoringSystem.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Run the application
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServerMonitoringSystem.dll"]

# Set or override environment variables
ENV ServerStatisticsSettings__ServerIdentifier=docker