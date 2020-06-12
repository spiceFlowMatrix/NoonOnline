using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class FileProgressBusiness
    {
        private readonly EFFileProgressSync _eFFileProgressSync;
        private readonly EFLessonRepository _eFLessonRepository;
        private readonly EFChapterRepository _eFChapterRepository;
        public FileProgressBusiness(
            EFFileProgressSync eFFileProgressSync,
            EFLessonRepository eFLessonRepository,
            EFChapterRepository eFChapterRepository
        )
        {
            _eFFileProgressSync = eFFileProgressSync;
            _eFLessonRepository = eFLessonRepository;
            _eFChapterRepository = eFChapterRepository;
        }

        public int AddRecord(FileProgress obj)
        {
            return _eFFileProgressSync.Insert(obj);
        }

        public async Task<int> AddRecordBulk(List<FileProgress> obj)
        {
            return await _eFFileProgressSync.SaveAsyncBulk(obj);
        }
        public async Task<int> UpdateAsyncBulk(List<FileProgress> obj)
        {
            return await _eFFileProgressSync.UpdateAsyncBulk(obj);
        }

        public List<FileProgress> GetAllByUser(List<long> userId)
        {
            return _eFFileProgressSync.ListQuery(b => b.IsDeleted != true && userId.Contains(b.UserId)).ToList();
        }
        public int UpdateRecord(FileProgress obj)
        {
            return _eFFileProgressSync.Update(obj);
        }

        public FileProgress GetExistRecord(long LessonId, long FileId, long UserId)
        {
            return _eFFileProgressSync.GetById(b => b.LessonId == LessonId && b.FileId == FileId && b.UserId == UserId);
        }

        public List<FileProgress> GetFileProgresses(int studentid, int courseid)
        {
            if (courseid != 0)
            {
                List<FileProgress> fileProgress = new List<FileProgress>();
                var chapterIds = _eFChapterRepository.ListQuery(b => b.CourseId == courseid && b.IsDeleted != true).Select(s => s.Id).ToList();
                if (chapterIds.Count > 0)
                {
                    var lessonIds = _eFLessonRepository.ListQuery(b => chapterIds.Contains(b.ChapterId ?? 0) && b.IsDeleted != true).Select(s => s.Id).ToList();
                    if (lessonIds.Count > 0)
                    {
                        var lessonfiles = _eFLessonRepository.GetLessonFileAll(b => lessonIds.Contains(b.LessionId) && b.IsDeleted != true).Select(s => s.FileId).ToList();
                        if (lessonfiles.Count > 0)
                        {
                            fileProgress = _eFFileProgressSync.ListQuery(b => lessonfiles.Contains(b.FileId) && b.UserId == studentid && b.IsDeleted != true).ToList();
                        }
                    }
                }
                return fileProgress;
            }
            else
            {
                return _eFFileProgressSync.ListQuery(b => b.UserId == studentid && b.IsDeleted != true).ToList();
            }
        }
    }
}
