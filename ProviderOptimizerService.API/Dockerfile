# =========================
#   BUILD STAGE (SDK)
# =========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copiamos TODO el repositorio y restauramos sobre la API
# (La .dockerignore evitará enviar bin/obj/node_modules, etc.)
COPY . .

RUN dotnet restore ./ProviderOptimizerService.API/ProviderOptimizerService.API.csproj

RUN dotnet publish ./ProviderOptimizerService.API/ProviderOptimizerService.API.csproj \
    -c Release -o /app/publish

# =========================
#   RUNTIME STAGE (ASP.NET)
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final
WORKDIR /app

ENV ASPNETCORE_URLS=http://+:8080 \
    DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "ProviderOptimizerService.API.dll"]