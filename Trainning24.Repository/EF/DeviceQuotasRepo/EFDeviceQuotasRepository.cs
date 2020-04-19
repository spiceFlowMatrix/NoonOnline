using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF.DeviceQuotasRepo
{
    public class EFDeviceQuotasRepository : IGenericRepository<DeviceQuotas>
    {
        private readonly EFGenericRepository<DeviceQuotas> _context;

        public EFDeviceQuotasRepository
        (
            EFGenericRepository<DeviceQuotas> context
        )
        {
            _context = context;
        }

        public int Delete(DeviceQuotas obj)
        {
            return _context.Delete(obj);
        }

        public List<DeviceQuotas> GetAll()
        {
            return _context.GetAll();
        }

        public List<DeviceQuotas> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public DeviceQuotas GetById(System.Linq.Expressions.Expression<Func<DeviceQuotas, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(DeviceQuotas obj)
        {
            return _context.Insert(obj);
        }

        public System.Linq.IQueryable<DeviceQuotas> ListQuery(System.Linq.Expressions.Expression<Func<DeviceQuotas, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(DeviceQuotas obj)
        {
            return _context.Update(obj);
        }
    }
}
