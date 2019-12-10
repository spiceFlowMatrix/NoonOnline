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
    public class EFStudentCourseRepository : IGenericRepository<UserCourse>
    {
        private readonly EFGenericRepository<UserCourse> _context;        

        public EFStudentCourseRepository
        (
            EFGenericRepository<UserCourse> context
        )
        {            
            _context = context;
        }

        public int Delete(long id,UserCourse obj)
        {
            return _context.Delete(obj,id.ToString());
        }

        public int Delete(UserCourse obj)
        {
            throw new NotImplementedException();
        }

        public List<UserCourse> GetAll()
        {
            return _context.GetAll();
        }

        public List<UserCourse> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public UserCourse GetById(Expression<Func<UserCourse, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public UserCourse GetById(int id)
        {
            return _context.GetById(id);
        }

        public int Insert(UserCourse obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<UserCourse> ListQuery(Expression<Func<UserCourse, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(UserCourse obj)
        {
            return _context.Update(obj);
        }
    }
}
