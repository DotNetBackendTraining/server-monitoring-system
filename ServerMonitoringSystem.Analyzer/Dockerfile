# Use the official .NET runtime as a parent image
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ServerMonitoringSystem.Analyzer/ServerMonitoringSystem.Analyzer.csproj", "ServerMonitoringSystem.Analyzer/"]
RUN dotnet restore "ServerMonitoringSystem.Analyzer/ServerMonitoringSystem.Analyzer.csproj"
COPY . .
WORKDIR "/src/ServerMonitoringSystem.Analyzer"
RUN dotnet build "ServerMonitoringSystem.Analyzer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ServerMonitoringSystem.Analyzer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServerMonitoringSystem.Analyzer.dll"]
