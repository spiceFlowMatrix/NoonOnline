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
    public class EFUserNotificationRepository : IGenericRepository<UserNotifications>
    {
        private readonly EFGenericRepository<UserNotifications> _context;
        public EFUserNotificationRepository
        (
            EFGenericRepository<UserNotifications> context
        )
        {
            _context = context;
        }

        public int Delete(UserNotifications obj)
        {
            throw new NotImplementedException();
        }

        public List<UserNotifications> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<UserNotifications> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public UserNotifications GetById(Expression<Func<UserNotifications, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(UserNotifications obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<UserNotifications> ListQuery(Expression<Func<UserNotifications, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(UserNotifications obj)
        {
            _context.Update(obj);
            return Save();
        }
    }
}
