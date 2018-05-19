namespace Common.BL.Search
{
    public class SearchContext
    {
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public string SortBy { get; set; }
        public string SortDirection { get; set; }
        //public SortDirection SortDirection { get; set; }
    }
}
