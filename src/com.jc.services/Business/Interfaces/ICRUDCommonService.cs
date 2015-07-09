using com.jc.services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.jc.services.Business.Interfaces
{
    public interface ICRUDCommonService<TEntity> 
        where TEntity : BaseEntity
    {
        Task<TEntity> GetById(int id, bool active = true);
        Task<IList<TEntity>> GetAll(int page = 0, int pageSize = Int32.MaxValue, bool active = true);
        Task Save(TEntity entity);
        Task Delete(TEntity entity, bool softDelete = true);
    }
}
