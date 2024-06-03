FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["deployable projects/LocationRetrievalApi/LocationRetrievalApi.csproj", "deployable projects/LocationRetrievalApi/"]
RUN dotnet restore "deployable projects/LocationRetrievalApi/LocationRetrievalApi.csproj"
COPY . .
WORKDIR "/src/deployable projects/LocationRetrievalApi"
RUN dotnet build "LocationRetrievalApi.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "LocationRetrievalApi.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "LocationRetrievalApi.dll"]
