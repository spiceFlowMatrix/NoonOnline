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
    public class LessonProgressController : ControllerBase
    {
        private readonly LessonBusiness _lessonBusiness;
        private readonly LessonProgressBusiness _lessonProgressBusiness;
        public LessonProgressController(
            LessonBusiness lessonBusiness,
            LessonProgressBusiness lessonProgressBusiness
            )
        {
            _lessonBusiness = lessonBusiness;
            _lessonProgressBusiness = lessonProgressBusiness;
        }
        #region TimeInterval Services
        [Authorize]
        [HttpPost("LessonProgressSync")]
        public async Task<IActionResult> LessonProgressSync([FromBody]List<LessonProgressDTO> obj)
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
                    var _progressAll = _lessonProgressBusiness.GetAllByUser(userIds);
                    if (_progressAll.Count > 0)
                    {
                        var UpdateRecordList = _progressAll.Where(a => obj.Where(b => b.lessonid == a.LessonId && b.chapterid == a.ChapterId && b.userid == a.UserId).Any()).ToList();
                    var insertRecordList = obj.Where(a => !_progressAll.Where(b => b.LessonId == a.lessonid && b.ChapterId == a.chapterid && b.UserId == a.userid).Any()).ToList();
                    if (UpdateRecordList.Count > 0)
                    {
                        UpdateRecordList.Select(a =>
                        {
                            a.Progress = obj.Where(b => b.chapterid == a.ChapterId && b.lessonid == a.LessonId && b.userid == a.UserId).FirstOrDefault().progress;
                            a.LastModificationTime = DateTime.Now.ToString();
                            a.LastModifierUserId = int.Parse(tc.Id);
                            return a;
                        }).ToList();
                        await _lessonProgressBusiness.UpdateAsyncBulk(UpdateRecordList);
                    }
                    if (insertRecordList.Count > 0)
                    {
                        List<LessonProgress> InsertProgress = insertRecordList.Select(a => new LessonProgress()
                        {
                            LessonId = a.lessonid,
                            ChapterId = a.chapterid,
                            UserId = a.userid,
                            Progress = a.progress,
                            CreatorUserId = int.Parse(tc.Id),
                            CreationTime = DateTime.Now.ToString()
                        }).ToList();
                        await _lessonProgressBusiness.AddRecordBulk(InsertProgress);
                        }
                    }
                    else
                    {
                        List<LessonProgress> InsertProgress = obj.Select(a => new LessonProgress()
                        {
                            LessonId = a.lessonid,
                            ChapterId = a.chapterid,
                            UserId = a.userid,
                            Progress = a.progress,
                            CreatorUserId = int.Parse(tc.Id),
                            CreationTime = DateTime.Now.ToString()
                        }).ToList();
                        await _lessonProgressBusiness.AddRecordBulk(InsertProgress);
                    }
                    #region Commeted Old code
                    //foreach (var data in obj)
                    //{
                    //    var _progress = _lessonProgressBusiness.GetExistRecord(data.chapterid, data.lessonid, data.userid);
                    //    if (_progress == null)
                    //    {
                    //        LessonProgress _lessonProgress = new LessonProgress
                    //        {
                    //            ChapterId = data.chapterid,
                    //            LessonId = data.lessonid,
                    //            UserId = data.userid,
                    //            Progress = data.progress,
                    //            CreatorUserId = int.Parse(tc.Id),
                    //            CreationTime = DateTime.Now.ToString()
                    //        };
                    //        _lessonProgressBusiness.AddRecord(_lessonProgress);
                    //    }
                    //    else
                    //    {
                    //        if (_progress.Progress < 100)
                    //        {
                    //            _progress.Progress = data.progress;
                    //            _progress.LastModificationTime = DateTime.Now.ToString();
                    //            _progress.LastModifierUserId = int.Parse(tc.Id);
                    //            _lessonProgressBusiness.UpdateRecord(_progress);
                    //        }
                    //    }
                    //} 
                    #endregion
                    successResponse.response_code = 0;
                    successResponse.message = "lesson progress synced";
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