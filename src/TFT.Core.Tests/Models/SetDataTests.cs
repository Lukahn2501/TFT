using TFT.Core.Models;

namespace TFT.Core.Tests.Models;

public class SetDataTests
{
    [Fact]
    public void SetData_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var setData = new SetData
        {
            Name = "Test Set",
            Mutator = "TFTTestSet"
        };

        // Assert
        Assert.Equal(0, setData.Id);
        Assert.Equal(0, setData.SetNumber);
        Assert.Empty(setData.Champions);
        Assert.Empty(setData.Traits);
    }

    [Fact]
    public void SetData_SetProperties_ShouldRetainValues()
    {
        // Arrange & Act
        var setData = new SetData
        {
            Id = 1,
            Name = "Ruination",
            Mutator = "TFTSet13",
            SetNumber = 13
        };

        // Assert
        Assert.Equal(1, setData.Id);
        Assert.Equal("Ruination", setData.Name);
        Assert.Equal("TFTSet13", setData.Mutator);
        Assert.Equal(13, setData.SetNumber);
    }

    [Fact]
    public void SetData_Champions_CanAddItems()
    {
        // Arrange
        var setData = new SetData
        {
            Name = "Test Set",
            Mutator = "TFTTestSet"
        };

        var champion = new Champion
        {
            ApiName = "TFT_Champion_Test",
            CharacterName = "Test",
            Name = "Test",
            SetData = setData
        };

        // Act
        setData.Champions.Add(champion);

        // Assert
        Assert.Single(setData.Champions);
        Assert.Same(champion, setData.Champions.First());
    }

    [Fact]
    public void SetData_Traits_CanAddItems()
    {
        // Arrange
        var setData = new SetData
        {
            Name = "Test Set",
            Mutator = "TFTTestSet"
        };

        var trait = new Trait
        {
            ApiName = "TFT_Trait_Test",
            Name = "Test Trait",
            SetData = setData
        };

        // Act
        setData.Traits.Add(trait);

        // Assert
        Assert.Single(setData.Traits);
        Assert.Same(trait, setData.Traits.First());
    }
}
