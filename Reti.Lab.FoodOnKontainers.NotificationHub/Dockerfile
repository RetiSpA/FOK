FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["Reti.Lab.FoodOnKontainers.NotificationHub/Reti.Lab.FoodOnKontainers.NotificationHub.csproj", "Reti.Lab.FoodOnKontainers.NotificationHub/"]
RUN dotnet restore "Reti.Lab.FoodOnKontainers.NotificationHub/Reti.Lab.FoodOnKontainers.NotificationHub.csproj"
COPY . .
WORKDIR "/src/Reti.Lab.FoodOnKontainers.NotificationHub"
RUN dotnet build "Reti.Lab.FoodOnKontainers.NotificationHub.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Reti.Lab.FoodOnKontainers.NotificationHub.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Reti.Lab.FoodOnKontainers.NotificationHub.dll"]