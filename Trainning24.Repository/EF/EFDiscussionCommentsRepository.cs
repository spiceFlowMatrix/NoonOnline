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
    public class EFDiscussionCommentsRepository : IGenericRepository<DiscussionComments>
    {
        private readonly EFGenericRepository<DiscussionComments> _context;
        public EFDiscussionCommentsRepository
        (
            EFGenericRepository<DiscussionComments> context
        )
        {
            _context = context;
        }

        public int Delete(DiscussionComments obj)
        {
            throw new NotImplementedException();
        }

        public List<DiscussionComments> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<DiscussionComments> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public DiscussionComments GetById(Expression<Func<DiscussionComments, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(DiscussionComments obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<DiscussionComments> ListQuery(Expression<Func<DiscussionComments, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(DiscussionComments obj)
        {
            _context.Update(obj);
            return Save();
        }
    }
}
