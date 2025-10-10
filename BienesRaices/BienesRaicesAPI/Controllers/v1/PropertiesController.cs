using Application.DTOs.Properties;
using Application.Features.Properties.Commands.CreateProperty;
using Application.Features.Properties.Commands.UpdateProperty;
using Application.Features.Properties.Commands.UpdatePropertyPrice;
using Application.Features.Properties.Commands.UploadPropertyImage;
using Application.Features.Properties.Queries.ListProperties;
using BienesRaicesAPI.Controllers.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BienesRaicesAPI.Controllers.v1
{
    [Authorize(Roles = "Admin,User")]
    public class PropertiesController(IMediator mediator) : V1Controller(mediator)
    {
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePropertyDto dto)
        {
            var command = new CreatePropertyCommand { Property = dto };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("UpdatePrice")]
        public async Task<IActionResult> UpdatePrice([FromBody] UpdatePropertyPriceCommand command)
        {
            //var command = new UpdatePropertyPriceCommand { IdProperty = id, NewPrice = newPrice };
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdatePropertyCommand command)
        {
            // Bind route id into the command and forward
            command.IdProperty = id;
            var result = await Mediator.Send(command);
            return Ok(result);
        }

        [HttpGet("ListPropertyFilters")]
        [AllowAnonymous]
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
