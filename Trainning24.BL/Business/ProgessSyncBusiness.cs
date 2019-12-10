using System;
using System.Collections.Generic;
using System.Linq;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class ProgessSyncBusiness
    {
        private readonly EFProgressSync _eFProgressSync;
        private readonly EFQuizTimerSync _eFQuizTimerSync;

        public ProgessSyncBusiness(
            EFProgressSync eFProgressSync,
            EFQuizTimerSync eFQuizTimerSync
        )
        {
            _eFProgressSync = eFProgressSync;
            _eFQuizTimerSync = eFQuizTimerSync;
        }

        public ProgessSync AddRecord(ProgessSync obj)
        {
            ProgessSync progessSync = new ProgessSync
            {
                GradeId = obj.GradeId,
                FileId = obj.FileId,
                IsStatus = true,
                LessonProgressId = obj.LessonProgressId,
                LessonId = obj.LessonId,
                LessonProgress = obj.LessonProgress,
                QuizId = obj.QuizId,
                TotalRecords = obj.TotalRecords,
                UserId = obj.UserId,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = Convert.ToInt32(obj.UserId)
            };

            _eFProgressSync.Insert(progessSync);
            return obj;
        }


        public QuizTimerSync AddQuizTimerRecord(QuizTimerSync obj)
        {
            QuizTimerSync quizTimerSync = new QuizTimerSync
            {
                isStatus = true,
                passingScore = obj.passingScore,
                quizId = obj.quizId,
                userId = obj.userId,
                quizTime = obj.quizTime,
                yourScore = obj.yourScore,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = Convert.ToInt32(obj.userId)
            };

            _eFQuizTimerSync.Insert(quizTimerSync);
            return obj;
        }

        public ProgessSync CheckRecordExists(long? gradeId, long? lessionId, long? quizId, long userId)
        {
            ProgessSync progessSync = _eFProgressSync.GetById(b => b.LessonId == lessionId && b.GradeId == gradeId
                                                                    && (b.QuizId == quizId || b.UserId == userId));
            if (progessSync != null)
            {
                return progessSync;
            }
            else
            {
                return null;
            }
        }

        public QuizTimerSync CheckTimerRecordExists(long userid, long quizid)
        {
            QuizTimerSync quizTimerSync = _eFQuizTimerSync.GetById(b => b.quizId == quizid && b.userId == userid);
            if (quizTimerSync != null)
            {
                return quizTimerSync;
            }
            else
            {
                return null;
            }
        }



        public ProgessSync UpdateRecord(ProgessSync data)
        {
            ProgessSync progessSync = new ProgessSync();
            if (data.LessonId != 0 && data.QuizId == 0)
            {
                progessSync = _eFProgressSync.GetById(b => b.UserId == data.UserId && b.LessonId == data.LessonId);
                if (progessSync != null)
                {
                    progessSync.GradeId = data.GradeId;
                    progessSync.FileId = data.FileId;
                    progessSync.IsStatus = true;
                    progessSync.LessonProgressId = data.LessonProgressId;
                    progessSync.LessonId = data.LessonId;
                    progessSync.LessonProgress = data.LessonProgress;
                    progessSync.QuizId = data.QuizId;
                    progessSync.TotalRecords = data.TotalRecords;
                    progessSync.UserId = data.UserId;

                    _eFProgressSync.Update(progessSync);
                }
            }
            else if (data.QuizId != 0 && data.LessonId == 0)
            {
                progessSync = _eFProgressSync.GetById(b => b.UserId == data.UserId && b.QuizId == data.QuizId);
                if (progessSync != null)
                {
                    progessSync.GradeId = data.GradeId;
                    progessSync.IsStatus = true;
                    progessSync.FileId = data.FileId;
                    progessSync.LessonProgressId = data.LessonProgressId;
                    progessSync.LessonId = data.LessonId;
                    progessSync.LessonProgress = data.LessonProgress;
                    progessSync.QuizId = data.QuizId;
                    progessSync.TotalRecords = data.TotalRecords;
                    progessSync.UserId = data.UserId;
                    _eFProgressSync.Update(progessSync);
                }
            }
            return progessSync;
        }


        public QuizTimerSync UpdateQuizTimerRecord(QuizTimerSync data)
        {
            QuizTimerSync quizTimerSync = new QuizTimerSync();
            quizTimerSync = _eFQuizTimerSync.GetById(b => b.userId == data.userId && b.quizId == data.quizId);
            if (quizTimerSync != null)
            {
                quizTimerSync.isStatus = data.isStatus;
                quizTimerSync.passingScore = data.passingScore;
                quizTimerSync.quizTime = data.quizTime;
                quizTimerSync.yourScore = data.yourScore;
                quizTimerSync.LastModificationTime = DateTime.Now.ToString();
                quizTimerSync.LastModifierUserId = int.Parse(data.userId.ToString());
                _eFQuizTimerSync.Update(quizTimerSync);

                return quizTimerSync;
            }
            return quizTimerSync;
        }

        public List<ProgessSync> GetRecordById(int userId)
        {
            return _eFProgressSync.ListQuery(b => b.UserId == userId).ToList();
        }

        public List<QuizTimerSync> GetTimerRecordById(long userId)
        {
            return _eFQuizTimerSync.ListQuery(b => b.userId == userId).ToList();
        }
    }
}
