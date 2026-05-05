using Microsoft.AspNetCore.Mvc;
using WhatsAppSalesAgent.Application.Features.Products.Commands.CreateProduct;
using WhatsAppSalesAgent.Application.Features.Products.Commands.UpdateProduct;
using WhatsAppSalesAgent.Application.Features.Products.Commands.UpdateStock;
using WhatsAppSalesAgent.Application.Features.Products.Queries.GetAllProducts;
using WhatsAppSalesAgent.Application.Features.Products.Queries.GetProductById;
using WhatsAppSalesAgent.Application.Features.Products.Queries.SearchProducts;

namespace WhatsAppSalesAgent.API.Controllers;

public class ProductsController : ApiControllerBase
{
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetAll([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new GetAllProductsQuery(pageNumber, pageSize), cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetProductByIdQuery(id), cancellationToken);
        return ToActionResult(result);
    }

    [HttpGet("search")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Search([FromQuery] string? keyword, [FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 20, CancellationToken cancellationToken = default)
    {
        var result = await Mediator.Send(new SearchProductsQuery(keyword, pageNumber, pageSize), cancellationToken);
        return ToActionResult(result);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Create([FromBody] CreateProductCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        if (result.IsFailure)
            return BadRequest(new { errors = result.Errors });

        return CreatedAtAction(nameof(GetById), new { id = result.Value }, result.Value);
    }

    [HttpPut("{id:guid}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductRequest request, CancellationToken cancellationToken)
    {
        var command = new UpdateProductCommand(id, request.Name, request.Description, request.Price, request.ImageUrl, request.IsActive);
        var result = await Mediator.Send(command, cancellationToken);
        return ToActionResult(result);
    }

    [HttpPatch("{id:guid}/stock")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> UpdateStock(Guid id, [FromBody] UpdateStockRequest request, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new UpdateStockCommand(id, request.NewQuantity), cancellationToken);
        return ToActionResult(result);
    }
}

public record UpdateProductRequest(string Name, string Description, decimal Price, string? ImageUrl, bool IsActive);
public record UpdateStockRequest(int NewQuantity);
