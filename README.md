# TFT Data API

> A modern REST API for Teamfight Tactics game data, powered by .NET 10 and PostgreSQL.

## Overview

This project provides a comprehensive backend service for fetching, parsing, and serving Teamfight Tactics game data from [Community Dragon](https://raw.communitydragon.org/). Built with .NET 10, it offers a clean REST API with interactive documentation via Scalar UI.

## Features

✨ **Comprehensive Data Coverage**
- 3,500+ champions across all game modes
- 2,000+ traits and synergies
- 1,900+ augments (Silver, Gold, Prismatic)
- 1,500+ items (craftable, consumables, emblems)
- 41 set variants (Standard, Hyper Roll, Double Up, etc.)

🚀 **Modern Architecture**
- .NET 10 with minimal API design
- Entity Framework Core for data access
- PostgreSQL database with optimized schema
- Docker containerization for easy deployment
- Beautiful API documentation with Scalar UI

🎯 **Developer Experience**
- Interactive API explorer at `/scalar/v1`
- OpenAPI specification at `/openapi/v1.json`
- Comprehensive filtering and querying
- CORS enabled for frontend integration

## Quick Start

### Using Docker (Recommended)

```bash
# Start everything (database + data loader + API)
docker compose up -d

# API available at http://localhost:5000
# Scalar UI at http://localhost:5000/scalar/v1
```

### Local Development

```bash
# Start PostgreSQL
docker compose -f docker-compose.dev.yml up -d

# Load data from Community Dragon
cd src/TFT.DataLoader && dotnet run

# Run API
cd src/TFT.Api && dotnet run
```

## API Endpoints

| Endpoint | Description | Example |
|----------|-------------|---------|
| `GET /api/champions` | List all champions | `?trait=Ambusher&cost=3` |
| `GET /api/champions/{name}` | Get champion details | `/api/champions/Jinx` |
| `GET /api/compositions/{trait}` | Get team composition | `/api/compositions/Automata` |
| `GET /api/traits` | List all traits | `?set=TFTSet13` |
| `GET /api/items` | List all items | - |
| `GET /api/augments` | List augments | `?tier=3&trait=Automata` |
| `GET /api/sets` | List all TFT sets | - |

## Technology Stack

- **.NET 10** - Modern C# framework
- **ASP.NET Core** - Minimal API
- **Entity Framework Core 10** - ORM
- **PostgreSQL 15** - Relational database
- **Scalar** - Beautiful API documentation
- **Docker** - Containerization

## Project Structure

```
TFT/
├── src/
│   ├── TFT.Core/              # Domain models & DTOs
│   ├── TFT.Infrastructure/    # EF Core & database
│   ├── TFT.DataLoader/        # Data fetching console app
│   └── TFT.Api/               # REST API
├── docker-compose.yml         # Full orchestration
├── docker-compose.dev.yml     # PostgreSQL only
└── Makefile                   # Convenience commands
```

## Data Source

Data is sourced from **Community Dragon** - the community-driven League of Legends and TFT data project:
- Base URL: `https://raw.communitydragon.org/pbe/cdragon/tft/`
- Format: JSON (~25MB per language)
- Languages: `en_us`, `fr_fr`, and more

## Documentation

For detailed information about:
- Data structure and schema → See `DOCUMENTATION.md`
- API usage and examples → Visit `/scalar/v1` when running
- Docker deployment → See `docker-compose.yml`
- Development setup → See commands below

## Makefile Commands

```bash
make help        # Show all available commands
make dev-up      # Start PostgreSQL for local dev
make run-loader  # Fetch and load data
make run-api     # Run API locally
make up          # Start all services
make down        # Stop all services
make logs        # View logs
make clean       # Clean everything
```

## Future Roadmap

- [ ] React frontend with team builder
- [ ] AI-powered composition recommendations
- [ ] Redis caching layer
- [ ] Automated updates on patch days
- [ ] Meta analytics and statistics
- [ ] Multi-language support

## License

MIT

---

**Built with ❤️ for the TFT community**
