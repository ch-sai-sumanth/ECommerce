using Catalog.Application.Commands;
using Catalog.Application.Mappers;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class CreateProductHandler : IRequestHandler<CreateProductCommand, ProductResponse>
{
    private readonly IProductRepository _productRepository;

    public CreateProductHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<ProductResponse> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
       //verify the brand and type
       var brand = await _productRepository.GetBrandByIdAsync(request.BrandId);
       var type = await  _productRepository.GetTypeByIdAsync(request.TypeId);

       if (brand is null || type is null)
       {
           throw new ApplicationException("Brand and type are Invalid");
       }

       var productEntity = request.ToEntity(brand, type);
       await _productRepository.CreateProductAsync(productEntity);

       return productEntity.ToResponse();
    }
}