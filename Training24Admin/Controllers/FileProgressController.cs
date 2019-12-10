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
    public class FileProgressController : ControllerBase
    {
        private readonly LessonBusiness _lessonBusiness;
        private readonly FileProgressBusiness _fileProgressBusiness;
        public FileProgressController(
            LessonBusiness lessonBusiness,
            FileProgressBusiness fileProgressBusiness
            )
        {
            _lessonBusiness = lessonBusiness;
            _fileProgressBusiness = fileProgressBusiness;
        }
        #region TimeInterval Services
        [Authorize]
        [HttpPost("FileProgressSync")]
        public IActionResult FileProgressSync([FromBody]List<FileProgressDTO> obj)
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
                    foreach (var data in obj)
                    {
                        var _progress = _fileProgressBusiness.GetExistRecord(data.lessonid, data.fileid, data.userid);
                        if (_progress == null)
                        {
                            FileProgress _fileProgress = new FileProgress
                            {
                                LessonId = data.lessonid,
                                FileId = data.fileid,
                                UserId = data.userid,
                                Progress = data.progress,
                                CreatorUserId = int.Parse(tc.Id),
                                CreationTime = DateTime.Now.ToString()
                            };
                            _fileProgressBusiness.AddRecord(_fileProgress);
                        }
                        else
                        {
                            if (_progress.Progress < 100)
                            {
                                _progress.Progress = data.progress;
                                _progress.LastModificationTime = DateTime.Now.ToString();
                                _progress.LastModifierUserId = int.Parse(tc.Id);
                                _fileProgressBusiness.UpdateRecord(_progress);
                            }
                        }
                    }
                    successResponse.response_code = 0;
                    successResponse.message = "file progress synced";
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