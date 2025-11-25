namespace TFT.Core.Models;

public class Champion
{
    public int Id { get; set; }
    public required string ApiName { get; set; }
    public required string CharacterName { get; set; }
    public required string Name { get; set; }
    public int Cost { get; set; }
    public string? Role { get; set; }
    public string? Icon { get; set; }
    public string? SquareIcon { get; set; }
    public string? TileIcon { get; set; }

    // Store complex JSON data as string
    public string? AbilityJson { get; set; }
    public string? StatsJson { get; set; }

    // Navigation properties
    public int SetDataId { get; set; }
    public SetData SetData { get; set; } = null!;
    public ICollection<ChampionTrait> ChampionTraits { get; set; } = new List<ChampionTrait>();
}
