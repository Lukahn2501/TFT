---
layout: default
title: Database Schema
nav_order: 3
---

# Database Schema

This document provides detailed information about the database structure and relationships.

## Entity Relationship Diagram

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

---

## Tables

### SetData

Represents a specific game mode variant (e.g., TFTSet13, TFTSet13_TURBO).

| Column | Type | Description |
|--------|------|-------------|
| Id | int | Primary key |
| Name | string | Set identifier (e.g., "TFTSet13") |
| MutatorSetData | jsonb | Raw JSON metadata |

**Indexes:** Name (unique)

---

### Champions

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

**Indexes:** ApiName, Name, Cost, SetDataId

**Relationships:**
- Many-to-one with SetData
- Many-to-many with Traits (via ChampionTrait)

---

### Traits

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

**Indexes:** ApiName, Name, SetDataId

**Relationships:**
- Many-to-one with SetData
- Many-to-many with Champions (via ChampionTrait)

---

### ChampionTrait

Join table for many-to-many relationship between Champions and Traits.

| Column | Type | Description |
|--------|------|-------------|
| ChampionId | int | Foreign key → Champions |
| TraitId | int | Foreign key → Traits |

**Primary Key:** (ChampionId, TraitId)

---

### Items

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

**Indexes:** ApiName (unique), Name

---

### Augments

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

**Indexes:** ApiName (unique), Name, Icon (for tier extraction)

---

## JSON Data Structures

### Champion Structure

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

**Ability Variables:** The `variables` array uses star-level indexing:
- Index 0: Base stats
- Index 1: 1-star
- Index 2: 2-star
- Index 3: 3-star
- Index 4+: Higher star levels

---

### Trait Structure

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

**Trait Effect Styles:**
- `style: 1` - Bronze tier (weakest)
- `style: 3` - Silver tier (medium)
- `style: 5` - Gold tier (strongest)
- Higher values indicate more powerful breakpoints

---

### Item Structure

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

## Game Mode Variants

Each set can have multiple variants:

| Mode Suffix | Description |
|-------------|-------------|
| `TFTSetX` | Standard ranked/normal mode |
| `TFTSetX_TURBO` | Hyper Roll mode (faster gameplay) |
| `TFTSetX_PAIRS` | Double Up mode (2v2v2v2) |
| `TFTSetX_PVEMODE` | PvE encounters |
| `TFTSetX_Evolved` | Mid-set update variant |
| `TFTSetX_CarouselOfChaos` | Special rotating game mode |

---

## Performance Considerations

### Key Indexes

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
