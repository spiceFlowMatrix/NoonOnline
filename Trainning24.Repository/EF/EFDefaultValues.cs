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
    public class EFDefaultValues : IGenericRepository<DefaultValues>
    {

        private readonly EFGenericRepository<DefaultValues> _context;


        public EFDefaultValues
        (
            EFGenericRepository<DefaultValues> context
        )
        {
            _context = context;
        }


        public int Delete(DefaultValues obj)
        {
            return _context.Delete(obj);
        }

        public List<DefaultValues> GetAll()
        {
            return _context.GetAll();
        }

        public List<DefaultValues> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public DefaultValues GetById(Expression<Func<DefaultValues, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(DefaultValues obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<DefaultValues> ListQuery(Expression<Func<DefaultValues, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(DefaultValues obj)
        {
            return _context.Update(obj);
        }
    }
}
