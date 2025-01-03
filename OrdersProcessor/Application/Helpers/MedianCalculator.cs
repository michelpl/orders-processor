using OrdersProcessor.Domain.Interfaces;

namespace OrdersProcessor.Application.Helpers;

public class MedianCalculator : IMedianCalculator
{
    public decimal Calculate(IEnumerable<decimal> values)
    {
        if (values == null || values.Count() == 0)
            throw new ArgumentException("Values collection cannot be null or empty");
        
        var count = values.Count();
        
        var sortedValues = values.OrderBy(v => v).ToList();

        if (count % 2 == 0)
            return (sortedValues[count / 2 - 1] + sortedValues[count / 2]) / 2;

        return sortedValues[count / 2];
    }
}