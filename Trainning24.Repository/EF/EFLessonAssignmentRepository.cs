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
    public class EFLessonAssignmentRepository : IGenericRepository<LessonAssignment>
    {
        private readonly EFGenericRepository<LessonAssignment> _context;
        private readonly EFGenericRepository<LessonAssignmentFile> _contextAssignmentFile;

        public EFLessonAssignmentRepository
        (
            EFGenericRepository<LessonAssignment> context,
            EFGenericRepository<LessonAssignmentFile> contextAssignmentFile
        )
        {
            _context = context;
            _contextAssignmentFile = contextAssignmentFile;
        }

        public int Delete(LessonAssignment obj)
        {
            return _context.Delete(obj, obj.Id.ToString());
        }

        public List<LessonAssignment> GetAll()
        {
            return _context.GetAll();
        }

        public List<LessonAssignment> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public LessonAssignment GetById(Expression<Func<LessonAssignment, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(LessonAssignment obj)
        {
            return _context.Insert(obj);
        }

        public List<LessonAssignmentFile> InsertLessionFile(List<LessonAssignmentFile> AssignmentFiles)
        {
            foreach (var obj in AssignmentFiles)
            {
                _contextAssignmentFile.Insert(obj);
            }

            return AssignmentFiles;
        }

        public List<LessonAssignmentFile> UpdateAssignmentFile(List<LessonAssignmentFile> AssignmentFiles)
        {
            List<LessonAssignmentFile> assignmentfilesList = _contextAssignmentFile.ListQuery(b => b.AssignmentId == AssignmentFiles.ElementAt(0).AssignmentId).ToList();
            foreach (var assignmentfiles in assignmentfilesList)
            {
                _contextAssignmentFile.Delete(assignmentfiles);
            }
            foreach (var obj in AssignmentFiles)
            {
                LessonAssignmentFile LessonAssignmentFile = _contextAssignmentFile.ListQuery(b => b.FileId == obj.FileId && b.AssignmentId == obj.AssignmentId && b.IsDeleted != true).SingleOrDefault();
                if (LessonAssignmentFile == null)
                    _contextAssignmentFile.Insert(obj);
            }
            return AssignmentFiles;
        }

        public List<LessonAssignmentFile> DeleteAssignmentFile(long id)
        {

            List<LessonAssignmentFile> AssignmentFiles = new List<LessonAssignmentFile>();
            AssignmentFiles = _contextAssignmentFile.ListQuery(b => b.AssignmentId == id && b.IsDeleted != true).ToList();


            foreach (var obj in AssignmentFiles)
            {
                _contextAssignmentFile.Delete(int.Parse(obj.Id.ToString()));
            }

            return AssignmentFiles;
        }

        public List<LessonAssignmentFile> GetAssignmentFile(long Id)
        {
            return _contextAssignmentFile.ListQuery(b => b.AssignmentId == Id && b.IsDeleted != true).ToList();
        }

        public LessonAssignment GetById(int Id)
        {
            return _context.GetById(b => b.Id == Id);
        }

        public int Update(LessonAssignment obj)
        {
            LessonAssignment Assignment = _context.GetById(b => b.Id == obj.Id);
            if (Assignment != null)
            {
                Assignment.Name = obj.Name;
                Assignment.Description = obj.Description;
                Assignment.Code = obj.Code;
                Assignment.LessonId = obj.LessonId;
                Assignment.LastModificationTime = DateTime.Now.ToString();
                Assignment.LastModifierUserId = obj.LastModifierUserId;
                return _context.Update(Assignment);
            }
            return 0;
        }

        public IQueryable<LessonAssignment> ListQuery(Expression<Func<LessonAssignment, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public LessonAssignmentFile DeleteAssignmentFileSingle(long id)
        {
            var assignmentfile = _contextAssignmentFile.GetById(b => b.Id == id && b.IsDeleted != true);
            if (assignmentfile != null)
            {
                _contextAssignmentFile.Delete(int.Parse(assignmentfile.Id.ToString()));
            }
            return assignmentfile;
        }
    }
}
