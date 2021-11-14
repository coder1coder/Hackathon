﻿namespace Hackathon.Common.Models
{
    public class GetFilterModel<T> where T: class
    {
        public T Filter { get; set; }

        public int? Page { get; set; }
        public int? PageSize { get; set; }

        public string SortBy { get; set; }
        public SortOrder SortOrder { get; set; } = SortOrder.Asc;
    }
}