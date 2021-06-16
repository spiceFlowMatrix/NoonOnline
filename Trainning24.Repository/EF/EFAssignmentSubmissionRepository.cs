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
    public class EFAssignmentSubmissionRepository : IGenericRepository<AssignmentSubmission>
    {
        private readonly EFGenericRepository<AssignmentSubmission> _context;
        private readonly EFGenericRepository<AssignmentSubmissionFile> _contextfiles;
        public EFAssignmentSubmissionRepository
        (
            EFGenericRepository<AssignmentSubmission> context,
            EFGenericRepository<AssignmentSubmissionFile> contextfiles
        )
        {
            _context = context;
            _contextfiles = contextfiles;
        }
        public int Delete(AssignmentSubmission obj)
        {
            return _context.Delete(obj, obj.Id.ToString());
        }
        public List<AssignmentSubmission> GetAll()
        {
            return _context.GetAll();
        }
        public List<AssignmentSubmission> GetAllActive()
        {
            throw new NotImplementedException();
        }
        public AssignmentSubmission GetById(Expression<Func<AssignmentSubmission, bool>> ex)
        {
            return _context.GetById(ex);
        }
        public int Insert(AssignmentSubmission obj)
        {
            return _context.Insert(obj);
        }
        public IQueryable<AssignmentSubmission> ListQuery(Expression<Func<AssignmentSubmission, bool>> where)
        {
            return _context.ListQuery(where);
        }
        public int Save()
        {
            return _context.Save();
        }
        public int Update(AssignmentSubmission obj)
        {
            return _context.Update(obj);
        }
        public List<AssignmentSubmissionFile> InsertSubmissionFile(List<AssignmentSubmissionFile> AssignmentFiles)
        {
            try
            {
                foreach (var obj in AssignmentFiles)
                {
                    _contextfiles.Insert(obj);
                }
            }
            catch (Exception ex)
            {

            }
            return AssignmentFiles;
        }

        public List<AssignmentSubmissionFile> GetAssignmentFile(long Id)
        {
            return _contextfiles.ListQuery(b => b.SubmissionId == Id && b.IsDeleted != true).ToList();
        }
    }
}
