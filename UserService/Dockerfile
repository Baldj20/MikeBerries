FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore UserService.sln
WORKDIR /src/UserService.API
RUN dotnet build "UserService.API.csproj" -c Release -o /build
RUN dotnet publish "UserService.API.csproj" -c Release -o /app

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
USER app
EXPOSE 8080
COPY --from=build /app .
ENTRYPOINT ["dotnet", "UserService.API.dll"]