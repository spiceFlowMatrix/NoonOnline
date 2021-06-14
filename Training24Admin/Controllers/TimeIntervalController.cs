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
using Trainning24.BL.ViewModels.TimeInterval;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TimeIntervalController : ControllerBase
    {
        private readonly TimeIntervalBusiness _timeIntervalBusiness;
        private readonly LessonBusiness _lessonBusiness;

        public TimeIntervalController(
            TimeIntervalBusiness timeIntervalBusiness,
            LessonBusiness lessonBusiness
            )
        {
            _timeIntervalBusiness = timeIntervalBusiness;
            _lessonBusiness = lessonBusiness;
        }

        #region TimeInterval Services
        [Authorize]
        [HttpGet("GetInterval")]
        public IActionResult GetInterval()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            TimeIntervalDTO obj = new TimeIntervalDTO();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    var data = _timeIntervalBusiness.Get();
                    if (data != null)
                    {
                        obj.id = data.Id;
                        obj.interval = data.Interval;
                        successResponse.data = obj;
                        successResponse.response_code = 0;
                        successResponse.message = "Interval get";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "No Interval found";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
                    }
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "You are not authorized.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(401, unsuccessResponse);
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

        [Authorize]
        [HttpPut("UpdateInterval")]
        public IActionResult UpdateInterval(TimeIntervalDTO obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    TimeInterval timeInterval = new TimeInterval();
                    timeInterval.Id = obj.id;
                    timeInterval.Interval = obj.interval;
                    timeInterval.LastModifierUserId = int.Parse(tc.Id);
                    timeInterval.LastModificationTime = DateTime.Now.ToString();
                    var data = _timeIntervalBusiness.UpdateInterval(timeInterval);
                    obj.id = data.Id;
                    obj.interval = data.Interval;
                    successResponse.data = obj;
                    successResponse.response_code = 0;
                    successResponse.message = "Interval updated";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "You are not authorized.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(401, unsuccessResponse);
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

        [HttpGet("GetIntervalPub")]
        public IActionResult GetIntervalPub()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            TimeIntervalDTO2 obj = new TimeIntervalDTO2();
            try
            {
                var data = _timeIntervalBusiness.Get();
                if (data != null)
                {
                    obj.interval = data.Interval;
                    successResponse.data = obj;
                    successResponse.response_code = 0;
                    successResponse.message = "Interval get";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 2;
                    unsuccessResponse.message = "No Interval found";
                    unsuccessResponse.status = "Failure";
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