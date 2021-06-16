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
    public class EFCourseDefinationRepository : IGenericRepository<CourseDefination>
    {
        private readonly EFGenericRepository<CourseDefination> _context;

        public EFCourseDefinationRepository
(
    EFGenericRepository<CourseDefination> context
)
        {
            _context = context;
        }


        public int Delete(CourseDefination obj)
        {
            return _context.Delete(obj,obj.DeleterUserId.ToString());
        }

        public List<CourseDefination> GetAll()
        {
            return _context.GetAll();
        }

        public List<CourseDefination> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public CourseDefination GetById(Expression<Func<CourseDefination, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(CourseDefination obj)
        {
            throw new NotImplementedException();
        }

        public IQueryable<CourseDefination> ListQuery(Expression<Func<CourseDefination, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(CourseDefination obj)
        {
            return _context.Update(obj);
        }
    }
}
