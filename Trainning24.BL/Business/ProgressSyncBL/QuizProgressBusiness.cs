using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class QuizProgressBusiness
    {
        private readonly EFQuizProgressSync _eFQuizProgressSync;
        private readonly EFChapterRepository _EFChapterRepository;
        public QuizProgressBusiness(
            EFQuizProgressSync eFQuizProgressSync,
             EFChapterRepository EFChapterRepository
        )
        {
            _eFQuizProgressSync = eFQuizProgressSync;
            _EFChapterRepository = EFChapterRepository;
        }

        public int AddRecord(QuizProgress obj)
        {
            return _eFQuizProgressSync.Insert(obj);
        }

        public int UpdateRecord(QuizProgress obj)
        {
            return _eFQuizProgressSync.Update(obj);
        }

        public QuizProgress GetExistRecord(long ChapterId, long QuizId, long UserId)
        {
            return _eFQuizProgressSync.GetById(b => b.ChapterId == ChapterId && b.QuizId == QuizId && b.UserId == UserId);
        }
        public List<QuizProgress> GetAllByUser(List<long> userId)
        {
            return _eFQuizProgressSync.ListQuery(b => b.IsDeleted != true && userId.Contains(b.UserId)).ToList();
        }
        public async Task<int> AddRecordBulk(List<QuizProgress> obj)
        {
            return await _eFQuizProgressSync.SaveAsyncBulk(obj);
        }
        public async Task<int> UpdateAsyncBulk(List<QuizProgress> obj)
        {
            return await _eFQuizProgressSync.UpdateAsyncBulk(obj);
        }
    }
}
