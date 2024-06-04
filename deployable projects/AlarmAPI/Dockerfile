FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["deployable projects/AlarmAPI/AlarmAPI.csproj", "deployable projects/AlarmAPI/"]
COPY ["shared/SharedClasses/SharedClasses.csproj", "shared/SharedClasses/"]
COPY ["shared/SharedModels/SharedModels.csproj", "shared/SharedModels/"]
RUN dotnet restore "deployable projects/AlarmAPI/AlarmAPI.csproj"
COPY . .
WORKDIR "/src/deployable projects/AlarmAPI"
RUN dotnet build "AlarmAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "AlarmAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "AlarmAPI.dll"]
