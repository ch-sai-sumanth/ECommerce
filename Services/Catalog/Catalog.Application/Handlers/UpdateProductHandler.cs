using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class UpdateProductHandler : IRequestHandler<UpdateProductCommand,bool>
{
    private readonly IProductRepository _productRepository;

    public UpdateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<bool> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
    {
        var existingProduct = _productRepository.GetProductAsync(request.Id).Result;

        if (existingProduct == null)
        {
            throw new KeyNotFoundException($"Product with id {request.Id} not found");
        }

        var brand = await _productRepository.GetBrandByIdAsync(request.BrandId);
        var type = await _productRepository.GetTypeByIdAsync(request.TypeId);

        if (brand == null || type == null)
        {
            throw new ApplicationException("Invalid Brand or Type Specified");
        }

        var updatedProduct = request.ToUpdateEntity(existingProduct,brand, type);

        return await _productRepository.UpdateProductAsync(updatedProduct);

    }
}