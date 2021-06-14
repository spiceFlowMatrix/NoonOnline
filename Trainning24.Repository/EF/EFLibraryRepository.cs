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
    public class EFLibraryRepository : IGenericRepository<Books>
    {
        private readonly EFGenericRepository<Books> _context;

        public EFLibraryRepository
        (
            EFGenericRepository<Books> context
        )
        {
            _context = context;
        }

        public int Delete(Books obj)
        {
            return _context.Delete(obj, obj.DeleterUserId.ToString());
        }

        public List<Books> GetAll()
        {
            return _context.GetAll();
        }

        public List<Books> GetAllActive()
        {
            return _context.GetAllActive();
        }

        public Books GetById(Expression<Func<Books, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(Books obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<Books> ListQuery(Expression<Func<Books, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(Books obj)
        {
            return _context.Update(obj);
        }
    }
}
