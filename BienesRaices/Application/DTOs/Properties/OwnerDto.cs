using System;

namespace Application.DTOs.Properties
{
    public class OwnerDto
    {
        public Guid IdOwner { get; set; }
        public string OwnerName { get; set; } = null!;
        public string? OwnerPhoto { get; set; }
    }
}
