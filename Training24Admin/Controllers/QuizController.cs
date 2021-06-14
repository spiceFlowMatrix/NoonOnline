using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Question;
using Trainning24.BL.ViewModels.Quiz;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class QuizController : ControllerBase
    {
        private readonly QuizBusiness QuizBusiness;
        private readonly LessonBusiness LessonBusiness;
        private IHostingEnvironment hostingEnvironment;
        private readonly NotificationThreadsBusiness _notificationThreadsBusiness;
        public QuizController(
            LessonBusiness LessonBusiness,
            QuizBusiness QuizBusiness,
            IHostingEnvironment hostingEnvironment,
            NotificationThreadsBusiness notificationThreadsBusiness
            )
        {
            this.QuizBusiness = QuizBusiness;
            this.LessonBusiness = LessonBusiness;
            this.hostingEnvironment = hostingEnvironment;
            _notificationThreadsBusiness = notificationThreadsBusiness;
        }

        [HttpGet]
        public IActionResult Get(int pagenumber, int perpagerecord, string search)
        {
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            PaginationModel paginationModel = new PaginationModel();
            try
            {
                paginationModel.pagenumber = pagenumber;
                paginationModel.perpagerecord = perpagerecord;
                paginationModel.search = search;

                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                List<Quiz> QuizList = new List<Quiz>();

                int totalCount = QuizBusiness.TotalQuizCount();
                QuizList = QuizBusiness.QuizList(paginationModel);

                successResponse.data = QuizList;
                successResponse.totalcount = totalCount;
                successResponse.response_code = 0;
                successResponse.message = "Quizs detail";
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
                Quiz quiz = QuizBusiness.QuizExistanceById(id);
                if (quiz == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Quiz not found.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
                }
                else
                {
                    successResponse.data = quiz;
                    successResponse.response_code = 0;
                    successResponse.message = "Quiz detail";
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

        [HttpPost]
        public IActionResult Post([FromBody]AddQuizModel CreateQuizViewModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            try
            {
                Quiz quiz = new Quiz
                {
                    Code = CreateQuizViewModel.Code,
                    Name = CreateQuizViewModel.Name,
                    NumQuestions = CreateQuizViewModel.NumQuestions,
                    PassMark = CreateQuizViewModel.PassMark,
                    TimeOut = CreateQuizViewModel.TimeOut
                };
                //Quiz Quiz = QuizBusiness.QuizExistance(quiz);
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (ModelState.IsValid)
                {
                    //if (Quiz != null)
                    //{
                    //    unsuccessResponse.response_code = 1;
                    //    unsuccessResponse.message = "Quiz already exist";
                    //    unsuccessResponse.status = "Unsuccess";
                    //    return StatusCode(401, unsuccessResponse);
                    //}
                    //else
                    //{
                    Quiz newQuiz = QuizBusiness.Create(CreateQuizViewModel, tc.Id);
                    _notificationThreadsBusiness.SendNotificationOnQuizAdd(newQuiz, jsonPath, Convert.ToInt32(tc.Id));
                    successResponse.data = newQuiz;
                    successResponse.response_code = 0;
                    successResponse.message = "Quiz added";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                    //}
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
        [Route("AddQuizQuestions")]
        public IActionResult AddQuizQuestions([FromBody]QuizQuestionModel objModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (ModelState.IsValid)
                {

                    QuizBusiness.AddQuizQuestions(objModel, tc.Id);

                    successResponse.response_code = 0;
                    successResponse.message = "Quiz Questions added";
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

        [HttpPut("{id}")]
        public IActionResult Put(int id, [FromBody]Quiz CreateQuizViewModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            try
            {
                CreateQuizViewModel.Id = id;
                Quiz Quiz = QuizBusiness.QuizExistanceById(id);
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    if (Quiz == null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Quiz not found.";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(404, unsuccessResponse);
                    }
                    else
                    {

                        Quiz newQuiz = QuizBusiness.Update(CreateQuizViewModel, tc.Id);
                        _notificationThreadsBusiness.SendNotificationOnQuizUpdate(newQuiz, jsonPath, Convert.ToInt32(tc.Id));
                        successResponse.data = newQuiz;
                        successResponse.response_code = 0;
                        successResponse.message = "Quiz updated";
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
                Quiz Quiz = QuizBusiness.QuizExistanceById(id);
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (Quiz == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Quiz not found.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
                }
                else
                {

                    QuizBusiness.Delete(Quiz, tc.Id);

                    successResponse.response_code = 0;
                    successResponse.message = "Quiz Deleted";
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

        //[HttpGet("Quizpriview/{id}")]
        //public IActionResult Quizpriview(int id)
        //{
        //    SuccessResponse successResponse = new SuccessResponse();
        //    UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

        //    try
        //    {
        //        string Authorization = Request.Headers["Authorization"];
        //        TokenClaims tc = General.GetClaims(Authorization);

        //            Quiz quiz = QuizBusiness.QuizExistanceById(id);
        //            if (quiz == null)
        //            {
        //                unsuccessResponse.response_code = 1;
        //                unsuccessResponse.message = "Quiz not found.";
        //                unsuccessResponse.status = "Unsuccess";
        //                return StatusCode(404, unsuccessResponse);
        //            }
        //            else
        //            {
        //                QuizPreviewModel quizPreview = new QuizPreviewModel();
        //                quizPreview.id = quiz.Id;
        //                quizPreview.name = quiz.Name;
        //                quizPreview.numquestions = quiz.NumQuestions;
        //                quizPreview.passmark = quiz.PassMark;
        //                quizPreview.timeout = quiz.TimeOut;
        //                quizPreview.code = quiz.Code;
        //                quizPreview.questions = QuizBusiness.getQuestions(quiz.Id);

        //                successResponse.data = quizPreview;
        //                successResponse.response_code = 0;
        //                successResponse.message = "Quiz detail";
        //                successResponse.status = "Success";
        //                return StatusCode(200, successResponse);
        //            }

        //    }
        //    catch (Exception ex)
        //    {
        //        unsuccessResponse.response_code = 2;
        //        unsuccessResponse.message = ex.Message;
        //        unsuccessResponse.status = "Failure";
        //        return StatusCode(500, unsuccessResponse);
        //    }
        //}

        [HttpGet("Quizpriview/{id}")]
        public IActionResult Quizpriview(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                NewResponseQuestionModel1 quizDetail = new NewResponseQuestionModel1();
                quizDetail = QuizBusiness.getQuizDetail1(id, long.Parse(tc.Id));
                if (quizDetail == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Quiz not found.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
                }
                else
                {
                    ResponseQuestionModel responseQuestionModel = new ResponseQuestionModel();

                    successResponse.data = quizDetail;
                    successResponse.response_code = 0;
                    successResponse.message = "Question detail";
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

        [HttpGet("CompleteQuizFiles/{id}/{modifiedDate?}")]
        public IActionResult CompleteQuizFiles(int id, string modifiedDate = null)
        {
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                DateTime? lastModifiedDate = null;

                if (DateTime.TryParse(modifiedDate, out DateTime dt) == true)
                {
                    lastModifiedDate = dt;
                }
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                var fileBytes = QuizBusiness.GetCompleteQuizFiles(id, long.Parse(tc.Id), lastModifiedDate);
                return File(fileBytes, "application/octet-stream", $"{id}.zip");
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpGet("CompleteQuizPreview/{id}/{modifiedDate?}")]
        public IActionResult CompleteQuizPreview(int id, string modifiedDate = null)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                DateTime? lastModifiedDate = null;

                if (DateTime.TryParse(modifiedDate, out DateTime dt) == true)
                {
                    lastModifiedDate = dt;
                }

                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                NewResponseQuestionModel1 quizDetail = new NewResponseQuestionModel1();
                quizDetail = QuizBusiness.GetCompleteQuizDetail(id, long.Parse(tc.Id), lastModifiedDate);
                if (quizDetail == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Quiz not found.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
                }
                else
                {
                    ResponseQuestionModel responseQuestionModel = new ResponseQuestionModel();

                    successResponse.data = quizDetail;
                    successResponse.response_code = 0;
                    successResponse.message = "Question detail";
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
    }
}
