using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFFeedback : IGenericRepository<Feedback>
    {


        private readonly EFGenericRepository<Feedback> _context;


        public EFFeedback
        (
            EFGenericRepository<Feedback> context
        )
        {
            _context = context;

        }

        public int Delete(Feedback obj)
        {
            throw new NotImplementedException();
        }

        public List<Feedback> GetAll()
        {
            return _context.GetAll();
        }

        public List<Feedback> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public Feedback GetById(System.Linq.Expressions.Expression<Func<Feedback, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(Feedback obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<Feedback> ListQuery(System.Linq.Expressions.Expression<Func<Feedback, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(Feedback obj)
        {
            _context.Update(obj);
            return Save();
        }
    }
}
