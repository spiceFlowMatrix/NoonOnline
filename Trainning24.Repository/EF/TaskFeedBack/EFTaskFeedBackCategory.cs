using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFTaskFeedBackCategory : IGenericRepository<TaskCategoryFeedBack>
    {
        private readonly EFGenericRepository<TaskCategoryFeedBack> _context;

        public EFTaskFeedBackCategory
        (
            EFGenericRepository<TaskCategoryFeedBack> context
        )
        {
            _context = context;
        }

        public int Delete(TaskCategoryFeedBack obj)
        {
            throw new NotImplementedException();
        }

        public List<TaskCategoryFeedBack> GetAll()
        {
            return _context.GetAll();
        }

        public List<TaskCategoryFeedBack> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public TaskCategoryFeedBack GetById(Expression<Func<TaskCategoryFeedBack, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(TaskCategoryFeedBack obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<TaskCategoryFeedBack> ListQuery(Expression<Func<TaskCategoryFeedBack, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(TaskCategoryFeedBack obj)
        {
            _context.Update(obj);
            return Save();
        }
    }
}
