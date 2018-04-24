using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Common.Extensions
{
    public static class ObjectExtensions
    {
        public static IDictionary<string, object> ObjectPropertiesToDictionary(this object obj)
        {
            if (obj == null)
                throw new ArgumentNullException("obj");

            var dict = new Dictionary<string, object>();
            PropertyInfo[] properties = obj.GetType().GetProperties();

            return properties.ToDictionary(key => key.Name, val => val.GetValue(obj));
        }

        public static object GetPropertyValue(this object obj, string propName)
        {
            if (obj == null)
                return null;

            PropertyInfo pi = obj.GetType().GetProperty(propName);
            return pi.GetValue(obj);
        }
    }
}
