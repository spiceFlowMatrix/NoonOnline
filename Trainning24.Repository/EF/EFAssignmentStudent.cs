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
    public class EFAssignmentStudent : IGenericRepository<AssignmentStudent>
    {
        private readonly EFGenericRepository<AssignmentStudent> _context;

        public EFAssignmentStudent
        (
            EFGenericRepository<AssignmentStudent> context
        )
        {
            _context = context;
        }

        public int Delete(AssignmentStudent obj)
        {
            return _context.Delete(obj,obj.Id.ToString());
        }

        public List<AssignmentStudent> GetAll()
        {
            return _context.GetAll();
        }

        public List<AssignmentStudent> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public AssignmentStudent GetById(Expression<Func<AssignmentStudent, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(AssignmentStudent obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<AssignmentStudent> ListQuery(Expression<Func<AssignmentStudent, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            return _context.Save();
        }

        public int Update(AssignmentStudent obj)
        {
            return _context.Update(obj);
        }
    }
}
