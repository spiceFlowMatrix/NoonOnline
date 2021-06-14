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
    public class EFStudentFileProgressRepository : IGenericRepository<StudentFileProgress>
    {
        private readonly EFGenericRepository<StudentFileProgress> _context;

        public EFStudentFileProgressRepository(
            EFGenericRepository<StudentFileProgress> context
            )
        {
            _context = context;
        }

        public int Insert(StudentFileProgress obj)
        {
            return _context.Insert(obj);
        }
        public int Delete(StudentFileProgress obj)
        {
            return _context.Delete(obj);
        }
        public StudentFileProgress GetById(Expression<Func<StudentFileProgress, bool>> ex)
        {
            return _context.GetById(ex);
        }
        public List<StudentFileProgress> GetAll()
        {
            return _context.GetAll();
        }
        public int Update(StudentFileProgress obj)
        {
            return _context.Update(obj);
        }
        public int Save()
        {
            return _context.Save();
        }
        public IQueryable<StudentFileProgress> ListQuery(Expression<Func<StudentFileProgress, bool>> where)
        {
            return _context.ListQuery(where);
        }
        public List<StudentFileProgress> GetAllActive()
        {
            throw new NotImplementedException();
        }
    }
}
