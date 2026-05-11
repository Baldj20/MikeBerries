FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore ApiGateway.sln
WORKDIR /src/ApiGateway
RUN dotnet build "ApiGateway.csproj" -c Release -o /build
RUN dotnet publish "ApiGateway.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
USER app
EXPOSE 8080
COPY --from=build /app .
ENTRYPOINT ["dotnet", "ApiGateway.dll"]
