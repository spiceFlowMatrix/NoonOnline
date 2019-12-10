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
    public class EFAgentCategoryBusinessRepository : IGenericRepository<AgentCategory>
    {
        private readonly EFGenericRepository<AgentCategory> _context;

        public EFAgentCategoryBusinessRepository
        (
            EFGenericRepository<AgentCategory> context
        )
        {
            _context = context;
        }

        public int Delete(AgentCategory obj)
        {
            return _context.Delete(obj, obj.DeleterUserId.ToString());
        }

        public List<AgentCategory> GetAll()
        {
            return _context.GetAll();
        }

        public List<AgentCategory> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public AgentCategory GetById(Expression<Func<AgentCategory, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(AgentCategory obj)
        {
            throw new NotImplementedException();
        }

        public IQueryable<AgentCategory> ListQuery(Expression<Func<AgentCategory, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(AgentCategory obj)
        {
            return _context.Update(obj);
        }
    }
}
