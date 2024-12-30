
namespace SalesProcessor.Domain.Interfaces;

public interface ICsvSalesParser
{
    Task<IAsyncEnumerable<Sale>> ParseAsync(IFormFile file);
}