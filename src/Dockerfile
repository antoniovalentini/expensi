FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers
COPY *.sln .
COPY Avalentini.Expensi.Core/*.csproj ./Avalentini.Expensi.Core/
COPY Avalentini.Expensi.Api/*.csproj ./Avalentini.Expensi.Api/
COPY Avalentini.Expensi.DbSeed/*.csproj ./Avalentini.Expensi.DbSeed/
COPY Avalentini.Expensi.WebApp/*.csproj ./Avalentini.Expensi.WebApp/
RUN dotnet restore

# Copy everything else and build
COPY Avalentini.Expensi.Core/. ./Avalentini.Expensi.Core/
COPY Avalentini.Expensi.Api/. ./Avalentini.Expensi.Api/

WORKDIR /app/Avalentini.Expensi.Api
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/Avalentini.Expensi.Api/out .
ENTRYPOINT ["dotnet", "Avalentini.Expensi.Api.dll"]