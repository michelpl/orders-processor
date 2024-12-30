using OrdersProcessor.Application.Helpers;

namespace OrdersProcessor.UnitTests.Application.Helpers;

public class MedianCalculatorTests
{
    private readonly MedianCalculator _medianCalculator;

    public MedianCalculatorTests()
    {
        _medianCalculator = new MedianCalculator();
    }

    [Fact]
    public void Calculate_ShouldReturnMedian_WhenListHasOddNumberOfElements()
    {
        // Arrange
        var values = new List<decimal> { 3, 1, 4, 1, 5 };

        // Act
        var median = _medianCalculator.Calculate(values);

        // Assert
        Assert.Equal(3, median);
    }

    [Fact]
    public void Calculate_ShouldReturnMedian_WhenListHasEvenNumberOfElements()
    {
        // Arrange
        var values = new List<decimal> { 1, 3, 3, 6 };

        // Act
        var median = _medianCalculator.Calculate(values);

        // Assert
        Assert.Equal(3, median);
    }

    [Fact]
    public void Calculate_ShouldReturnElement_WhenListHasOneElement()
    {
        // Arrange
        var values = new List<decimal> { 42 };

        // Act
        var median = _medianCalculator.Calculate(values);

        // Assert
        Assert.Equal(42, median);
    }

    [Fact]
    public void Calculate_ShouldThrowArgumentException_WhenListIsEmpty()
    {
        // Arrange
        var values = new List<decimal>();

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _medianCalculator.Calculate(values));
        Assert.Equal("Values collection cannot be null or empty", exception.Message);
    }

    [Fact]
    public void Calculate_ShouldThrowArgumentException_WhenListIsNull()
    {
        // Arrange
        List<decimal> values = null;

        // Act & Assert
        var exception = Assert.Throws<ArgumentException>(() => _medianCalculator.Calculate(values));
        Assert.Equal("Values collection cannot be null or empty", exception.Message);
    }
}