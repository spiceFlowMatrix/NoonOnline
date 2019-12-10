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
    public class EFTaskActivityFeedBack : IGenericRepository<TaskActivityFeedBack>
    {
        private readonly EFGenericRepository<TaskActivityFeedBack> _context;

        public EFTaskActivityFeedBack
        (
            EFGenericRepository<TaskActivityFeedBack> context 
        )
        {
            _context = context;
        }

        public int Delete(TaskActivityFeedBack obj)
        {
            throw new NotImplementedException();
        }

        public List<TaskActivityFeedBack> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<TaskActivityFeedBack> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public TaskActivityFeedBack GetById(Expression<Func<TaskActivityFeedBack, bool>> ex)
        {
            throw new NotImplementedException();
        }

        public int Insert(TaskActivityFeedBack obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<TaskActivityFeedBack> ListQuery(Expression<Func<TaskActivityFeedBack, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(TaskActivityFeedBack obj)
        {
            throw new NotImplementedException();
        }
    }
}
