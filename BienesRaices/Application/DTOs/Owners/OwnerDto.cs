namespace Application.DTOs.Owners
{
    public class OwnerDto
    {
        public Guid IdOwner { get; set; }
        public string Name { get; set; } = null!;
        public string? Photo { get; set; }
        public string? Address { get; set; }
        public DateTime? Birthday { get; set; }
    }
}
