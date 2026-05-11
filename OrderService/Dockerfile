FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore OrderService.sln
WORKDIR /src/OrderService.Presentation
RUN dotnet build "OrderService.Presentation.csproj" -c Release -o /build
RUN dotnet publish "OrderService.Presentation.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
USER app
EXPOSE 8080
COPY --from=build /app .
ENTRYPOINT ["dotnet", "OrderService.Presentation.dll"]
