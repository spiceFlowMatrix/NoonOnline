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
    public class EFDiscountBusinessRepository : IGenericRepository<Discount>
    {
        private readonly EFGenericRepository<Discount> _context;

        public EFDiscountBusinessRepository
(
    EFGenericRepository<Discount> context
)
        {
            _context = context;
        }

        public int Delete(Discount obj)
        {
            return _context.Delete(obj, obj.DeleterUserId.ToString());
        }

        public List<Discount> GetAll()
        {
            return _context.GetAll();
        }

        public List<Discount> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public Discount GetById(Expression<Func<Discount, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(Discount obj)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Discount> ListQuery(Expression<Func<Discount, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(Discount obj)
        {
            return _context.Update(obj);
        }
    }
}
