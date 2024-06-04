FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["deployable projects/AlarmConsumer/AlarmConsumer.csproj", "deployable projects/AlarmConsumer/"]
COPY ["shared/SharedModels/SharedModels.csproj", "shared/SharedModels/"]
COPY ["shared/SharedClasses/SharedClasses.csproj", "shared/SharedClasses/"]
RUN dotnet restore "deployable projects/AlarmConsumer/AlarmConsumer.csproj"
COPY . .
WORKDIR "/src/deployable projects/AlarmConsumer"
RUN dotnet build "AlarmConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AlarmConsumer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AlarmConsumer.dll"]
