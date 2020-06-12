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
    public class EFChapterProgressSync : IGenericRepository<ChapterProgress>
    {
        private readonly EFGenericRepository<ChapterProgress> _context;

        public EFChapterProgressSync(
            EFGenericRepository<ChapterProgress> context
            )
        {
            _context = context;
        }
        public int Delete(ChapterProgress obj)
        {
            throw new NotImplementedException();
        }

        public List<ChapterProgress> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<ChapterProgress> GetAllActive()
        {
            throw new NotImplementedException();
        }


        public async Task<int> SaveAsyncBulk(List<ChapterProgress> obj)
        {
            return await _context.SaveAsyncBulk(obj);
        }

        public async Task<int> UpdateAsyncBulk(List<ChapterProgress> obj)
        {
            return await _context.UpdateAsyncBulk(obj);
        }
        public ChapterProgress GetById(Expression<Func<ChapterProgress, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(ChapterProgress obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<ChapterProgress> ListQuery(Expression<Func<ChapterProgress, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(ChapterProgress obj)
        {
            return _context.Update(obj);
        }
    }
}
