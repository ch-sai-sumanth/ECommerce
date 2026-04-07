using Ordering.Application.Abstractions;
using Ordering.Application.DTOs;

namespace Ordering.Application.Orders.GetOrder;

public record GetOrderListQuery(string UserName):IQuery<List<OrderDto>>;