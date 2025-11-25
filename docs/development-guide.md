---
layout: default
title: Development Guide
nav_order: 4
---

# Development Guide

This guide covers setting up your development environment and contributing to the TFT Data API project.

## Prerequisites

- .NET 10 SDK
- PostgreSQL 15+
- Docker (optional but recommended)

---

## Local Setup

### 1. Clone Repository

```bash
git clone <repo-url>
cd TFT
```

### 2. Start PostgreSQL

Using Docker (recommended):

```bash
docker compose -f docker-compose.dev.yml up -d
```

Or install PostgreSQL locally and create the database:

```sql
CREATE DATABASE tft_data;
```

### 3. Load Data

```bash
cd src/TFT.DataLoader
dotnet run
```

This will:
- Fetch data from Community Dragon
- Parse and transform the JSON
- Load data into PostgreSQL

### 4. Run API

```bash
cd src/TFT.Api
dotnet run
```

The API will be available at `http://localhost:5000`.

---

## Configuration

### Connection Strings

Edit `appsettings.json` in both DataLoader and Api projects:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=tft_data;Username=postgres;Password=postgres"
  }
}
```

### Community Dragon Settings

Edit `appsettings.json` in DataLoader:

```json
{
  "CommunityDragon": {
    "BaseUrl": "https://raw.communitydragon.org/pbe/cdragon/tft/",
    "Language": "en_us"
  }
}
```

Available languages: `en_us`, `fr_fr`, and more.

---

## Docker

### Build Images

```bash
docker compose build
```

### Run All Services

```bash
docker compose up -d
```

This starts:
- PostgreSQL database
- DataLoader (one-time data load)
- API service

### View Logs

```bash
docker compose logs -f api
docker compose logs -f dataloader
docker compose logs -f postgres
```

### Stop Services

```bash
docker compose down
```

### Clean Everything

```bash
docker compose down -v
docker rmi tft-api tft-dataloader 2>/dev/null || true
```

---

## Makefile Commands

The project includes a Makefile for convenience:

```bash
make help        # Show all available commands
make dev-up      # Start PostgreSQL for local dev
make run-loader  # Fetch and load data
make run-api     # Run API locally
make up          # Start all services
make down        # Stop all services
make logs        # View logs
make clean       # Clean everything
make test        # Run tests
make restore     # Restore NuGet packages
```

---

## Testing

### Run Tests

```bash
dotnet test --verbosity normal
```

Or using Make:

```bash
make test
```

### Manual API Testing

```bash
# Test sets endpoint
curl http://localhost:5000/api/sets | jq

# Test champions with filtering
curl "http://localhost:5000/api/champions?trait=Automata" | jq

# Test specific champion
curl http://localhost:5000/api/champions/Jinx | jq

# Test augments by tier
curl "http://localhost:5000/api/augments?tier=3" | jq
```

### Interactive Testing

Navigate to `http://localhost:5000/scalar/v1` for interactive API testing with Scalar UI.

---

## Project Structure

```
TFT/
├── src/
│   ├── TFT.Core/              # Domain models & DTOs
│   │   ├── Models/            # Entity models
│   │   └── DTOs/              # Data Transfer Objects
│   ├── TFT.Infrastructure/    # EF Core & database
│   │   └── Data/              # DbContext & configurations
│   ├── TFT.DataLoader/        # Data fetching console app
│   │   └── Services/          # Data loading services
│   ├── TFT.Api/               # REST API
│   │   └── Program.cs         # API entry point
│   └── TFT.Core.Tests/        # Unit tests
├── docs/                      # Documentation (GitHub Pages)
├── docker-compose.yml         # Full orchestration
├── docker-compose.dev.yml     # PostgreSQL only
├── Makefile                   # Convenience commands
└── TFT.sln                    # Solution file
```

---

## Data Source

### Community Dragon

Data is fetched from [Community Dragon](https://raw.communitydragon.org/), a community-driven project that provides game data.

**Base URL:** `https://raw.communitydragon.org/pbe/cdragon/tft/`

### Update Frequency

Data is typically updated:
- After each patch (every 2 weeks)
- When new sets are released
- When balance changes are made

---

## Common Patterns

### Naming Conventions

- API names follow pattern: `TFT{SetNumber}_{Type}_{Name}`
- Example: `TFT13_Singed`, `TFT_Item_RabadonsDeathcap`

### Text Formatting

Descriptions may contain special formatting tags:
- `<TFTBonus>` - Bonus values
- `<magicDamage>` - Damage type indicators
- `@VariableName@` - Dynamic text substitution

### Asset Paths

All asset paths follow Riot's internal structure:
- Icons: `ASSETS/UX/TFT/...`
- Splashes: `ASSETS/UX/TFT/ChampionSplashes/...`
- Traits: `ASSETS/UX/TraitIcons/...`

---

## Troubleshooting

### Common Issues

**Issue:** DataLoader fails with "null reference"
- **Solution:** Data may have null names - filtering is implemented

**Issue:** API returns empty results
- **Solution:** Ensure DataLoader ran successfully and populated database

**Issue:** Port 5000 already in use
- **Solution:** Stop existing process: `pkill -f "dotnet.*TFT.Api"`

**Issue:** PostgreSQL connection refused
- **Solution:** Ensure PostgreSQL is running: `docker compose -f docker-compose.dev.yml up -d`

---

## Contributing

When contributing:

1. Follow existing code patterns
2. Update tests for new features
3. Document API changes
4. Test with full data load
5. Update documentation if needed

### Code Style

- Use C# conventions
- Follow minimal API patterns
- Use async/await for I/O operations
- Add XML documentation for public APIs

### Pull Request Process

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Run tests
5. Submit a pull request

---

## Future Roadmap

- [ ] React frontend with team builder
- [ ] AI-powered composition recommendations
- [ ] Redis caching layer
- [ ] Automated updates on patch days
- [ ] Meta analytics and statistics
- [ ] Multi-language support

---

## License

MIT
