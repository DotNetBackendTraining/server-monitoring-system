# Base image for the runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY ["ServerMonitoringSystem.SignalRHub/ServerMonitoringSystem.SignalRHub.csproj", "ServerMonitoringSystem.SignalRHub/"]
RUN dotnet restore "ServerMonitoringSystem.SignalRHub/ServerMonitoringSystem.SignalRHub.csproj"

COPY . .
WORKDIR "/src/ServerMonitoringSystem.SignalRHub"
RUN dotnet build "ServerMonitoringSystem.SignalRHub.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ServerMonitoringSystem.SignalRHub.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServerMonitoringSystem.SignalRHub.dll"]
