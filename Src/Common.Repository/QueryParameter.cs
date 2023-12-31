﻿using System.Data;

namespace Common.Repository
{
    public class QueryParameter
    {
        public QueryParameter(string name, object value)
        {
            Name = name;
            Value = value;
        }

        public string Name { get; set; }
        public object Value { get; set; }
        public ParameterDirection? Direction { get; set; }
    }
}
