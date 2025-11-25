namespace TFT.Core.Models;

public class Item
{
    public int Id { get; set; }
    public required string ApiName { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    public bool IsUnique { get; set; }

    // Store as JSON strings
    public string? CompositionJson { get; set; }
    public string? EffectsJson { get; set; }
    public string? AssociatedTraitsJson { get; set; }
    public string? IncompatibleTraitsJson { get; set; }
    public string? TagsJson { get; set; }
}
