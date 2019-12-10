using System;
using System.Collections.Generic;
using System.Linq;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class UserQuizResultBusiness
    {
        private readonly EFUserQuizResultSync _eFUserQuizResultSync;
        private readonly EFChapterRepository _eFChapterRepository;
        private readonly EFChapterQuizRepository _eFChapterQuizRepository;
        public UserQuizResultBusiness(
            EFUserQuizResultSync eFUserQuizResultSync,
            EFChapterRepository eFChapterRepository,
            EFChapterQuizRepository eFChapterQuizRepository
        )
        {
            _eFUserQuizResultSync = eFUserQuizResultSync;
            _eFChapterRepository = eFChapterRepository;
            _eFChapterQuizRepository = eFChapterQuizRepository;
        }

        public int AddRecord(UserQuizResult obj)
        {
            return _eFUserQuizResultSync.Insert(obj);
        }

        public int UpdateRecord(UserQuizResult obj)
        {
            return _eFUserQuizResultSync.Update(obj);
        }

        public UserQuizResult GetExistRecord(long QuizId, long UserId)
        {
            return _eFUserQuizResultSync.GetById(b => b.QuizId == QuizId && b.UserId == UserId);
        }

        public List<UserQuizResult> GetQuizProgressByCourseId(List<int> studentid, int courseid)
        {
            if (courseid != 0)
            {
                List<UserQuizResult> quizProgresses = new List<UserQuizResult>();
                List<long> chapterQuiz = new List<long>();
                var chapterIds = _eFChapterRepository.ListQuery(b => b.CourseId == courseid && b.IsDeleted != true).Select(s => s.Id).ToList();
                if (chapterIds.Count > 0)
                {
                    chapterQuiz = _eFChapterQuizRepository.ListQuery(b => chapterIds.Contains(b.ChapterId) && b.IsDeleted != true).Select(s => s.QuizId).ToList();
                    if (chapterQuiz.Count > 0)
                    {
                        foreach (var quizid in chapterQuiz)
                        {
                            var quizprogress = _eFUserQuizResultSync.ListQuery(b => studentid.Contains((int)b.UserId) && b.QuizId == quizid && b.IsDeleted != true).ToList();
                            if (quizprogress.Count > 0)
                            {
                                quizProgresses.AddRange(quizprogress);
                            }
                        }
                    }
                }
                return quizProgresses;
            }
            else
            {
                return _eFUserQuizResultSync.ListQuery(b => studentid.Contains((int)b.UserId) && b.IsDeleted != true).ToList();
            }
        }
    }
}
