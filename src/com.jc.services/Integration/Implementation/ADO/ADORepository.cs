using com.jc.providers.CustomAttributes;
using com.jc.services.Domain;
using com.jc.services.Domain.Enum;
using com.jc.services.Integration.Interfaces;
using com.jc.services.Integration.Interfaces.ADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Data.Common;
using com.jc.services.Business.Common;

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
            string selectSP = Helpers.GetADOAttributeValue<TEntity, DatabaseMappingAttribute>(ADOEnum.Select);
            string idColumnName = Helpers.GetColumnMappingNameFromPrimaryKeyAttribute<TEntity>();

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
                string selectSP = Helpers.GetADOAttributeValue<TEntity, DatabaseMappingAttribute>(ADOEnum.Select);
                string idColumnName = Helpers.GetColumnMappingNameFromPrimaryKeyAttribute<TEntity>();

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
            string insertSP = Helpers.GetADOAttributeValue<TEntity, DatabaseMappingAttribute>(ADOEnum.Insert);
            await InsertOrUpdate(entity, insertSP, ADOEnum.Insert);
        }

        public async Task Update(TEntity entity)
        {
            string updateSP = Helpers.GetADOAttributeValue<TEntity, DatabaseMappingAttribute>(ADOEnum.Update);
            await InsertOrUpdate(entity, updateSP, ADOEnum.Update);
        }

        public async Task Delete(TEntity entity)
        {
            string deleteSP = Helpers.GetADOAttributeValue<TEntity, DatabaseMappingAttribute>(ADOEnum.Delete);
            string idColumnName = Helpers.GetColumnMappingNameFromPrimaryKeyAttribute<TEntity>();

            List<KeyValuePair<string, object>> keyValuePairList = new List<KeyValuePair<string, object>>();
            //TODO: Modify 0 for 'ALL' or something different so it can allow the sproc to retrieve all the rows
            keyValuePairList.Add(new KeyValuePair<string, object>(idColumnName, entity.Id));

            await _context.PersistEntity<TEntity>(deleteSP, keyValuePairList);
        }

        private async Task InsertOrUpdate(TEntity entity, string storeProcedure, ADOEnum adoEnum)
        {
            List<KeyValuePair<string, object>> keyValuePairList = Helpers.GetPropertyNameAndValue<TEntity>(entity);
            await _context.PersistEntity<TEntity>(storeProcedure, keyValuePairList);
        }

        public IADOContext Context
        {
            get { return _context; }
        }


        public Task Commit()
        {
            throw new NotImplementedException();
        }
    }
}
