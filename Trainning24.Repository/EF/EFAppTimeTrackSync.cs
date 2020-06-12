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
    public class EFAppTimeTrackSync : IGenericRepository<AppTimeTrack>
    {
        private readonly EFGenericRepository<AppTimeTrack> _context;

        public EFAppTimeTrackSync(
            EFGenericRepository<AppTimeTrack> context
            )
        {
            _context = context;
        }
        public int Delete(AppTimeTrack obj)
        {
            throw new NotImplementedException();
        }

        public List<AppTimeTrack> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<AppTimeTrack> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public AppTimeTrack GetById(Expression<Func<AppTimeTrack, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(AppTimeTrack obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<AppTimeTrack> ListQuery(Expression<Func<AppTimeTrack, bool>> where)
        {
            return _context.ListQuery(where);
        }
        public async Task<int> SaveAsyncBulk(List<AppTimeTrack> obj)
        {
            return await _context.SaveAsyncBulk(obj);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(AppTimeTrack obj)
        {
            return _context.Update(obj);
        }

        public async Task<AppTimeTrack> GetRecors(Expression<Func<AppTimeTrack, bool>> ex)
        {
            return await _context.GetByIdAsyncSingle(ex);
        }

        public async Task<int> UpdateRecordBulk(List<AppTimeTrack> dto)
        {
            return await _context.UpdateAsyncBulk(dto);
        }
    }
}
