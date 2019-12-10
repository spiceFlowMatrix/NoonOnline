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
    public class EFDeposit : IGenericRepository<Deposit>
    {
        private readonly EFGenericRepository<Deposit> _context;

        public EFDeposit
        (
            EFGenericRepository<Deposit> context
        )
        {
            _context = context;
        }

        public int Delete(Deposit obj)
        {
            return _context.Delete(obj);
        }

        public List<Deposit> GetAll()
        {
            return _context.GetAll();
        }

        public List<Deposit> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public Deposit GetById(Expression<Func<Deposit, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(Deposit obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<Deposit> ListQuery(Expression<Func<Deposit, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(Deposit obj)
        {
            return _context.Update(obj);
        }
    }
}
