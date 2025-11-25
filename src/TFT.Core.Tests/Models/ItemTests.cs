using TFT.Core.Models;

namespace TFT.Core.Tests.Models;

public class ItemTests
{
    [Fact]
    public void Item_DefaultValues_ShouldBeCorrect()
    {
        // Arrange & Act
        var item = new Item
        {
            ApiName = "TFT_Item_Test",
            Name = "Test Item"
        };

        // Assert
        Assert.Equal(0, item.Id);
        Assert.Null(item.Description);
        Assert.Null(item.Icon);
        Assert.False(item.IsUnique);
        Assert.Null(item.CompositionJson);
        Assert.Null(item.EffectsJson);
        Assert.Null(item.AssociatedTraitsJson);
        Assert.Null(item.IncompatibleTraitsJson);
        Assert.Null(item.TagsJson);
    }

    [Fact]
    public void Item_SetProperties_ShouldRetainValues()
    {
        // Arrange & Act
        var item = new Item
        {
            Id = 1,
            ApiName = "TFT_Item_InfinityEdge",
            Name = "Infinity Edge",
            Description = "Critical strikes deal bonus damage.",
            Icon = "/items/infinity_edge.png",
            IsUnique = true,
            CompositionJson = "[\"TFT_Item_BFSword\",\"TFT_Item_BFSword\"]",
            EffectsJson = "{\"critDamage\":15}",
            AssociatedTraitsJson = "[\"Assassin\"]",
            IncompatibleTraitsJson = "[\"Tank\"]",
            TagsJson = "[\"offensive\"]"
        };

        // Assert
        Assert.Equal(1, item.Id);
        Assert.Equal("TFT_Item_InfinityEdge", item.ApiName);
        Assert.Equal("Infinity Edge", item.Name);
        Assert.Equal("Critical strikes deal bonus damage.", item.Description);
        Assert.Equal("/items/infinity_edge.png", item.Icon);
        Assert.True(item.IsUnique);
        Assert.Equal("[\"TFT_Item_BFSword\",\"TFT_Item_BFSword\"]", item.CompositionJson);
        Assert.Equal("{\"critDamage\":15}", item.EffectsJson);
        Assert.Equal("[\"Assassin\"]", item.AssociatedTraitsJson);
        Assert.Equal("[\"Tank\"]", item.IncompatibleTraitsJson);
        Assert.Equal("[\"offensive\"]", item.TagsJson);
    }
}
