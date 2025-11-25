# Build stage
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copy solution and project files
COPY ["TFT.sln", "./"]
COPY ["src/TFT.Api/TFT.Api.csproj", "src/TFT.Api/"]
COPY ["src/TFT.Core/TFT.Core.csproj", "src/TFT.Core/"]
COPY ["src/TFT.Infrastructure/TFT.Infrastructure.csproj", "src/TFT.Infrastructure/"]

# Restore dependencies
RUN dotnet restore "src/TFT.Api/TFT.Api.csproj"

# Copy all source files
COPY . .

# Build the application
WORKDIR "/src/src/TFT.Api"
RUN dotnet build "TFT.Api.csproj" -c Release -o /app/build

# Publish stage
FROM build AS publish
RUN dotnet publish "TFT.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Create non-root user
RUN useradd -m -u 1000 tftuser && chown -R tftuser:tftuser /app
USER tftuser

COPY --from=publish /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "TFT.Api.dll"]
