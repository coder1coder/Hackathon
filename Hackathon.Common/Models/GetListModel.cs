using Hackathon.Common.Models.Base;

namespace Hackathon.Common.Models
{
    public class GetListModel<T>: Pagination where T: class
    {
        public T Filter { get; set; }
        public string SortBy { get; set; }
        public SortOrder SortOrder { get; set; } = SortOrder.Asc;
    }
}