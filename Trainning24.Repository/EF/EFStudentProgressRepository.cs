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
    public class EFStudentProgressRepository : IGenericRepository<StudentProgress>
    {

        private readonly EFGenericRepository<StudentProgress> _context;

        public EFStudentProgressRepository(
            EFGenericRepository<StudentProgress> context
            )
        {
            _context = context;
        }

        public int Insert(StudentProgress obj)
        {
            return _context.Insert(obj);
        }
        public int Delete(StudentProgress obj)
        {
            return _context.Delete(obj);
        }
        public StudentProgress GetById(Expression<Func<StudentProgress, bool>> ex)
        {
            return _context.GetById(ex);
        }
        public List<StudentProgress> GetAll()
        {
            return _context.GetAll();
        }
        public int Update(StudentProgress obj)
        {
            return _context.Update(obj);
        }
        public int Save()
        {
            return _context.Save();
        }
        public IQueryable<StudentProgress> ListQuery(Expression<Func<StudentProgress, bool>> where)
        {
            return _context.ListQuery(where);
        }
        public List<StudentProgress> GetAllActive()
        {
            throw new NotImplementedException();
        }
    }
}
