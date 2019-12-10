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
    public class EFCourseItemProgressSync : IGenericRepository<CourseItemProgressSync>
    {
        private readonly EFGenericRepository<CourseItemProgressSync> _context;

        public EFCourseItemProgressSync(
            EFGenericRepository<CourseItemProgressSync> context
            )
        {
            _context = context;
        }

        public int Delete(CourseItemProgressSync obj)
        {
            throw new NotImplementedException();
        }

        public List<CourseItemProgressSync> GetAll()
        {
            return _context.GetAll();
        }

        public List<CourseItemProgressSync> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public CourseItemProgressSync GetById(Expression<Func<CourseItemProgressSync, bool>> ex)
        {
            throw new NotImplementedException();
        }

        public int Insert(CourseItemProgressSync obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<CourseItemProgressSync> ListQuery(Expression<Func<CourseItemProgressSync, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(CourseItemProgressSync obj)
        {
            throw new NotImplementedException();
        }
    }
}
