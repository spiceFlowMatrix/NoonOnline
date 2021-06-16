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
    public class EFSalesTaxRepository : IGenericRepository<SalesTax>
    {
        private readonly EFGenericRepository<SalesTax> _context;

        public EFSalesTaxRepository
        (
            EFGenericRepository<SalesTax> context
        )
        {
            _context = context;
        }

        public int Delete(SalesTax obj)
        {            
            return _context.Delete(obj,obj.DeleterUserId.ToString());
        }

        public List<SalesTax> GetAll()
        {
            return _context.GetAll();
        }

        public List<SalesTax> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public SalesTax GetById(Expression<Func<SalesTax, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public SalesTax Get()
        {
            return _context.GetAll().FirstOrDefault();
        }

        public int Insert(SalesTax obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<SalesTax> ListQuery(Expression<Func<SalesTax, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(SalesTax obj)
        {            
            return _context.Update(obj);
        }
    }
}
