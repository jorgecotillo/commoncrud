using com.jc.services.Domain;
using com.jc.services.Integration.Interfaces;
using com.jc.services.Integration.Interfaces.EF;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Threading.Tasks;

namespace com.jc.services.Integration.Implementation.EF
{
    /// <summary>
    /// Entity Framework repository
    /// </summary>
    public partial class EFRepository<TEntity> : IRepository<TEntity, IQueryable<TEntity>>
        where TEntity : BaseEntity
    {
        private readonly IDbContext _context;
        protected IDbSet<TEntity> _entities;

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="context">Object context</param>
        public EFRepository(IDbContext context)
        {
            this._context = context;
        }

        public async Task<IList<TEntity>> GetAll(int page = 0, int pageSize = Int32.MaxValue, bool active = true)
        {
            var query = this.Context;

            if (active)
                query = query
                    .Where(entity => entity.Active == 1);

            query = query
                .OrderBy(entity => entity.Id)
                .Skip(page)
                .Take(pageSize);

            return await query.ToListAsync();
        }

        public async Task<TEntity> GetById(int id, bool active = true)
        {
            //return 
                //await this
                var x = Context
                .Where(entity => entity.Id == id && entity.Active == 1);
                return await x.FirstOrDefaultAsync();
        }

        public async Task Insert(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                
                entity.Active = 1;

                this.Entities.Add(entity);
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage) + Environment.NewLine;

                var fail = new Exception(msg, dbEx);
                //Debug.WriteLine(fail.Message, fail);
                throw fail;
            }
        }

        public async Task Update(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");
                //await this._context.SaveChangesAsync();
            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                //Debug.WriteLine(fail.Message, fail);
                throw fail;
            }
        }

        public async Task Delete(TEntity entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                this.Entities.Remove(entity);
                //await this._context.SaveChangesAsync();

            }
            catch (DbEntityValidationException dbEx)
            {
                var msg = string.Empty;

                foreach (var validationErrors in dbEx.EntityValidationErrors)
                    foreach (var validationError in validationErrors.ValidationErrors)
                        msg += Environment.NewLine + string.Format("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);

                var fail = new Exception(msg, dbEx);
                //Debug.WriteLine(fail.Message, fail);
                throw fail;
            }
        }

        public IQueryable<TEntity> Context
        {
            get
            {
                return this.Entities.AsNoTracking();
            }
        }

        public async Task Commit()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        private IDbSet<TEntity> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<TEntity>();
                return _entities;
            }
        }
    }
}
