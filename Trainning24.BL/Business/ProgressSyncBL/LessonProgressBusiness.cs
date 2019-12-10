using System;
using System.Collections.Generic;
using System.Linq;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class LessonProgressBusiness
    {
        private readonly EFLessonProgressSync _eFLessonProgressSync;
        private readonly EFLessonRepository _eFLessonRepository;
        private readonly EFChapterRepository _eFChapterRepository;
        public LessonProgressBusiness(
            EFLessonProgressSync eFLessonProgressSync,
            EFLessonRepository eFLessonRepository,
            EFChapterRepository eFChapterRepository
        )
        {
            _eFLessonProgressSync = eFLessonProgressSync;
            _eFLessonRepository = eFLessonRepository;
            _eFChapterRepository = eFChapterRepository;
        }

        public int AddRecord(LessonProgress obj)
        {
            return _eFLessonProgressSync.Insert(obj);
        }

        public int UpdateRecord(LessonProgress obj)
        {
            return _eFLessonProgressSync.Update(obj);
        }

        public LessonProgress GetExistRecord(long ChapterId, long LessonId, long UserId)
        {
            return _eFLessonProgressSync.GetById(b => b.ChapterId == ChapterId && b.LessonId == LessonId && b.UserId == UserId);
        }

        public List<LessonProgress> GetLessonProgresses(List<int> studentid, int courseid)
        {
            if (courseid != 0)
            {
                List<LessonProgress> lessonProgress = new List<LessonProgress>();
                var chapterIds = _eFChapterRepository.ListQuery(b => b.CourseId == courseid && b.IsDeleted != true).Select(s => s.Id).ToList();
                if (chapterIds.Count > 0)
                {
                    var lessonIds = _eFLessonRepository.ListQuery(b => chapterIds.Contains(b.ChapterId ?? 0) && b.IsDeleted != true).Select(s => s.Id).ToList();
                    if (lessonIds.Count > 0)
                    {
                        lessonProgress = _eFLessonProgressSync.ListQuery(b => lessonIds.Contains(b.LessonId) && studentid.Contains((int)b.UserId) && b.IsDeleted != true).ToList();
                    }
                }
                return lessonProgress;
            }
            else
            {
                return _eFLessonProgressSync.ListQuery(b => studentid.Contains((int)b.UserId) && b.IsDeleted != true).ToList();
            }
        }
    }
}
