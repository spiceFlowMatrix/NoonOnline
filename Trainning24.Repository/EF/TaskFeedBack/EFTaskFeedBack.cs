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
    public class EFTaskFeedBack : IGenericRepository<TaskFeedBack>
    {
        private readonly EFGenericRepository<TaskFeedBack> _context;

        public EFTaskFeedBack
        (
            EFGenericRepository<TaskFeedBack> context
        )
        {
            _context = context;
        }

        public int Delete(TaskFeedBack obj)
        {
            throw new NotImplementedException();
        }

        public List<TaskFeedBack> GetAll()
        {
            return _context.GetAll();
        }

        public List<TaskFeedBack> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public TaskFeedBack GetById(Expression<Func<TaskFeedBack, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(TaskFeedBack obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<TaskFeedBack> ListQuery(Expression<Func<TaskFeedBack, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(TaskFeedBack obj)
        {
            _context.Update(obj);
            return Save();
        }
    }
}
