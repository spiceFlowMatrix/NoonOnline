using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Trainning24.Abstract.Infrastructure.IGeneric;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF.Generics;

namespace Trainning24.Repository.EF
{
    public class EFFileProgressSync : IGenericRepository<FileProgress>
    {
        private readonly EFGenericRepository<FileProgress> _context;

        public EFFileProgressSync(
            EFGenericRepository<FileProgress> context
            )
        {
            _context = context;
        }
        public int Delete(FileProgress obj)
        {
            throw new NotImplementedException();
        }

        public List<FileProgress> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<FileProgress> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public FileProgress GetById(Expression<Func<FileProgress, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(FileProgress obj)
        {
            return _context.Insert(obj);
        }

        public async Task<int> SaveAsyncBulk(List<FileProgress> obj)
        {
            return await _context.SaveAsyncBulk(obj);
        }

        public async Task<int> UpdateAsyncBulk(List<FileProgress> obj)
        {
            return await _context.UpdateAsyncBulk(obj);
        }
        public IQueryable<FileProgress> ListQuery(Expression<Func<FileProgress, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(FileProgress obj)
        {
            return _context.Update(obj);
        }
    }
}
