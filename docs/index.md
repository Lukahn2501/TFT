---
layout: default
title: Home
nav_order: 1
---

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

## Documentation Sections

- [API Reference](api-reference.md) - Complete API endpoint documentation
- [Database Schema](database-schema.md) - Database structure and relationships
- [Development Guide](development-guide.md) - Setup and contribution guidelines

## Technology Stack

| Technology | Purpose |
|------------|---------|
| .NET 10 | Modern C# framework |
| ASP.NET Core | Minimal API |
| Entity Framework Core 10 | ORM |
| PostgreSQL 15 | Relational database |
| Scalar | Beautiful API documentation |
| Docker | Containerization |

## Project Structure

```
TFT/
├── src/
│   ├── TFT.Core/              # Domain models & DTOs
│   ├── TFT.Infrastructure/    # EF Core & database
│   ├── TFT.DataLoader/        # Data fetching console app
│   └── TFT.Api/               # REST API
├── docs/                      # Documentation (you are here)
├── docker-compose.yml         # Full orchestration
├── docker-compose.dev.yml     # PostgreSQL only
└── Makefile                   # Convenience commands
```

## Data Source

Data is sourced from **Community Dragon** - the community-driven League of Legends and TFT data project:
- Base URL: `https://raw.communitydragon.org/pbe/cdragon/tft/`
- Format: JSON (~25MB per language)
- Languages: `en_us`, `fr_fr`, and more

## License

MIT

---

**Built with ❤️ for the TFT community**
