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
    public class EFAddtionalServicesRepository : IGenericRepository<AddtionalServices>
    {
        private readonly EFGenericRepository<AddtionalServices> _context;

        public EFAddtionalServicesRepository
        (
            EFGenericRepository<AddtionalServices> context
        )
        {
            _context = context;
        }

        public int Delete(AddtionalServices obj)
        {            
            return _context.Delete(obj,obj.DeleterUserId.ToString());
        }

        public List<AddtionalServices> GetAll()
        {
            return _context.GetAll();
        }

        public List<AddtionalServices> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public AddtionalServices GetById(Expression<Func<AddtionalServices, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(AddtionalServices obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<AddtionalServices> ListQuery(Expression<Func<AddtionalServices, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(AddtionalServices obj)
        {            
            return _context.Update(obj);
        }
    }
}
