using com.jc.providers.BaseDomain;
using com.jc.providers.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace com.jc.providers.Helpers
{
    internal static class Helper
    {
        public static TEntity ToEntity<TEntity>(object entity)
        { 
            try
            {
                //TODO: Use reflection to create a new entity.
                return default(TEntity);
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static TEntity ToEntity<TEntity>(DbDataReader reader)
            where TEntity : BaseEntity, new()
        {
            TEntity newEntity = new TEntity();
            try
            {
                PropertyInfo[] properties = newEntity.GetType().GetProperties();
                int ordinal = 0;
                object val = new object();
                foreach (PropertyInfo property in properties)
                {
                    ColumnMappingAttribute attribute = property.GetCustomAttribute<ColumnMappingAttribute>();
                    if (attribute != null)
                    {
                        ordinal = reader.GetOrdinal(attribute.ColumnName);
                        val = reader.GetValue(ordinal);
                        property.SetValue(newEntity, val);
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return newEntity;
        }

        /// <summary>
        /// Method that returns the first ocurrence of the attribute associated to a property
        /// </summary>
        /// <typeparam name="TClass">Class that holds the properties</typeparam>
        /// <typeparam name="TAttribute">Attribute to lookup in all the properties</typeparam>
        /// <returns>Property name that is associated to the attribute being search</returns>
        public static string GetPropertyNameFromAttribute<TClass, TAttribute>()
            where TAttribute : Attribute
        {
            PropertyInfo property =
                typeof(TClass).GetProperties()
                .Where(attribute => attribute.GetCustomAttributes<TAttribute>().Count() > 0)
                .FirstOrDefault();

            if (property == null)
                return string.Empty;

            return property.Name;
        }
    }
}
