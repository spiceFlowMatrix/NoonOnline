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
    public class EFSchoolRepository : IGenericRepository<School>
    {
        private readonly EFGenericRepository<School> _context;

        public EFSchoolRepository
        (
            EFGenericRepository<School> context
        )
        {
            _context = context;
        }

        public int Delete(School obj)
        {
            return _context.Delete(obj, obj.DeleterUserId.ToString());
        }

        public List<School> GetAll()
        {
            return _context.GetAll();
        }

        public List<School> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public School GetById(Expression<Func<School, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(School obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<School> ListQuery(Expression<Func<School, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(School obj)
        {
            return _context.Update(obj);
        }
    }
}
