using Application.Wrappers.Common;
using MediatR;

namespace Application.Features.RandomNumbers.Queries.GenerateRandomNumber
{
    public class GenerateRandomNumberQuery : IRequest<BaseWrapperResponse<int>>
    {
    }
}
