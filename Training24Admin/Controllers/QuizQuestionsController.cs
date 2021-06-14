using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Training24Admin.Model;
using Trainning24.Domain.Entity;
using Trainning24.BL.Business;

namespace Training24Admin.Controllers
{
    
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class QuizQuestionsController : ControllerBase
    {
        private readonly QuizQuestionBusiness QuizQuestionBusiness;
        private readonly LessonBusiness LessonBusiness;

        public QuizQuestionsController(
            QuizQuestionBusiness QuizQuestionBusiness,
            LessonBusiness LessonBusiness
            )
        {
            this.QuizQuestionBusiness = QuizQuestionBusiness;
            this.LessonBusiness = LessonBusiness;
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                QuizQuestion Quiz = QuizQuestionBusiness.QuizQuestionExistanceById(id);
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (Quiz == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Quiz Question not found.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
                }
                else
                {
                    //if (tc.RoleId == ((int)RoleType.Admin).ToString())
                    //{
                        QuizQuestionBusiness.Delete(Quiz, tc.Id);

                        successResponse.response_code = 0;
                        successResponse.message = "Quiz Question Deleted";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    //}
                    //else
                    //{
                    //    unsuccessResponse.response_code = 1;
                    //    unsuccessResponse.message = "You are not authorized.";
                    //    unsuccessResponse.status = "Unsuccess";
                    //    return StatusCode(401, unsuccessResponse);
                    //}
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