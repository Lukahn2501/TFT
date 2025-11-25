using System.Text.Json.Serialization;

namespace TFT.Core.DTOs;

/// <summary>
/// Root DTO for parsing Community Dragon JSON
/// </summary>
public class TftDataDto
{
    [JsonPropertyName("items")]
    public List<ItemDto> Items { get; set; } = new();
    
    [JsonPropertyName("sets")]
    public Dictionary<string, object> Sets { get; set; } = new();
    
    [JsonPropertyName("setData")]
    public List<SetDataDto> SetData { get; set; } = new();
}

public class ItemDto
{
    [JsonPropertyName("apiName")]
    public string ApiName { get; set; } = string.Empty;
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("desc")]
    public string? Desc { get; set; }
    
    [JsonPropertyName("icon")]
    public string? Icon { get; set; }
    
    [JsonPropertyName("composition")]
    public List<string>? Composition { get; set; }
    
    [JsonPropertyName("effects")]
    public Dictionary<string, object>? Effects { get; set; }
    
    [JsonPropertyName("associatedTraits")]
    public List<string>? AssociatedTraits { get; set; }
    
    [JsonPropertyName("incompatibleTraits")]
    public List<string>? IncompatibleTraits { get; set; }
    
    [JsonPropertyName("tags")]
    public List<string>? Tags { get; set; }
    
    [JsonPropertyName("unique")]
    public bool Unique { get; set; }
}

public class SetDto
{
    [JsonPropertyName("number")]
    public int Number { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("mutator")]
    public string? Mutator { get; set; }
}

public class SetDataDto
{
    [JsonPropertyName("number")]
    public int Number { get; set; }
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("mutator")]
    public string Mutator { get; set; } = string.Empty;
    
    [JsonPropertyName("champions")]
    public List<ChampionDto> Champions { get; set; } = new();
    
    [JsonPropertyName("traits")]
    public List<TraitDto> Traits { get; set; } = new();
}

public class ChampionDto
{
    [JsonPropertyName("apiName")]
    public string ApiName { get; set; } = string.Empty;
    
    [JsonPropertyName("characterName")]
    public string CharacterName { get; set; } = string.Empty;
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("cost")]
    public int Cost { get; set; }
    
    [JsonPropertyName("role")]
    public string? Role { get; set; }
    
    [JsonPropertyName("icon")]
    public string? Icon { get; set; }
    
    [JsonPropertyName("squareIcon")]
    public string? SquareIcon { get; set; }
    
    [JsonPropertyName("tileIcon")]
    public string? TileIcon { get; set; }
    
    [JsonPropertyName("traits")]
    public List<string> Traits { get; set; } = new();
    
    [JsonPropertyName("ability")]
    public object? Ability { get; set; }
    
    [JsonPropertyName("stats")]
    public object? Stats { get; set; }
}

public class TraitDto
{
    [JsonPropertyName("apiName")]
    public string ApiName { get; set; } = string.Empty;
    
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    
    [JsonPropertyName("desc")]
    public string? Desc { get; set; }
    
    [JsonPropertyName("icon")]
    public string? Icon { get; set; }
    
    [JsonPropertyName("effects")]
    public List<object>? Effects { get; set; }
}
