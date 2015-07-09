using com.jc.services.Domain;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Threading.Tasks;

namespace com.jc.services.Integration.Interfaces.ADO
{
    public interface IADORepository<TEntity, TContextType>
        where TEntity : BaseEntity
    {
        Task<IList<TEntity>> GetAll(int page = 0, int pageSize = Int32.MaxValue, bool active = true);
        Task<TEntity> GetById(int id, bool active = true);
        Task Insert(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TEntity entity, bool softDelete = true);
        TContextType AdoWrapper { get; }
    }
}
