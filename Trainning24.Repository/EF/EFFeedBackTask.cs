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
    public class EFFeedBackTask : IGenericRepository<FeedBackTask>
    {
        private readonly EFGenericRepository<FeedBackTask> _context;

        public EFFeedBackTask
        (
            EFGenericRepository<FeedBackTask> context
        )
        {
            _context = context;
        }


        public int Delete(FeedBackTask obj)
        {
            return _context.Delete(obj, obj.DeleterUserId.ToString());
        }

        public List<FeedBackTask> GetAll()
        {
            return _context.GetAll();
        }

        public List<FeedBackTask> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public FeedBackTask GetById(Expression<Func<FeedBackTask, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(FeedBackTask obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<FeedBackTask> ListQuery(Expression<Func<FeedBackTask, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(FeedBackTask obj)
        {
            return _context.Update(obj);
        }
    }



    public class EFFeedBackTaskStatus : IGenericRepository<FeedBackTaskStatus>
    {
        private readonly EFGenericRepository<FeedBackTaskStatus> _context;

        public EFFeedBackTaskStatus
        (
            EFGenericRepository<FeedBackTaskStatus> context
        )
        {
            _context = context;
        }


        public int Delete(FeedBackTaskStatus obj)
        {
            return _context.Delete(obj, obj.DeleterUserId.ToString());
        }

        public List<FeedBackTaskStatus> GetAll()
        {
            return _context.GetAll();
        }

        public List<FeedBackTaskStatus> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public FeedBackTaskStatus GetById(Expression<Func<FeedBackTaskStatus, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(FeedBackTaskStatus obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<FeedBackTaskStatus> ListQuery(Expression<Func<FeedBackTaskStatus, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(FeedBackTaskStatus obj)
        {
            return _context.Update(obj);
        }
    }



    public class EFFeedBackTaskStatusOption : IGenericRepository<FeedBackTaskStatusOption>
    {
        private readonly EFGenericRepository<FeedBackTaskStatusOption> _context;

        public EFFeedBackTaskStatusOption
        (
            EFGenericRepository<FeedBackTaskStatusOption> context
        )
        {
            _context = context;
        }


        public int Delete(FeedBackTaskStatusOption obj)
        {
            return _context.Delete(obj, obj.DeleterUserId.ToString());
        }

        public List<FeedBackTaskStatusOption> GetAll()
        {
            return _context.GetAll();
        }

        public List<FeedBackTaskStatusOption> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public FeedBackTaskStatusOption GetById(Expression<Func<FeedBackTaskStatusOption, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(FeedBackTaskStatusOption obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<FeedBackTaskStatusOption> ListQuery(Expression<Func<FeedBackTaskStatusOption, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(FeedBackTaskStatusOption obj)
        {
            return _context.Update(obj);
        }
    }
}
