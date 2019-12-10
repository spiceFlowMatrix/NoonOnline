using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFUserQuizResultSync : IGenericRepository<UserQuizResult>
    {
        private readonly EFGenericRepository<UserQuizResult> _context;

        public EFUserQuizResultSync(
            EFGenericRepository<UserQuizResult> context
            )
        {
            _context = context;
        }
        public int Delete(UserQuizResult obj)
        {
            throw new NotImplementedException();
        }

        public List<UserQuizResult> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<UserQuizResult> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public UserQuizResult GetById(Expression<Func<UserQuizResult, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(UserQuizResult obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<UserQuizResult> ListQuery(Expression<Func<UserQuizResult, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(UserQuizResult obj)
        {
            return _context.Update(obj);
        }
    }
}
