using Hackathon.Common.Models;

namespace Hackathon.Contracts.Requests
{
    public class GetFilterRequest<T> where T: class
    {
        public T Filter { get; set; }

        public long Page { get; set; } = 1;
        public long PageSize { get; set; } = 10;

        public string SortBy { get; set; }
        public SortOrder SortOrder { get; set; }
    }
}