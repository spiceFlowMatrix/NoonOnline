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
    public class EFDiscussionCommentFilesRepository : IGenericRepository<DiscussionCommentFiles>
    {
        private readonly EFGenericRepository<DiscussionCommentFiles> _context;
        private readonly EFGenericRepository<FileTypes> _fileTypesContext;

        public EFDiscussionCommentFilesRepository
        (
            EFGenericRepository<DiscussionCommentFiles> context,
            //Training24Context training24Context
            EFGenericRepository<FileTypes> fileTypesContext
        )
        {
            //_updateContext = training24Context;
            _context = context;
            _fileTypesContext = fileTypesContext;
        }


        public FileTypes FileType(DiscussionCommentFiles files)
        {
            var roleData = new FileTypes();
            try
            {
                //roleData = _fileTypesContext.ListQuery(r => r.Id == files.FileTypeId).FirstOrDefault();
                roleData = _fileTypesContext.GetById(b => b.Id == files.FileTypeId);
            }
            catch (Exception ex)
            {

            }
            return roleData;
        }

        public int Delete(DiscussionCommentFiles obj)
        {
            return _context.Delete(obj,obj.Id.ToString());
        }

        public List<DiscussionCommentFiles> GetAll()
        {
            return _context.GetAll();
        }

        public List<DiscussionCommentFiles> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public DiscussionCommentFiles GetById(Expression<Func<DiscussionCommentFiles, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(DiscussionCommentFiles obj)
        {
            return _context.Insert(obj);
        }


        public DiscussionCommentFiles GetById(long Id)
        {
            return _context.GetById(b => b.Id == Id && b.IsDeleted != true);
        }


        public DiscussionCommentFiles GetByUrl(string url)
        {
            return _context.GetById(b => b.Url == url);
        }

        public int Update(DiscussionCommentFiles obj)
        {
            DiscussionCommentFiles Files = _context.GetById(b => b.Id == obj.Id);
            Files.Duration = obj.Duration;
            Files.FileName = obj.FileName;
            Files.FileSize = obj.FileSize;
            Files.FileTypeId = obj.FileTypeId;
            Files.TotalPages = obj.TotalPages;
            Files.Url = obj.Url;            
            Files.Name = obj.Name;                        
            Files.LastModificationTime = DateTime.Now.ToString();
            Files.LastModifierUserId = obj.LastModifierUserId;
            return _context.Update(Files);
        }


        public int UpdateTest(DiscussionCommentFiles obj)
        {
                return _context.Update(obj);
        }

        public IQueryable<DiscussionCommentFiles> ListQuery(Expression<Func<DiscussionCommentFiles, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

    }
}
