using Discount.Application.DTOs;
using Discount.Core.Entities;
using MediatR;

namespace Discount.Application.Commands;

public record CreateDiscountCommand(string ProductName,string Description,int Amount) : IRequest<CouponDto>
{

}