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
    public class EFErpAccountsRepository : IGenericRepository<ERPAccounts>
    {
        private readonly EFGenericRepository<ERPAccounts> _context;

        public EFErpAccountsRepository 
        (
            EFGenericRepository<ERPAccounts> context
        )
        {
            _context = context;
        }

        public int Delete(ERPAccounts obj)
        {            
            return _context.Delete(obj,obj.DeleterUserId.ToString());
        }

        public List<ERPAccounts> GetAll()
        {
            return _context.GetAll();
        }

        public List<ERPAccounts> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public ERPAccounts GetById(Expression<Func<ERPAccounts, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public ERPAccounts Get()
        {
            return _context.GetAll().FirstOrDefault();
        }

        public int Insert(ERPAccounts obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<ERPAccounts> ListQuery(Expression<Func<ERPAccounts, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(ERPAccounts obj)
        {            
            return _context.Update(obj);
        }
    }
}
