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
    public class EFDeviceQuotaExtensionRequestRepository : IGenericRepository<DeviceQuotaExtensionRequest>
    {
        private readonly EFGenericRepository<DeviceQuotaExtensionRequest> _context;

        public EFDeviceQuotaExtensionRequestRepository(
            EFGenericRepository<DeviceQuotaExtensionRequest> context
            )
        {
            _context = context;
        }

        public int Delete(DeviceQuotaExtensionRequest obj)
        {
            return _context.Delete(obj);
        }

        public List<DeviceQuotaExtensionRequest> GetAll()
        {
            return _context.GetAll();
        }

        public List<DeviceQuotaExtensionRequest> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public DeviceQuotaExtensionRequest GetById(Expression<Func<DeviceQuotaExtensionRequest, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(DeviceQuotaExtensionRequest obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<DeviceQuotaExtensionRequest> ListQuery(Expression<Func<DeviceQuotaExtensionRequest, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(DeviceQuotaExtensionRequest obj)
        {
            return _context.Update(obj);
        }
    }
}
