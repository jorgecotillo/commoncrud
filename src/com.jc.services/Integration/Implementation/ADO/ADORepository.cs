using com.jc.providers.CustomAttributes;
using com.jc.services.Domain;
using com.jc.services.Domain.Enum;
using com.jc.services.Integration.Implementation.Common;
using com.jc.services.Integration.Interfaces;
using com.jc.services.Integration.Interfaces.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Common;

namespace com.jc.services.Integration.Implementation.ADO
{
    public class ADORepository<TEntity> : IRepository<TEntity, IADOContext>
        where TEntity : BaseEntity, new()
    {
        readonly IADOContext _context;
        public ADORepository(IADOContext context)
        {
            _context = context;
        }

        public async Task<IList<TEntity>> GetAll(int page = 0, int pageSize = Int32.MaxValue, bool active = true)
        {
            string selectSP = GetADOAttributeValue<TEntity, DatabaseMappingAttribute>(ADOEnum.Select);
            string idColumnName = Helper.GetPropertyNameFromAttribute<TEntity, PrimaryKeyAttribute>();

            List<KeyValuePair<string, object>> keyValuePairList = new List<KeyValuePair<string, object>>();
            //TODO: Modify 0 for 'ALL' or something different so it can allow the sproc to retrieve all the rows
            keyValuePairList.Add(new KeyValuePair<string, object>(idColumnName, 0));
            keyValuePairList.Add(new KeyValuePair<string, object>("page", page));
            keyValuePairList.Add(new KeyValuePair<string, object>("pageSize", pageSize));

            return await _context.GetMultipleEntities<TEntity>(selectSP, keyValuePairList);
        }

        public async Task<TEntity> GetById(int id, bool active = true)
        {
            try
            {
                string selectSP = GetADOAttributeValue<TEntity, DatabaseMappingAttribute>(ADOEnum.Select);
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

        public async Task Insert(TEntity entity)
        {
            string insertSP = GetADOAttributeValue<TEntity, DatabaseMappingAttribute>(ADOEnum.Insert);
            await InsertOrUpdate(entity, insertSP, ADOEnum.Insert);
        }

        public async Task Update(TEntity entity)
        {
            string updateSP = GetADOAttributeValue<TEntity, DatabaseMappingAttribute>(ADOEnum.Update);
            await InsertOrUpdate(entity, updateSP, ADOEnum.Update);
        }

        public async Task Delete(TEntity entity)
        {
            string deleteSP = GetADOAttributeValue<TEntity, DatabaseMappingAttribute>(ADOEnum.Delete);
            string idColumnName = Helper.GetPropertyNameFromAttribute<TEntity, PrimaryKeyAttribute>();

            List<KeyValuePair<string, object>> keyValuePairList = new List<KeyValuePair<string, object>>();
            //TODO: Modify 0 for 'ALL' or something different so it can allow the sproc to retrieve all the rows
            keyValuePairList.Add(new KeyValuePair<string, object>(idColumnName, entity.Id));

            await _context.PersistEntity<TEntity>(deleteSP, keyValuePairList);
        }

        private async Task InsertOrUpdate(TEntity entity, string storeProcedure, ADOEnum adoEnum)
        {
            List<KeyValuePair<string, object>> keyValuePairList = Helper.GetPropertyKeyAndValue<TEntity>(entity);
            await _context.PersistEntity<TEntity>(storeProcedure, keyValuePairList);
        }

        private static string GetADOAttributeValue<TClass, TAttribute>(ADOEnum adoEnum)
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

        public IADOContext Context
        {
            get { return _context; }
        }
    }
}
