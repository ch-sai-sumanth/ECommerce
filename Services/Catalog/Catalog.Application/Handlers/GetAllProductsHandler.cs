using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using Catalog.Core.Specifications;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetAllProductsHandler : IRequestHandler<GetAllProductsQuery, Pagination<ProductResponse>>
{
    private readonly IProductRepository _productRepository;

    public GetAllProductsHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<Pagination<ProductResponse>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
    {
        var productsList = await _productRepository.GetProductsAsync(request.catalogSpecParams);
        var productResponseList = productsList.ToResponse();
        return productResponseList;

    }
}