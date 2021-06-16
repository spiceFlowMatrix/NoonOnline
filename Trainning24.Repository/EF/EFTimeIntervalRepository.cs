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
    public class EFTimeIntervalRepository : IGenericRepository<TimeInterval>
    {
        private readonly EFGenericRepository<TimeInterval> _context;

        public EFTimeIntervalRepository
        (
            EFGenericRepository<TimeInterval> context
        )
        {
            _context = context;
        }

        public int Delete(TimeInterval obj)
        {            
            return _context.Delete(obj,obj.DeleterUserId.ToString());
        }

        public List<TimeInterval> GetAll()
        {
            return _context.GetAll();
        }

        public List<TimeInterval> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public TimeInterval GetById(Expression<Func<TimeInterval, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public TimeInterval Get()
        {
            return _context.GetAll().FirstOrDefault();
        }

        public int Insert(TimeInterval obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<TimeInterval> ListQuery(Expression<Func<TimeInterval, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(TimeInterval obj)
        {            
            return _context.Update(obj);
        }
    }
}
