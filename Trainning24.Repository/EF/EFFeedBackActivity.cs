using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFFeedBackActivity : IGenericRepository<FeedBackActivity>
    {

        private readonly EFGenericRepository<FeedBackActivity> _context;

        public EFFeedBackActivity
        (
            EFGenericRepository<FeedBackActivity> context
        )
        {
            _context = context;
        }


        public int Delete(FeedBackActivity obj)
        {
            return _context.Delete(obj, obj.DeleterUserId.ToString());
        }

        public List<FeedBackActivity> GetAll()
        {
            return _context.GetAll();
        }

        public List<FeedBackActivity> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public FeedBackActivity GetById(Expression<Func<FeedBackActivity, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(FeedBackActivity obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<FeedBackActivity> ListQuery(Expression<Func<FeedBackActivity, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(FeedBackActivity obj)
        {
            return _context.Update(obj);
        }
    }
}
