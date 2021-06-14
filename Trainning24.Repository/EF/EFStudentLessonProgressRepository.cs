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
    public class EFStudentCourseProgressRepository : IGenericRepository<StudentCourseProgress>
    {
        private readonly EFGenericRepository<StudentCourseProgress> _context;

        public EFStudentCourseProgressRepository(
            EFGenericRepository<StudentCourseProgress> context
            ) {
            _context = context;
        }

        public int Insert(StudentCourseProgress obj)
        {            
            return _context.Insert(obj);
        }
        public int Delete(StudentCourseProgress obj)
        {
            return _context.Delete(obj);
        }
        public StudentCourseProgress GetById(Expression<Func<StudentCourseProgress, bool>> ex)
        {
            return _context.GetById(ex);
        }
        public List<StudentCourseProgress> GetAll()
        {
            return _context.GetAll();
        }
        public int Update(StudentCourseProgress obj)
        {
            return _context.Update(obj);        
        }
        public int Save()
        {
            return _context.Save();
        }
        public IQueryable<StudentCourseProgress> ListQuery(Expression<Func<StudentCourseProgress, bool>> where)
        {
            return _context.ListQuery(where);
        }
        public List<StudentCourseProgress> GetAllActive()
        {
            throw new NotImplementedException();
        }
    }
}
