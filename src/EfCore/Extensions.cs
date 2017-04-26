using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using Domain.Entities;
using Domain.Entities.Base;

namespace EfCore
{
    public static class Extensions
    {
        public static string GetTableName<TEntity, TPrimaryKey>() where TEntity :IEntity<TPrimaryKey>
        {
            var type = typeof(TEntity);
            var tableArr = type.GetTypeInfo().GetCustomAttribute<TableAttribute>();
            return tableArr?.Name ?? type.Name;
        }
    }
}
