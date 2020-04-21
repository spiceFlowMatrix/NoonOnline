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
    public class EFRoleRepository : IGenericRepository<Role>
    {
        private readonly EFGenericRepository<Role> _context;

        public EFRoleRepository
        (
            EFGenericRepository<Role> context
        )
        {
            _context = context;
        }

        public int Delete(Role obj)
        {
            throw new NotImplementedException();
        }

        public List<Role> GetAll()
        {
            return _context.GetAll();
        }

        public List<Role> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public Role GetById(Expression<Func<Role, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public async Task<Role> GetByIdAsync(Expression<Func<Role, bool>> ex)
        {
            return await _context.GetByIdAsyncTest(ex);
        }

        public int Insert(Role obj)
        {
            throw new NotImplementedException();
        }

        public IQueryable<Role> ListQuery(Expression<Func<Role, bool>> where)
        {
            throw new NotImplementedException();
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(Role obj)
        {
            throw new NotImplementedException();
        }
    }
}
