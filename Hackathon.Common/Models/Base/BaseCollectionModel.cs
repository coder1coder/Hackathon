using System.Collections.Generic;

namespace Hackathon.Common.Models.Base
{
    public class BaseCollectionModel<T> where T: class
    {
        public IReadOnlyCollection<T> Items { get; set; }
        public long TotalCount { get; set; }
    }
}