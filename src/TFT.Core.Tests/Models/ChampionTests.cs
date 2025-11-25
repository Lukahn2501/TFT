using TFT.Core.Models;

namespace TFT.Core.Tests.Models;

public class ChampionTests
{
    [Fact]
    public void Champion_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var champion = new Champion
        {
            ApiName = "TFT_Champion_Test",
            CharacterName = "TestChampion",
            Name = "Test Champion"
        };

        // Assert
        Assert.Equal(0, champion.Id);
        Assert.Equal(0, champion.Cost);
        Assert.Null(champion.Role);
        Assert.Null(champion.Icon);
        Assert.Null(champion.SquareIcon);
        Assert.Null(champion.TileIcon);
        Assert.Null(champion.AbilityJson);
        Assert.Null(champion.StatsJson);
        Assert.Equal(0, champion.SetDataId);
        Assert.Empty(champion.ChampionTraits);
    }

    [Fact]
    public void Champion_SetProperties_ShouldRetainValues()
    {
        // Arrange
        var setData = new SetData { Name = "Set 13", Mutator = "TFTSet13" };
        var champion = new Champion
        {
            Id = 1,
            ApiName = "TFT_Champion_Ahri",
            CharacterName = "Ahri",
            Name = "Ahri",
            Cost = 4,
            Role = "Carry",
            Icon = "/champions/ahri.png",
            SquareIcon = "/champions/ahri_square.png",
            TileIcon = "/champions/ahri_tile.png",
            AbilityJson = "{\"name\":\"Orb of Deception\"}",
            StatsJson = "{\"health\":800}",
            SetDataId = 1,
            SetData = setData
        };

        // Assert
        Assert.Equal(1, champion.Id);
        Assert.Equal("TFT_Champion_Ahri", champion.ApiName);
        Assert.Equal("Ahri", champion.CharacterName);
        Assert.Equal("Ahri", champion.Name);
        Assert.Equal(4, champion.Cost);
        Assert.Equal("Carry", champion.Role);
        Assert.Equal("/champions/ahri.png", champion.Icon);
        Assert.Equal("/champions/ahri_square.png", champion.SquareIcon);
        Assert.Equal("/champions/ahri_tile.png", champion.TileIcon);
        Assert.Equal("{\"name\":\"Orb of Deception\"}", champion.AbilityJson);
        Assert.Equal("{\"health\":800}", champion.StatsJson);
        Assert.Equal(1, champion.SetDataId);
        Assert.Same(setData, champion.SetData);
    }

    [Fact]
    public void Champion_ChampionTraits_CanAddItems()
    {
        // Arrange
        var champion = new Champion
        {
            ApiName = "TFT_Champion_Test",
            CharacterName = "Test",
            Name = "Test"
        };

        var trait = new Trait
        {
            ApiName = "TFT_Trait_Test",
            Name = "Test Trait"
        };

        var championTrait = new ChampionTrait
        {
            ChampionId = 1,
            Champion = champion,
            TraitId = 1,
            Trait = trait
        };

        // Act
        champion.ChampionTraits.Add(championTrait);

        // Assert
        Assert.Single(champion.ChampionTraits);
        Assert.Same(championTrait, champion.ChampionTraits.First());
    }
}
