using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TFT.Core.DTOs;
using TFT.Core.Models;
using TFT.Infrastructure.Data;

namespace TFT.DataLoader.Services;

public class DataLoaderService
{
    private readonly TftContext _context;
    private readonly ILogger<DataLoaderService> _logger;

    public DataLoaderService(TftContext context, ILogger<DataLoaderService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task LoadDataAsync(TftDataDto data, CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("Starting data load process...");

        // Clear existing data
        await ClearExistingDataAsync(cancellationToken);

        // Load Items (these are global, not set-specific)
        await LoadItemsAsync(data.Items, data.SetData, cancellationToken);

        // Load SetData with Champions and Traits
        await LoadSetDataAsync(data.SetData, cancellationToken);

        _logger.LogInformation("Data load completed successfully!");
    }

    private async Task ClearExistingDataAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Clearing existing data...");

        // Order matters due to foreign keys
        _context.ChampionTraits.RemoveRange(_context.ChampionTraits);
        _context.Champions.RemoveRange(_context.Champions);
        _context.Traits.RemoveRange(_context.Traits);
        _context.SetData.RemoveRange(_context.SetData);
        _context.Items.RemoveRange(_context.Items);
        _context.Augments.RemoveRange(_context.Augments);

        await _context.SaveChangesAsync(cancellationToken);
        _logger.LogInformation("Existing data cleared");
    }

    private async Task LoadItemsAsync(List<ItemDto> itemDtos, List<SetDataDto> setDataDtos, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Loading {Count} items...", itemDtos.Count);

        // First, find the TFTSet16 setData to get the list of valid items and augments
        var tftSet16Data = setDataDtos.FirstOrDefault(s => s.Mutator == "TFTSet16");
        var validItemApiNames = tftSet16Data?.Items?.ToHashSet(StringComparer.OrdinalIgnoreCase) ?? new HashSet<string>();
        var validAugmentApiNames = tftSet16Data?.Augments?.ToHashSet(StringComparer.OrdinalIgnoreCase) ?? new HashSet<string>();
        _logger.LogInformation("Found {ItemCount} items and {AugmentCount} augments in TFTSet16 setData", validItemApiNames.Count, validAugmentApiNames.Count);

        var items = new List<Item>();
        var augments = new List<Augment>();

        foreach (var dto in itemDtos)
        {
            // Skip items with empty names
            if (string.IsNullOrWhiteSpace(dto.Name))
                continue;

            // Determine if it's an augment based on icon path
            var isAugment = dto.Icon?.Contains("Augments", StringComparison.OrdinalIgnoreCase) ?? false;

            if (isAugment)
            {
                // Filter augments based on TFTSet16 setData augments list (same as items)
                if (validAugmentApiNames.Contains(dto.ApiName ?? ""))
                {
                    var tier = ExtractAugmentTier(dto.Icon);

                    augments.Add(new Augment
                    {
                        ApiName = dto.ApiName ?? string.Empty,
                        Name = dto.Name,
                        Description = dto.Desc,
                        Icon = dto.Icon,
                        Tier = tier,
                        IsUnique = dto.Unique,
                        EffectsJson = dto.Effects != null ? JsonSerializer.Serialize(dto.Effects) : null,
                        AssociatedTraitsJson = dto.AssociatedTraits != null ? JsonSerializer.Serialize(dto.AssociatedTraits) : null,
                        IncompatibleTraitsJson = dto.IncompatibleTraits != null ? JsonSerializer.Serialize(dto.IncompatibleTraits) : null,
                        TagsJson = dto.Tags != null ? JsonSerializer.Serialize(dto.Tags) : null
                    });
                }
            }
            else
            {
                // Filter items based on TFTSet16 setData items list
                if (validItemApiNames.Contains(dto.ApiName ?? ""))
                {
                    items.Add(new Item
                    {
                        ApiName = dto.ApiName ?? string.Empty,
                        Name = dto.Name,
                        Description = dto.Desc,
                        Icon = dto.Icon,
                        IsUnique = dto.Unique,
                        CompositionJson = dto.Composition != null ? JsonSerializer.Serialize(dto.Composition) : null,
                        EffectsJson = dto.Effects != null ? JsonSerializer.Serialize(dto.Effects) : null,
                        AssociatedTraitsJson = dto.AssociatedTraits != null ? JsonSerializer.Serialize(dto.AssociatedTraits) : null,
                        IncompatibleTraitsJson = dto.IncompatibleTraits != null ? JsonSerializer.Serialize(dto.IncompatibleTraits) : null,
                        TagsJson = dto.Tags != null ? JsonSerializer.Serialize(dto.Tags) : null
                    });
                }
            }
        }

        await _context.Items.AddRangeAsync(items, cancellationToken);
        await _context.Augments.AddRangeAsync(augments, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Loaded {ItemCount} items and {AugmentCount} augments", items.Count, augments.Count);
    }

    private async Task LoadSetDataAsync(List<SetDataDto> setDataDtos, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Loading {Count} set data entries...", setDataDtos.Count);

        // Filter to only load TFTSet16
        var tftSet16 = setDataDtos.FirstOrDefault(s => s.Mutator == "TFTSet16");

        if (tftSet16 == null)
        {
            _logger.LogWarning("TFTSet16 not found in data source!");
            return;
        }

        _logger.LogInformation("Processing set: {SetName} ({Mutator})", tftSet16.Name, tftSet16.Mutator);

        // Create SetData entity
        var setData = new SetData
        {
            Name = tftSet16.Name,
            Mutator = tftSet16.Mutator,
            SetNumber = tftSet16.Number
        };

        await _context.SetData.AddAsync(setData, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken); // Save to get ID

        // Load Traits for this set
        var traitMap = await LoadTraitsAsync(setData.Id, tftSet16.Traits, cancellationToken);

        // Load Champions for this set
        await LoadChampionsAsync(setData.Id, tftSet16.Champions, traitMap, cancellationToken);

        _logger.LogInformation("TFTSet16 data loaded successfully");
    }

    private async Task<Dictionary<string, Trait>> LoadTraitsAsync(
        int setDataId,
        List<TraitDto> traitDtos,
        CancellationToken cancellationToken)
    {
        var traits = new List<Trait>();

        foreach (var dto in traitDtos)
        {
            traits.Add(new Trait
            {
                ApiName = dto.ApiName,
                Name = dto.Name,
                Description = dto.Desc,
                Icon = dto.Icon,
                EffectsJson = dto.Effects != null ? JsonSerializer.Serialize(dto.Effects) : null,
                SetDataId = setDataId
            });
        }

        await _context.Traits.AddRangeAsync(traits, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Loaded {Count} traits for set {SetDataId}", traits.Count, setDataId);

        // Return a map of display Name -> Trait (champions use display names like "Yordle", not "TFT16_Yordle")
        return traits.ToDictionary(t => t.Name, t => t);
    }

    private async Task LoadChampionsAsync(
        int setDataId,
        List<ChampionDto> championDtos,
        Dictionary<string, Trait> traitMap,
        CancellationToken cancellationToken)
    {
        var champions = new List<Champion>();
        var championTraits = new List<ChampionTrait>();

        foreach (var dto in championDtos)
        {
            // Skip champions with empty names
            if (string.IsNullOrWhiteSpace(dto.Name))
                continue;

            var champion = new Champion
            {
                ApiName = dto.ApiName,
                CharacterName = dto.CharacterName,
                Name = dto.Name,
                Cost = dto.Cost,
                Role = dto.Role,
                Icon = dto.Icon,
                SquareIcon = dto.SquareIcon,
                TileIcon = dto.TileIcon,
                AbilityJson = dto.Ability != null ? JsonSerializer.Serialize(dto.Ability) : null,
                StatsJson = dto.Stats != null ? JsonSerializer.Serialize(dto.Stats) : null,
                SetDataId = setDataId
            };

            champions.Add(champion);
        }

        await _context.Champions.AddRangeAsync(champions, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // Now create the many-to-many relationships
        for (int i = 0; i < champions.Count; i++)
        {
            var champion = champions[i];

            // Find the corresponding DTO by matching the ApiName
            var dto = championDtos.FirstOrDefault(d => d.ApiName == champion.ApiName);
            if (dto == null)
                continue;

            foreach (var traitApiName in dto.Traits)
            {
                if (traitMap.TryGetValue(traitApiName, out var trait))
                {
                    championTraits.Add(new ChampionTrait
                    {
                        ChampionId = champion.Id,
                        TraitId = trait.Id
                    });
                }
                else
                {
                    _logger.LogWarning("Trait {TraitApiName} not found for champion {ChampionName}",
                        traitApiName, champion.Name);
                }
            }
        }

        await _context.ChampionTraits.AddRangeAsync(championTraits, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        _logger.LogInformation("Loaded {ChampionCount} champions with {RelationCount} trait relationships for set {SetDataId}",
            champions.Count, championTraits.Count, setDataId);
    }

    private static int ExtractAugmentTier(string? iconPath)
    {
        if (string.IsNullOrEmpty(iconPath))
            return 1;

        // Extract tier from icon path like "..._I.TFT_Set13.tex" (Silver)
        if (iconPath.Contains("_III.", StringComparison.OrdinalIgnoreCase))
            return 3; // Prismatic
        if (iconPath.Contains("_II.", StringComparison.OrdinalIgnoreCase))
            return 2; // Gold

        return 1; // Silver (default)
    }
}
