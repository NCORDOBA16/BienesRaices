namespace Application.DTOs.Properties
{
    public class CreatePropertyDto
    {
        public string Title { get; set; } = null!;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string CodeInternal { get; set; } = null!;
        public Guid IdOwner { get; set; }
        public string Year { get; set; }
        // Optionally client can send image urls
        //public IList<string>? ImageUrls { get; set; }

    }
}
