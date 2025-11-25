namespace TFT.Core.Models;

/// <summary>
/// Join table for Many-to-Many relationship between Champions and Traits
/// </summary>
public class ChampionTrait
{
    public int ChampionId { get; set; }
    public Champion Champion { get; set; } = null!;

    public int TraitId { get; set; }
    public Trait Trait { get; set; } = null!;
}
