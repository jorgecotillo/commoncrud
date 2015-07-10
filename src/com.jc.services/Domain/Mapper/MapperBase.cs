using com.jc.services.Integration.Implementation.Common;
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
        public virtual TEntity Map(IDataRecord record)
        {
            return new TEntity();
        }

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

        public virtual TEntity Map(XmlReader record)
        {
            TEntity entity = new TEntity();

            
            return new TEntity();
        }

        public IList<TEntity> MapAll(XmlReader xdoc)
        {   
            IList<TEntity> collection = new List<TEntity>();
           
            return collection;
        }
    }
}
