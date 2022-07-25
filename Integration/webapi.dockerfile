### STAGE 1: Build ###

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env

WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.csproj ./

RUN dotnet restore -r linux-arm64

# Copy everything else and build
COPY . .

RUN dotnet publish -c Release -o out

### STAGE 2: Setup ###
FROM mcr.microsoft.com/dotnet/aspnet:6.0

WORKDIR /app

COPY --from=build-env /app/out .

ENTRYPOINT ["dotnet", "CarbonaraWebAPI.dll"]
