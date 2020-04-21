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
    public class EFUserRoleRepository : IGenericRepository<UserRole>
    {

        private readonly EFGenericRepository<UserRole> _context;

        public EFUserRoleRepository
        (
            EFGenericRepository<UserRole> context
        )
        {
            _context = context;
        }

        public int Delete(UserRole obj)
        {
            throw new NotImplementedException();
        }

        public List<UserRole> GetAll()
        {
            return _context.GetAll();
        }

        public int DeleteUserRole(UserRole userRole)
        {
            return _context.DeleteRow(userRole);
        }

        public List<UserRole> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public UserRole GetById(Expression<Func<UserRole, bool>> ex)
        {
            throw new NotImplementedException();
        }

        public int Insert(UserRole obj)
        {
            return _context.Insert(obj);
        }

        public async Task<int> InsertAsync(UserRole obj)
        {
            return await _context.InsertAsync(obj);
        }

        public IQueryable<UserRole> ListQuery(Expression<Func<UserRole, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public async Task<List<UserRole>> ListQueryAsync(Expression<Func<UserRole, bool>> where)
        {
            return await _context.FindByConditionAsync(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(UserRole obj)
        {
            throw new NotImplementedException();
        }
    }
}
