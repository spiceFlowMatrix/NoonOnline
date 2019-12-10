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
    public class EFDiscussionFilesRepository : IGenericRepository<DiscussionFiles>
    {
        private readonly EFGenericRepository<DiscussionFiles> _context;
        //private static Training24Context _updateContext;
        private readonly EFGenericRepository<FileTypes> _fileTypesContext;

        public EFDiscussionFilesRepository
        (
            EFGenericRepository<DiscussionFiles> context,
            //Training24Context training24Context
            EFGenericRepository<FileTypes> fileTypesContext
        )
        {
            //_updateContext = training24Context;
            _context = context;
            _fileTypesContext = fileTypesContext;
        }


        public FileTypes FileType(DiscussionFiles files)
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

        public int Delete(DiscussionFiles obj)
        {
            return _context.Delete(obj,obj.Id.ToString());
        }

        public List<DiscussionFiles> GetAll()
        {
            return _context.GetAll();
        }

        public List<DiscussionFiles> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public DiscussionFiles GetById(Expression<Func<DiscussionFiles, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(DiscussionFiles obj)
        {
            return _context.Insert(obj);
        }


        public DiscussionFiles GetById(long Id)
        {
            return _context.GetById(b => b.Id == Id && b.IsDeleted != true);
        }


        public DiscussionFiles GetByUrl(string url)
        {
            return _context.GetById(b => b.Url == url);
        }

        public int Update(DiscussionFiles obj)
        {            
            DiscussionFiles Files = _context.GetById(b => b.Id == obj.Id);
            Files.DeletionTime = obj.Description;
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


        public int UpdateTest(DiscussionFiles obj)
        {
                return _context.Update(obj);
        }

        public IQueryable<DiscussionFiles> ListQuery(Expression<Func<DiscussionFiles, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

    }
}
