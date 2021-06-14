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
    public class EFStudentLessonProgressRepository : IGenericRepository<StudentLessonProgress>
    {

        private readonly EFGenericRepository<StudentLessonProgress> _context;

        public EFStudentLessonProgressRepository(
            EFGenericRepository<StudentLessonProgress> context
            )
        {
            _context = context;
        }


        public int Insert(StudentLessonProgress obj)
        {
            return _context.Insert(obj);
        }
        public int Delete(StudentLessonProgress obj)
        {
            return _context.Delete(obj);
        }
        public StudentLessonProgress GetById(Expression<Func<StudentLessonProgress, bool>> ex)
        {
            return _context.GetById(ex);
        }
        public List<StudentLessonProgress> GetAll()
        {
            return _context.GetAll();
        }
        public int Update(StudentLessonProgress obj)
        {
            return _context.Update(obj);
        }
        public int Save()
        {
            return _context.Save();
        }
        public IQueryable<StudentLessonProgress> ListQuery(Expression<Func<StudentLessonProgress, bool>> where)
        {
            return _context.ListQuery(where);
        }
        public List<StudentLessonProgress> GetAllActive()
        {
            throw new NotImplementedException();
        }
    }
}
