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
    public class EFGradeRepository : IGenericRepository<Grade>
    {
        private readonly EFGenericRepository<Grade> _context;

        public EFGradeRepository
        (
            EFGenericRepository<Grade> context
        )
        {
            _context = context;
        }

        public int Delete(Grade obj)
        {            
            return _context.Delete(obj,obj.DeleterUserId.ToString());
        }

        public List<Grade> GetAll()
        {
            return _context.GetAll();
        }

        public List<Grade> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public Grade GetById(Expression<Func<Grade, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(Grade obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<Grade> ListQuery(Expression<Func<Grade, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(Grade obj)
        {            
            return _context.Update(obj);
        }
    }
}
