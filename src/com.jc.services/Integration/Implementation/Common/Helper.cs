using com.jc.providers.CustomAttributes;
using com.jc.services.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace com.jc.services.Integration.Implementation.Common
{
    internal static class Helper
    {
        public static TEntity ToEntity<TEntity>(object sourceObject)
            where TEntity : BaseEntity, new()
        {
            try
            {
                TEntity receiverObject = new TEntity();
                foreach (PropertyInfo prop in sourceObject.GetType().GetProperties())
                {
                    object propValue = prop.GetValue(sourceObject);
                    PropertyInfo genericPropertyInfo =
                                                        receiverObject
                                                            .GetType()
                                                            .GetProperties()
                                                            .Where(genProperty => genProperty.Name == prop.Name)
                                                            .FirstOrDefault();
                    genericPropertyInfo.SetValue(receiverObject, propValue);
                }
                return receiverObject;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static TEntity ToEntity<TEntity>(IDataReader reader)
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
                    else
                    {
                        ordinal = reader.GetOrdinal(property.Name);
                        val = reader.GetValue(ordinal);
                        property.SetValue(newEntity, val);
                    }
                }
                return newEntity;
            }
            catch (Exception)
            {
                throw;
            }
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

        public static List<KeyValuePair<string, dynamic>> GetPropertyKeyAndValue<TEntity>(TEntity entity)
            where TEntity : class
        {
            List<KeyValuePair<string, dynamic>> propertyList = new List<KeyValuePair<string, dynamic>>();
            foreach (PropertyInfo property in entity.GetType().GetProperties())
            {
                propertyList.Add(new KeyValuePair<string, dynamic>(property.Name, property.GetValue(entity)));
            }
            return propertyList;
        }
    }
}
