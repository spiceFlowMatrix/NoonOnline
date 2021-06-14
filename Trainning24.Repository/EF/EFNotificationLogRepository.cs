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
    public class EFNotificationLogRepository : IGenericRepository<NotificationLog>
    {
        private readonly EFGenericRepository<NotificationLog> _context;

        public EFNotificationLogRepository
        (
            EFGenericRepository<NotificationLog> context
        )
        {
            _context = context;
        }

        public int Delete(NotificationLog obj)
        {            
            return _context.Delete(obj,obj.DeleterUserId.ToString());
        }

        public List<NotificationLog> GetAll()
        {
            return _context.GetAll();
        }

        public List<NotificationLog> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public NotificationLog GetById(Expression<Func<NotificationLog, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(NotificationLog obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<NotificationLog> ListQuery(Expression<Func<NotificationLog, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(NotificationLog obj)
        {            
            return _context.Update(obj);
        }
    }
}
