using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Linq.Expressions;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Repository.EF.Generics;
using Trainning24.Domain.Entity;

namespace Trainning24.Repository.EF
{
    public class EFManagementInfoRepository : IGenericRepository<ManagementInfo>
    {
        private readonly EFGenericRepository<ManagementInfo> _context;
        public EFManagementInfoRepository
        (
            EFGenericRepository<ManagementInfo> context
        )
        {
            _context = context;
        }

        public int Delete(ManagementInfo obj)
        {
            throw new NotImplementedException();
        }

        public List<ManagementInfo> GetAll()
        {
            return _context.GetAll();
        }

        public ManagementInfo GetSingle()
        {
            return _context.GetAll().FirstOrDefault();
        }

        public List<ManagementInfo> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public ManagementInfo GetById(Expression<Func<ManagementInfo, bool>> ex)
        {
            throw new NotImplementedException();
        }

        public int Insert(ManagementInfo obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<ManagementInfo> ListQuery(Expression<Func<ManagementInfo, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(ManagementInfo obj)
        {
            return _context.Update(obj);
        }
    }
}
