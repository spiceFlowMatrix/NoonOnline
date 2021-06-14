using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF.Device
{
    public class EFDeviceOSRepository : IGenericRepository<DeviceOS>
    {
        private readonly EFGenericRepository<DeviceOS> _context;
        public EFDeviceOSRepository
        (
            EFGenericRepository<DeviceOS> context
        )
        {
            _context = context;
        }

        public int Delete(DeviceOS obj)
        {
            return _context.Delete(obj);
        }

        public List<DeviceOS> GetAll()
        {
            return _context.GetAll();
        }

        public List<DeviceOS> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public DeviceOS GetById(Expression<Func<DeviceOS, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(DeviceOS obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<DeviceOS> ListQuery(Expression<Func<DeviceOS, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(DeviceOS obj)
        {
            return _context.Update(obj);
        }
    }
}
