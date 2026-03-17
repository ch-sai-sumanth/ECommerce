using Catalog.Application.Commands;
using Catalog.Application.DTOs;
using Catalog.Application.Mappers;
using Catalog.Application.Queries;
using Catalog.Core.Specifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("api/v1/[controller]")]
public class CatalogController  : ControllerBase
{
    private readonly IMediator _mediator;

    public CatalogController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet("GetProducts")]
    public async Task<ActionResult<IList<ProductDto>>> GetProducts([FromQuery]CatalogSpecParams  specParams)
    {
        var query = new GetAllProductsQuery(specParams);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("GetAllBrands")]
    public async Task<ActionResult<IList<BrandDto>>> GetBrands()
    {
        var query = new GetAllBrandsQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("GetAllTypes")]
    public async Task<ActionResult<Pagination<TypeDto>>> GetTypes()
    {
        var query = new GetAllTypesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("Brands/{id}")]
    public async Task<ActionResult<BrandDto>> GetBrandById(string id)
    {
        var query = new GetBrandByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("Brands/{brand}",Name="GetProductsByBrand")]
    public async Task<ActionResult<ProductDto>> GetBrandByName(string brand)
    {
        var query = new GetProductsByBrandQuery(brand);
        var result = await _mediator.Send(query);

        if (result == null || !result.Any())
        {
            return NotFound();
        }

        var dtoList = result.Select(p=>p.ToDto()).ToList();
        return Ok(dtoList);
    }

    [HttpGet("Products/{id}")]
    public async Task<ActionResult<ProductDto>> GetProductById(string id)
    {
        var query = new GetProductByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("ProductName/{productName}")]
    public async Task<ActionResult<IList<ProductDto>>> GetProductByName(string productName)
    {
        var query = new GetProductsByNameQuery(productName);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("Type/{typeId}")]
    public async Task<ActionResult<TypeDto>> GetTypeById(string typeId)
    {
        var query = new GetProductByIdQuery(typeId);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult<ProductDto>> CreateProduct([FromBody] CreateProductCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ProductDto>> UpdateProduct(string id, [FromBody] UpdateProductDto productDto)
    {
        var command = productDto.ToCommand(id);
        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProduct(string id)
    {
        var command = new DeleteProductCommand(id);
        var result =  await _mediator.Send(command);

        if (!result)
        {
            return NotFound();
        }
        return NoContent();
    }
}