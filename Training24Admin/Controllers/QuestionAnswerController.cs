using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trainning24.BL.Business;
using Microsoft.AspNetCore.Authorization;
using Training24Admin.Model;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionAnswerController : ControllerBase
    {
        private readonly QuestionAnswerBusiness QuestionAnswerBusiness;
        private readonly LessonBusiness LessonBusiness;

        public QuestionAnswerController(
            QuestionAnswerBusiness QuestionAnswerBusiness,
            LessonBusiness LessonBusiness
            )
        {
            this.QuestionAnswerBusiness = QuestionAnswerBusiness;
            this.LessonBusiness = LessonBusiness;
        }

        [HttpGet]
        public IActionResult Get(int pagenumber, int perpagerecord)
        {
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            PaginationModel paginationModel = new PaginationModel();
            try
            {
                paginationModel.pagenumber = pagenumber;
                paginationModel.perpagerecord = perpagerecord;
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                List<QuestionAnswer> QuestionAnswerList = new List<QuestionAnswer>();

                    int totalCount = QuestionAnswerBusiness.TotalQuestionAnswerCount();
                    QuestionAnswerList = QuestionAnswerBusiness.QuestionAnswerList(paginationModel);

                    successResponse.data = QuestionAnswerList;
                    successResponse.totalcount = totalCount;
                    successResponse.response_code = 0;
                    successResponse.message = "Answers detail";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);

            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                QuestionAnswer QuestionAnswer = QuestionAnswerBusiness.QuestionAnswerExistanceById(id);
                    if (QuestionAnswer == null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Answer not found.";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(404, unsuccessResponse);
                    }
                    else
                    {
                        successResponse.data = QuestionAnswer;
                        successResponse.response_code = 0;
                        successResponse.message = "Answer detail";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
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

        //[HttpPost]
        //public IActionResult Post([FromBody]QuestionAnswer CreateQuestionAnswerViewModel)
        //{
        //    SuccessResponse successResponse = new SuccessResponse();
        //    UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
        //    try
        //    {
        //        string Authorization = Request.Headers["Authorization"];
        //        TokenClaims tc = General.GetClaims(Authorization);

        //        if (ModelState.IsValid)
        //        {
        //                QuestionAnswer newQuestionAnswer = QuestionAnswerBusiness.Create(CreateQuestionAnswerViewModel, tc.Id);

        //                successResponse.response_code = 0;
        //                successResponse.message = "Answer added";
        //                successResponse.status = "Success";
        //                return StatusCode(200, successResponse);
        //        }
        //        else
        //        {
        //            return StatusCode(406, ModelState);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        unsuccessResponse.response_code = 2;
        //        unsuccessResponse.message = ex.Message;
        //        unsuccessResponse.status = "Failure";
        //        return StatusCode(500, unsuccessResponse);
        //    }
        //}

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]QuestionAnswer CreateQuestionAnswerViewModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                CreateQuestionAnswerViewModel.Id = id;
                QuestionAnswer QuestionAnswer = QuestionAnswerBusiness.QuestionAnswerExistanceById(id);
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    if (QuestionAnswer == null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Answer not found.";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(404, unsuccessResponse);
                    }
                    else
                    {

                            QuestionAnswer newQuestionAnswer = QuestionAnswerBusiness.Update(CreateQuestionAnswerViewModel, tc.Id);

                            successResponse.response_code = 0;
                            successResponse.message = "Answer updated";
                            successResponse.status = "Success";
                            return StatusCode(200, successResponse);
                    }
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                QuestionAnswer QuestionAnswer = QuestionAnswerBusiness.QuestionAnswerExistanceById(id);
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (QuestionAnswer == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Answer not found.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
                }
                else
                {

                        QuestionAnswerBusiness.Delete(QuestionAnswer, tc.Id);

                        successResponse.response_code = 0;
                        successResponse.message = "Answer Deleted";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);

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

        [HttpGet]
        [Route("GetAnswersByQuestion/{id}")]
        public IActionResult GetAnswersByQuestion(long id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                List<QuestionAnswer> lstAnswers = QuestionAnswerBusiness.GetAnswersByQuestionId(id);

                    successResponse.data = lstAnswers;
                    successResponse.response_code = 0;
                    successResponse.message = "Answers";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);

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