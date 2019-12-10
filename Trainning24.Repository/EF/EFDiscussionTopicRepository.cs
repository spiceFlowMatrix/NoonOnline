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
    public class EFDiscussionTopicRepository : IGenericRepository<DiscussionTopic>
    {
        private readonly EFGenericRepository<DiscussionTopic> _context;
        public EFDiscussionTopicRepository
        (
            EFGenericRepository<DiscussionTopic> context
        )
        {
            _context = context;
        }

        public int Delete(DiscussionTopic obj)
        {
            throw new NotImplementedException();
        }

        public List<DiscussionTopic> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<DiscussionTopic> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public DiscussionTopic GetById(Expression<Func<DiscussionTopic, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(DiscussionTopic obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<DiscussionTopic> ListQuery(Expression<Func<DiscussionTopic, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(DiscussionTopic obj)
        {
            _context.Update(obj);
            return Save();
        }
    }
}
