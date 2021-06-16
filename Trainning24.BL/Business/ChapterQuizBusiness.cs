using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class ChapterQuizBusiness
    {
        private readonly EFChapterQuizRepository EFChapterQuizRepository;
        private readonly EFChapterRepository _eFChapterRepository;
        private readonly EFStudentCourseRepository _eFStudentCourseRepository;
        public ChapterQuizBusiness(
            EFChapterQuizRepository EFChapterQuizRepository,
            EFChapterRepository eFChapterRepository,
            EFStudentCourseRepository eFStudentCourseRepository
            )
        {
            this.EFChapterQuizRepository = EFChapterQuizRepository;
            _eFChapterRepository = eFChapterRepository;
            _eFStudentCourseRepository = eFStudentCourseRepository;
        }

        public ChapterQuiz AddNewQuiz(ChapterQuiz obj)
        {
            EFChapterQuizRepository.Insert(obj);
            return obj;
        }

        public ChapterQuiz CheckExistance(int id)
        {
            return EFChapterQuizRepository.GetById(b => b.QuizId == id);
        }

        public ChapterQuiz Update(ChapterQuiz obj)
        {
            EFChapterQuizRepository.Update(obj);
            return obj;
        }

        public ChapterQuiz GetChaperQuizByQuizId(long id)
        {
            return EFChapterQuizRepository.GetChaperQuizByQuizId(id);
        }

        public int GetQuizCount(List<int> studentid, int courseid)
        {
            int QuizCount = 0;
            if (courseid != 0)
            {
                var chapterIds = _eFChapterRepository.ListQuery(b => b.CourseId == courseid && b.IsDeleted != true).Select(s => s.Id).ToList();
                if (chapterIds.Count > 0)
                {
                    QuizCount = EFChapterQuizRepository.ListQuery(b => chapterIds.Contains(b.ChapterId) && b.IsDeleted != true).Count();
                }
                return QuizCount;
            }
            else
            {
                var userCourse = _eFStudentCourseRepository.ListQuery(b => studentid.Contains((int)b.UserId) && b.IsDeleted != true).Select(b => b.CourseId).ToList();
                if (userCourse.Count > 0)
                {
                    var chapterIds = _eFChapterRepository.ListQuery(b => userCourse.Contains(b.CourseId) && b.IsDeleted != true).Select(s => s.Id).ToList();
                    if (chapterIds.Count > 0)
                    {
                        QuizCount = EFChapterQuizRepository.ListQuery(b => chapterIds.Contains(b.ChapterId) && b.IsDeleted != true).Count();
                    }
                }
                return QuizCount;
            }
        }
    }
}
