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
using Trainning24.BL.ViewModels.Question;
using Trainning24.BL.ViewModels.LessonFile;
using System.Net.Http;
using System.IO;
using Trainning24.BL.ViewModels.QuestionFile;
using Trainning24.BL.ViewModels.QuestionAnswer;
using Google.Cloud.Storage.V1;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Hosting;
using Google.Apis.Auth.OAuth2;
using Trainning24.BL.ViewModels.Files;

namespace Training24Admin.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController : ControllerBase
    {
        private readonly QuestionBusiness QuestionBusiness;
        private readonly QuestionAnswerBusiness QuestionAnswerBusiness;
        private readonly LessonBusiness LessonBusiness;
        private IHostingEnvironment hostingEnvironment;
        private readonly FilesBusiness FilesBusiness;
        private readonly QuizBusiness QuizBusiness;

        public QuestionsController(
            IHostingEnvironment hostingEnvironment,
            QuestionBusiness QuestionBusiness,
            QuestionAnswerBusiness QuestionAnswerBusiness,
            LessonBusiness LessonBusiness,
            FilesBusiness FilesBusiness,
            QuizBusiness QuizBusiness
        )
        {
            this.hostingEnvironment = hostingEnvironment;
            this.QuestionBusiness = QuestionBusiness;
            this.QuestionAnswerBusiness = QuestionAnswerBusiness;
            this.LessonBusiness = LessonBusiness;
            this.FilesBusiness = FilesBusiness;
            this.QuizBusiness = QuizBusiness;
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
                List<Question> QuestionList = new List<Question>();
                //if (tc.RoleId == ((int)RoleType.Admin).ToString())
                //{
                int totalCount = QuestionBusiness.TotalQuestionCount();
                List<ResponseQuestionModel> lstData = QuestionBusiness.QuestionList(paginationModel).Select(f => new ResponseQuestionModel()
                {
                    Id = f.Id,
                    QuestionTypeId = f.QuestionTypeId,
                    QuestionText = f.QuestionText,
                    Explanation = f.Explanation,
                    IsMultiAnswer = f.IsMultiAnswer,
                    QuesionType = f.QuestionType.Code
                }).ToList();

                successResponse.data = lstData;
                successResponse.totalcount = totalCount;
                successResponse.response_code = 0;
                successResponse.message = "Questions";
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
                Question Question = QuestionBusiness.QuestionExistanceById(id);
                if (Question == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Question not found.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
                }
                else
                {
                    ResponseQuestionModel responseQuestionModel = new ResponseQuestionModel();
                    responseQuestionModel.Id = Question.Id;
                    responseQuestionModel.Explanation = Question.Explanation;
                    responseQuestionModel.IsMultiAnswer = Question.IsMultiAnswer;
                    responseQuestionModel.QuestionText = Question.QuestionText;
                    responseQuestionModel.QuestionTypeId = Question.QuestionTypeId;
                    responseQuestionModel.files = QuestionBusiness.GetLessionFilesByLessionId(Question.Id);
                    successResponse.data = responseQuestionModel;
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

        [HttpPost("UploadQuizDetail")]
        public async Task<object> Post()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);

            AddQuestionModel addQuestionModel = new AddQuestionModel();
            addQuestionModel.quizid = long.Parse(Request.Form["quizid"]);

            int noanswer = int.Parse(Request.Form["noanswer"]);

            string Authorization = Request.Headers["Authorization"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            for (int i = 0; i < 1; i++)
            {
                try
                {
                    QuestionModel questions = new QuestionModel();
                    questions.id = long.Parse(Request.Form["questions[" + i + "].id"]);
                    // var allFiles = Request.Form.Files.ToList();

                    questions.questiontypeid = long.Parse(Request.Form["questions[" + i + "].questiontypeid"]);
                    questions.questiontext = Request.Form["questions[" + i + "].questiontext"];
                    questions.explanation = Request.Form["questions[" + i + "].explanation"];
                    questions.ismultianswer = bool.Parse(Request.Form["questions[" + i + "].ismultianswer"]);
                    questions.questiontype = long.Parse(Request.Form["questions[" + i + "].questiontype"]);
                    List<UpdateQuestionFileModel> questionfiles = new List<UpdateQuestionFileModel>();

                    List<QuestionAnswerModel> answers = new List<QuestionAnswerModel>();

                    Question newQuestion = new Question();
                    if (questions.id == 0)
                    {
                        newQuestion = QuestionBusiness.CreateTest(questions, tc.Id); //inserting question
                        questions.id = newQuestion.Id;
                        QuestionBusiness.QuestionQuizMapping(newQuestion.Id, addQuestionModel.quizid);
                    }
                    else
                    {
                        newQuestion = QuestionBusiness.UpdateTest(questions, tc.Id); //updating question
                        questions.id = newQuestion.Id;
                    }

                    for (int j = 0; j < noanswer; j++)
                    {
                        if (string.IsNullOrEmpty(Request.Form["questions[" + i + "].answers[" + j + "].answer"]))
                        {
                            continue;
                        }
                        QuestionAnswerModel singleanswer = new QuestionAnswerModel();
                        singleanswer.id = long.Parse(Request.Form["questions[" + i + "].answers[" + j + "].id"]);
                        singleanswer.answer = Request.Form["questions[" + i + "].answers[" + j + "].answer"];
                        singleanswer.extratext = Request.Form["questions[" + i + "].answers[" + j + "].extratext"];
                        singleanswer.iscorrect = bool.Parse(Request.Form["questions[" + i + "].answers[" + j + "].iscorrect"]);
                        singleanswer.questionid = newQuestion.Id;

                        QuestionAnswer newQuestionAnswer = new QuestionAnswer();
                        if (singleanswer.id == 0)
                        {
                            newQuestionAnswer = QuestionAnswerBusiness.CreateTest(singleanswer, tc.Id); //inserting answer
                            singleanswer.id = newQuestionAnswer.Id;
                        }
                        else
                        {
                            newQuestionAnswer = QuestionAnswerBusiness.UpdateTest(singleanswer, tc.Id); //updating answer
                            singleanswer.id = newQuestionAnswer.Id;
                        }

                        List<UpdateQuestionFileModel> answerfiles = new List<UpdateQuestionFileModel>();

                        //QuestionAnswerBusiness.AnswerFileRemoving(newQuestionAnswer.Id);

                        //uploading answer images
                        //for (int k = 0; k < allFiles.Count; k++)
                        //{
                        //var singleFile = allFiles[k];
                        //if (singleFile.Name == "questions[" + i + "].answers[" + j + "].filename")
                        //{
                        //string fileName = "";
                        //IFormFile file = null;
                        //if (Request.Form.Files.Count != 0)
                        //    file = Request.Form.Files[k];
                        //var imageAcl = PredefinedObjectAcl.PublicRead;
                        //fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        //var ext = fileName.Substring(fileName.LastIndexOf("."));
                        //var extension = ext.ToLower();
                        //Guid imageGuid = Guid.NewGuid();
                        //fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;
                        //string mediaLink = "";
                        //var imageObject = await storage.UploadObjectAsync(
                        //    bucket: "t24-primary-image-storage",
                        //    objectName: fileName,
                        //    contentType: file.ContentType,
                        //    source: file.OpenReadStream(),
                        //    options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        //);
                        //mediaLink = imageObject.MediaLink;

                        if (!string.IsNullOrEmpty(Request.Form["questions[" + i + "].answers[" + j + "].filename"].ToString()))
                        {

                            string mediaLink = "";
                            var BucketName = General.getBucketName(3);

                            if (!string.IsNullOrEmpty(Request.Form["questions[" + i + "].answers[" + j + "].filename"].ToString()))
                            {
                                var storageObject = storage.GetObject(BucketName, Request.Form["questions[" + i + "].answers[" + j + "].filename"].ToString());
                                mediaLink = storageObject.MediaLink;
                            }

                            AddFilesModel FilesModel = new AddFilesModel();
                            FilesModel.Url = mediaLink;
                            FilesModel.Name = Request.Form["questions[" + i + "].answers[" + j + "].filename"].ToString();
                            FilesModel.FileName = Request.Form["questions[" + i + "].answers[" + j + "].filename"].ToString();
                            FilesModel.FileTypeId = 3;
                            //FilesModel.Id = long.Parse(Request.Form["id"]);

                            if (!string.IsNullOrEmpty(Request.Form["duration"]))
                                FilesModel.Duration = Request.Form["duration"];
                            if (!string.IsNullOrEmpty(Request.Form["totalpages"]))
                                FilesModel.TotalPages = int.Parse(Request.Form["totalpages"]);


                            Files newFiles = new Files();
                            if (FilesModel.Id == 0)
                            {
                                newFiles = FilesBusiness.Create(FilesModel, int.Parse(tc.Id));
                            }
                            else
                            {
                                newFiles = FilesBusiness.Update(FilesModel, int.Parse(tc.Id));
                            }

                            UpdateQuestionFileModel singleanswerFile = new UpdateQuestionFileModel();
                            singleanswerFile.fileid = newFiles.Id;
                            singleanswerFile.Url = newFiles.Url;
                            answerfiles.Add(singleanswerFile);
                            QuestionAnswerBusiness.AnswerFileMapping(newQuestionAnswer.Id, newFiles.Id);
                        }
                        //}
                        //}
                        singleanswer.images = answerfiles;
                        answers.Add(singleanswer);
                    }

                    questions.answers = answers;
                    addQuestionModel.questions = questions;


                    ////uploading question images
                    //for (int j = 0; j < allFiles.Count; j++)
                    //{
                    //var singleFile = allFiles[j];
                    //if (singleFile.Name == "questions[" + i + "].filename")
                    //{
                    //string questionfileName = "";
                    //IFormFile questionfile = null;
                    //if (Request.Form.Files.Count != 0)
                    //    questionfile = Request.Form.Files[j];
                    //var imageAcl = PredefinedObjectAcl.PublicRead;
                    //questionfileName = ContentDispositionHeaderValue.Parse(questionfile.ContentDisposition).FileName.Trim('"');
                    //var ext = questionfileName.Substring(questionfileName.LastIndexOf("."));
                    //var extension = ext.ToLower();
                    //Guid imageGuid = Guid.NewGuid();
                    //questionfileName = questionfileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;
                    //string mediaLink = "";
                    //var imageObject = await storage.UploadObjectAsync(
                    //    bucket: "t24-primary-image-storage",
                    //    objectName: questionfileName,
                    //    contentType: questionfile.ContentType,
                    //    source: questionfile.OpenReadStream(),
                    //    options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                    //);
                    //mediaLink = imageObject.MediaLink;
                    if (!string.IsNullOrEmpty(Request.Form["questions[" + i + "].filename"].ToString()))
                    {
                        string mediaLink = "";
                        var BucketName = General.getBucketName(3);

                        if (!string.IsNullOrEmpty(Request.Form["questions[" + i + "].filename"].ToString()))
                        {
                            var storageObject = storage.GetObject(BucketName, Request.Form["questions[" + i + "].filename"].ToString());
                            mediaLink = storageObject.MediaLink;
                        }

                        AddFilesModel FilesModel = new AddFilesModel();
                        if (!string.IsNullOrEmpty(Request.Form["description"]))
                            FilesModel.Description = Request.Form["description"];
                        FilesModel.Url = mediaLink;
                        FilesModel.Name = Request.Form["questions[" + i + "].filename"].ToString();
                        FilesModel.FileName = Request.Form["questions[" + i + "].filename"].ToString();
                        FilesModel.FileTypeId = 3;

                        if (!string.IsNullOrEmpty(Request.Form["duration"]))
                            FilesModel.Duration = Request.Form["duration"];
                        if (!string.IsNullOrEmpty(Request.Form["totalpages"]))
                            FilesModel.TotalPages = int.Parse(Request.Form["totalpages"]);

                        Files newFiles = new Files();
                        if (FilesModel.Id == 0)
                        {
                            newFiles = FilesBusiness.Create(FilesModel, int.Parse(tc.Id));
                        }
                        else
                        {
                            newFiles = FilesBusiness.Update(FilesModel, int.Parse(tc.Id));
                        }

                        UpdateQuestionFileModel singleQuestionFile = new UpdateQuestionFileModel();
                        singleQuestionFile.fileid = newFiles.Id;
                        singleQuestionFile.Url = newFiles.Url;
                        questionfiles.Add(singleQuestionFile);

                        QuestionBusiness.QuestionFileMapping(newQuestion.Id, newFiles.Id);
                    }
                    //}
                    //}
                    questions.images = questionfiles;


                }
                catch (Exception ex)
                {
                    unsuccessResponse.response_code = 2;
                    unsuccessResponse.message = ex.Message;
                    unsuccessResponse.status = "Failure";
                    return StatusCode(500, unsuccessResponse);
                }
            }

            return addQuestionModel;
        }

        [HttpGet("getQuizDetail/{id}")]
        public IActionResult getQuizDetail(long id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                NewResponseQuestionModel quizDetail = new NewResponseQuestionModel();
                quizDetail = QuizBusiness.getQuizDetail(id);
                if (quizDetail == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Quiz not found.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
                }
                else
                {
                    // ResponseQuestionModel responseQuestionModel = new ResponseQuestionModel();

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

        [HttpPost("DeleteQuizDetail")]
        public IActionResult DeleteQuizDetail(DeleteQuizDetailModel deleteQuizDetailModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                QuestionBusiness.DeleteQuizDetail(deleteQuizDetailModel, long.Parse(tc.Id));

                successResponse.response_code = 0;
                successResponse.message = "Record Deleted";
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

        [HttpPost]
        public IActionResult Post([FromBody]Question CreateQuestionViewModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                Question Question = QuestionBusiness.QuestionExistance(CreateQuestionViewModel);
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (ModelState.IsValid)
                {
                    if (Question != null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Question already exist";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(401, unsuccessResponse);
                    }
                    else
                    {
                        Question newQuestion = QuestionBusiness.Create(CreateQuestionViewModel, tc.Id);

                        successResponse.response_code = 0;
                        successResponse.message = "Question added";
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

        [HttpPost]
        [Route("AddQuestionAnswer")]
        public IActionResult AddQuestionAnswer([FromBody]QuestionModel CreateQuestionViewModel)
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
                    QuestionAnswerBusiness.Create(CreateQuestionViewModel, tc.Id);

                    successResponse.response_code = 0;
                    successResponse.message = "Question added";
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
        public IActionResult Put(int id, [FromBody]QuestionModel CreateQuestionViewModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                CreateQuestionViewModel.id = id;
                Question Question = QuestionBusiness.QuestionExistanceById(id);
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    if (Question == null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Question not found.";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(404, unsuccessResponse);
                    }
                    else
                    {

                        QuestionModel newQuestion = QuestionBusiness.Update(CreateQuestionViewModel, tc.Id);

                        successResponse.response_code = 0;
                        successResponse.message = "Question updated";
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
                Question Question = QuestionBusiness.QuestionExistanceById(id);
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (Question == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Question not found.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
                }
                else
                {

                    QuestionBusiness.Delete(Question, tc.Id);

                    successResponse.response_code = 0;
                    successResponse.message = "Question Deleted";
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
        [Route("GetQuestionsByQuiz/{id}")]
        public IActionResult GetQuestionsByQuiz(long id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                List<ResponseQuestionModel> lstAnswers = QuestionBusiness.GetQuestionsByQuizId(id).Select(f => new ResponseQuestionModel()
                {
                    Id = f.Id,
                    QuestionTypeId = f.QuestionTypeId,
                    QuestionText = f.QuestionText,
                    Explanation = f.Explanation,
                    IsMultiAnswer = f.IsMultiAnswer,
                    QuesionType = f.QuestionType.Code
                }).ToList();

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