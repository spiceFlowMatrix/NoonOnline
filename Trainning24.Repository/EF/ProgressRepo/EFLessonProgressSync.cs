using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFLessonProgressSync : IGenericRepository<LessonProgress>
    {
        private readonly EFGenericRepository<LessonProgress> _context;

        public EFLessonProgressSync(
            EFGenericRepository<LessonProgress> context
            )
        {
            _context = context;
        }
        public int Delete(LessonProgress obj)
        {
            throw new NotImplementedException();
        }

        public List<LessonProgress> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<LessonProgress> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public LessonProgress GetById(Expression<Func<LessonProgress, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(LessonProgress obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<LessonProgress> ListQuery(Expression<Func<LessonProgress, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(LessonProgress obj)
        {
            return _context.Update(obj);
        }
    }
}
