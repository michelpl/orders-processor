using SalesProcessor.Domain.Entities;

namespace SalesProcessor.Domain.Interfaces;

public interface ISalesProcessingService
{
    public Task<SalesSummary> ProcessSalesSummary(IAsyncEnumerable<Sale> salesList);
}