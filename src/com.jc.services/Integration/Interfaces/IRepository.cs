using com.jc.services.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace com.jc.services.Integration.Interfaces
{
    public interface IRepository<TEntity, TContext> 
        where TEntity : BaseEntity
    {
        Task<IList<TEntity>> GetAll(int page = 0, int pageSize = Int32.MaxValue, bool active = true);
        Task<TEntity> GetById(int id, bool active = true);
        Task Insert(TEntity entity);
        Task Update(TEntity entity);
        Task Delete(TEntity entity);
        TContext Context { get; }
        Task Commit();
    }
}
