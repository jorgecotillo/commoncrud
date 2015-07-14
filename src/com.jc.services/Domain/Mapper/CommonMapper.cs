using com.jc.providers.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace com.jc.services.Domain.Mapper
{
    public class CommonMapper<TEntity> : MapperBase<TEntity>
         where TEntity : BaseEntity, new()
    {
        public override TEntity Map(System.Data.IDataRecord record)
        {   
            try
            {
                TEntity newEntity = new TEntity();

                PropertyInfo[] properties = newEntity.GetType().GetProperties();
                int ordinal = 0;
                object val = new object();
                foreach (PropertyInfo property in properties)
                {
                    ColumnMappingAttribute attribute = property.GetCustomAttribute<ColumnMappingAttribute>();
                    if (attribute != null)
                    {
                        ordinal = record.GetOrdinal(attribute.ColumnName);
                        val = record.GetValue(ordinal);

                        val = Convert.ChangeType(val, property.PropertyType);

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

        public override TEntity Map(System.Xml.XmlReader record)
        {
            try
            {
                IDictionary<string, object> xmlFields = ReadXmlValues(record);

                foreach (PropertyInfo property in typeof(TEntity).GetProperties())
                {
                    //TODO: Still in progress.
                }
                return null;
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        public override TEntity Map(object generalObject)
        {
            try
            {
                TEntity receiverObject = new TEntity();
                foreach (PropertyInfo prop in generalObject.GetType().GetProperties())
                {
                    object propValue = prop.GetValue(generalObject);
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
    }
}
