using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BienesRaicesAPI.Controllers.Common
{
    [Route("api/v1/[controller]")]
    public abstract class V1Controller(IMediator mediator) : BaseController(mediator)
    {
    }
}
