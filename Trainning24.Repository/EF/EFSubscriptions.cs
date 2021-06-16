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
    public class EFSubscriptions : IGenericRepository<Subscriptions>
    {
        private readonly EFGenericRepository<Subscriptions> _context;

        public EFSubscriptions
        (
            EFGenericRepository<Subscriptions> context
        )
        {
            _context = context;
        }

        public int Delete(Subscriptions obj)
        {
            return _context.Delete(obj);
        }

        public List<Subscriptions> GetAll()
        {
            return _context.GetAll();
        }

        public List<Subscriptions> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public Subscriptions GetById(Expression<Func<Subscriptions, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(Subscriptions obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<Subscriptions> ListQuery(Expression<Func<Subscriptions, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(Subscriptions obj)
        {
            return _context.Update(obj);
        }


        public Subscriptions GetSubscriptionsByMetadataId(long id)
        {
            return _context.ListQuery(b => b.SubscriptionMetadataId == id).SingleOrDefault();
        }
    }
}
