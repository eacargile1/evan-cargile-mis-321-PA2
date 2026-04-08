# Heroku container deploy — single web dyno serves API + wwwroot SPA
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src
COPY LTS.sln ./
COPY src/LTS.Core/LTS.Core.csproj src/LTS.Core/
COPY src/LTS.Infrastructure/LTS.Infrastructure.csproj src/LTS.Infrastructure/
COPY src/LTS.Api/LTS.Api.csproj src/LTS.Api/
RUN dotnet restore src/LTS.Api/LTS.Api.csproj
COPY src/ src/
RUN dotnet publish src/LTS.Api/LTS.Api.csproj -c Release -o /app/out --no-restore

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app
COPY --from=build /app/out .
# Heroku sets PORT at runtime; Program.cs also reads PORT via WebHost
CMD ["dotnet", "LTS.Api.dll"]
