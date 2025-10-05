namespace Application.Models.Paging
{
    public abstract class BasePagedModel
    {
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
    }
}
