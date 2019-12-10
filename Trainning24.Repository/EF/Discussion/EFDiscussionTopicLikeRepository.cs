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
    public class EFDiscussionTopicLikeRepository : IGenericRepository<DiscussionTopicLikes>
    {
        private readonly EFGenericRepository<DiscussionTopicLikes> _context;
        public EFDiscussionTopicLikeRepository
        (
            EFGenericRepository<DiscussionTopicLikes> context
        )
        {
            _context = context;
        }

        public int Delete(DiscussionTopicLikes obj)
        {
            throw new NotImplementedException();
        }

        public List<DiscussionTopicLikes> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<DiscussionTopicLikes> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public DiscussionTopicLikes GetById(Expression<Func<DiscussionTopicLikes, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(DiscussionTopicLikes obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<DiscussionTopicLikes> ListQuery(Expression<Func<DiscussionTopicLikes, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(DiscussionTopicLikes obj)
        {
            _context.Update(obj);
            return Save();
        }
    }
}
