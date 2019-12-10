using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFChapterQuizRepository : IGenericRepository<ChapterQuiz>
    {
        private readonly EFGenericRepository<ChapterQuiz> _context;
        public EFChapterQuizRepository
        (
            EFGenericRepository<ChapterQuiz> context
        )
        {
            _context = context;
        }
        public int Delete(ChapterQuiz obj)
        {
            throw new NotImplementedException();
        }

        public List<ChapterQuiz> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<ChapterQuiz> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public ChapterQuiz GetById(Expression<Func<ChapterQuiz, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(ChapterQuiz obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<ChapterQuiz> ListQuery(Expression<Func<ChapterQuiz, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(ChapterQuiz obj)
        {
            return _context.Update(obj);
        }

        public ChapterQuiz GetChaperQuizByQuizId(long id)
        {
            return _context.GetById(b => b.QuizId == id);
        }
    }
}
