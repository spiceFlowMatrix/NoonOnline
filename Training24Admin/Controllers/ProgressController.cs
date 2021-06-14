using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.StudentChapterProgress;
using Trainning24.BL.ViewModels.StudentCourseProgress;
using Trainning24.BL.ViewModels.StudentLessonProgress;
using Trainning24.BL.ViewModels.StudentProgress;

namespace Training24Admin.Controllers
{
    
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ProgressController : ControllerBase
    {
        private readonly ProgressBusiness ProgressBusiness;
        private readonly LessonBusiness LessonBusiness;

        public ProgressController(
            ProgressBusiness ProgressBusiness,
            LessonBusiness LessonBusiness
            )
        {
            this.ProgressBusiness = ProgressBusiness;
            this.LessonBusiness = LessonBusiness;
        }

        [HttpPost]
        [Route("AddStudentCourseProgress")]
        public IActionResult AddStudentCourseProgress([FromBody] AddStudentCourseProgressModel AddStudentCourseProgressModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (ModelState.IsValid)
                {        
                    successResponse.data = ProgressBusiness.Create(AddStudentCourseProgressModel, int.Parse(tc.Id)); ;
                    successResponse.response_code = 0;
                    successResponse.message = "StudentCourseProgress Created";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    return StatusCode(406, ModelState);
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

        [HttpPost]
        [Route("AddStudentChapterProgress")]
        public IActionResult AddStudentChapterProgress([FromBody] AddStudentChapterProgressModel AddStudentChapterProgressModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (ModelState.IsValid)
                {                  
                    successResponse.data = ProgressBusiness.Create(AddStudentChapterProgressModel, int.Parse(tc.Id)); ;
                    successResponse.response_code = 0;
                    successResponse.message = "StudentChapterProgress Created";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    return StatusCode(406, ModelState);
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

        [HttpPost]
        [Route("AddStudentProgress")]
        public IActionResult AddStudentProgress([FromBody] AddStudentProgressModel AddStudentProgressModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (ModelState.IsValid)
                {                 
                    successResponse.data = ProgressBusiness.Create(AddStudentProgressModel, int.Parse(tc.Id)); ;
                    successResponse.response_code = 0;
                    successResponse.message = "StudentProgress Created";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    return StatusCode(406, ModelState);
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

        [HttpPost]
        [Route("AddStudentLessonProgress")]
        public IActionResult AddStudentLessonProgress([FromBody] AddStudentLessonProgressModel AddStudentLessonProgressModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (ModelState.IsValid)
                {                 
                    if(AddStudentLessonProgressModel.id == 0)
                        successResponse.data = ProgressBusiness.Create(AddStudentLessonProgressModel, int.Parse(tc.Id)); 
                    else
                        successResponse.data = ProgressBusiness.Update(AddStudentLessonProgressModel, int.Parse(tc.Id)); 
                    successResponse.response_code = 0;
                    successResponse.message = "StudentLessonProgress Created";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    return StatusCode(406, ModelState);
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




    }
}