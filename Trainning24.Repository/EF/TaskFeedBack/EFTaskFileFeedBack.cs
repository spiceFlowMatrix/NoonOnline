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
    public class EFTaskFileFeedback : IGenericRepository<TaskFileFeedBack>
    {
        private readonly EFGenericRepository<TaskFileFeedBack> _context;

        public EFTaskFileFeedback
        (
            EFGenericRepository<TaskFileFeedBack> context 
        )
        {
            _context = context;
        }

        public int Delete(TaskFileFeedBack obj)
        {
            throw new NotImplementedException();
        }

        public List<TaskFileFeedBack> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<TaskFileFeedBack> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public TaskFileFeedBack GetById(Expression<Func<TaskFileFeedBack, bool>> ex)
        {
            throw new NotImplementedException();
        }

        public int Insert(TaskFileFeedBack obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<TaskFileFeedBack> ListQuery(Expression<Func<TaskFileFeedBack, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(TaskFileFeedBack obj)
        {
            throw new NotImplementedException();
        }
        public List<TaskFileFeedBack> GetFeedBackFile(long Id)
        {
            return _context.ListQuery(b => b.TaskId == Id && b.IsDeleted != true).ToList();
        }
    }
}
