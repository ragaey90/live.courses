using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace live.courses.PL
{
    interface IRepository<TEntity>
    {
        List<TEntity> GetAllBind();
        IQueryable<TEntity> GetAll();
        TEntity GetAllById(params object[] id);
        TEntity Add(TEntity entity);
        bool Update(TEntity entity);
        bool Delete(TEntity entity);

    }
}
