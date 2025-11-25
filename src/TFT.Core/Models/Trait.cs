namespace TFT.Core.Models;

public class Trait
{
    public int Id { get; set; }
    public required string ApiName { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Icon { get; set; }
    
    // Store effects as JSON string
    public string? EffectsJson { get; set; }
    
    // Navigation properties
    public int SetDataId { get; set; }
    public SetData SetData { get; set; } = null!;
    public ICollection<ChampionTrait> ChampionTraits { get; set; } = new List<ChampionTrait>();
}
