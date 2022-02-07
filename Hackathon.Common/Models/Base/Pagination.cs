namespace Hackathon.Common.Models.Base
{
    public class Pagination
    {
        private int _page = 1;

        public int Page
        {
            get => _page;
            set => _page = value > 0 ? value : 1;
        }
        public int PageSize { get; set; } = 10;
    }
}