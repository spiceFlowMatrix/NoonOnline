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
    public class EFLogObjectRepository : IGenericRepository<LogObject>
    {
        private readonly EFGenericRepository<LogObject> _context;

        public EFLogObjectRepository
        (
            EFGenericRepository<LogObject> context
        )
        {
            _context = context;
        }

        public int Delete(LogObject obj)
        {            
            return _context.Delete(obj,obj.ActionUserId.ToString());
        }

        public List<LogObject> GetAll()
        {
            return _context.GetAll();
        }

        public List<LogObject> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public LogObject GetById(Expression<Func<LogObject, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(LogObject obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<LogObject> ListQuery(Expression<Func<LogObject, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(LogObject obj)
        {            
            return _context.Update(obj);
        }
    }
}
