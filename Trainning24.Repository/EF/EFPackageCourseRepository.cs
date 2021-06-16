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
    public class EFPackageCourseRepository : IGenericRepository<PackageCourse>
    {
        private readonly EFGenericRepository<PackageCourse> _context;

        public EFPackageCourseRepository
        (
            EFGenericRepository<PackageCourse> context
        )
        {
            _context = context;
        }

        public int Delete(PackageCourse obj)
        {            
            return _context.Delete(obj,obj.DeleterUserId.ToString());
        }

        public List<PackageCourse> GetAll()
        {
            return _context.GetAll();
        }

        public List<PackageCourse> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public PackageCourse GetById(Expression<Func<PackageCourse, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(PackageCourse obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<PackageCourse> ListQuery(Expression<Func<PackageCourse, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(PackageCourse obj)
        {            
            return _context.Update(obj);
        }
    }
}
