# ---------- Stage 1: Build ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

COPY QudraTech.sln .
COPY src/QudraTech.Api/QudraTech.Api.csproj src/QudraTech.Api/
COPY src/QudraTech.Application/QudraTech.Application.csproj src/QudraTech.Application/
COPY src/QudraTech.Domain/QudraTech.Domain.csproj src/QudraTech.Domain/
COPY src/QudraTech.Infrastructure/QudraTech.Infrastructure.csproj src/QudraTech.Infrastructure/
COPY tests/QudraTech.Tests/QudraTech.Tests.csproj tests/QudraTech.Tests/
RUN dotnet restore

COPY src/ src/
RUN dotnet publish src/QudraTech.Api/QudraTech.Api.csproj -c Release -o /app --no-restore

# ---------- Stage 2: Runtime ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app
COPY --from=build /app .

EXPOSE 8080
ENTRYPOINT ["dotnet", "QudraTech.Api.dll"]