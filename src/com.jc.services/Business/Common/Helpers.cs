using com.jc.providers.CustomAttributes;
using com.jc.services.Domain;
using com.jc.services.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace com.jc.services.Business.Common
{
    internal static class Helpers
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
            where TClass : class
        {
            PropertyInfo property =
                typeof(TClass).GetProperties()
                .Where(attribute => attribute.GetCustomAttributes<TAttribute>().Count() > 0)
                .FirstOrDefault();

            if (property == null)
                return string.Empty;

            return property.Name;
        }

        public static string GetColumnMappingNameFromPrimaryKeyAttribute<TEntity>()
            where TEntity : BaseEntity
        {
            PropertyInfo property = GetPropertyInfoFromAttribute<TEntity, PrimaryKeyAttribute>();

            if (property == null)
                throw new ArgumentException("No Primary Key attribute found");

            return GetColumnMappingAttributeValue(property);
        }

        public static PropertyInfo GetPropertyInfoFromAttribute<TClass, TAttribute>()
            where TAttribute : Attribute
            where TClass : class
        {
            PropertyInfo property =
                typeof(TClass).GetProperties()
                .Where(attribute => attribute.GetCustomAttributes<TAttribute>().Count() > 0)
                .FirstOrDefault();

            if (property == null)
                throw new ArgumentException("No property found based on custom attribute");

            return property;
        }

        public static List<KeyValuePair<string, dynamic>> GetPropertyNameAndValue<TEntity>(TEntity entity)
            where TEntity : class
        {
            List<KeyValuePair<string, dynamic>> propertyList = new List<KeyValuePair<string, dynamic>>();
            foreach (PropertyInfo property in entity.GetType().GetProperties())
            {
                propertyList.Add(new KeyValuePair<string, dynamic>(property.Name, property.GetValue(entity)));
            }
            return propertyList;
        }

        public static string GetADOAttributeValue<TClass, TAttribute>(ADOEnum adoEnum)
            where TAttribute : Attribute
            where TClass : class
        {
            try
            {
                // Using reflection.
                //System.Attribute[] attrs = System.Attribute.GetCustomAttributes(t);  // Reflection. 
                IEnumerable<TAttribute> attrs = typeof(TClass).GetCustomAttributes<TAttribute>();

                // Getting output. 
                foreach (Attribute attr in attrs)
                {
                    DatabaseMappingAttribute a = (DatabaseMappingAttribute)attr;
                    switch (adoEnum)
                    {
                        case ADOEnum.Select:
                            return a.SelectSP;
                        case ADOEnum.Insert:
                            return a.InsertSP;
                        case ADOEnum.Update:
                            return a.UpdateSP;
                        case ADOEnum.Delete:
                            return a.DeleteSP;
                        default:
                            return string.Empty;
                    }
                }
                return string.Empty;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetColumnMappingAttributeValue<TEntity>(string propertyName)
        {
            try
            {
                if (String.IsNullOrEmpty(propertyName))
                    throw new ArgumentException("Property name is empty");

                PropertyInfo property = typeof(TEntity).GetProperty(propertyName);

                if (property == null)
                    throw new ArgumentException("No property found");

                ColumnMappingAttribute attr = property.GetCustomAttribute<ColumnMappingAttribute>();

                if (attr == null)
                    throw new ArgumentException("No custom attribute found");

                return attr.ColumnName;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public static string GetColumnMappingAttributeValue(PropertyInfo property)
        {
            try
            {
                if (property == null)
                    throw new ArgumentException("No property passed");

                ColumnMappingAttribute attr = property.GetCustomAttribute<ColumnMappingAttribute>();

                if (attr == null)
                    throw new ArgumentException("No custom attribute found");

                return attr.ColumnName;
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
