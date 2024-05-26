FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ServerMonitoringSystem/ServerMonitoringSystem.csproj", "ServerMonitoringSystem/"]
RUN dotnet restore "ServerMonitoringSystem/ServerMonitoringSystem.csproj"
COPY . .
WORKDIR "/src/ServerMonitoringSystem"
RUN dotnet build "ServerMonitoringSystem.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "ServerMonitoringSystem.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ServerMonitoringSystem.dll"]
