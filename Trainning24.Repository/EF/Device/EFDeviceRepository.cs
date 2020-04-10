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
            return _context.Delete(obj);
        }

        public List<Devices> GetAll()
        {
            return _context.GetAll();
        }

        public List<Devices> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public Devices GetById(Expression<Func<Devices, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(Devices obj)
        {
            return _context.Insert(obj);
        }

        public List<Devices> ListQuery(Expression<Func<Devices, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(Devices obj)
        {
            return _context.Update(obj);
        }
    }
}
