using Application.Wrappers.Common;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Application.Features.Properties.Commands.UploadPropertyImage
{
    public class UploadPropertyImageCommand : IRequest<BaseWrapperResponse<string>>
    {
        public Guid PropertyId { get; set; }
        public IFormFile PropertyImage { get; set; } = null!;
    }
}
