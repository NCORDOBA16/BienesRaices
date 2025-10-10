using Microsoft.AspNetCore.Http;

namespace Application.DTOs.Owners
{
    public class CreateOwnerDto
    {
        public string Name { get; set; } = null!;
        public IFormFile? Photo { get; set; }
        public string? Address { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
