using com.jc.services.Business.Interfaces;
using com.jc.services.Domain;
using com.jc.services.Integration.Implementation.ADO;
using com.jc.services.Integration.Interfaces.ADO;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using com.jc.services.Integration.Interfaces;

namespace com.jc.services.Business.Implementations
{
    public class ADOCRUDCommonService<TEntity> : ICRUDCommonService<TEntity>
        where TEntity : BaseEntity
    {
        #region Fields
        readonly IRepository<TEntity, IADOContext> _commonRepository;
        #endregion

        #region Constructor
        public ADOCRUDCommonService(IRepository<TEntity, IADOContext> commonRepository)
        {
            _commonRepository = commonRepository;
        }

        public async Task Delete(TEntity entity, bool softDelete = true)
        {
            await _commonRepository.Delete(entity);
        }

        public async Task<IList<TEntity>> GetAll(int page = 0, int pageSize = int.MaxValue, bool active = true)
        {
            return await _commonRepository.GetAll(page, pageSize, active);
        }

        public async Task<TEntity> GetById(int id, bool active = true)
        {
            return await _commonRepository.GetById(id, active);
        }
        #endregion

        public async Task Save(TEntity entity)
        {
            if (entity.Id == 0)
                await _commonRepository.Insert(entity);
            else
                await _commonRepository.Update(entity);
        }
    }
}
