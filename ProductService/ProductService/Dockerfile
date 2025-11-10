FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ProductService.sln .
COPY ["ProductService.API/ProductService.API.csproj", "ProductService.API/"]
COPY ["ProductService.BLL/ProductService.BLL.csproj", "ProductService.BLL/"]
COPY ["ProductService.DAL/ProductService.DAL.csproj", "ProductService.DAL/"]
COPY ["ProductService.UnitTests/ProductService.UnitTests.csproj", "ProductService.UnitTests/"]
COPY ["ProductService.IntegrationTests/ProductService.IntegrationTests.csproj", "ProductService.IntegrationTests/"]
RUN dotnet restore ProductService.sln
COPY . .
RUN dotnet publish "ProductService.API/ProductService.API.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
USER app
EXPOSE 8080
COPY --from=build /app /
ENTRYPOINT ["dotnet", "ProductService.API.dll"]