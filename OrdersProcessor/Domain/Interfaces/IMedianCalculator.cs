namespace OrdersProcessor.Domain.Interfaces;

public interface IMedianCalculator
{
    decimal Calculate(List<decimal> values);
}