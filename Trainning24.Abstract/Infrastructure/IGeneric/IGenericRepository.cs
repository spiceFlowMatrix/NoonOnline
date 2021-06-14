using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;

namespace Trainning24.Abstract.Infrastructure.IGeneric
{
    public interface IGenericRepository<T> where T : IEntityBase
    {

        List<T> GetAll();

        List<T> GetAllActive();

        IQueryable<T> ListQuery(Expression<Func<T, bool>> where);

        T GetById(Expression<Func<T, bool>> ex);

        int Insert(T obj);

        int Update(T obj);

        int Delete(T obj);

        int Save();
    }
}
