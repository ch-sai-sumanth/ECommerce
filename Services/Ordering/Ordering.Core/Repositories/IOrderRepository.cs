using Ordering.Core.Entities;

namespace Ordering.Core.Repositories;

public interface IOrderRepository : IAsyncRepository<Order>
{
    Task<List<Order>> GetOrdersByUserName(string userName);
}