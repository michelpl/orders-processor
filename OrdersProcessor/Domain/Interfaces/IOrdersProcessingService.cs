using OrdersProcessor.Domain.Entities;

namespace OrdersProcessor.Domain.Interfaces;

public interface IOrdersProcessingService
{
    public Task<OrdersSummary> GetOrdersSummary(IAsyncEnumerable<Order> orderList);
}