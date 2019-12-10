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
    public class UserQuizResultController : ControllerBase
    {
        private readonly LessonBusiness _lessonBusiness;
        private readonly UserQuizResultBusiness _userQuizResultBusiness;
        public UserQuizResultController(
            LessonBusiness lessonBusiness,
            UserQuizResultBusiness userQuizResultBusiness
            )
        {
            _lessonBusiness = lessonBusiness;
            _userQuizResultBusiness = userQuizResultBusiness;
        }
        #region TimeInterval Services
        //[Authorize]
        [HttpPost("UserQuizResultSync")]
        public IActionResult UserQuizResultSync([FromBody]List<UserQuizResultDTO> obj)
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
                        // var _progress = _userQuizResultBusiness.GetExistRecord(data.quizid, data.userid);
                        // if (_progress == null)
                        // {
                        UserQuizResult _userQuizResult = new UserQuizResult
                        {
                            QuizId = data.quizid,
                            UserId = data.userid,
                            TotalQuestion = data.totalquestion,
                            AnsweredQuestion = data.answeredquestion,
                            PerformDate = data.performdate,
                            PassingScore = data.passingscore,
                            Score = data.score,
                            CreatorUserId = int.Parse(tc.Id),
                            CreationTime = DateTime.Now.ToString()
                        };
                        _userQuizResultBusiness.AddRecord(_userQuizResult);
                        // }
                    }
                    successResponse.response_code = 0;
                    successResponse.message = "user quiz result synced";
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