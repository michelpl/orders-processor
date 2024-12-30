namespace SalesProcessor.Domain.Entities;

public class SalesSummary
{
    public decimal MedianUnitCost { get; set; }
    public string MostCommonRegion { get; set; }
    public DateTime FirstOrderDate { get; set; }
    public DateTime LastOrderDate { get; set; }
    public int DaysBetweenOrders { get; set; }
    public decimal TotalRevenue { get; set; }
}