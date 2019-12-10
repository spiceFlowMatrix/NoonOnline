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
    public class EFLessonAssignmentSubmissionRepository : IGenericRepository<LessonAssignmentSubmission>
    {
        private readonly EFGenericRepository<LessonAssignmentSubmission> _context;
        private readonly EFGenericRepository<LessonAssignmentSubmissionFile> _contextfiles;
        public EFLessonAssignmentSubmissionRepository
        (
            EFGenericRepository<LessonAssignmentSubmission> context,
            EFGenericRepository<LessonAssignmentSubmissionFile> contextfiles
        )
        {
            _context = context;
            _contextfiles = contextfiles;
        }
        public int Delete(LessonAssignmentSubmission obj)
        {
            return _context.Delete(obj, obj.Id.ToString());
        }
        public List<LessonAssignmentSubmission> GetAll()
        {
            return _context.GetAll();
        }
        public List<LessonAssignmentSubmission> GetAllActive()
        {
            throw new NotImplementedException();
        }
        public LessonAssignmentSubmission GetById(Expression<Func<LessonAssignmentSubmission, bool>> ex)
        {
            return _context.GetById(ex);
        }
        public int Insert(LessonAssignmentSubmission obj)
        {
            return _context.Insert(obj);
        }
        public IQueryable<LessonAssignmentSubmission> ListQuery(Expression<Func<LessonAssignmentSubmission, bool>> where)
        {
            return _context.ListQuery(where);
        }
        public int Save()
        {
            return _context.Save();
        }
        public int Update(LessonAssignmentSubmission obj)
        {
            return _context.Update(obj);
        }
        public List<LessonAssignmentSubmissionFile> InsertSubmissionFile(List<LessonAssignmentSubmissionFile> AssignmentFiles)
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
        public List<LessonAssignmentSubmissionFile> GetAssignmentFile(long Id)
        {
            return _contextfiles.ListQuery(b => b.SubmissionId == Id && b.IsDeleted != true).ToList();
        }
    }
}
