using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFTermsAndConditionRepo : IGenericRepository<TermsAndConditions>
    {
        private readonly EFGenericRepository<TermsAndConditions> _context;

        public EFTermsAndConditionRepo(
            EFGenericRepository<TermsAndConditions> context
            )
        {
            _context = context;
        }
        public int Delete(TermsAndConditions obj)
        {
            throw new NotImplementedException();
        }

        public List<TermsAndConditions> GetAll()
        {
            return _context.GetAll();
        }

        public List<TermsAndConditions> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public TermsAndConditions GetById(Expression<Func<TermsAndConditions, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(TermsAndConditions obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<TermsAndConditions> ListQuery(Expression<Func<TermsAndConditions, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(TermsAndConditions obj)
        {
            return _context.Update(obj);
        }
    }
}
