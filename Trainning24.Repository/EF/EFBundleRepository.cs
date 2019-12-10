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
    public class EFBundleRepository : IGenericRepository<Bundle>
    {
        private readonly EFGenericRepository<Bundle> _context;
        //private static Training24Context _updateContext;

        public EFBundleRepository
        (
            EFGenericRepository<Bundle> context
            //Training24Context training24Context
        )
        {
            //_updateContext = training24Context;
            _context = context;
        }

        public int Delete(Bundle obj)
        {
            return _context.Delete(obj,obj.Id.ToString());
        }

        public List<Bundle> GetAll()
        {
            return _context.GetAll();
        }

        public List<Bundle> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public Bundle GetById(Expression<Func<Bundle, bool>> ex)
        {
            throw new NotImplementedException();
        }

        public int Insert(Bundle obj)
        {
            return _context.Insert(obj);
        }


        public Bundle GetById(int Id)
        {
            return _context.GetById(b => b.Id == Id);
        }

        public int Update(Bundle obj)
        {            
            Bundle bundle = _context.GetById(b => b.Id == obj.Id);
            bundle.Name = obj.Name;                        
            bundle.LastModificationTime = DateTime.Now.ToString();
            bundle.LastModifierUserId = obj.LastModifierUserId;
            return _context.Update(bundle);
        }

        public IQueryable<Bundle> ListQuery(Expression<Func<Bundle, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

    }
}
