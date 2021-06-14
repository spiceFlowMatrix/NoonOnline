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
    public class EFFeedbackStaffRepository: IGenericRepository<FeedBackStaff>
    {
        private readonly EFGenericRepository<FeedBackStaff> _context;

        public EFFeedbackStaffRepository
        (
            EFGenericRepository<FeedBackStaff> context
        )
        {
            _context = context;
        }


        public int Delete(FeedBackStaff obj)
        {
            return _context.Delete(obj, obj.DeleterUserId.ToString());
        }

        public List<FeedBackStaff> GetAll()
        {
            return _context.GetAll();
        }

        public List<FeedBackStaff> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public FeedBackStaff GetById(Expression<Func<FeedBackStaff, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(FeedBackStaff obj)
        {
            return _context.Insert(obj);
        }

        public async Task<int> InsertAsync(FeedBackStaff obj)
        {
            return await _context.InsertAsync(obj);
        }

        public IQueryable<FeedBackStaff> ListQuery(Expression<Func<FeedBackStaff, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(FeedBackStaff obj)
        {
            return _context.Update(obj);
        }
    }
}
