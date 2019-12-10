using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Trainning24.BL.Business;
using Microsoft.AspNetCore.Authorization;
using Trainning24.Domain.Entity;
using Training24Admin.Model;
using Trainning24.BL.ViewModels.Users;

namespace Training24Admin.Controllers
{
    
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionTypeController : ControllerBase
    {
        private readonly QuestionTypeBusiness QuestionTypeBusiness;
        private readonly LessonBusiness LessonBusiness;

        public QuestionTypeController(
            LessonBusiness LessonBusiness,
            QuestionTypeBusiness QuestionTypeBusiness
            )
        {
            this.QuestionTypeBusiness = QuestionTypeBusiness;
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
                List<QuestionType> QuestionTypeList = new List<QuestionType>();
                //if (tc.RoleId == ((int)RoleType.Admin).ToString())
                //{
                    int totalCount = QuestionTypeBusiness.TotalQuestionTypeCount();
                    QuestionTypeList = QuestionTypeBusiness.QuestionTypeList(paginationModel);

                    successResponse.data = QuestionTypeList;
                    successResponse.totalcount = totalCount;
                    successResponse.response_code = 0;
                    successResponse.message = "QuestionTypes detail";
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
                //if (tc.RoleId == ((int)RoleType.Admin).ToString())
                //{
                QuestionType QuestionType = QuestionTypeBusiness.QuestionTypeExistanceById(id);
                    if (QuestionType == null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "QuestionType not found.";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(404, unsuccessResponse);
                    }
                    else
                    {
                        successResponse.data = QuestionType;
                        successResponse.response_code = 0;
                        successResponse.message = "QuestionType detail";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                //}
                //else
                //{
                //    unsuccessResponse.response_code = 1;
                //    unsuccessResponse.message = "You are not authorized.";
                //    unsuccessResponse.status = "Unsuccess";
                //    return StatusCode(401, unsuccessResponse);
                //}
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
        public IActionResult Post([FromBody]QuestionType CreateQuestionTypeViewModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                QuestionType QuestionType = QuestionTypeBusiness.QuestionTypeExistance(CreateQuestionTypeViewModel);
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (ModelState.IsValid)
                {
                    if (QuestionType != null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "QuestionType already exist";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(401, unsuccessResponse);
                    }
                    else
                    {
                        //if (tc.RoleId == ((int)RoleType.Admin).ToString())
                        //{
                            QuestionType newQuestionType = QuestionTypeBusiness.Create(CreateQuestionTypeViewModel, tc.Id);

                            successResponse.response_code = 0;
                            successResponse.message = "QuestionType added";
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

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]QuestionType CreateQuestionTypeViewModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                CreateQuestionTypeViewModel.Id = id;
                QuestionType QuestionType = QuestionTypeBusiness.QuestionTypeExistanceById(id);
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    if (QuestionType == null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Question Type not found.";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(404, unsuccessResponse);
                    }
                    else
                    {
                        //if (tc.RoleId == ((int)RoleType.Admin).ToString())
                        //{
                            QuestionType newQuestionType = QuestionTypeBusiness.Update(CreateQuestionTypeViewModel, tc.Id);

                            successResponse.response_code = 0;
                            successResponse.message = "QuestionType updated";
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
                QuestionType QuestionType = QuestionTypeBusiness.QuestionTypeExistanceById(id);
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (QuestionType == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Question Type not found.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
                }
                else
                {
                    //if (tc.RoleId == ((int)RoleType.Admin).ToString())
                    //{
                        QuestionTypeBusiness.Delete(QuestionType, tc.Id);

                        successResponse.response_code = 0;
                        successResponse.message = "Question Type Deleted";
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
