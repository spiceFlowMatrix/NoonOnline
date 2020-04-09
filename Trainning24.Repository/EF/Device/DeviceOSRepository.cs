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
    public class DeviceOSRepository : IGenericRepository<DeviceOS>
    {
        private readonly EFGenericRepository<DeviceOS> _context;
        public DeviceOSRepository
        (
            EFGenericRepository<DeviceOS> context
        )
        {
            _context = context;
        }

        public int Delete(DeviceOS obj)
        {
            throw new NotImplementedException();
        }

        public List<DeviceOS> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<DeviceOS> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public DeviceOS GetById(Expression<Func<DeviceOS, bool>> ex)
        {
            throw new NotImplementedException();
        }

        public int Insert(DeviceOS obj)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DeviceOS> ListQuery(Expression<Func<DeviceOS, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(DeviceOS obj)
        {
            throw new NotImplementedException();
        }
    }
}
