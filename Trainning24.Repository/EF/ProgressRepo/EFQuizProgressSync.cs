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
    public class EFQuizProgressSync : IGenericRepository<QuizProgress>
    {
        private readonly EFGenericRepository<QuizProgress> _context;

        public EFQuizProgressSync(
            EFGenericRepository<QuizProgress> context
            )
        {
            _context = context;
        }
        public int Delete(QuizProgress obj)
        {
            throw new NotImplementedException();
        }

        public List<QuizProgress> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<QuizProgress> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public QuizProgress GetById(Expression<Func<QuizProgress, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(QuizProgress obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<QuizProgress> ListQuery(Expression<Func<QuizProgress, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(QuizProgress obj)
        {
            return _context.Update(obj);
        }
        public async Task<int> SaveAsyncBulk(List<QuizProgress> obj)
        {
            return await _context.SaveAsyncBulk(obj);
        }

        public async Task<int> UpdateAsyncBulk(List<QuizProgress> obj)
        {
            return await _context.UpdateAsyncBulk(obj);
        }
    }
}
