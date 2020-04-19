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
            return _context.Delete(obj);
        }

        public List<DeviceTags> GetAll()
        {
            return _context.GetAll();
        }

        public List<DeviceTags> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public DeviceTags GetById(Expression<Func<DeviceTags, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(DeviceTags obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<DeviceTags> ListQuery(Expression<Func<DeviceTags, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(DeviceTags obj)
        {
            return _context.Update(obj);
        }
    }
}
