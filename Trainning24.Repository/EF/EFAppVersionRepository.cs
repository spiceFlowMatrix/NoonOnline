using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFAppVersionRepository : IGenericRepository<AppVersion>
    {
        private readonly EFGenericRepository<AppVersion> _context;

        public EFAppVersionRepository(
       EFGenericRepository<AppVersion> context
       )
        {
            _context = context;
        }
        public int Delete(AppVersion obj)
        {
            return _context.Delete(obj);
        }

        public List<AppVersion> GetAll()
        {
            return _context.GetAll();
        }

        public List<AppVersion> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public AppVersion GetById(System.Linq.Expressions.Expression<Func<AppVersion, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(AppVersion obj)
        {
            return _context.Insert(obj);
        }

        public System.Linq.IQueryable<AppVersion> ListQuery(System.Linq.Expressions.Expression<Func<AppVersion, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(AppVersion obj)
        {
            return _context.Update(obj);
        }
    }
}
