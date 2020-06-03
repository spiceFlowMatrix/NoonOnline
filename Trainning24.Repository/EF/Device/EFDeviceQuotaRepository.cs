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
    public class EFDeviceQuotaRepository : IGenericRepository<DeviceQuotas>
    {
        private readonly EFGenericRepository<DeviceQuotas> _context;

        public EFDeviceQuotaRepository(
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

        public DeviceQuotas GetById(Expression<Func<DeviceQuotas, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(DeviceQuotas obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<DeviceQuotas> ListQuery(Expression<Func<DeviceQuotas, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(DeviceQuotas obj)
        {
            return _context.Update(obj);
        }
    }
}
