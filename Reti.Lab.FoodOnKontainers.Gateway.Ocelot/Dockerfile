FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["Reti.Lab.FoodOnKontainers.Gateway.Ocelot/Reti.Lab.FoodOnKontainers.Gateway.Ocelot.csproj", "Reti.Lab.FoodOnKontainers.Gateway.Ocelot/"]
RUN dotnet restore "Reti.Lab.FoodOnKontainers.Gateway.Ocelot/Reti.Lab.FoodOnKontainers.Gateway.Ocelot.csproj"
COPY . .
WORKDIR "/src/Reti.Lab.FoodOnKontainers.Gateway.Ocelot"
RUN dotnet build "Reti.Lab.FoodOnKontainers.Gateway.Ocelot.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "Reti.Lab.FoodOnKontainers.Gateway.Ocelot.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Reti.Lab.FoodOnKontainers.Gateway.Ocelot.dll"]