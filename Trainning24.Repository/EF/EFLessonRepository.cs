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
    public class EFLessonRepository : IGenericRepository<Lesson>
    {
        private readonly EFGenericRepository<Lesson> _context;
        private readonly EFGenericRepository<LessonFile> _contextLessonFile;
        //private static Training24Context _updateContext;

        public EFLessonRepository
        (
            EFGenericRepository<Lesson> context,
            //Training24Context training24Context,
            EFGenericRepository<LessonFile> contextLessonFile
        )
        {
            //_updateContext = training24Context;
            _context = context;
            _contextLessonFile = contextLessonFile;
        }

        public int Delete(Lesson obj)
        {
            return _context.Delete(obj, obj.Id.ToString());
        }

        public List<Lesson> GetAll()
        {
            return _context.GetAll();
        }

        public List<Lesson> GetAllActive()
        {
            throw new NotImplementedException();
        }

        public Lesson GetById(Expression<Func<Lesson, bool>> ex)
        {
            return _context.GetById(ex);
        }

        public int Insert(Lesson obj)
        {
            return _context.Insert(obj);
        }


        public List<LessonFile> InsertLessionFile(List<LessonFile> lessonFiles)
        {
            foreach (var obj in lessonFiles)
            {
                LessonFile lessonFile = _contextLessonFile.ListQuery(b => b.FileId == obj.FileId && b.LessionId == obj.LessionId).SingleOrDefault();
                if (lessonFile == null)
                    _contextLessonFile.Insert(obj);
            }

            return lessonFiles;
        }

        public List<LessonFile> UpdateLessionFile(List<LessonFile> lessonFiles)
        {
            List<LessonFile> lessonFileslist = _contextLessonFile.ListQuery(b => b.LessionId == lessonFiles.ElementAt(0).LessionId && b.IsDeleted != true).ToList();
            foreach (var lessonfilesdb in lessonFileslist)
            {
                _contextLessonFile.Delete(lessonfilesdb);
            }
            foreach (var obj in lessonFiles)
            {
                LessonFile lessonFile = _contextLessonFile.ListQuery(b => b.FileId == obj.FileId && b.LessionId == obj.LessionId && b.IsDeleted != true).SingleOrDefault();
                if (lessonFile == null)
                    _contextLessonFile.Insert(obj);
            }
            return lessonFiles;
        }


        public List<LessonFile> GetLessionFile(long Id)
        {
            return _contextLessonFile.ListQuery(b => b.LessionId == Id && b.IsDeleted != true).ToList();
        }

        public LessonFile GetSingleLessionFile(long Id)
        {
            return _contextLessonFile.GetById(b => b.LessionId == Id && b.IsDeleted != true);
        }

        public Lesson GetById(int Id)
        {
            return _context.GetById(b => b.Id == Id);
        }

        public int Update(Lesson obj)
        {
            Lesson Lesson = _context.GetById(b => b.Id == obj.Id);
            Lesson.ChapterId = obj.ChapterId;
            Lesson.Code = obj.Code;
            Lesson.Description = obj.Description;
            Lesson.IsDeleted = obj.IsDeleted;
            Lesson.ItemOrder = obj.ItemOrder;
            Lesson.Name = obj.Name;
            Lesson.LastModificationTime = DateTime.Now.ToString();
            Lesson.LastModifierUserId = obj.LastModifierUserId;
            return _context.Update(Lesson);
        }

        public IQueryable<Lesson> ListQuery(Expression<Func<Lesson, bool>> where)
        {
            return _context.ListQuery(where);
        }

        public int Save()
        {
            throw new NotImplementedException();
        }

        public async Task<int> DeleteLessonById(List<long> dto)
        {
            return await _context.DeleteAsyncBulk(dto);
        }

        public async Task<int> SaveLessonFileBulk(List<LessonFile> dto)
        {
            return await _contextLessonFile.SaveAsyncBulk(dto);
        }

        public async Task<int> UpdateLessonFileBulk(List<LessonFile> dto) 
        {
            return await _contextLessonFile.UpdateAsyncBulk(dto); 
        }

        public async Task<LessonFile> GetLessonFile(Expression<Func<LessonFile, bool>> ex)
        {
            return await _contextLessonFile.GetByIdAsyncSingle(ex);
        }

        public List<LessonFile> GetLessonFileAll(Expression<Func<LessonFile, bool>> ex)
        {
            return _contextLessonFile.ListQuery(ex).ToList();
        }
    }
}
