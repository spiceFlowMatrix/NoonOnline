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
    public class EFDeviceTagsRepository : IGenericRepository<DeviceTags>
    {
        private readonly EFGenericRepository<DeviceTags> _context;

        public EFDeviceTagsRepository(
            EFGenericRepository<DeviceTags> context
            )
        {
            _context = context;
        }

        public int Delete(DeviceTags obj)
        {
            throw new NotImplementedException();
        }

        public List<DeviceTags> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<DeviceTags> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public DeviceTags GetById(Expression<Func<DeviceTags, bool>> ex)
        {
            throw new NotImplementedException();
        }

        public int Insert(DeviceTags obj)
        {
            throw new NotImplementedException();
        }

        public IQueryable<DeviceTags> ListQuery(Expression<Func<DeviceTags, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(DeviceTags obj)
        {
            throw new NotImplementedException();
        }
    }
}
