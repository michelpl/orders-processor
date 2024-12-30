
namespace OrdersProcessor.Domain.Interfaces;

public interface ICsvOrdersParser
{
    Task<IAsyncEnumerable<Order>> ParseAsync(IFormFile file);
}