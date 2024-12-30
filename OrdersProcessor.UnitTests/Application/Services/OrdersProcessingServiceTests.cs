using Moq;
using OrdersProcessor.Application.Services;
using OrdersProcessor.Domain.Interfaces;

namespace OrdersProcessor.UnitTests.Application.Services;

public class OrdersProcessingServiceTests
{
    private readonly Mock<IMedianCalculator> _medianCalculatorMock;
    private readonly Mock<IRegionAnalyzer> _regionAnalyzerMock;
    private readonly OrdersProcessingService _service;

    public OrdersProcessingServiceTests()
    {
        _medianCalculatorMock = new Mock<IMedianCalculator>();
        _regionAnalyzerMock = new Mock<IRegionAnalyzer>();
        _service = new OrdersProcessingService(_medianCalculatorMock.Object, _regionAnalyzerMock.Object);
    }

    [Fact]
    public async Task GetOrdersSummary_ShouldCalculateCorrectTotalRevenue()
    {
        // Arrange
        var orders = GetSampleOrders();
        var expectedTotalRevenue = orders.Sum(o => o.TotalRevenue);

        _medianCalculatorMock.Setup(mc => mc.Calculate(It.IsAny<IEnumerable<decimal>>()))
            .Returns(100); // Valor fictício para simplificar
        _regionAnalyzerMock.Setup(ra => ra.GetMostCommonRegion(It.IsAny<List<string>>()))
            .Returns("Europe");

        // Act
        var summary = await _service.GetOrdersSummary(orders.ToAsyncEnumerable());

        // Assert
        Assert.Equal(expectedTotalRevenue, summary.TotalRevenue);
    }

    [Fact]
    public async Task GetOrdersSummary_ShouldCalculateMedianUnitCost()
    {
        // Arrange
        var orders = GetSampleOrders();
        var unitCosts = orders.Select(o => o.UnitCost).ToList();
        decimal expectedMedian = 263.33m; // Valor fictício

        _medianCalculatorMock.Setup(mc => mc.Calculate(unitCosts)).Returns(expectedMedian);
        _regionAnalyzerMock.Setup(ra => ra.GetMostCommonRegion(It.IsAny<List<string>>()))
            .Returns("Europe");

        // Act
        var summary = await _service.GetOrdersSummary(orders.ToAsyncEnumerable());

        // Assert
        Assert.Equal(expectedMedian, summary.MedianUnitCost);
    }

    [Fact]
    public async Task GetOrdersSummary_ShouldReturnMostCommonRegion()
    {
        // Arrange
        var orders = GetSampleOrders();
        var regions = orders.Select(o => o.Region).ToList();
        string expectedRegion = "Sub-Saharan Africa";

        _regionAnalyzerMock.Setup(ra => ra.GetMostCommonRegion(regions)).Returns(expectedRegion);
        _medianCalculatorMock.Setup(mc => mc.Calculate(It.IsAny<IEnumerable<decimal>>()))
            .Returns(100); // Valor fictício para simplificar

        // Act
        var summary = await _service.GetOrdersSummary(orders.ToAsyncEnumerable());

        // Assert
        Assert.Equal(expectedRegion, summary.MostCommonRegion);
    }

    [Fact]
    public async Task GetOrdersSummary_ShouldCalculateCorrectDateRange()
    {
        // Arrange
        var orders = GetSampleOrders();
        DateTime expectedFirstDate = orders.Min(o => o.OrderDate);
        DateTime expectedLastDate = orders.Max(o => o.OrderDate);

        _medianCalculatorMock.Setup(mc => mc.Calculate(It.IsAny<IEnumerable<decimal>>()))
            .Returns(100); // Valor fictício para simplificar
        _regionAnalyzerMock.Setup(ra => ra.GetMostCommonRegion(It.IsAny<List<string>>()))
            .Returns("Europe");

        // Act
        var summary = await _service.GetOrdersSummary(orders.ToAsyncEnumerable());

        // Assert
        Assert.Equal(expectedFirstDate, summary.FirstOrderDate);
        Assert.Equal(expectedLastDate, summary.LastOrderDate);
        Assert.Equal((expectedLastDate - expectedFirstDate).Days, summary.DaysBetweenOrders);
    }

    private List<Order> GetSampleOrders()
    {
        return new List<Order>
        {
            new Order { Region = "Sub-Saharan Africa", OrderDate = new DateTime(2014, 10, 8), TotalRevenue = 142509.72m, UnitCost = 97.44m },
            new Order { Region = "Europe", OrderDate = new DateTime(2013, 12, 29), TotalRevenue = 1253749.86m, UnitCost = 97.44m },
            new Order { Region = "Asia", OrderDate = new DateTime(2017, 6, 14), TotalRevenue = 693578.12m, UnitCost = 90.93m },
            new Order { Region = "Europe", OrderDate = new DateTime(2013, 2, 20), TotalRevenue = 125562.72m, UnitCost = 35.84m },
            new Order { Region = "Sub-Saharan Africa", OrderDate = new DateTime(2015, 12, 9), TotalRevenue = 93169.38m, UnitCost = 6.92m }
        };
    }
}