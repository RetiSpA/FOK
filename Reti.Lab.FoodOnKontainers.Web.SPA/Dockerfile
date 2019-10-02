ARG NODE_IMAGE=node:10.15

FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src

FROM ${NODE_IMAGE} AS node-build
WORKDIR /web
COPY Reti.Lab.FoodOnKontainers.Web.SPA .
WORKDIR /web/ClientApp
RUN npm install
RUN npm run build:prod

FROM build AS publish
WORKDIR "/src/Reti.Lab.FoodOnKontainers.Web.SPA/ClientApp/dist"
COPY --from=node-build /web/ClientApp/dist .
WORKDIR /src
COPY . . 
WORKDIR "/src/Reti.Lab.FoodOnKontainers.Web.SPA"
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "Reti.Lab.FoodOnKontainers.Web.SPA.dll"]