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
    public class EFCourseGradeRepository : IGenericRepository<CourseGrade>
    {
        private readonly EFGenericRepository<CourseGrade> _context;

        public EFCourseGradeRepository
        (
            EFGenericRepository<CourseGrade> context
        )
        {
            _context = context;
        }

        public int Delete(CourseGrade obj)
        {
            return _context.Delete(obj, obj.DeleterUserId.ToString());
        }

        public List<CourseGrade> GetAll()
        {
            return _context.GetAll();
        }

        public List<CourseGrade> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public CourseGrade GetById(Expression<Func<CourseGrade, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(CourseGrade obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<CourseGrade> ListQuery(Expression<Func<CourseGrade, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(CourseGrade obj)
        {
            return _context.Update(obj);
        }
    }
}
