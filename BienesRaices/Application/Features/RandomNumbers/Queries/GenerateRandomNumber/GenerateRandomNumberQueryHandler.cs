using System.Security.Cryptography;
using Application.Wrappers;
using Application.Wrappers.Common;
using MediatR;

namespace Application.Features.RandomNumbers.Queries.GenerateRandomNumber
{
    public class GenerateRandomNumberQueryHandler : IRequestHandler<GenerateRandomNumberQuery, BaseWrapperResponse<int>>
    {
        public async Task<BaseWrapperResponse<int>> Handle(GenerateRandomNumberQuery request, CancellationToken cancellationToken)
        {
            int randomNumber = RandomNumberGenerator.GetInt32(1, 101);
            return await Task.FromResult(new WrapperResponse<int>(randomNumber));
        }
    }
}