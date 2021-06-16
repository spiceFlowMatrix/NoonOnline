using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.AddtionalServices;
using Trainning24.BL.ViewModels.ProgressSyncDTO;
using Trainning24.BL.ViewModels.TimeInterval;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class QuizProgressController : ControllerBase
    {
        private readonly LessonBusiness _lessonBusiness;
        private readonly QuizProgressBusiness _quizProgressBusiness;
        public QuizProgressController(
            LessonBusiness lessonBusiness,
            QuizProgressBusiness quizProgressBusiness
            )
        {
            _lessonBusiness = lessonBusiness;
            _quizProgressBusiness = quizProgressBusiness;
        }
        #region TimeInterval Services
        [Authorize]
        [HttpPost("QuizProgressSync")]
        public async Task<IActionResult> QuizProgressSync([FromBody]List<QuizProgressDTO> obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                if (obj.Count > 0)
                {
                    obj.RemoveAll(x => x.progress == 0);
                    if (obj.Count == 0)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "no data to sync";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(406, unsuccessResponse);
                    }
                    var userIds = obj.Select(a => a.userid).ToList();
                    var _progressAll = _quizProgressBusiness.GetAllByUser(userIds);
                    if (_progressAll.Count > 0)
                    {
                        var UpdateRecordList = _progressAll.Where(a => obj.Where(b => b.chapterid == a.ChapterId && b.quizid == a.QuizId && b.userid == a.UserId).Any()).ToList();
                        var insertRecordList = obj.Where(a => !_progressAll.Where(b => b.ChapterId == a.chapterid && b.QuizId == a.quizid && b.UserId == a.userid).Any()).ToList();
                        if (UpdateRecordList.Count > 0)
                        {
                            UpdateRecordList.Select(a =>
                            {
                                a.Progress = obj.Where(b => b.chapterid == a.ChapterId && b.quizid == a.QuizId && b.userid == a.UserId).FirstOrDefault().progress;
                                a.LastModificationTime = DateTime.Now.ToString();
                                a.LastModifierUserId = int.Parse(tc.Id);
                                return a;
                            }).ToList();
                            await _quizProgressBusiness.UpdateAsyncBulk(UpdateRecordList);
                        }
                        if (insertRecordList.Count > 0)
                        {
                            List<QuizProgress> InsertProgress = insertRecordList.Select(a => new QuizProgress()
                            {
                                ChapterId = a.chapterid,
                                QuizId = a.quizid,
                                UserId = a.userid,
                                Progress = a.progress,
                                CreatorUserId = int.Parse(tc.Id),
                                CreationTime = DateTime.UtcNow.ToString()
                            }).ToList();
                            await _quizProgressBusiness.AddRecordBulk(InsertProgress);
                        }
                    }
                    else
                    {
                        List<QuizProgress> InsertProgress = obj.Select(a => new QuizProgress()
                        {
                            ChapterId = a.chapterid,
                            QuizId = a.quizid,
                            UserId = a.userid,
                            Progress = a.progress,
                            CreatorUserId = int.Parse(tc.Id),
                            CreationTime = DateTime.UtcNow.ToString()
                        }).ToList();
                        await _quizProgressBusiness.AddRecordBulk(InsertProgress);
                    }

                    #region Commeted Old code
                    //foreach (var data in obj)
                    //{
                    //    var _progress = _quizProgressBusiness.GetExistRecord(data.chapterid, data.quizid, data.userid);
                    //    if (_progress == null)
                    //    {
                    //        QuizProgress _quizProgress = new QuizProgress
                    //        {
                    //            ChapterId = data.chapterid,
                    //            QuizId = data.quizid,
                    //            UserId = data.userid,
                    //            Progress = data.progress,
                    //            CreatorUserId = int.Parse(tc.Id),
                    //            CreationTime = DateTime.UtcNow.ToString()
                    //        };
                    //        _quizProgressBusiness.AddRecord(_quizProgress);
                    //    }
                    //    else
                    //    {
                    //        if (_progress.Progress < 100)
                    //        {
                    //            _progress.Progress = data.progress;
                    //            _progress.LastModificationTime = DateTime.UtcNow.ToString();
                    //            _progress.LastModifierUserId = int.Parse(tc.Id);
                    //            _quizProgressBusiness.UpdateRecord(_progress);
                    //        }
                    //    }
                    //} 
                    #endregion
                    successResponse.response_code = 0;
                    successResponse.message = "quiz progress synced";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "no data to sync";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(406, unsuccessResponse);
                }
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }
        #endregion
    }
}