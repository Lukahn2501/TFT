---
layout: default
title: API Reference
nav_order: 2
---

# API Reference

This document provides detailed information about all available API endpoints.

## Base URL

```
http://localhost:5000/api
```

## Endpoints Overview

| Endpoint | Method | Description |
|----------|--------|-------------|
| `/api/champions` | GET | List all champions |
| `/api/champions/{name}` | GET | Get champion details |
| `/api/compositions/{trait}` | GET | Get team composition |
| `/api/traits` | GET | List all traits |
| `/api/items` | GET | List all items |
| `/api/augments` | GET | List augments |
| `/api/sets` | GET | List all TFT sets |

---

## Champions

### GET `/api/champions`

Get all champions with optional filtering.

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `trait` | string | Filter by trait name |
| `cost` | int | Filter by cost (1-5) |
| `set` | string | Filter by set name |

**Example Request:**

```bash
curl "http://localhost:5000/api/champions?trait=Automata&cost=3"
```

**Example Response:**

```json
[
  {
    "apiName": "TFT13_Jinx",
    "name": "Jinx",
    "cost": 3,
    "traits": ["Rebel", "Ambusher"],
    "stats": {
      "hp": 750,
      "damage": 55,
      "armor": 25
    }
  }
]
```

---

### GET `/api/champions/{name}`

Get a specific champion by name.

**Path Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `name` | string | Champion name |

**Example Request:**

```bash
curl "http://localhost:5000/api/champions/Jinx"
```

**Response:** Single champion object or 404 if not found.

---

## Compositions

### GET `/api/compositions/{trait}`

Get team composition recommendations for a trait.

**Path Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `trait` | string | Trait name |

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `set` | string | Filter by set name |

**Example Request:**

```bash
curl "http://localhost:5000/api/compositions/Automata?set=TFTSet13"
```

**Example Response:**

```json
{
  "trait": "Automata",
  "breakpoints": [2, 4, 6],
  "champions": [
    {"name": "Blitzcrank", "cost": 1},
    {"name": "Camille", "cost": 2}
  ]
}
```

---

## Traits

### GET `/api/traits`

Get all traits with optional filtering.

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `set` | string | Filter by set name |

**Example Request:**

```bash
curl "http://localhost:5000/api/traits?set=TFTSet13"
```

---

## Items

### GET `/api/items`

Get all items.

**Example Request:**

```bash
curl "http://localhost:5000/api/items"
```

**Example Response:**

```json
[
  {
    "apiName": "TFT_Item_RabadonsDeathcap",
    "name": "Rabadon's Deathcap",
    "desc": "This humble hat can help you make, or unmake, the world itself.",
    "composition": ["TFT_Item_NeedlesslyLargeRod", "TFT_Item_NeedlesslyLargeRod"],
    "effects": {
      "AP": 50.0,
      "BonusDamage": 0.15
    }
  }
]
```

---

## Augments

### GET `/api/augments`

Get all augments with optional filtering.

**Query Parameters:**

| Parameter | Type | Description |
|-----------|------|-------------|
| `tier` | int | Filter by tier (1=Silver, 2=Gold, 3=Prismatic) |
| `trait` | string | Filter by associated trait |

**Example Request:**

```bash
curl "http://localhost:5000/api/augments?tier=3"
```

**Augment Tiers:**

| Tier | Name | Icon Pattern |
|------|------|--------------|
| 1 | Silver | `_I.TFT_SetX` |
| 2 | Gold | `_II.TFT_SetX` |
| 3 | Prismatic | `_III.TFT_SetX` |

---

## Sets

### GET `/api/sets`

Get all available sets with champion and trait counts.

**Example Request:**

```bash
curl "http://localhost:5000/api/sets"
```

**Example Response:**

```json
[
  {
    "name": "TFTSet13",
    "championCount": 60,
    "traitCount": 28
  }
]
```

---

## Interactive Documentation

For interactive API testing, visit the Scalar UI at:

```
http://localhost:5000/scalar/v1
```

The OpenAPI specification is available at:

```
http://localhost:5000/openapi/v1.json
```

---

## Error Responses

All endpoints return standard HTTP status codes:

| Code | Description |
|------|-------------|
| 200 | Success |
| 400 | Bad Request - Invalid parameters |
| 404 | Not Found - Resource doesn't exist |
| 500 | Internal Server Error |

**Error Response Format:**

```json
{
  "error": "Error message description",
  "status": 404
}
```
