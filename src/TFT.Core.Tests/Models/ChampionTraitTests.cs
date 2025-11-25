using TFT.Core.Models;

namespace TFT.Core.Tests.Models;

public class ChampionTraitTests
{
    [Fact]
    public void ChampionTrait_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var championTrait = new ChampionTrait();

        // Assert
        Assert.Equal(0, championTrait.ChampionId);
        Assert.Equal(0, championTrait.TraitId);
    }

    [Fact]
    public void ChampionTrait_SetProperties_ShouldRetainValues()
    {
        // Arrange
        var champion = new Champion
        {
            Id = 1,
            ApiName = "TFT_Champion_Test",
            CharacterName = "Test",
            Name = "Test"
        };

        var trait = new Trait
        {
            Id = 1,
            ApiName = "TFT_Trait_Test",
            Name = "Test Trait"
        };

        // Act
        var championTrait = new ChampionTrait
        {
            ChampionId = 1,
            Champion = champion,
            TraitId = 1,
            Trait = trait
        };

        // Assert
        Assert.Equal(1, championTrait.ChampionId);
        Assert.Same(champion, championTrait.Champion);
        Assert.Equal(1, championTrait.TraitId);
        Assert.Same(trait, championTrait.Trait);
    }

    [Fact]
    public void ChampionTrait_BiDirectionalRelationship_ShouldWork()
    {
        // Arrange
        var champion = new Champion
        {
            Id = 1,
            ApiName = "TFT_Champion_Test",
            CharacterName = "Test",
            Name = "Test"
        };

        var trait = new Trait
        {
            Id = 1,
            ApiName = "TFT_Trait_Test",
            Name = "Test Trait"
        };

        var championTrait = new ChampionTrait
        {
            ChampionId = champion.Id,
            Champion = champion,
            TraitId = trait.Id,
            Trait = trait
        };

        // Act
        champion.ChampionTraits.Add(championTrait);
        trait.ChampionTraits.Add(championTrait);

        // Assert
        Assert.Single(champion.ChampionTraits);
        Assert.Single(trait.ChampionTraits);
        Assert.Same(championTrait, champion.ChampionTraits.First());
        Assert.Same(championTrait, trait.ChampionTraits.First());
    }
}
