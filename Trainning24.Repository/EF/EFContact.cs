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
    public class EFContact : IGenericRepository<Contact>
    {
        private readonly EFGenericRepository<Contact> _context;

        public EFContact(
            EFGenericRepository<Contact> context
            ) {
        _context = context;
    }


        public int Delete(Contact obj)
        {
            throw new NotImplementedException();
        }

        public List<Contact> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<Contact> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public Contact GetById(Expression<Func<Contact, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(Contact obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<Contact> ListQuery(Expression<Func<Contact, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(Contact obj)
        {
            throw new NotImplementedException();
        }
    }
}
