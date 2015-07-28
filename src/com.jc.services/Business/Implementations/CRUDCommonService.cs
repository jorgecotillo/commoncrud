using com.jc.services.Business.Interfaces;
using com.jc.services.Domain;
using com.jc.services.Integration.Interfaces.EF;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using com.jc.services.Integration.Interfaces;
using System.Linq;

namespace com.jc.services.Business.Implementations
{
    public class CRUDCommonService<TEntity, TContext> : ICRUDCommonService<TEntity> 
        where TEntity : BaseEntity
    {
        #region Fields
        readonly IRepository<TEntity, TContext> _commonRepository;
        #endregion

        #region Constructor
        public CRUDCommonService(IRepository<TEntity, TContext> commonRepository)
        {
            _commonRepository = commonRepository;
        }
        #endregion

        public async Task Delete(TEntity entity, bool softDelete = true)
        {
            if (softDelete)
            {
                entity.Active = 0;
                await _commonRepository.Update(entity);
            }
            else
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

        public async Task Save(TEntity entity)
        {
            if (entity.Id == 0)
                await _commonRepository.Insert(entity);
            else
                await _commonRepository.Update(entity);
        }


        public async Task Commit()
        {
            await _commonRepository.Commit(); 
        }
    }
}
