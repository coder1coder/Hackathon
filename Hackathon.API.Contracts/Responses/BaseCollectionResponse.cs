using System.Collections.Generic;

namespace Hackathon.Contracts.Responses
{
    public class BaseCollectionResponse<T>
    {
        public List<T> Items { get; set; }
        public long TotalCount { get; set; }
    }
}