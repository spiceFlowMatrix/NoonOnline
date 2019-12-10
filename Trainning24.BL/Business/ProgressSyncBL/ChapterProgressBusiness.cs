using System;
using System.Collections.Generic;
using System.Linq;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class ChapterProgressBusiness
    {
        private readonly EFChapterProgressSync _eFChapterProgressSync;
        private readonly EFChapterRepository _eFChapterRepository;
        public ChapterProgressBusiness(
            EFChapterProgressSync eFChapterProgressSync,
            EFChapterRepository eFChapterRepository
        )
        {
            _eFChapterProgressSync = eFChapterProgressSync;
            _eFChapterRepository = eFChapterRepository;
        }

        public int AddRecord(ChapterProgress obj)
        {
            return _eFChapterProgressSync.Insert(obj);
        }

        public int UpdateRecord(ChapterProgress obj)
        {
            return _eFChapterProgressSync.Update(obj);
        }

        public ChapterProgress GetExistRecord(long CourseId, long ChapterId, long UserId)
        {
            return _eFChapterProgressSync.GetById(b => b.CourseId == CourseId && b.ChapterId == ChapterId && b.UserId == UserId);
        }

        public List<ChapterProgress> GetChapterProgresses(int studentid, int courseid)
        {
            if (courseid != 0)
            {
                List<ChapterProgress> chapterProgresses = new List<ChapterProgress>();
                var chapterIds = _eFChapterRepository.ListQuery(b => b.CourseId == courseid && b.IsDeleted != true).Select(s => s.Id).ToList();
                if (chapterIds.Count > 0)
                {
                    chapterProgresses = _eFChapterProgressSync.ListQuery(b => chapterIds.Contains(b.ChapterId) && b.UserId == studentid && b.IsDeleted != true).ToList();
                }
                return chapterProgresses;
            }
            else
            {
                return _eFChapterProgressSync.ListQuery(b => b.UserId == studentid && b.IsDeleted != true).ToList();
            }
        }
    }
}
