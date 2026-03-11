using Catalog.Application.Responses;
using MediatR;

namespace Catalog.Application.Queries;

public record GetTypeByIdQuery(string typeId):IRequest<TypesResponse>
{

}