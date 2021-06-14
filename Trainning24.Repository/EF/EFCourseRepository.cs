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
    public class EFCourseRepository : IGenericRepository<Course>
    {
        private readonly EFGenericRepository<Course> _context;
        //private static Training24Context _updateContext;

        public EFCourseRepository
        (
            EFGenericRepository<Course> context
            //Training24Context training24Context
        )
        {
            //_updateContext = training24Context;
            _context = context;
        }

        public int Delete(Course obj)
        {
            return _context.Delete(obj, obj.Id.ToString());
        }

        public List<Course> GetAll()
        {
            return _context.GetAll();
        }

        public List<Course> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public Course GetById(Expression<Func<Course, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(Course obj)
        {
            return _context.Insert(obj);
        }


        public Course GetById(long Id)
        {
            return _context.GetById(b => b.Id == Id && b.IsDeleted != true);
        }

        public int Update(Course obj)
        {
            Course Course = _context.GetById(b => b.Id == obj.Id);
            Course.Name = obj.Name;
            Course.Code = obj.Code;
            Course.Description = obj.Description;            
            Course.PassMark = obj.PassMark;
            Course.istrial = obj.istrial;

            if (!string.IsNullOrEmpty(obj.Image))
                Course.Image = obj.Image;

            return _context.Update(Course);
        }

        public IQueryable<Course> ListQuery(Expression<Func<Course, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }
    }
}
