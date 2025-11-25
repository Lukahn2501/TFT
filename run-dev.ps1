# Development startup script
Write-Host "🚀 Starting TFT Development Environment..." -ForegroundColor Green

# Check if PostgreSQL is running
$postgresRunning = docker ps --filter "name=tft-postgres-dev" --format "{{.Names}}"
if (-not $postgresRunning) {
    Write-Host "📦 Starting PostgreSQL..." -ForegroundColor Yellow
    docker compose -f docker-compose.dev.yml up -d
    Start-Sleep -Seconds 3
}
else {
    Write-Host "✓ PostgreSQL is already running" -ForegroundColor Green
}

# Navigate to API directory and start with hot reload
Write-Host "🔥 Starting API with Hot Reload..." -ForegroundColor Yellow
Write-Host "💡 Press Ctrl+R to restart, Ctrl+C to stop" -ForegroundColor Cyan
Write-Host ""
Set-Location src/TFT.Api
dotnet watch run
