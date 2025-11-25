using TFT.Core.Models;

namespace TFT.Core.Tests.Models;

public class TraitTests
{
    [Fact]
    public void Trait_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var trait = new Trait
        {
            ApiName = "TFT_Trait_Test",
            Name = "Test Trait"
        };

        // Assert
        Assert.Equal(0, trait.Id);
        Assert.Null(trait.Description);
        Assert.Null(trait.Icon);
        Assert.Null(trait.EffectsJson);
        Assert.Equal(0, trait.SetDataId);
        Assert.Empty(trait.ChampionTraits);
    }

    [Fact]
    public void Trait_SetProperties_ShouldRetainValues()
    {
        // Arrange
        var setData = new SetData { Name = "Set 13", Mutator = "TFTSet13" };

        // Act
        var trait = new Trait
        {
            Id = 1,
            ApiName = "TFT_Trait_Arcana",
            Name = "Arcana",
            Description = "Arcana champions gain bonus ability power.",
            Icon = "/traits/arcana.png",
            EffectsJson = "[{\"minUnits\":2,\"maxUnits\":3}]",
            SetDataId = 1,
            SetData = setData
        };

        // Assert
        Assert.Equal(1, trait.Id);
        Assert.Equal("TFT_Trait_Arcana", trait.ApiName);
        Assert.Equal("Arcana", trait.Name);
        Assert.Equal("Arcana champions gain bonus ability power.", trait.Description);
        Assert.Equal("/traits/arcana.png", trait.Icon);
        Assert.Equal("[{\"minUnits\":2,\"maxUnits\":3}]", trait.EffectsJson);
        Assert.Equal(1, trait.SetDataId);
        Assert.Same(setData, trait.SetData);
    }

    [Fact]
    public void Trait_ChampionTraits_CanAddItems()
    {
        // Arrange
        var trait = new Trait
        {
            ApiName = "TFT_Trait_Test",
            Name = "Test Trait"
        };

        var champion = new Champion
        {
            ApiName = "TFT_Champion_Test",
            CharacterName = "Test",
            Name = "Test"
        };

        var championTrait = new ChampionTrait
        {
            ChampionId = 1,
            Champion = champion,
            TraitId = 1,
            Trait = trait
        };

        // Act
        trait.ChampionTraits.Add(championTrait);

        // Assert
        Assert.Single(trait.ChampionTraits);
        Assert.Same(championTrait, trait.ChampionTraits.First());
    }
}
