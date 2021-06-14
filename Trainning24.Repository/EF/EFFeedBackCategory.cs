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
    public class EFFeedBackCategory : IGenericRepository<FeedBackCategory>
    {
        private readonly EFGenericRepository<FeedBackCategory> _context;


        public EFFeedBackCategory
        (
            EFGenericRepository<FeedBackCategory> context
        )
        {
            _context = context;

        }

        public int Delete(FeedBackCategory obj)
        {
            throw new NotImplementedException();
        }

        public List<FeedBackCategory> GetAll()
        {
            return _context.GetAll();
        }

        public List<FeedBackCategory> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public FeedBackCategory GetById(Expression<Func<FeedBackCategory, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(FeedBackCategory obj)
        {
            throw new NotImplementedException();
        }

        public IQueryable<FeedBackCategory> ListQuery(Expression<Func<FeedBackCategory, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(FeedBackCategory obj)
        {
            throw new NotImplementedException();
        }
    }
}
