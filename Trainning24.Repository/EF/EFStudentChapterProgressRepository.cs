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
    public class EFStudentChapterProgressRepository : IGenericRepository<StudentChapterProgress>
    {

        private readonly EFGenericRepository<StudentChapterProgress> _context;

        public EFStudentChapterProgressRepository(
            EFGenericRepository<StudentChapterProgress> context
            )
        {
            _context = context;
        }


        public int Insert(StudentChapterProgress obj)
        {
            return _context.Insert(obj);
        }
        public int Delete(StudentChapterProgress obj)
        {
            return _context.Delete(obj);
        }
        public StudentChapterProgress GetById(Expression<Func<StudentChapterProgress, bool>> ex)
        {
            return _context.GetById(ex);
        }
        public List<StudentChapterProgress> GetAll()
        {
            return _context.GetAll();
        }
        public int Update(StudentChapterProgress obj)
        {
            return _context.Update(obj);
        }
        public int Save()
        {
            return _context.Save();
        }
        public IQueryable<StudentChapterProgress> ListQuery(Expression<Func<StudentChapterProgress, bool>> where)
        {
            return _context.ListQuery(where);
        }
        public List<StudentChapterProgress> GetAllActive()
        {
            throw new NotImplementedException();
        }
    }
}
