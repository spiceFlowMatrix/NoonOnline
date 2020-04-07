using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = nameof(SwaggerGrouping.Chapter))]
    //[Authorize]
    public class ChapterController : ControllerBase
    {
        private readonly ChapterBusiness ChapterBusiness;
        private readonly LessonBusiness LessonBusiness;
        private IHostingEnvironment hostingEnvironment;
        private readonly NotificationThreadsBusiness _notificationThreadsBusiness;

        public ChapterController
        (
            ChapterBusiness ChapterBusiness,
            LessonBusiness LessonBusiness,
            IHostingEnvironment hostingEnvironment,
            NotificationThreadsBusiness notificationThreadsBusiness
        )
        {
            this.ChapterBusiness = ChapterBusiness;
            this.LessonBusiness = LessonBusiness;
            this.hostingEnvironment = hostingEnvironment;
            _notificationThreadsBusiness = notificationThreadsBusiness;
        }

        [HttpPost("ChapterOrderChange")]
        public IActionResult ChapterOrderChange([FromBody]List<ChapterOrderChangeModel> sequencelist)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                //get claims after decoding id_token 
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);


                foreach (var sequence in sequencelist)
                {

                    Chapter newChapter = ChapterBusiness.getChapterById(int.Parse(sequence.chapterid.ToString()));
                    UpdateChapterModel updateChapterModel = new UpdateChapterModel();
                    updateChapterModel.id = int.Parse(newChapter.Id.ToString());
                    updateChapterModel.code = newChapter.Code;
                    updateChapterModel.courseid = newChapter.CourseId;
                    updateChapterModel.name = newChapter.Name;
                    updateChapterModel.quizid = newChapter.QuizId;
                    updateChapterModel.itemorder = sequence.itemorder;

                    Chapter updatedChapter = ChapterBusiness.Update(updateChapterModel, int.Parse(tc.Id));
                }

                successResponse.data = null;
                successResponse.response_code = 0;
                successResponse.message = "Chapter order changed";
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
        public IActionResult Post([FromBody] AddChapterModel ChapterModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            //get claims after decoding id_token 
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            try
            {
                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("1")))
                    {
                        Chapter newChapter = ChapterBusiness.Create(ChapterModel, int.Parse(tc.Id));
                        ResponseChapterModel responseChapterModel = new ResponseChapterModel();
                        responseChapterModel.Code = newChapter.Code;
                        responseChapterModel.Courseid = newChapter.Id;
                        responseChapterModel.Name = newChapter.Name;
                        responseChapterModel.itemorder = newChapter.ItemOrder;
                        if (newChapter.QuizId != null)
                            responseChapterModel.quizid = newChapter.QuizId;
                        responseChapterModel.Id = int.Parse(newChapter.Id.ToString());
                        _notificationThreadsBusiness.SendNotificationOnChapterAdd(newChapter, jsonPath, int.Parse(tc.Id));
                        successResponse.data = responseChapterModel;
                        successResponse.response_code = 0;
                        successResponse.message = "Chapter Created";
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
        public IActionResult Put(int id, UpdateChapterModel updateChapterModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            //get claims after decoding id_token 
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("1")))
                    {
                        updateChapterModel.id = id;
                        string oldchaptername = ChapterBusiness.GetChapterNameById(updateChapterModel.id);
                        Chapter newChapter = ChapterBusiness.Update(updateChapterModel, int.Parse(tc.Id));
                        ResponseChapterModel responseChapterModel = new ResponseChapterModel();
                        responseChapterModel.Name = newChapter.Name;
                        responseChapterModel.Code = newChapter.Code;
                        responseChapterModel.Courseid = newChapter.CourseId;
                        if (newChapter.QuizId != null)
                            responseChapterModel.quizid = newChapter.QuizId;
                        //If name changed then send push notification to teacher and student
                        if (oldchaptername != newChapter.Name)
                        {
                            _notificationThreadsBusiness.SendNotificationOnChapterUpdate(oldchaptername, newChapter, jsonPath, int.Parse(tc.Id));
                        }
                        successResponse.data = responseChapterModel;
                        successResponse.response_code = 0;
                        successResponse.message = "Chapter updated";
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


            //get claims after decoding id_token 
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            try
            {

                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("1")))
                    {
                        Chapter Chapter = ChapterBusiness.Delete(id, int.Parse(tc.Id));

                        ResponseChapterModel ResponseChapterModel = new ResponseChapterModel();
                        ResponseChapterModel.Id = int.Parse(Chapter.Id.ToString());
                        ResponseChapterModel.Name = Chapter.Name;

                        successResponse.data = ResponseChapterModel;
                        successResponse.response_code = 0;
                        successResponse.message = "Chapter Deleted";
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

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //get claims after decoding id_token 
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            try
            {

                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("1")))
                    {
                        ResponseChapterModel responseChapterModel = ChapterBusiness.getChapterByIdWithAssignments(id);

                        successResponse.data = responseChapterModel;
                        successResponse.response_code = 0;
                        successResponse.message = "Chapter Detail";
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

        [HttpGet]
        public IActionResult Get(int pagenumber, int perpagerecord, string search)
        {
            PaginationModel paginationModel = new PaginationModel();
            paginationModel.pagenumber = pagenumber;
            paginationModel.perpagerecord = perpagerecord;
            paginationModel.search = search;

            //get claims after decoding id_token 
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                List<Chapter> ChapterList = new List<Chapter>();
                List<ResponseChapterModel> ChapterResponseList = new List<ResponseChapterModel>();
                ChapterList = ChapterBusiness.ChapterList(paginationModel, out int total);

                foreach (Chapter newChapter in ChapterList)
                {
                    ResponseChapterModel responseChapterModel = new ResponseChapterModel();
                    responseChapterModel.Code = newChapter.Code;
                    responseChapterModel.Courseid = newChapter.CourseId;
                    responseChapterModel.Id = int.Parse(newChapter.Id.ToString());
                    responseChapterModel.Name = newChapter.Name;
                    if (newChapter.QuizId != null)
                        responseChapterModel.quizid = newChapter.QuizId;
                    ChapterResponseList.Add(responseChapterModel);
                }

                successResponse.data = ChapterResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Chapter Details";
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

        [HttpGet("GetChapterByCourseId/{id}")]
        public IActionResult GetLessonByCourseId(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    List<Chapter> newChapter = ChapterBusiness.GetChapterByCourseId(id);
                    List<ResponseChapterModelByCourseid> assignmentDetailModels = new List<ResponseChapterModelByCourseid>();


                    foreach (var chapter in newChapter)
                    {
                        ResponseChapterModelByCourseid rlModel = new ResponseChapterModelByCourseid
                        {
                            id = int.Parse(chapter.Id.ToString()),
                            Code = chapter.Code,
                            CourseId = chapter.CourseId,
                            ItemOrder = chapter.ItemOrder,
                            Name = chapter.Name
                        };

                        assignmentDetailModels.Add(rlModel);
                    }



                    successResponse.data = assignmentDetailModels;
                    successResponse.response_code = 0;
                    successResponse.message = "Lesson Detail";
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

        [Authorize]
        [HttpGet("GetChapterByCourse/{id}")]
        public IActionResult GetChapterByCourse(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                List<Chapter> chapterList = ChapterBusiness.GetChapterByCourseId(id);
                List<ChapterDTO> chapterDTOs = new List<ChapterDTO>();
                foreach (var chapter in chapterList)
                {
                    ChapterDTO chapterDTO = new ChapterDTO
                    {
                        Id = int.Parse(chapter.Id.ToString()),
                        courseid = chapter.CourseId,
                        Name = chapter.Name
                    };

                    chapterDTOs.Add(chapterDTO);
                }
                successResponse.data = chapterDTOs;
                successResponse.response_code = 0;
                successResponse.message = "Chapter Detail";
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
