using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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

        public async Task<int> AddQuizTimerRecordInBulk(List<QuizTimerSync> obj)
        {
            List<QuizTimerSync> InsertProgress = obj.Select(a => new QuizTimerSync()
            {
                isStatus = true,
                passingScore = a.passingScore,
                quizId = a.quizId,
                userId = a.userId,
                quizTime = a.quizTime,
                yourScore = a.yourScore,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = Convert.ToInt32(a.userId)
            }).ToList();
            return  await _eFQuizTimerSync.AddRecordBulk(InsertProgress);
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

        public async Task<int> ProgessSyncRecord(List<ProgessSync> obj)
        {

            if (obj.Count > 0)
            {
                obj.RemoveAll(x => x.LessonProgress == 0);
                if (obj.Count == 0)
                {
                    return 1;
                }
                var userIds = obj.Select(a => a.UserId).ToList();
                var _progressAll = _eFProgressSync.ListQuery(b => b.IsDeleted != true && userIds.Contains(b.UserId)).ToList();
                if (_progressAll.Count > 0)
                {
                    var UpdateRecordList = _progressAll.Where(a => obj.Where(b => (b.UserId == a.UserId && b.LessonId == a.LessonId) || (b.UserId == a.UserId && b.QuizId == a.QuizId)).Any()).ToList();
                    var insertRecordList = obj.Where(a => !_progressAll.Where(b => (b.UserId == a.UserId && b.LessonId == a.LessonId) || (b.UserId == a.UserId && b.QuizId == a.QuizId)).Any()).ToList();
                    if (UpdateRecordList.Count > 0)
                    {
                        UpdateRecordList.Select(a =>
                        {
                            a.IsStatus = true;
                            a.LessonProgress = obj.Where(b => (b.UserId == a.UserId && b.LessonId == a.LessonId) || (b.UserId == a.UserId && b.QuizId == a.QuizId)).FirstOrDefault().LessonProgress;
                            a.TotalRecords = obj.Where(b => (b.UserId == a.UserId && b.LessonId == a.LessonId) || (b.UserId == a.UserId && b.QuizId == a.QuizId)).FirstOrDefault().TotalRecords;
                            return a;
                        }).ToList();
                        await _eFProgressSync.UpdateAsyncBulk(UpdateRecordList);
                    }
                    if (insertRecordList.Count > 0)
                    {
                        List<ProgessSync> InsertProgress = insertRecordList.Select(a => new ProgessSync()
                        {
                            GradeId = a.GradeId,
                            FileId = a.FileId,
                            IsStatus = true,
                            LessonProgressId = a.LessonProgressId,
                            LessonId = a.LessonId,
                            LessonProgress = a.LessonProgress,
                            QuizId = a.QuizId,
                            TotalRecords = a.TotalRecords,
                            UserId = a.UserId,
                            IsDeleted = false,
                            CreationTime = DateTime.Now.ToString(),
                            CreatorUserId = Convert.ToInt32(a.UserId)
                        }).ToList();
                        await _eFProgressSync.AddRecordBulk(InsertProgress);
                    }
                }
                else
                {
                    List<ProgessSync> InsertProgress = obj.Select(a => new ProgessSync()
                    {
                        GradeId = a.GradeId,
                        FileId = a.FileId,
                        IsStatus = true,
                        LessonProgressId = a.LessonProgressId,
                        LessonId = a.LessonId,
                        LessonProgress = a.LessonProgress,
                        QuizId = a.QuizId,
                        TotalRecords = a.TotalRecords,
                        UserId = a.UserId,
                        IsDeleted = false,
                        CreationTime = DateTime.Now.ToString(),
                        CreatorUserId = Convert.ToInt32(a.UserId)
                    }).ToList();
                    await _eFProgressSync.AddRecordBulk(InsertProgress);
                }
                return 0;
            }
            return 1;
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
