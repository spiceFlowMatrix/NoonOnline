using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Trainning24.Abstract.Entity;
using Trainning24.Domain.Entity;

namespace Trainning24.Repository.EF.Generics
{
    public class EFGenericRepository<T> where T : class, IEntityBase
    {
        private readonly Training24Context _context;

        protected DbSet<T> _objSet;

        public EFGenericRepository
        (Training24Context context)
        {
            _context = context;
            _objSet = _context.Set<T>();
        }

        public int Insert(T obj)
        {
            _objSet.Add(obj);
            return Save();
        }

        public async Task<int> InsertAsync(T obj)
        {
            await _objSet.AddAsync(obj);
            return await SaveAsync();
        }

        public int Delete(T obj, string Id)
        {
            _objSet.Find(obj.Id).IsDeleted = true;
            _objSet.Find(obj.Id).DeletionTime = DateTime.Now.ToString();
            _objSet.Find(obj.Id).DeleterUserId = obj.DeleterUserId;
            return Save();
        }

        public int Delete(int id)
        {
            _objSet.Find(long.Parse(id.ToString())).IsDeleted = true;
            return Save();
        }

        public async Task<int> DeleteAsyncBulk(List<long> dto)
        {
            foreach (var dt in dto)
            {
                var data = await _objSet.FindAsync(long.Parse(dt.ToString()));
                data.IsDeleted = true;
                await SaveAsync();
            }
            return 1;
        }

        public int Delete(T obj)
        {
            _objSet.Find(obj.Id).IsDeleted = true;
            return Save();
        }

        public int DeleteRow(T obj)
        {
            _objSet.Remove(obj);
            return Save();
        }

        public int Update(T obj)
        {
            _objSet.Update(obj);
            return Save();
        }

        public int UpdateList(List<T> obj)
        {
            _objSet.UpdateRange(obj);
            return Save();
        }

        public List<T> GetAll()
        {
            return _objSet.Where(b => b.IsDeleted != true).ToList();
        }

        public T GetById(int id)
        {
            return _objSet.FirstOrDefault(x => x.Id == id);
        }

        public T GetById(Expression<Func<T, bool>> where)
        {
            return _objSet.FirstOrDefault(where);
        }

        public async Task<T> GetByIdAsyncTest(Expression<Func<T, bool>> where)
        {
            return await _objSet.FirstOrDefaultAsync(where);
        }

        public IQueryable<T> ListQuery(Expression<Func<T, bool>> where)
        {
            return _objSet.Where(where);
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public List<T> GetAllActive()
        {
            return _objSet.Where(x => x.IsDeleted.Value).ToList();
        }

        //public async Task<IEnumerable<T>> FindByConditionAsync(Expression<Func<T, bool>> expression)
        //{
        //    return await _objSet.Where(expression).ToListAsync();
        //}
        public async Task<List<T>> FindByConditionAsync(Expression<Func<T, bool>> where)
        {
            return await _objSet.Where(where).ToListAsync();
        }

        public async Task<List<T>> GetAllAsync()
        {
            return await _objSet.Where(b => b.IsDeleted != true).OrderByDescending(b => b.Id).ToListAsync();
        }

        public async Task<List<T>> GetByIdAsync(Expression<Func<T, bool>> where)
        {
            return await _objSet.Where(where).OrderByDescending(b => b.Id).ToListAsync();
        }

        public async Task<int> SaveAsyncBulk(List<T> dto)
        {
            await _objSet.AddRangeAsync(dto);
            return await SaveAsync();
        }

        public async Task<int> UpdateAsyncBulk(List<T> dto)
        {
            _objSet.UpdateRange(dto);
            return await SaveAsync();
        }

        public async Task<T> GetByIdAsyncSingle(Expression<Func<T, bool>> where)
        {
            return await _objSet.FirstOrDefaultAsync(where);
        }
    }
}
