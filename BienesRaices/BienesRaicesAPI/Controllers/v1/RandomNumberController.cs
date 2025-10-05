using Application.Features.RandomNumbers.Queries.GenerateRandomNumber;
using Application.Wrappers.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using BienesRaicesAPI.Controllers.Common;

namespace BienesRaicesAPI.Controllers.v1
{
    public class RandomNumberController(IMediator mediator) : V1Controller(mediator)
    {
        [HttpGet]
        public async Task<ActionResult<BaseWrapperResponse<int>>> GenerateRandomNumber()
        {
            var result = await Mediator.Send(new GenerateRandomNumberQuery());
            return Ok(result);
        }
    }
}
