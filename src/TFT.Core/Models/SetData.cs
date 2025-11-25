namespace TFT.Core.Models;

public class SetData
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public required string Mutator { get; set; }
    public int SetNumber { get; set; }

    // Navigation properties
    public ICollection<Champion> Champions { get; set; } = new List<Champion>();
    public ICollection<Trait> Traits { get; set; } = new List<Trait>();
}
