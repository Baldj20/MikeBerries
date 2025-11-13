FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore ProductService.sln
WORKDIR /src/ProductService.API
RUN dotnet build "ProductService.API.csproj" -c Release -o /build
RUN dotnet publish "ProductService.API.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
USER app
EXPOSE 8080
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ProductService.API.dll"]