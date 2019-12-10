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
    public class EFBundleCourseCourseRepository : IGenericRepository<BundleCourse>
    {
        private readonly EFGenericRepository<BundleCourse> _context;
        //private static Training24Context _updateContext;

        public EFBundleCourseCourseRepository
        (
            EFGenericRepository<BundleCourse> context
            //Training24Context training24Context
        )
        {
            //_updateContext = training24Context;
            _context = context;
        }

        public int Delete(BundleCourse obj)
        {
            return _context.Delete(obj,obj.Id.ToString());
        }

        public List<BundleCourse> GetAll()
        {
            return _context.GetAll();
        }

        public List<BundleCourse> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public BundleCourse GetById(Expression<Func<BundleCourse, bool>> ex)
        {
            throw new NotImplementedException();
        }

        public int Insert(BundleCourse obj)
        {
            return _context.Insert(obj);
        }


        public BundleCourse GetById(int Id)
        {
            return _context.GetById(b => b.Id == Id);
        }

        public int Update(BundleCourse obj)
        {            
            BundleCourse BundleCourse = _context.GetById(b => b.Id == obj.Id);
            BundleCourse.BundleId = obj.BundleId;
            BundleCourse.CourseId = obj.CourseId;
            BundleCourse.LastModificationTime = DateTime.Now.ToString();
            BundleCourse.LastModifierUserId = obj.LastModifierUserId;
            return _context.Update(BundleCourse);
        }

        public IQueryable<BundleCourse> ListQuery(Expression<Func<BundleCourse, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public List<BundleCourse> ListQuery1(Expression<Func<BundleCourse, bool>> where)
        {
            return _context.ListQuery(where).ToList();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

    }
}
