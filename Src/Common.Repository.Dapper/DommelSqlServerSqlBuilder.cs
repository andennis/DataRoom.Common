using System;
using System.Reflection;
using Dommel;

namespace Common.Repository.Dapper
{
    public sealed class DommelSqlServerSqlBuilder : DommelMapper.ISqlBuilder
    {
        public string BuildInsert(string tableName, string[] columnNames, string[] paramNames, PropertyInfo keyProperty)
        {
            return string.Format("set nocount on insert into {0} ({1}) values ({2}) select cast(scope_identity() as {3})", 
                tableName, 
                string.Join(",", columnNames), 
                string.Join(",", paramNames), 
                GetSqlTypeName(keyProperty.PropertyType));
        }

        private string GetSqlTypeName(Type type)
        {
            if (type == typeof(long))
                return "bigint";
            if (type == typeof(int))
                return "int";

            throw new CommonRepositoryException($"Type '{type.Name}' is not supported for Key property");
        }
    }

}
