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
    public class EFAssignmentRepository : IGenericRepository<Assignment>
    {
        private readonly EFGenericRepository<Assignment> _context;
        private readonly EFGenericRepository<AssignmentFile> _contextAssignmentFile;
        //private static Training24Context _updateContext;

        public EFAssignmentRepository
        (
            EFGenericRepository<Assignment> context,
            //Training24Context training24Context,
            EFGenericRepository<AssignmentFile> contextAssignmentFile
        )
        {
            //_updateContext = training24Context;
            _context = context;
            _contextAssignmentFile = contextAssignmentFile;
        }

        public int Delete(Assignment obj)
        {
            return _context.Delete(obj, obj.Id.ToString());
        }

        public List<Assignment> GetAll()
        {
            return _context.GetAll();
        }

        public List<Assignment> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public Assignment GetById(Expression<Func<Assignment, bool>> ex)
        {
            throw new NotImplementedException();
        }

        public int Insert(Assignment obj)
        {
            return _context.Insert(obj);
        }


        public List<AssignmentFile> InsertAssignmentFile(List<AssignmentFile> AssignmentFiles)
        {
            foreach (var obj in AssignmentFiles)
            {
                _contextAssignmentFile.Insert(obj);
            }
            return AssignmentFiles;
        }

        public List<AssignmentFile> UpdateAssignmentFile(List<AssignmentFile> AssignmentFiles)
        {
            List<AssignmentFile> assignmentfilelist = _contextAssignmentFile.ListQuery(b => b.AssignmentId == AssignmentFiles.ElementAt(0).AssignmentId).ToList();
            foreach (var file in assignmentfilelist)
            {
                _contextAssignmentFile.Delete(file);
            }
            foreach (var obj in AssignmentFiles)
            {
                _contextAssignmentFile.Insert(obj);
            }
            return AssignmentFiles;
        }

        public List<AssignmentFile> DeleteAssignmentFile(long id)
        {
            List<AssignmentFile> AssignmentFiles = new List<AssignmentFile>();
            AssignmentFiles = _contextAssignmentFile.ListQuery(b => b.AssignmentId == id && b.IsDeleted != true).ToList();
            foreach (var obj in AssignmentFiles)
            {
                _contextAssignmentFile.Delete(int.Parse(obj.Id.ToString()));
            }
            return AssignmentFiles;
        }

        public List<AssignmentFile> GetAssignmentFile(long Id)
        {
            return _contextAssignmentFile.ListQuery(b => b.AssignmentId == Id && b.IsDeleted != true).ToList();
        }

        public Assignment GetById(int Id)
        {
            return _context.GetById(b => b.Id == Id);
        }

        public int Update(Assignment obj)
        {
            Assignment Assignment = _context.GetById(b => b.Id == obj.Id);
            if (Assignment != null)
            {
                Assignment.Name = obj.Name;
                Assignment.Description = obj.Description;
                Assignment.Code = obj.Code;
                Assignment.ChapterId = obj.ChapterId;
                Assignment.LastModificationTime = DateTime.Now.ToString();
                Assignment.LastModifierUserId = obj.LastModifierUserId;
                return _context.Update(Assignment);
            }
            else
            {
                return 0;
            }
        }

        public int UpdateItemOrder(Assignment obj)
        {
            return _context.Update(obj);
        }

        public IQueryable<Assignment> ListQuery(Expression<Func<Assignment, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public AssignmentFile DeleteAssignmentFileSingle(long id)
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
