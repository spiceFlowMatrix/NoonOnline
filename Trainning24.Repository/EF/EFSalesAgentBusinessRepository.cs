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
    public class EFSalesAgentBusinessRepository : IGenericRepository<SalesAgent>
    {
        private readonly EFGenericRepository<SalesAgent> _context;
        private readonly EFGenericRepository<Currency> _contextCurrency;

        public EFSalesAgentBusinessRepository
(
    EFGenericRepository<SalesAgent> context,
    EFGenericRepository<Currency> contextcurrency
)
        {
            _context = context;
            _contextCurrency = contextcurrency;
        }



        public int InsertCurrency(Currency obj)
        {
            return _contextCurrency.Insert(obj);
        }


        public int Delete(SalesAgent obj)
        {
            return _context.Delete(obj, obj.DeleterUserId.ToString());
        }

        public List<SalesAgent> GetAll()
        {
            return _context.GetAll();
        }

        public List<Currency> getAllCurrency()
        {
            return _contextCurrency.GetAll();
        }

        public List<SalesAgent> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public SalesAgent GetById(Expression<Func<SalesAgent, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(SalesAgent obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<SalesAgent> ListQuery(Expression<Func<SalesAgent, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(SalesAgent obj)
        {
            return _context.Update(obj);
        }
    }
}
