using OrdersProcessor.Application.Helpers;

namespace OrdersProcessor.UnitTests.Application.Helpers;

public class RegionAnalyzerTests
{
    private readonly RegionAnalyzer _regionAnalyzer;

    public RegionAnalyzerTests()
    {
        _regionAnalyzer = new RegionAnalyzer();
    }

    [Fact]
    public void GetMostCommonRegion_ShouldReturnMostCommonRegion_WhenListHasMultipleRegions()
    {
        // Arrange
        var regions = new List<string>
        {
            "Sub-Saharan Africa", "Europe", "Middle East and North Africa",
            "Europe", "Sub-Saharan Africa", "Sub-Saharan Africa", "Asia",
            "Europe", "Central America and the Caribbean", "Europe"
        };

        // Act
        var mostCommonRegion = _regionAnalyzer.GetMostCommonRegion(regions);

        // Assert
        Assert.Equal("Europe", mostCommonRegion);
    }


    [Fact]
    public void GetMostCommonRegion_ShouldReturnFirstMostCommonRegion_WhenListHasTiedRegions()
    {
        // Arrange
        var regions = new List<string>
        {
            "Europe", "Sub-Saharan Africa", "Europe", "Sub-Saharan Africa",
            "Middle East and North Africa", "Asia"
        };

        // Act
        var mostCommonRegion = _regionAnalyzer.GetMostCommonRegion(regions);

        // Assert
        Assert.Equal("Europe", mostCommonRegion); // Primeiro empate na lista
    }

    [Fact]
    public void GetMostCommonRegion_ShouldReturnRegion_WhenListHasOneRegion()
    {
        // Arrange
        var regions = new List<string> { "Middle East and North Africa" };

        // Act
        var mostCommonRegion = _regionAnalyzer.GetMostCommonRegion(regions);

        // Assert
        Assert.Equal("Middle East and North Africa", mostCommonRegion);
    }

    [Fact]
    public void GetMostCommonRegion_ShouldThrowArgumentException_WhenListIsEmpty()
    {
        // Arrange
        var regions = new List<string>();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _regionAnalyzer.GetMostCommonRegion(regions));
        Assert.Equal("Regions collection cannot be null or empty", exception.Message);
    }

    [Fact]
    public void GetMostCommonRegion_ShouldThrowArgumentException_WhenListIsNull()
    {
        // Arrange
        List<string> regions = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _regionAnalyzer.GetMostCommonRegion(regions));
        Assert.Equal("Regions collection cannot be null or empty", exception.Message);
    }
}