using com.jc.providers.BaseDomain;
using com.jc.providers.CustomAttributes;
using com.jc.providers.Enums;
using com.jc.providers.Helpers;
using com.jc.providers.Interfaces;
using com.jc.providers.Interfaces.RelationalDB;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace com.jc.providers.Implementations.RelationalDB.SQLServer.ADO
{
    public class ADORepository<TEntity> : IRepository<TEntity> 
        where TEntity : BaseEntity, new()
    {
        readonly IADOContext _context;
        public ADORepository(IADOContext context)
        {
            _context = context;
        }
        public IEnumerable<TEntity> Table
        {
            get
            {
                string selectSP = GetAttributeValue<TEntity, DatabaseMappingAttribute>(ADOEnum.Select);
                string idColumnName = Helper.GetPropertyNameFromAttribute<TEntity, PrimaryKeyAttribute>();

                List<KeyValuePair<string, object>> keyValuePairList = new List<KeyValuePair<string, object>>();
                //TODO: Modify 0 for 'ALL' or something different so it can allow the sproc to retrieve all the rows
                keyValuePairList.Add(new KeyValuePair<string, object>(idColumnName, 0));

                return _context.GetMultipleEntities<TEntity>(selectSP, keyValuePairList).Result;
            }
        }

        public void Delete(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public async Task<TEntity> GetById(int id)
        {
            try
            {
                string selectSP = GetAttributeValue<TEntity, DatabaseMappingAttribute>(ADOEnum.Select);
                string idColumnName = Helper.GetPropertyNameFromAttribute<TEntity, PrimaryKeyAttribute>();

                List<KeyValuePair<string, object>> keyValuePairList = new List<KeyValuePair<string, object>>();
                keyValuePairList.Add(new KeyValuePair<string, object>(idColumnName, id));
                TEntity entity = await _context.GetSingleEntity<TEntity>(selectSP, keyValuePairList);

                return entity;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public void Insert(TEntity entity)
        {
            throw new NotImplementedException();
        }

        public void Update(TEntity entity)
        {
            throw new NotImplementedException();
        }

        private static string GetAttributeValue<TClass, TAttribute>(ADOEnum adoEnum)
            where TAttribute : Attribute
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
    }
}
