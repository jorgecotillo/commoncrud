using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace com.jc.services.Domain.Mapper
{
    public abstract class MapperBase<TEntity>
        where TEntity : BaseEntity, new()
    {
        public abstract TEntity Map(IDataRecord record);

        public IList<TEntity> MapAll(IDataReader reader)
        {
            IList<TEntity> collection = new List<TEntity>();

            while (reader.Read())
            {
                try
                {
                    collection.Add(Map(reader));
                }
                catch
                {
                    throw;
                }
            }
            return collection;
        }

        public abstract TEntity Map(XmlReader record);

        public IList<TEntity> MapAll(XmlReader xdoc)
        {   
            IList<TEntity> collection = new List<TEntity>();
           
            return collection;
        }

        public IDictionary<string, object> ReadXmlValues(XmlReader reader)
        {
            var fields = new Dictionary<string, object>();
            string xmlValue = string.Empty;
            while (reader.Read())
            {
                if (reader.IsStartElement() &&
                   !string.IsNullOrEmpty(reader.Name) &&
                   reader.Name.Equals("results", StringComparison.InvariantCultureIgnoreCase))
                {
                    fields.Add(reader.Name, reader.GetValueAsync());
                }
            }
            return fields;
        }

        public abstract TEntity Map(object generalObject);
    }
}
