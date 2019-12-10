using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFStudParentRepository : IGenericRepository<StudParent>
    {
        private readonly EFGenericRepository<StudParent> _context;

        public EFStudParentRepository(
            EFGenericRepository<StudParent> context
            )
        {
            _context = context;
        }
        public int Delete(StudParent obj)
        {
            throw new NotImplementedException();
        }

        public List<StudParent> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<StudParent> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public StudParent GetById(Expression<Func<StudParent, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(StudParent obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<StudParent> ListQuery(Expression<Func<StudParent, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(StudParent obj)
        {
            return _context.Update(obj);
        }
    }
}
