using OrdersProcessor.Domain.Entities;
using OrdersProcessor.Domain.Interfaces;

namespace OrdersProcessor.Application.Services;

public class OrdersProcessingService(
    IMedianCalculator medianCalculator,
    IRegionAnalyzer regionAnalyzer)
    : IOrdersProcessingService
{
    public async Task<OrdersSummary> GetOrdersSummary(IAsyncEnumerable<Order> orderList)
    {
        ArgumentNullException.ThrowIfNull(orderList);

        var unitCosts = new List<decimal>();
        var regions = new List<string>();
        var orderDates = new List<DateTime>();
        decimal totalRevenue = 0;

        await foreach (var order in orderList)
        {
            totalRevenue += order.TotalRevenue;
            
            unitCosts.Add(order.UnitCost);
            regions.Add(order.Region);
            orderDates.Add(order.OrderDate);
        }

        var lastDate = orderDates.Max();
        var firstDate = orderDates.Min();
        
        return new OrdersSummary
        {
            TotalRevenue = totalRevenue,
            MedianUnitCost = medianCalculator.Calculate(unitCosts),
            MostCommonRegion = regionAnalyzer.GetMostCommonRegion(regions),
            FirstOrderDate = firstDate,
            LastOrderDate = lastDate,
            DaysBetweenOrders = (lastDate - firstDate).Days
        };
    }

}