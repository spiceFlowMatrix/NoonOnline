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
    public class EFDeviceRepository : IGenericRepository<Devices>
    {
        private readonly EFGenericRepository<Devices> _context;

        public EFDeviceRepository(
            EFGenericRepository<Devices> context
            )
        {
            _context = context;
        }


        public int Delete(Devices obj)
        {
            throw new NotImplementedException();
        }

        public List<Devices> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Devices> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public Devices GetById(Expression<Func<Devices, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(Devices obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<Devices> ListQuery(Expression<Func<Devices, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(Devices obj)
        {
            throw new NotImplementedException();
        }
    }
}
