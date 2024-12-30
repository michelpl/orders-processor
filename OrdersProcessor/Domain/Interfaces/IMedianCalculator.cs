namespace OrdersProcessor.Domain.Interfaces;

public interface IMedianCalculator
{
    decimal Calculate(IEnumerable<decimal> values);
}