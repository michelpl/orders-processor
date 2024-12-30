using SalesProcessor.Domain.Entities;
using SalesProcessor.Domain.Interfaces;

namespace SalesProcessor.Application.Services;

public class SalesProcessingService : ISalesProcessingService
{
    public Task<SalesSummary> ProcessSalesSummary(IAsyncEnumerable<Sale> salesList)
    {
        throw new NotImplementedException();
    }
}