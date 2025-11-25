# TFT Data Analysis

This repository contains localized game data for **Teamfight Tactics (TFT)**, Riot Games' auto-battler game mode.

## Data Files

- `en_us.json` - English (US) localization (~25MB)
- `fr_fr.json` - French (France) localization

---

## en_us.json Structure

The JSON file contains three main top-level keys:

### 1. `items` (3,415 entries)

Contains all game items including:

| Category | Count | Description |
|----------|-------|-------------|
| **Augments** | ~1,663 | Special bonuses players can select during a match |
| **Craftable Items** | ~334 | Items created by combining components |
| **Champion Items** | ~101 | Unit-specific items |
| **Consumables** | ~64 | One-time use items |
| **Encounter Items** | ~73 | Items from special events |
| **Assist/Support** | ~117 | Supporting game mechanics |
| **Others** | Various | Set-specific mechanics, emblems, etc. |

#### Item Structure

```json
{
  "apiName": "TFT_Item_RabadonsDeathcap",
  "name": "Rabadon's Deathcap",
  "desc": "This humble hat can help you make, or unmake, the world itself.",
  "icon": "ASSETS/Maps/TFT/Icons/Items/Hexcore/TFT_Item_RabadonsDeathcap.TFT_Set13.tex",
  "composition": ["TFT_Item_NeedlesslyLargeRod", "TFT_Item_NeedlesslyLargeRod"],
  "effects": {"AP": 50.0, "BonusDamage": 0.15},
  "associatedTraits": [],
  "incompatibleTraits": [],
  "tags": [],
  "unique": false
}
```

---

### 2. `sets` (11 sets)

A simplified mapping of TFT sets containing basic set information:

| Set # | Name |
|-------|------|
| 1 | TutorialV2 |
| 3 | Galaxies |
| 4 | Fates II |
| 5 | Set 5 |
| 7 | Set7 |
| 10 | Set10 |
| 12 | Set12 |
| 13 | Set13 |
| 14 | Set14 |
| 15 | Set15 |
| 16 | Set16 |

Each set contains:
- `name` - Set display name
- `champions` - List of champions
- `traits` - List of traits

---

### 3. `setData` (41 game mode variants)

Detailed set data organized by **game mode mutators**. Each set can have multiple variants:

| Mode Suffix | Description |
|-------------|-------------|
| `TFTSetX` | Standard ranked/normal mode |
| `TFTSetX_TURBO` | Hyper Roll mode |
| `TFTSetX_PAIRS` | Double Up mode |
| `TFTSetX_PVEMODE` | PvE encounters |
| `TFTSetX_Evolved` | Mid-set update variant |
| `TFTSetX_CarouselOfChaos` | Special game mode |

#### Champion Structure

```json
{
  "apiName": "TFT13_Singed",
  "characterName": "TFT13_Singed",
  "name": "Singed",
  "cost": 1,
  "role": "APTank",
  "traits": ["Chem-Baron", "Sentinel"],
  "icon": "...",
  "squareIcon": "...",
  "tileIcon": "...",
  "ability": {
    "name": "Dangerous Mutations",
    "desc": "Gain Durability and grant allies...",
    "icon": "...",
    "variables": [
      {"name": "AttackSpeed", "value": [1.0, 100.0, 115.0, 130.0, 150.0, 1.0, 1.0]},
      {"name": "Duration", "value": [4.0, 4.0, 4.0, 4.0, 4.0, 4.0, 4.0]}
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

**Note:** The `variables` array in abilities uses star-level indexing (0=base, 1=1-star, 2=2-star, 3=3-star, 4=3-star+, etc.)

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

**Trait Effect Styles:**
- `style: 1` - Bronze tier
- `style: 3` - Silver tier  
- `style: 5` - Gold tier
- Higher values = more powerful breakpoints

---

## Summary Statistics

| Metric | Count |
|--------|-------|
| Total Items | 3,415 |
| Total Set Variants | 41 |
| Unique Sets | 11 |
| Champion Entries (all variants) | 3,527 |
| Trait Entries (all variants) | 2,028 |
| Unique Items | 528 |
| Items with Effects | 2,200 |
| Items with Associated Traits | 851 |

---

## Use Cases

This data can be used for:
- Building TFT companion apps
- Creating team composition tools
- Analyzing game balance and statistics
- Building item recommendation systems
- Creating educational resources for new players
- Developing strategy guides

---

## Augment Tiers

Augments are identified by their icon path naming convention:
- **Silver (Tier 1)**: `_I.TFT_SetX`
- **Gold (Tier 2)**: `_II.TFT_SetX`
- **Prismatic (Tier 3)**: `_III.TFT_SetX`

---

## Notes

- API names use the format `TFT{SetNumber}_{Type}_{Name}`
- Descriptions may contain HTML-like formatting tags (e.g., `<TFTBonus>`, `<magicDamage>`)
- Some values use placeholder patterns like `@VariableName@` for dynamic text substitution
- Trait styles correlate with visual tier representation in-game
