using Application.DTOs.Properties;
using Application.Features.Properties.Commands.CreateProperty;
using Application.Features.Properties.Commands.UpdateProperty;
using Application.Features.Properties.Commands.UpdatePropertyPrice;
using Application.Features.Properties.Commands.UploadPropertyImage;
using Application.Features.Properties.Queries.ListProperties;
using Application.Wrappers.Common;
using BienesRaicesAPI.Controllers.Common;
using BienesRaicesAPI.Filters;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Threading;

namespace BienesRaicesAPI.Controllers.v1
{
    public class PropertiesController(IMediator mediator) : V1Controller(mediator)
    {
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Create([FromBody] CreatePropertyDto dto)
        {
            var command = new CreatePropertyCommand { Property = dto };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("{id}/price")]
        [ValidateModel]
        public async Task<IActionResult> UpdatePrice([FromRoute] Guid id, [FromBody] decimal newPrice)
        {
            var command = new UpdatePropertyPriceCommand { IdProperty = id, NewPrice = newPrice };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        [ValidateModel]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePropertyCommand command)
        {
            // Bind route id into the command and forward
            command.IdProperty = id;
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetPropertiesQuery query, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(query, cancellationToken);
            return Ok(result);
        }

        [HttpPost("UploadPropertyImage")]
        public async Task<IActionResult> UploadPropertySwagger([FromForm] UploadPropertyImageCommand command)
        {
            var result = await Mediator.Send(command);
            return Ok(result);
        }
    }
}
