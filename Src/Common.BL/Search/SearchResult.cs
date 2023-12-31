﻿using System.Collections.Generic;

namespace Common.BL.Search
{
    public class SearchResult<TEntity> where TEntity : class
    {
        private IEnumerable<TEntity> _data;
        public IEnumerable<TEntity> Data 
        { 
            get => _data ?? (_data = new TEntity[0]);
            set => _data = value;
        }
        public long TotalCount { get; set; }
    }
}
