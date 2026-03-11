using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Application.Responses;
using Catalog.Core.Repositories;
using MediatR;

namespace Catalog.Application.Handlers;

public class GetTypeByIdHandler : IRequestHandler<GetTypeByIdQuery, TypesResponse>
{
    private readonly IProductRepository _productRepository;

    public GetTypeByIdHandler(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    public async Task<TypesResponse> Handle(GetTypeByIdQuery request, CancellationToken cancellationToken)
    {
        var productType = await _productRepository.GetTypeByIdAsync(request.typeId);

        return productType.ToResponse();
    }
}