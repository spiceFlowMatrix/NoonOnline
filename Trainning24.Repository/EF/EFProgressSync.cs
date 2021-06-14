using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFProgressSync : IGenericRepository<ProgessSync>
    {
        private readonly EFGenericRepository<ProgessSync> _context;

        public EFProgressSync(
            EFGenericRepository<ProgessSync> context
            )
        {
            _context = context;
        }
        public int Delete(ProgessSync obj)
        {
            throw new NotImplementedException();
        }

        public List<ProgessSync> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<ProgessSync> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public ProgessSync GetById(Expression<Func<ProgessSync, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(ProgessSync obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<ProgessSync> ListQuery(Expression<Func<ProgessSync, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(ProgessSync obj)
        {
            return _context.Update(obj);
        }
        public async Task<int> AddRecordBulk(List<ProgessSync> obj)
        {
            return await _context.SaveAsyncBulk(obj);
        }
        public async Task<int> UpdateAsyncBulk(List<ProgessSync> obj)
        {
            return await _context.UpdateAsyncBulk(obj);
        }
    }


    public class EFQuizTimerSync : IGenericRepository<QuizTimerSync>
    {
        private readonly EFGenericRepository<QuizTimerSync> _context;

        public EFQuizTimerSync(
            EFGenericRepository<QuizTimerSync> context
            )
        {
            _context = context;
        }
        public int Delete(QuizTimerSync obj)
        {
            throw new NotImplementedException();
        }

        public List<QuizTimerSync> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<QuizTimerSync> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public QuizTimerSync GetById(Expression<Func<QuizTimerSync, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(QuizTimerSync obj)
        {
            return _context.Insert(obj);
        }
        public async Task<int> AddRecordBulk(List<QuizTimerSync> obj)
        {
            return await _context.SaveAsyncBulk(obj);
        }
        public async Task<int> UpdateAsyncBulk(List<QuizTimerSync> obj)
        {
            return await _context.UpdateAsyncBulk(obj);
        }
        public IQueryable<QuizTimerSync> ListQuery(Expression<Func<QuizTimerSync, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(QuizTimerSync obj)
        {
            return _context.Update(obj);
        }
    }
}
