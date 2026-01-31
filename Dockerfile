FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

COPY ["./src/Expensi.Api", "/src/Expensi.Api"]
COPY ["./src/Expensi.Domain", "/src/Expensi.Domain"]
COPY ["./src/Expensi.Infrastructure", "/src/Expensi.Infrastructure"]
COPY ["Directory.Build.props", "Directory.Build.props"]
COPY ["Directory.Packages.props", "Directory.Packages.props"]

WORKDIR "/src/Expensi.Api"

RUN dotnet restore "Expensi.Api.csproj"
RUN dotnet build "Expensi.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Expensi.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Expensi.Api.dll"]
