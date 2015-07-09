using com.jc.providers.CustomAttributes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace com.jc.services.Domain
{
    /// <summary>
    /// Base class for entities
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        [PrimaryKey]
        [ColumnMapping("Id")]
        public int Id { get; set; }
        
        [JsonProperty("active")]
        public virtual bool Active { get; set; }

        [JsonProperty("created_on")]
        public DateTime CreatedOn { get; set; }

        [JsonProperty("last_updated")]
        public DateTime LastUpdated { get; set; }

        public override bool Equals(object obj)
        {
            return Equals(obj as BaseEntity);
        }

        private static bool IsTransient(BaseEntity obj)
        {
            return obj != null && Equals(obj.Id, default(int));
        }

        private Type GetUnproxiedType()
        {
            return GetType();
        }

        public virtual bool Equals(BaseEntity other)
        {
            if (other == null)
                return false;

            if (ReferenceEquals(this, other))
                return true;

            if (!IsTransient(this) &&
                !IsTransient(other) &&
                Equals(Id, other.Id))
            {
                var otherType = other.GetUnproxiedType();
                var thisType = GetUnproxiedType();
                return thisType.IsAssignableFrom(otherType) ||
                        otherType.IsAssignableFrom(thisType);
            }

            return false;
        }

        public override int GetHashCode()
        {
            if (Equals(Id, default(int)))
                return base.GetHashCode();
            return Id.GetHashCode();
        }

        public static bool operator ==(BaseEntity x, BaseEntity y)
        {
            return Equals(x, y);
        }

        public static bool operator !=(BaseEntity x, BaseEntity y)
        {
            return !(x == y);
        }

        public virtual TEntity ToEntity<TEntity>(object sourceObject)
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

        public virtual TEntity ToEntity<TEntity>(IDataReader reader)
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
    }
}
