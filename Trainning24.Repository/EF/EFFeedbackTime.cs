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
    public class EFFeedbackTime : IGenericRepository<FeebackTime>
    {
        private readonly EFGenericRepository<FeebackTime> _context;
 

        public EFFeedbackTime
        (
            EFGenericRepository<FeebackTime> context 
        )
        {
             _context = context;
        }



        public EFFeedbackTime()
        {

        }

        public int Delete(FeebackTime obj)
        {
            throw new NotImplementedException();
        }

        public List<FeebackTime> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<FeebackTime> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public FeebackTime GetById(System.Linq.Expressions.Expression<Func<FeebackTime, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(FeebackTime obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<FeebackTime> ListQuery(System.Linq.Expressions.Expression<Func<FeebackTime, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(FeebackTime obj)
        {
            throw new NotImplementedException();
        }
    }
}
