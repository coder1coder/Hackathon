namespace Hackathon.Common.Models.Base;

public class PaginationSort: Pagination
{
    public string SortBy { get; set; }
    public SortOrder SortOrder { get; set; } = SortOrder.Asc;
}