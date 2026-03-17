using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Responses;
using Catalog.Core.Entities;
using Catalog.Core.Specifications;

namespace Catalog.Application.Mappers;

public static class ProductMapper
{

    public static ProductResponse ToResponse(this Product product)
    {
        if(product==null) return null;

        return new ProductResponse()
        {
            Id = product.Id,
            Name = product.Name,
            Summary = product.Summary,
            Description = product.Description,
            ImageFile = product.ImageFile,
            Price = product.Price,
            Brand = product.Brand,
            Type = product.Type,
            CreatedDate = product.CreatedDate,
        };
    }


    public static Pagination<ProductResponse> ToResponse(this Pagination<Product> pagination)
    {
        return new Pagination<ProductResponse>(pagination.PageIndex, pagination.PageSize, pagination.Count,
            pagination.Data.Select(p => p.ToResponse()).ToList());

    }

    public static IList<ProductResponse> ToResponseList(this IEnumerable<Product> products)
    {
        return products.Select(p=>p.ToResponse()).ToList();
    }

    public static Product ToEntity(this CreateProductCommand command, ProductBrand brand, ProductType type)
    {
        return new Product()
        {
            Id = command.Id,
            Name = command.Name,
            Summary = command.Summary,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Brand = brand,
            Type = type,
            Price = command.Price,
            CreatedDate = DateTime.Now
        };
    }

    public static Product ToUpdateEntity(this UpdateProductCommand command, Product existing, ProductBrand brand, ProductType type)
    {
        var updatedProduct = new Product()
        {
            Id = existing.Id,
            Name = command.Name,
            Summary = command.Summary,
            Description = command.Description,
            ImageFile = command.ImageFile,
            Brand = brand,
            Type = type,
            Price = command.Price,
            CreatedDate = DateTime.Now
        };

        return updatedProduct;
    }

    public static ProductDto ToDto(this ProductResponse productResponse)
    {
        if(productResponse==null) return null;

        var dto = new ProductDto(
            productResponse.Id,
            productResponse.Name,
            productResponse.Summary,
            productResponse.Description,
            productResponse.ImageFile,
            new BrandDto(productResponse.Brand.Id, productResponse.Brand.Name),
            new TypeDto(productResponse.Type.Id, productResponse.Type.Name),
            productResponse.Price,
            DateTime.Now
        );

        return dto;
    }


    public static UpdateProductCommand ToCommand(this UpdateProductDto productDto, string id)
    {
        var command = new UpdateProductCommand()
        {
            Id = id,
            Name = productDto.Name,
            Summary = productDto.Summary,
            Description = productDto.Description,
            ImageFile = productDto.ImageFile,
            BrandId = productDto.BrandId,
            TypeId = productDto.TypeId,
            Price = productDto.Price,

        };
        return command;
    }
}