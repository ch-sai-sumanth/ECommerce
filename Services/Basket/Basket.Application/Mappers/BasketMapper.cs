using Basket.Application.Commands;
using Basket.Application.DTOs;
using Basket.Application.Responses;
using Basket.Core.Entities;

namespace Basket.Application.Mappers;

public static class BasketMapper
{

    public static ShoppingCart ToEntity(this CreateShoppingCartCommand command)
    {
        return new ShoppingCart
        {
            UserName = command.UserName,
            Items = command.Items.Select(item => new ShoppingCartItem
            {
                Quantity = item.Quantity,
                ImageFile = item.ImageFile,
                Price = item.Price,
                ProductId = item.ProductId,
                ProductName = item.ProductName
            }).ToList()
        };
    }

    public static ShoppingCartResponse ToResponse(this ShoppingCart shoppingCart)
    {
        return new ShoppingCartResponse
        {
            UserName = shoppingCart.UserName,
            Items = shoppingCart.Items.Select(item => new ShoppingCartItemResponse
            {
                Quantity = item.Quantity,
                ImageFile = item.ImageFile,
                Price = item.Price,
                ProductId = item.ProductId,
                ProductName = item.ProductName
            }).ToList()
        };
    }
}