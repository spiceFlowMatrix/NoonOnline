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
    public class EFChapterRepository : IGenericRepository<Chapter>
    {
        private readonly EFGenericRepository<Chapter> _context;
        //private static Training24Context _updateContext;

        public EFChapterRepository
        (
            EFGenericRepository<Chapter> context
            //Training24Context training24Context
        )
        {
            //_updateContext = training24Context;
            _context = context;
        }

        public int Delete(Chapter obj)
        {
            return _context.Delete(obj, obj.Id.ToString());
        }

        public List<Chapter> GetAll()
        {
            return _context.GetAll();
        }

        public List<Chapter> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public Chapter GetById(Expression<Func<Chapter, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(Chapter obj)
        {            
            return _context.Insert(obj);
        }


        public Chapter GetById(int Id)
        {
            return _context.GetById(b => b.Id == Id);
        }

        public int Update(Chapter obj)
        {
            Chapter Chapter = _context.GetById(b => b.Id == obj.Id);
            Chapter.Code = obj.Code;
            Chapter.CourseId = obj.CourseId;
            Chapter.Name = obj.Name;
            Chapter.QuizId = obj.QuizId;
            Chapter.LastModificationTime = DateTime.Now.ToString();
            Chapter.LastModifierUserId = obj.LastModifierUserId;
            Chapter.ItemOrder = obj.ItemOrder;
            return _context.Update(Chapter);
        }

        public IQueryable<Chapter> ListQuery(Expression<Func<Chapter, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }
    }
}
