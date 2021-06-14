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
    public class EFFeedbackFile : IGenericRepository<FeedbackFile>
    {

        private readonly EFGenericRepository<FeedbackFile> _context;
 

        public EFFeedbackFile
        (
            EFGenericRepository<FeedbackFile> context 
        )
        {
            _context = context;
 
        }


        public int Delete(FeedbackFile obj)
        {
            throw new NotImplementedException();
        }

        public List<FeedbackFile> GetAll()
        {
            throw new NotImplementedException();
        }

        public List<FeedbackFile> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public FeedbackFile GetById(Expression<Func<FeedbackFile, bool>> ex)
        {
            throw new NotImplementedException();
        }

        public int Insert(FeedbackFile obj)
        {
            return _context.Insert(obj);
        }

        public IQueryable<FeedbackFile> ListQuery(Expression<Func<FeedbackFile, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public int Update(FeedbackFile obj)
        {
            throw new NotImplementedException();
        }
    }
}
