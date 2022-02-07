using Hackathon.Common.Models;
using Hackathon.Common.Models.Base;

namespace Hackathon.Contracts.Requests
{
    public class GetListRequest<T>: Pagination where T: class
    {
        public T Filter { get; set; }

        public string SortBy { get; set; }
        public SortOrder SortOrder { get; set; }
    }
}