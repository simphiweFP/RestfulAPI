FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

COPY ShippingApi.sln ./
COPY ShippingApi/ShippingApi.csproj ShippingApi/
COPY ShippingApi.UnitTests/ShippingApi.UnitTests.csproj ShippingApi.UnitTests/
COPY ShippingApi.IntegrationTests/ShippingApi.IntegrationTests.csproj ShippingApi.IntegrationTests/
RUN dotnet restore ShippingApi.sln

COPY . .
RUN dotnet publish ShippingApi/ShippingApi.csproj -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS runtime
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "ShippingApi.dll"]
