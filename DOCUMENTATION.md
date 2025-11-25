# TFT Data API - Technical Documentation

This document provides detailed technical information about the TFT data structure, database schema, and API implementation.

## Table of Contents

- [Data Source](#data-source)
- [JSON Structure](#json-structure)
- [Database Schema](#database-schema)
- [API Implementation](#api-implementation)
- [Development Guide](#development-guide)

---

## Data Source

### Community Dragon

Data is fetched from [Community Dragon](https://raw.communitydragon.org/), a community-driven project that provides game data for League of Legends and Teamfight Tactics.

**Base URL**: `https://raw.communitydragon.org/pbe/cdragon/tft/`

**Available Languages**:
- `en_us.json` - English (US) - ~25MB
- `fr_fr.json` - French (France) - ~25MB
- Additional languages available

### Update Frequency

Data is typically updated:
- After each patch (every 2 weeks)
- When new sets are released
- When balance changes are made

---

## JSON Structure

The Community Dragon JSON file (`en_us.json`) contains three main top-level keys:

### 1. `items` (3,415 entries)

Contains all game items including augments, craftable items, consumables, and more.

#### Categories

| Category | Count | Description |
|----------|-------|-------------|
| **Augments** | ~1,663 | Special bonuses players select during match |
| **Craftable Items** | ~334 | Items created by combining components |
| **Champion Items** | ~101 | Unit-specific items |
| **Consumables** | ~64 | One-time use items |
| **Encounter Items** | ~73 | Items from special PvE events |
| **Assist/Support** | ~117 | Supporting game mechanics |
| **Others** | Various | Set-specific mechanics, emblems |

#### Item Structure

```json
{
  "apiName": "TFT_Item_RabadonsDeathcap",
  "name": "Rabadon's Deathcap",
  "desc": "This humble hat can help you make, or unmake, the world itself.",
  "icon": "ASSETS/Maps/TFT/Icons/Items/Hexcore/TFT_Item_RabadonsDeathcap.TFT_Set13.tex",
  "composition": ["TFT_Item_NeedlesslyLargeRod", "TFT_Item_NeedlesslyLargeRod"],
  "effects": {
    "AP": 50.0,
    "BonusDamage": 0.15
  },
  "associatedTraits": [],
  "incompatibleTraits": [],
  "tags": [],
  "unique": false
}
```

#### Augment Tiers

Augments are identified by their icon path:
- **Silver (Tier 1)**: `_I.TFT_SetX` in icon path
- **Gold (Tier 2)**: `_II.TFT_SetX` in icon path
- **Prismatic (Tier 3)**: `_III.TFT_SetX` in icon path

---

### 2. `sets` (11 sets)

Simplified mapping of TFT sets with basic information.

#### Available Sets

| Set # | Name | Description |
|-------|------|-------------|
| 1 | TutorialV2 | Tutorial set |
| 3 | Galaxies | Third major set |
| 4 | Fates II | Fourth major set |
| 5 | Set 5 | Fifth major set |
| 7 | Set7 | Seventh major set |
| 10 | Set10 | Tenth major set |
| 12 | Set12 | Twelfth major set |
| 13 | Set13 | Current set |
| 14 | Set14 | Next set |
| 15 | Set15 | Future set |
| 16 | Set16 | Future set |

#### Set Structure

```json
{
  "name": "Set13",
  "champions": ["TFT13_Singed", "TFT13_Jinx", ...],
  "traits": ["Automata", "Chem-Baron", ...]
}
```

---

### 3. `setData` (41 game mode variants)

Detailed set data organized by game mode mutators. Each set can have multiple variants.

#### Game Mode Variants

| Mode Suffix | Description |
|-------------|-------------|
| `TFTSetX` | Standard ranked/normal mode |
| `TFTSetX_TURBO` | Hyper Roll mode (faster gameplay) |
| `TFTSetX_PAIRS` | Double Up mode (2v2v2v2) |
| `TFTSetX_PVEMODE` | PvE encounters |
| `TFTSetX_Evolved` | Mid-set update variant |
| `TFTSetX_CarouselOfChaos` | Special rotating game mode |

#### Champion Structure

```json
{
  "apiName": "TFT13_Singed",
  "characterName": "TFT13_Singed",
  "name": "Singed",
  "cost": 1,
  "role": "APTank",
  "traits": ["Chem-Baron", "Sentinel"],
  "icon": "ASSETS/UX/TFT/ChampionSplashes/TFT13_Singed.TFT_Set13.dds",
  "squareIcon": "ASSETS/UX/TFT/ChampionTiles/TFT13_Stage_Singed.TFT_Set13.dds",
  "tileIcon": "ASSETS/UX/TraitIcons/Trait_Icon_13_Singed.TFT_Set13.dds",
  "ability": {
    "name": "Dangerous Mutations",
    "desc": "Gain Durability and grant allies...",
    "icon": "ASSETS/...",
    "variables": [
      {
        "name": "AttackSpeed",
        "value": [1.0, 100.0, 115.0, 130.0, 150.0, 1.0, 1.0]
      },
      {
        "name": "Duration",
        "value": [4.0, 4.0, 4.0, 4.0, 4.0, 4.0, 4.0]
      }
    ]
  },
  "stats": {
    "armor": 40.0,
    "attackSpeed": 0.6,
    "critChance": 0.25,
    "critMultiplier": 1.4,
    "damage": 55.0,
    "hp": 650.0,
    "initialMana": 0,
    "magicResist": 40.0,
    "mana": 50.0,
    "range": 1.0
  }
}
```

**Ability Variables**: The `variables` array uses star-level indexing:
- Index 0: Base stats
- Index 1: 1-star
- Index 2: 2-star
- Index 3: 3-star
- Index 4+: Higher star levels

#### Trait Structure

```json
{
  "apiName": "TFT13_Hextech",
  "name": "Automata",
  "desc": "Automata gain a crystal when they deal damage...",
  "icon": "ASSETS/UX/TraitIcons/Trait_Icon_13_Automata.TFT_Set13.tex",
  "effects": [
    {
      "minUnits": 2,
      "maxUnits": 3,
      "style": 1,
      "variables": {
        "MagicDamage": 150.0,
        "Resists": 25.0,
        "TriggerNumCrystals": 20.0
      }
    },
    {
      "minUnits": 4,
      "maxUnits": 5,
      "style": 3,
      "variables": {
        "MagicDamage": 450.0,
        "Resists": 60.0
      }
    }
  ]
}
```

**Trait Effect Styles**:
- `style: 1` - Bronze tier (weakest)
- `style: 3` - Silver tier (medium)
- `style: 5` - Gold tier (strongest)
- Higher values indicate more powerful breakpoints

---

## Database Schema

### Entity Relationship Diagram

```
┌─────────────┐
│   SetData   │
└─────┬───────┘
      │
      ├─────────────┐
      │             │
      ▼             ▼
┌─────────┐   ┌─────────┐
│Champion │   │ Trait   │
└────┬────┘   └────┬────┘
     │             │
     └──────┬──────┘
            │
            ▼
    ┌───────────────┐
    │ChampionTrait  │  (Join Table)
    └───────────────┘

┌─────────┐
│  Item   │  (Global)
└─────────┘

┌─────────┐
│ Augment │  (Global)
└─────────┘
```

### Tables

#### `SetData`
Represents a specific game mode variant (e.g., TFTSet13, TFTSet13_TURBO).

| Column | Type | Description |
|--------|------|-------------|
| Id | int | Primary key |
| Name | string | Set identifier (e.g., "TFTSet13") |
| MutatorSetData | jsonb | Raw JSON metadata |

**Indexes**: Name (unique)

---

#### `Champions`
Champions within a specific set.

| Column | Type | Description |
|--------|------|-------------|
| Id | int | Primary key |
| ApiName | string | API identifier |
| CharacterName | string | Character name |
| Name | string | Display name |
| Cost | int | Gold cost (1-5) |
| Role | string | Role (e.g., "APTank", "ADCarry") |
| Icon | string | Icon path |
| SquareIcon | string | Square icon path |
| TileIcon | string | Tile icon path |
| Ability | jsonb | Ability details (JSON) |
| Stats | jsonb | Champion stats (JSON) |
| SetDataId | int | Foreign key → SetData |

**Indexes**: ApiName, Name, Cost, SetDataId

**Relationships**:
- Many-to-one with SetData
- Many-to-many with Traits (via ChampionTrait)

---

#### `Traits`
Synergies/traits within a specific set.

| Column | Type | Description |
|--------|------|-------------|
| Id | int | Primary key |
| ApiName | string | API identifier |
| Name | string | Display name |
| Description | string | Trait description |
| Icon | string | Icon path |
| Effects | jsonb | Trait effects (JSON) |
| SetDataId | int | Foreign key → SetData |

**Indexes**: ApiName, Name, SetDataId

**Relationships**:
- Many-to-one with SetData
- Many-to-many with Champions (via ChampionTrait)

---

#### `ChampionTrait`
Join table for many-to-many relationship.

| Column | Type | Description |
|--------|------|-------------|
| ChampionId | int | Foreign key → Champions |
| TraitId | int | Foreign key → Traits |

**Primary Key**: (ChampionId, TraitId)

---

#### `Items`
Global items across all sets.

| Column | Type | Description |
|--------|------|-------------|
| Id | int | Primary key |
| ApiName | string | API identifier |
| Name | string | Display name |
| Description | string | Item description |
| Icon | string | Icon path |
| Composition | jsonb | Component items (JSON array) |
| Effects | jsonb | Item effects (JSON) |
| AssociatedTraits | jsonb | Associated traits (JSON array) |
| IncompatibleTraits | jsonb | Incompatible traits (JSON array) |
| IsUnique | bool | Whether item is unique |

**Indexes**: ApiName (unique), Name

---

#### `Augments`
Global augments across all sets.

| Column | Type | Description |
|--------|------|-------------|
| Id | int | Primary key |
| ApiName | string | API identifier |
| Name | string | Display name |
| Description | string | Augment description |
| Icon | string | Icon path |
| Effects | jsonb | Augment effects (JSON) |
| AssociatedTraits | jsonb | Associated traits (JSON array) |
| IncompatibleTraits | jsonb | Incompatible traits (JSON array) |

**Indexes**: ApiName (unique), Name, Icon (for tier extraction)

---

## API Implementation

### Architecture

The API follows a minimal API pattern with ASP.NET Core 10.

#### Layers

```
┌────────────────────┐
│   HTTP Requests    │
└─────────┬──────────┘
          │
          ▼
┌────────────────────┐
│  ASP.NET Core API  │  (Program.cs)
│  - Route Handlers  │
│  - Query Params    │
└─────────┬──────────┘
          │
          ▼
┌────────────────────┐
│  EF Core Context   │  (TftContext)
│  - DbSets          │
│  - LINQ Queries    │
└─────────┬──────────┘
          │
          ▼
┌────────────────────┐
│    PostgreSQL      │
└────────────────────┘
```

### Endpoints

#### GET `/api/champions`

Get all champions with optional filtering.

**Query Parameters**:
- `trait` (string): Filter by trait name
- `cost` (int): Filter by cost (1-5)
- `set` (string): Filter by set name

**Response**: Array of champions

**Example**:
```bash
GET /api/champions?trait=Automata&cost=3
```

---

#### GET `/api/champions/{name}`

Get a specific champion by name.

**Path Parameters**:
- `name` (string): Champion name

**Response**: Single champion or 404

**Example**:
```bash
GET /api/champions/Jinx
```

---

#### GET `/api/compositions/{trait}`

Get team composition recommendations for a trait.

**Path Parameters**:
- `trait` (string): Trait name

**Query Parameters**:
- `set` (string): Filter by set name

**Response**: Object with trait details and champions

**Example**:
```bash
GET /api/compositions/Automata?set=TFTSet13
```

---

#### GET `/api/traits`

Get all traits with optional filtering.

**Query Parameters**:
- `set` (string): Filter by set name

**Response**: Array of traits

**Example**:
```bash
GET /api/traits?set=TFTSet13
```

---

#### GET `/api/items`

Get all items.

**Response**: Array of items

**Example**:
```bash
GET /api/items
```

---

#### GET `/api/augments`

Get all augments with optional filtering.

**Query Parameters**:
- `tier` (int): Filter by tier (1=Silver, 2=Gold, 3=Prismatic)
- `trait` (string): Filter by associated trait

**Response**: Array of augments

**Example**:
```bash
GET /api/augments?tier=3
```

---

#### GET `/api/sets`

Get all available sets with champion and trait counts.

**Response**: Array of sets with metadata

**Example**:
```bash
GET /api/sets
```

---

## Development Guide

### Prerequisites

- .NET 10 SDK
- PostgreSQL 15+
- Docker (optional)

### Local Setup

1. **Clone repository**
   ```bash
   git clone <repo-url>
   cd TFT
   ```

2. **Start PostgreSQL**
   ```bash
   docker compose -f docker-compose.dev.yml up -d
   ```

3. **Load data**
   ```bash
   cd src/TFT.DataLoader
   dotnet run
   ```

4. **Run API**
   ```bash
   cd src/TFT.Api
   dotnet run
   ```

### Configuration

#### Connection Strings

Edit `appsettings.json` in both DataLoader and Api projects:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Host=localhost;Port=5432;Database=tft_data;Username=postgres;Password=postgres"
  }
}
```

#### Community Dragon URL

Edit `appsettings.json` in DataLoader:

```json
{
  "CommunityDragon": {
    "BaseUrl": "https://raw.communitydragon.org/pbe/cdragon/tft/",
    "Language": "en_us"
  }
}
```

Change `Language` to support other languages (e.g., `fr_fr`).

### Docker

#### Build Images

```bash
docker compose build
```

#### Run All Services

```bash
docker compose up -d
```

#### View Logs

```bash
docker compose logs -f api
docker compose logs -f dataloader
docker compose logs -f postgres
```

### Testing

#### Manual API Testing

```bash
# Test sets endpoint
curl http://localhost:5000/api/sets | jq

# Test champions with filtering
curl http://localhost:5000/api/champions?trait=Automata | jq

# Test specific champion
curl http://localhost:5000/api/champions/Jinx | jq

# Test augments by tier
curl http://localhost:5000/api/augments?tier=3 | jq
```

#### Using Scalar UI

Navigate to `http://localhost:5000/scalar/v1` for interactive API testing.

---

## Data Statistics

| Metric | Count |
|--------|-------|
| Total Items | 3,415 |
| Regular Items | 1,496 |
| Augments | 1,910 |
| Total Set Variants | 41 |
| Unique Sets | 11 |
| Champion Entries (all variants) | 3,527 |
| Trait Entries (all variants) | 2,028 |

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

## Performance Considerations

### Database Indexes

Key indexes for performance:
- `Champions.ApiName` - Champion lookups
- `Champions.Cost` - Filtering by cost
- `Traits.Name` - Trait lookups
- `Items.ApiName` - Item lookups
- `Augments.Icon` - Tier extraction

### Query Optimization

- Use `.AsNoTracking()` for read-only queries
- Include related data with `.Include()`
- Filter at database level with `Where()` before loading

### Caching (Future)

Consider adding Redis for:
- Frequently accessed sets
- Popular champion compositions
- Item/augment lookups

---

## Troubleshooting

### Common Issues

**Issue**: DataLoader fails with "null reference"
- **Solution**: Data may have null names - filtering is implemented

**Issue**: API returns empty results
- **Solution**: Ensure DataLoader ran successfully and populated database

**Issue**: Port 5000 already in use
- **Solution**: Stop existing process with `pkill -f "dotnet.*TFT.Api"`

**Issue**: PostgreSQL connection refused
- **Solution**: Ensure PostgreSQL is running: `docker compose -f docker-compose.dev.yml up -d`

---

## Contributing

When contributing:
1. Follow existing code patterns
2. Update tests for new features
3. Document API changes in this file
4. Test with full data load

---

## License

MIT
