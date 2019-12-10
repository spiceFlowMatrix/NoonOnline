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
    public class EFDiscussionCommentLikeRepository : IGenericRepository<DiscussionCommentLikes>
    {
        private readonly EFGenericRepository<DiscussionCommentLikes> _context;
        public EFDiscussionCommentLikeRepository
        (
            EFGenericRepository<DiscussionCommentLikes> context
        )
        {
            _context = context;
        }

        public int Delete(DiscussionCommentLikes obj)
        {
            throw new NotImplementedException();
        }

        public List<DiscussionCommentLikes> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<DiscussionCommentLikes> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public DiscussionCommentLikes GetById(Expression<Func<DiscussionCommentLikes, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(DiscussionCommentLikes obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<DiscussionCommentLikes> ListQuery(Expression<Func<DiscussionCommentLikes, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(DiscussionCommentLikes obj)
        {
            _context.Update(obj);
            return Save();
        }
    }
}
