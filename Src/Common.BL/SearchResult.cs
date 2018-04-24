using System.Collections.Generic;

namespace Common.BL
{
    public class SearchResult<TEntity> where TEntity : class
    {
        private IEnumerable<TEntity> _data;
        public IEnumerable<TEntity> Data 
        { 
            get { return _data ?? (_data = new TEntity[0]); }
            set { _data = value; }
        }
        public int TotalCount { get; set; }
    }
}
