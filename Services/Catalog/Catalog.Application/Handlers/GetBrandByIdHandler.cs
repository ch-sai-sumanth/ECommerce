using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetBrandByIdHandler :  IRequestHandler<GetBrandByIdQuery, BrandResponse>
{
    private readonly IProductRepository _productRepository;

    public GetBrandByIdHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<BrandResponse> Handle(GetBrandByIdQuery request, CancellationToken cancellationToken)
    {
        var productBrand = await _productRepository.GetBrandByIdAsync(request.brandId);

        return productBrand.ToResponse();
    }
}