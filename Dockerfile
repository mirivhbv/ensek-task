FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY src/MeterReadingApi ./MeterReadingApi
COPY src/MeterReadingApi.Core ./MeterReadingApi.Core
WORKDIR /src/MeterReadingApi
RUN dotnet restore "MeterReadingApi.csproj"
RUN dotnet publish "MeterReadingApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MeterReadingApi.dll"]
