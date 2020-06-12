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
    public class ChapterProgressController : ControllerBase
    {
        private readonly LessonBusiness _lessonBusiness;
        private readonly ChapterProgressBusiness _chapterProgressBusiness;
        public ChapterProgressController(
            LessonBusiness lessonBusiness,
            ChapterProgressBusiness chapterProgressBusiness
            )
        {
            _lessonBusiness = lessonBusiness;
            _chapterProgressBusiness = chapterProgressBusiness;
        }
        #region TimeInterval Services
        [Authorize]
        [HttpPost("ChapterProgressSync")]
        public async Task<IActionResult> ChapterProgressSync([FromBody]List<ChapterProgressDTO> obj)
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
                    var _progressAll = _chapterProgressBusiness.GetAllByUser(userIds);
                    if (_progressAll.Count > 0)
                    {
                        var UpdateRecordList = _progressAll.Where(a => obj.Where(b => b.courseid == a.CourseId && b.chapterid == a.ChapterId && b.userid == a.UserId).Any()).ToList();
                    var insertRecordList = obj.Where(a => !_progressAll.Where(b => b.CourseId == a.courseid && b.ChapterId == a.chapterid && b.UserId == a.userid).Any()).ToList();
                    if (UpdateRecordList.Count > 0)
                    {
                        UpdateRecordList.Select(a =>
                        {
                            a.Progress = obj.Where(b => b.chapterid == a.ChapterId && b.courseid == a.CourseId && b.userid == a.UserId).FirstOrDefault().progress;
                            a.LastModificationTime = DateTime.Now.ToString();
                            a.LastModifierUserId = int.Parse(tc.Id);
                            return a;
                        }).ToList();
                        await _chapterProgressBusiness.UpdateAsyncBulk(UpdateRecordList);
                    }
                    if (insertRecordList.Count > 0)
                    {
                        List<ChapterProgress> InsertProgress = insertRecordList.Select(a => new ChapterProgress()
                        {
                            CourseId = a.courseid,
                            ChapterId = a.chapterid,
                            UserId = a.userid,
                            Progress = a.progress,
                            CreatorUserId = int.Parse(tc.Id),
                            CreationTime = DateTime.Now.ToString()
                        }).ToList();
                        await _chapterProgressBusiness.AddRecordBulk(InsertProgress);
                    }
                    }
                    else
                    {
                        List<ChapterProgress> InsertProgress = obj.Select(a => new ChapterProgress()
                        {
                            CourseId = a.courseid,
                            ChapterId = a.chapterid,
                            UserId = a.userid,
                            Progress = a.progress,
                            CreatorUserId = int.Parse(tc.Id),
                            CreationTime = DateTime.Now.ToString()
                        }).ToList();
                        await _chapterProgressBusiness.AddRecordBulk(InsertProgress);
                    }
                    #region Comment Old code
                    //foreach (var data in obj)
                    //{
                    //    var _progress = _chapterProgressBusiness.GetExistRecord(data.courseid, data.chapterid, data.userid);
                    //    if (_progress == null)
                    //    {
                    //        ChapterProgress _chapterProgress = new ChapterProgress
                    //        {
                    //            CourseId = data.courseid,
                    //            ChapterId = data.chapterid,
                    //            UserId = data.userid,
                    //            Progress = data.progress,
                    //            CreatorUserId = int.Parse(tc.Id),
                    //            CreationTime = DateTime.Now.ToString()
                    //        };
                    //        _chapterProgressBusiness.AddRecord(_chapterProgress);
                    //    }
                    //    else
                    //    {
                    //        if (_progress.Progress < 100)
                    //        {
                    //            _progress.Progress = data.progress;
                    //            _progress.LastModificationTime = DateTime.Now.ToString();
                    //            _progress.LastModifierUserId = int.Parse(tc.Id);
                    //            _chapterProgressBusiness.UpdateRecord(_progress);
                    //        }
                    //    }
                    //} 
                    #endregion
                    successResponse.response_code = 0;
                    successResponse.message = "chapter progress synced";
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