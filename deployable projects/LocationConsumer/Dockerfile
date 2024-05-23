FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["deployable projects/LocationConsumer/LocationConsumer.csproj", "deployable projects/LocationConsumer/"]
COPY ["libaries/CassandraFactory/CassandraFactory.csproj", "libaries/CassandraFactory/"]
COPY ["shared/SharedModels/SharedModels.csproj", "shared/SharedModels/"]
RUN dotnet restore "deployable projects/LocationConsumer/LocationConsumer.csproj"
COPY . .
WORKDIR "/src/deployable projects/LocationConsumer"
RUN dotnet build "LocationConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LocationConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LocationConsumer.dll"]
