using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using live.courses.Models;

namespace live.courses.PL
{
    public class Repository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private adv_coursesEntities _ctx;
        private DbSet<TEntity> _set;
        public Repository(adv_coursesEntities ctx)
        {
            _ctx = ctx;
            _set = _ctx.Set<TEntity>();
        }

        public List<TEntity> GetAllBind()
        {
            return _set.ToList();
        }

        public IQueryable<TEntity> GetAll()
        {
            return _set;
        }

        public TEntity GetAllById(params object[] id)
        {
            return _set.Find(id);
        }

        public TEntity Add(TEntity entity)
        {
            _set.Add(entity);
            return _ctx.SaveChanges() > 0 ? entity : null;
        }

        public bool Update(TEntity entity)
        {
            _set.Attach(entity);
            _ctx.Entry(entity).State = EntityState.Modified;
            return _ctx.SaveChanges() > 0;
        }

        public bool Delete(TEntity entity)
        {
            _set.Remove(entity);
            return _ctx.SaveChanges() > 0;
        }
    }
}