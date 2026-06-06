using Microsoft.AspNetCore.Mvc;
using BubbleShop.Application.Features.Customers.Commands.CreateOrUpdateCustomer;
using BubbleShop.Application.Features.Customers.Queries.GetCustomerByWhatsAppNumber;

namespace BubbleShop.API.Controllers;

public class CustomersController : ApiControllerBase
{
    [HttpGet("{whatsAppNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> GetByWhatsAppNumber(string whatsAppNumber, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(new GetCustomerByWhatsAppNumberQuery(whatsAppNumber), cancellationToken);
        return Ok(result.Value);
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateOrUpdate([FromBody] CreateOrUpdateCustomerCommand command, CancellationToken cancellationToken)
    {
        var result = await Mediator.Send(command, cancellationToken);
        return ToActionResult(result);
    }
}
