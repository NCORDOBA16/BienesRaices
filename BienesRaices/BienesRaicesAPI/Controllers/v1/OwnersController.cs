using Application.DTOs.Owners;
using Application.Features.Owners.Commands.CreateOwner;
using Application.Features.Owners.Queries.ListOwners;
using Application.Wrappers.Common;
using BienesRaicesAPI.Controllers.Common;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BienesRaicesAPI.Controllers.v1
{
  
    public class OwnersController(IMediator mediator) : V1Controller(mediator)
    {
        private readonly IMediator _mediator = mediator;

        [HttpPost("CreateOwner")]
       [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseWrapperResponse<OwnerDto>>> Create([FromForm] CreateOwnerDto dto)
        {
            var command = new CreateOwnerCommand { Owner = dto };
            var result = await _mediator.Send(command);

            // If creation returns the created Owner DTO with IdOwner populated, return 201
            if (result?.Data != null)
            {
                return CreatedAtAction(nameof(GetById), new { id = result.Data.IdOwner }, result);
            }

            return Ok(result);
        }

        [HttpGet("ListOwner")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<BaseWrapperResponse<IEnumerable<OwnerDto>>>> GetAll()
        {
            var query = new GetOwnersQuery();
            var result = await _mediator.Send(query);
            return Ok(result);
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin,User")]
        public async Task<ActionResult<BaseWrapperResponse<OwnerDto>>> GetById(Guid id)
        {
            var query = new Application.Features.Owners.Queries.GetOwnerById.GetOwnerByIdQuery { Id = id };
            var result = await _mediator.Send(query);
            return Ok(result);
        }
    }
}
