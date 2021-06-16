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
    public class EFLogsRepository : IGenericRepository<Logs>
    {
        private readonly EFGenericRepository<Logs> _context;

        public EFLogsRepository
        (
            EFGenericRepository<Logs> context
        )
        {
            _context = context;
        }

        public int Delete(Logs obj)
        {            
            return _context.Delete(obj,obj.DeleterUserId.ToString());
        }

        public List<Logs> GetAll()
        {
            return _context.GetAll();
        }

        public List<Logs> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public Logs GetById(Expression<Func<Logs, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(Logs obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<Logs> ListQuery(Expression<Func<Logs, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(Logs obj)
        {            
            return _context.Update(obj);
        }
    }
}
