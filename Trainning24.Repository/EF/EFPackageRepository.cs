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
    public class EFPackageRepository : IGenericRepository<Package>
    {
        private readonly EFGenericRepository<Package> _context;

        public EFPackageRepository
        (
            EFGenericRepository<Package> context
        )
        {
            _context = context;
        }

        public int Delete(Package obj)
        {            
            return _context.Delete(obj,obj.DeleterUserId.ToString());
        }

        public List<Package> GetAll()
        {
            return _context.GetAll();
        }

        public List<Package> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public Package GetById(Expression<Func<Package, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(Package obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<Package> ListQuery(Expression<Func<Package, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(Package obj)
        {            
            return _context.Update(obj);
        }
    }
}
