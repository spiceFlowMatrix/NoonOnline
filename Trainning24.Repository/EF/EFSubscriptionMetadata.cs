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
    public class EFSubscriptionMetadata : IGenericRepository<SubscriptionMetadata>
    {
        private readonly EFGenericRepository<SubscriptionMetadata> _context;

        public EFSubscriptionMetadata
        (
            EFGenericRepository<SubscriptionMetadata> context
        )
        {
            _context = context;
        }

        public int Delete(SubscriptionMetadata obj)
        {
            return _context.Delete(obj);
        }

        public List<SubscriptionMetadata> GetAll()
        {
            return _context.GetAll().OrderByDescending(b => b.Id).ToList();
        }

        public List<SubscriptionMetadata> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public SubscriptionMetadata GetById(Expression<Func<SubscriptionMetadata, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(SubscriptionMetadata obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<SubscriptionMetadata> ListQuery(Expression<Func<SubscriptionMetadata, bool>> where)
        {
            return _context.ListQuery(where);
        }


        public async Task<List<SubscriptionMetadata>> SubscriptionMetaDataListAsync(Expression<Func<SubscriptionMetadata, bool>> where)
        {
            return await _context.FindByConditionAsync(where);
        }

        public List<SubscriptionMetadata> SubscriptionMetaDataList(Expression<Func<SubscriptionMetadata, bool>> where)
        {
            return _context.ListQuery(where).ToList();
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(SubscriptionMetadata obj)
        {
            return _context.Update(obj);
        }

        public List<SubscriptionMetadata> GetPurchaseList(string search)
        {
            return _context.ListQuery(b => b.Status.Any(k => b.Status.ToLower().Contains(search.ToLower())) ||
            b.PurchageId.Any(k => b.PurchageId.ToLower().Contains(search.ToLower()))).ToList();
        }

        public async Task<List<SubscriptionMetadata>> GetAllAsync()
        {
            return await _context.GetAllAsync();
        }
        public async Task<List<SubscriptionMetadata>> GetByAgentIdAsync(Expression<Func<SubscriptionMetadata, bool>> where)
        {
            return await _context.GetByIdAsync(where);
        }
    }
}
