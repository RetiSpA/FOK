FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY Reti.Lab.FoodOnKontainers.Deliveries.Api/Reti.Lab.FoodOnKontainers.Deliveries.Api.csproj Reti.Lab.FoodOnKontainers.Deliveries.Api/
RUN dotnet restore Reti.Lab.FoodOnKontainers.Deliveries.Api/Reti.Lab.FoodOnKontainers.Deliveries.Api.csproj
COPY . .
WORKDIR /src/Reti.Lab.FoodOnKontainers.Deliveries.Api
RUN dotnet build Reti.Lab.FoodOnKontainers.Deliveries.Api.csproj -c Release -o /app

FROM build AS publish
RUN dotnet publish Reti.Lab.FoodOnKontainers.Deliveries.Api.csproj -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Reti.Lab.FoodOnKontainers.Deliveries.Api.dll"]
