using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFUserSessionsRepository
    {
        private readonly EFGenericRepository<UserSessions> _context;
        //private static Training24Context _updateContext;


        public EFUserSessionsRepository
        (
            EFGenericRepository<UserSessions> context
        //Training24Context training24Context
        )
        {
            _context = context;
            //_updateContext = training24Context;
        }

        public int Insert(UserSessions obj)
        {
            return _context.Insert(obj);
        }

        public void Update(UserSessions obj)
        {
            UserSessions uSession = _context.GetById(b => b.Id == obj.Id);
            if (uSession != null)
            {
                uSession.AccessToken = obj.AccessToken;
                uSession.LastModificationTime = DateTime.Now.ToString();
                uSession.LastModifierUserId = 1;

                _context.Update(uSession);
            }
        }

        public UserSessions GetUserSessionByAccessToken(string token)
        {
            return _context.GetById(f => f.AccessToken == token && f.IsDeleted == false);
        }

        public void DeleteSession(string token, string uId)
        {
            UserSessions obj = _context.GetById(f => f.AccessToken == token && f.IsDeleted == false);
            if (obj != null)
            {
                obj.IsDeleted = true;
                obj.DeletionTime = DateTime.Now.ToString();
                obj.DeleterUserId = int.Parse(uId);
                _context.Update(obj);
            }
        }

        public IQueryable<UserSessions> GetUserSessions(Expression<Func<UserSessions, bool>> where)
        {
            return _context.ListQuery(where);
        }
    }
}
