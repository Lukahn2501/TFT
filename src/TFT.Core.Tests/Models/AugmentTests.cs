using TFT.Core.Models;

namespace TFT.Core.Tests.Models;

public class AugmentTests
{
    [Fact]
    public void Augment_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var augment = new Augment
        {
            ApiName = "TFT_Augment_Test",
            Name = "Test Augment"
        };

        // Assert
        Assert.Equal(0, augment.Id);
        Assert.Null(augment.Description);
        Assert.Null(augment.Icon);
        Assert.Equal(0, augment.Tier);
        Assert.False(augment.IsUnique);
        Assert.Null(augment.EffectsJson);
        Assert.Null(augment.AssociatedTraitsJson);
        Assert.Null(augment.IncompatibleTraitsJson);
        Assert.Null(augment.TagsJson);
    }

    [Fact]
    public void Augment_SetProperties_ShouldRetainValues()
    {
        // Arrange & Act
        var augment = new Augment
        {
            Id = 1,
            ApiName = "TFT_Augment_CyberneticImplants",
            Name = "Cybernetic Implants",
            Description = "Your team gains bonus health and attack damage.",
            Icon = "/augments/cybernetic_implants.png",
            Tier = 2, // Gold
            IsUnique = true,
            EffectsJson = "{\"health\":150,\"attackDamage\":15}",
            AssociatedTraitsJson = "[\"Cybernetic\"]",
            IncompatibleTraitsJson = "[\"Mech\"]",
            TagsJson = "[\"trait\",\"offensive\"]"
        };

        // Assert
        Assert.Equal(1, augment.Id);
        Assert.Equal("TFT_Augment_CyberneticImplants", augment.ApiName);
        Assert.Equal("Cybernetic Implants", augment.Name);
        Assert.Equal("Your team gains bonus health and attack damage.", augment.Description);
        Assert.Equal("/augments/cybernetic_implants.png", augment.Icon);
        Assert.Equal(2, augment.Tier);
        Assert.True(augment.IsUnique);
        Assert.Equal("{\"health\":150,\"attackDamage\":15}", augment.EffectsJson);
        Assert.Equal("[\"Cybernetic\"]", augment.AssociatedTraitsJson);
        Assert.Equal("[\"Mech\"]", augment.IncompatibleTraitsJson);
        Assert.Equal("[\"trait\",\"offensive\"]", augment.TagsJson);
    }

    [Theory]
    [InlineData(1, "Silver")]
    [InlineData(2, "Gold")]
    [InlineData(3, "Prismatic")]
    public void Augment_Tier_ShouldSupportAllTiers(int tier, string description)
    {
        // Arrange & Act
        var augment = new Augment
        {
            ApiName = $"TFT_Augment_{description}",
            Name = $"{description} Augment",
            Tier = tier
        };

        // Assert
        Assert.Equal(tier, augment.Tier);
    }
}
