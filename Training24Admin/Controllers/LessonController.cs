using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Lesson;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.LessonFile;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.BL.ViewModels.LessonAssignment;
using Trainning24.BL.ViewModels.LessonAssignmentFile;
using Trainning24.BL.ViewModels.AssignmentSubmission;

namespace Training24Admin.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize]
    public class LessonController : ControllerBase
    {
        private readonly LessonBusiness LessonBusiness;
        private readonly ChapterBusiness chapterBusiness;
        private readonly QuizBusiness QuizBusiness;
        private readonly ChapterQuizBusiness ChapterQuizBusiness;
        private IHostingEnvironment hostingEnvironment;
        private readonly NotificationThreadsBusiness _notificationThreadsBusiness;
        private readonly LessonAssignmentBusiness _lessonAssignmentBusiness;
        private readonly AssignmentBusiness _assignmentBusiness;
        private readonly LessonAssignmentSubmissionBusinees _lessonAssignmentSubmissionBusinees;
        private readonly UsersBusiness usersBusiness;
        public LessonController
        (
            IHostingEnvironment hostingEnvironment,
            LessonBusiness LessonBusiness,
            ChapterBusiness chapterBusiness,
            QuizBusiness QuizBusiness,
            ChapterQuizBusiness ChapterQuizBusiness,
            NotificationThreadsBusiness notificationThreadsBusiness,
            LessonAssignmentBusiness lessonAssignmentBusiness,
            AssignmentBusiness assignmentBusiness,
            LessonAssignmentSubmissionBusinees lessonAssignmentSubmissionBusinees,
            UsersBusiness usersBusiness
        )
        {
            this.hostingEnvironment = hostingEnvironment;
            this.LessonBusiness = LessonBusiness;
            this.chapterBusiness = chapterBusiness;
            this.QuizBusiness = QuizBusiness;
            this.ChapterQuizBusiness = ChapterQuizBusiness;
            _notificationThreadsBusiness = notificationThreadsBusiness;
            _lessonAssignmentBusiness = lessonAssignmentBusiness;
            _assignmentBusiness = assignmentBusiness;
            _lessonAssignmentSubmissionBusinees = lessonAssignmentSubmissionBusinees;
            this.usersBusiness = usersBusiness;
        }

        [HttpPost("LessonOrderChange")]
        public IActionResult LessonOrderChange([FromBody]LessonOrderChangeModel sequencedetail)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (sequencedetail.previousdetail != null)
                {
                    List<Lesson> previouslessonList = LessonBusiness.GetLessonByGivenChapterId(int.Parse(sequencedetail.newdetail.chapterid.ToString())).ToList();
                    foreach (var newlessonsequence in sequencedetail.newdetail.lessondetaillist)
                    {
                        if (newlessonsequence.type == 1)
                        {
                            Lesson lesson = LessonBusiness.getLessonById(int.Parse(newlessonsequence.id.ToString()));
                            if (lesson != null)
                            {
                                lesson.ItemOrder = newlessonsequence.itemorder;
                                lesson.ChapterId = sequencedetail.newdetail.chapterid;
                                lesson.LastModifierUserId = Convert.ToInt32(tc.Id);
                                lesson.LastModificationTime = DateTime.Now.ToString();
                                LessonBusiness.UpdateItemOrder(lesson);
                            }
                            else
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "lesson not found.";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(401, unsuccessResponse);
                            }
                        }
                        else if (newlessonsequence.type == 2)
                        {
                            ChapterQuiz quiz = ChapterQuizBusiness.CheckExistance(Convert.ToInt32(newlessonsequence.id));
                            if (quiz != null)
                            {
                                quiz.ItemOrder = newlessonsequence.itemorder;
                                quiz.ChapterId = sequencedetail.newdetail.chapterid;
                                quiz.LastModifierUserId = Convert.ToInt32(tc.Id);
                                quiz.LastModificationTime = DateTime.Now.ToString();
                                ChapterQuizBusiness.Update(quiz);
                            }
                            else
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "Quiz not found.";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(401, unsuccessResponse);
                            }
                        }
                        else if (newlessonsequence.type == 3)
                        {
                            Assignment assignment = _assignmentBusiness.GetAssignmentById(Convert.ToInt32(newlessonsequence.id));
                            if (assignment != null)
                            {
                                assignment.ItemOrder = newlessonsequence.itemorder;
                                assignment.ChapterId = sequencedetail.newdetail.chapterid;
                                assignment.LastModifierUserId = Convert.ToInt32(tc.Id);
                                assignment.LastModificationTime = DateTime.Now.ToString();
                                _assignmentBusiness.UpdateItemOrder(assignment);
                            }
                            else
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "assignment not found.";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(401, unsuccessResponse);
                            }
                        }
                    }
                }
                else
                {
                    List<Lesson> previouslessonList = LessonBusiness.GetLessonByGivenChapterId(int.Parse(sequencedetail.newdetail.chapterid.ToString())).ToList();
                    foreach (var newlessonsequence in sequencedetail.newdetail.lessondetaillist)
                    {
                        if (newlessonsequence.type == 1)
                        {
                            Lesson lesson = LessonBusiness.getLessonById(int.Parse(newlessonsequence.id.ToString()));
                            if (lesson != null)
                            {
                                lesson.ItemOrder = newlessonsequence.itemorder;
                                lesson.ChapterId = sequencedetail.newdetail.chapterid;
                                lesson.LastModifierUserId = Convert.ToInt32(tc.Id);
                                lesson.LastModificationTime = DateTime.Now.ToString();
                                LessonBusiness.UpdateItemOrder(lesson);
                            }
                            else
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "lesson not found.";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(401, unsuccessResponse);
                            }
                        }
                        else if (newlessonsequence.type == 2)
                        {
                            ChapterQuiz quiz = ChapterQuizBusiness.CheckExistance(Convert.ToInt32(newlessonsequence.id));
                            if (quiz != null)
                            {
                                quiz.ItemOrder = newlessonsequence.itemorder;
                                quiz.ChapterId = sequencedetail.newdetail.chapterid;
                                quiz.LastModifierUserId = Convert.ToInt32(tc.Id);
                                quiz.LastModificationTime = DateTime.Now.ToString();
                                ChapterQuizBusiness.Update(quiz);
                            }
                            else
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "Quiz not found.";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(401, unsuccessResponse);
                            }
                        }
                        else if (newlessonsequence.type == 3)
                        {
                            Assignment assignment = _assignmentBusiness.GetAssignmentById(Convert.ToInt32(newlessonsequence.id));
                            if (assignment != null)
                            {
                                assignment.ItemOrder = newlessonsequence.itemorder;
                                assignment.ChapterId = sequencedetail.newdetail.chapterid;
                                assignment.LastModifierUserId = Convert.ToInt32(tc.Id);
                                assignment.LastModificationTime = DateTime.Now.ToString();
                                _assignmentBusiness.UpdateItemOrder(assignment);
                            }
                            else
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "assignment not found.";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(401, unsuccessResponse);
                            }
                        }
                    }
                }
                successResponse.data = null;
                successResponse.response_code = 0;
                successResponse.message = "Lesson order changed";
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
        public IActionResult Post(AddLessonDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    AddLessonModel LessonModel = new AddLessonModel
                    {
                        name = dto.lesson.name,
                        code = dto.lesson.code,
                        description = dto.lesson.description,
                    };
                    if (dto.chapterid != 0)
                        LessonModel.chapterid = dto.chapterid;
                    List<AddLessonFileModelDTO> lessionfiles = dto.lesson.files;
                    Lesson newLesson = LessonBusiness.Create(LessonModel, int.Parse(tc.Id));
                    List<ResponseLessonFileModel> ResponselessionFile = new List<ResponseLessonFileModel>();
                    if (lessionfiles.Count != 0)
                    {
                        List<LessonFile> lessionFileList = new List<LessonFile>();
                        foreach (var file in lessionfiles)
                        {
                            LessonFile lessonFile = new LessonFile
                            {
                                CreationTime = DateTime.Now.ToString(),
                                CreatorUserId = int.Parse(tc.Id),
                                IsDeleted = false,
                                LessionId = newLesson.Id,
                                FileId = file.Id
                            };
                            lessionFileList.Add(lessonFile);
                        }
                        ResponselessionFile = LessonBusiness.CreateLessonFile(lessionFileList);
                    }
                    List<ResponseLessonAssignmentFileDTO> ResponseAssignmentFile = new List<ResponseLessonAssignmentFileDTO>();
                    ResponseLessionAssignmentDTO responseLessionAssignmentDTO = null;
                    if (!string.IsNullOrEmpty(dto.lesson.assignment.name))
                    {
                        dto.lesson.assignment.lessonid = newLesson.Id;
                        LessonAssignment newAssignment = _lessonAssignmentBusiness.Create(dto.lesson.assignment, int.Parse(tc.Id));
                        if (dto.lesson.assignment.files.Count != 0)
                        {
                            List<LessonAssignmentFile> lessonAssignmentFiles = new List<LessonAssignmentFile>();
                            foreach (var fileId in dto.lesson.assignment.files)
                            {
                                LessonAssignmentFile lessonAssignmentFile = new LessonAssignmentFile
                                {
                                    CreationTime = DateTime.Now.ToString(),
                                    CreatorUserId = int.Parse(tc.Id),
                                    IsDeleted = false,
                                    AssignmentId = newAssignment.Id,
                                    FileId = fileId
                                };
                                lessonAssignmentFiles.Add(lessonAssignmentFile);
                            }
                            ResponseAssignmentFile = _lessonAssignmentBusiness.CreateAssignmentFile(lessonAssignmentFiles);
                        }
                        responseLessionAssignmentDTO = new ResponseLessionAssignmentDTO
                        {
                            id = int.Parse(newAssignment.Id.ToString()),
                            name = newAssignment.Name,
                            code = newAssignment.Code,
                            description = newAssignment.Description,
                            assignmentfiles = ResponseAssignmentFile,
                        };
                    }
                    ResponseLessonModel responseLessonModel = new ResponseLessonModel
                    {
                        id = int.Parse(newLesson.Id.ToString()),
                        name = newLesson.Name,
                        code = newLesson.Code,
                        description = newLesson.Description,
                        itemorder = newLesson.ItemOrder,
                        assignment = responseLessionAssignmentDTO
                    };
                    Chapter chapter = new Chapter();
                    responseLessonModel.chapter = null;

                    if (newLesson.ChapterId.ToString() != null)
                    {
                        chapter = chapterBusiness.getChapterById(int.Parse(newLesson.ChapterId.ToString()));
                        ResponseChapterModel ResponseChapterModel = new ResponseChapterModel
                        {
                            Code = chapter.Code,
                            Courseid = chapter.CourseId,
                            Id = int.Parse(chapter.Id.ToString()),
                            Name = chapter.Name
                        };
                        responseLessonModel.chapter = ResponseChapterModel;
                    }
                    responseLessonModel.lessonfiles = ResponselessionFile;
                    _notificationThreadsBusiness.SendNotificationOnLessionAdd(newLesson, jsonPath, int.Parse(tc.Id));
                    successResponse.data = responseLessonModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Lesson Created";
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

        [HttpPut]
        public IActionResult Put(AddLessonDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    AddLessonModel updateLessonModel = new AddLessonModel
                    {
                        id = dto.lesson.id,
                        name = dto.lesson.name,
                        code = dto.lesson.code,
                        description = dto.lesson.description,
                        chapterid = dto.chapterid
                    };
                    Lesson newLesson = LessonBusiness.Update(updateLessonModel, int.Parse(tc.Id));
                    List<ResponseLessonFileModel> ResponselessionFile = new List<ResponseLessonFileModel>();
                    List<AddLessonFileModelDTO> lessionfiles = dto.lesson.files;
                    if (lessionfiles.Count != 0)
                    {
                        List<LessonFile> lessionFileList = new List<LessonFile>();
                        foreach (var file in lessionfiles)
                        {
                            LessonFile lessonFile = new LessonFile
                            {
                                LastModificationTime = DateTime.Now.ToString(),
                                LastModifierUserId = int.Parse(tc.Id),
                                IsDeleted = false,
                                LessionId = newLesson.Id,
                                FileId = file.Id
                            };
                            lessionFileList.Add(lessonFile);
                        }
                        ResponselessionFile = LessonBusiness.UpdateLessonFile(lessionFileList);
                    }
                    List<ResponseLessonAssignmentFileDTO> ResponseAssignmentFile = new List<ResponseLessonAssignmentFileDTO>();
                    ResponseLessionAssignmentDTO responseLessionAssignmentDTO = null;
                    if (!string.IsNullOrEmpty(dto.lesson.assignment.name))
                    {
                        dto.lesson.assignment.lessonid = newLesson.Id;
                        LessonAssignment newAssignment = null;
                        if (dto.lesson.assignment.id != 0)
                        {
                            newAssignment = _lessonAssignmentBusiness.Update(dto.lesson.assignment, int.Parse(tc.Id));
                        }
                        else
                        {
                            newAssignment = _lessonAssignmentBusiness.Create(dto.lesson.assignment, int.Parse(tc.Id));
                        }
                        if (dto.lesson.assignment.files.Count != 0)
                        {
                            List<LessonAssignmentFile> lessonAssignmentFiles = new List<LessonAssignmentFile>();
                            foreach (var fileId in dto.lesson.assignment.files)
                            {
                                LessonAssignmentFile lessonAssignmentFile = new LessonAssignmentFile
                                {
                                    CreationTime = DateTime.Now.ToString(),
                                    CreatorUserId = int.Parse(tc.Id),
                                    IsDeleted = false,
                                    AssignmentId = newAssignment.Id,
                                    FileId = fileId
                                };
                                lessonAssignmentFiles.Add(lessonAssignmentFile);
                            }
                            ResponseAssignmentFile = _lessonAssignmentBusiness.UpdateAssignmentFile(lessonAssignmentFiles);
                        }
                        responseLessionAssignmentDTO = new ResponseLessionAssignmentDTO
                        {
                            id = int.Parse(newAssignment.Id.ToString()),
                            name = newAssignment.Name,
                            code = newAssignment.Code,
                            description = newAssignment.Description,
                            assignmentfiles = ResponseAssignmentFile,
                        };
                    }
                    ResponseLessonModel responseLessonModel = new ResponseLessonModel
                    {
                        id = int.Parse(newLesson.Id.ToString()),
                        name = newLesson.Name,
                        code = newLesson.Code,
                        description = newLesson.Description,
                        itemorder = newLesson.ItemOrder,
                        assignment = responseLessionAssignmentDTO
                    };
                    Chapter chapter = new Chapter();
                    responseLessonModel.chapter = null;
                    if (newLesson.ChapterId.ToString() != null)
                    {
                        chapter = chapterBusiness.getChapterById(int.Parse(newLesson.ChapterId.ToString()));
                        ResponseChapterModel ResponseChapterModel = new ResponseChapterModel
                        {
                            Code = chapter.Code,
                            Courseid = chapter.CourseId,
                            Id = int.Parse(chapter.Id.ToString()),
                            Name = chapter.Name
                        };
                        responseLessonModel.chapter = ResponseChapterModel;
                    }
                    responseLessonModel.lessonfiles = ResponselessionFile;
                    _notificationThreadsBusiness.SendNotificationOnLessionUpdate(newLesson, jsonPath, int.Parse(tc.Id));
                    successResponse.data = responseLessonModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Lesson updated";
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

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
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

                    Lesson newLesson = LessonBusiness.Delete(id, int.Parse(tc.Id));

                    ResponseLessonModel responseLessonModel = new ResponseLessonModel
                    {
                        id = int.Parse(newLesson.Id.ToString())
                    };
                    responseLessonModel.id = int.Parse(newLesson.Id.ToString());
                    responseLessonModel.name = newLesson.Name;
                    responseLessonModel.code = newLesson.Code;
                    responseLessonModel.description = newLesson.Description;
                    Chapter chapter = new Chapter();
                    responseLessonModel.chapter = null;

                    if (newLesson.ChapterId != null)
                    {
                        chapter = chapterBusiness.getChapterById(int.Parse(newLesson.ChapterId.ToString()));
                        ResponseChapterModel ResponseChapterModel = new ResponseChapterModel
                        {
                            Code = chapter.Code,
                            Courseid = chapter.CourseId,
                            Id = int.Parse(chapter.Id.ToString()),
                            Name = chapter.Name
                        };
                        responseLessonModel.chapter = ResponseChapterModel;
                    }


                    successResponse.data = responseLessonModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Lesson Deleted";
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
                if (ModelState.IsValid)
                {
                    ResponseLessonModel responseLessonModel = null;
                    Lesson newLesson = LessonBusiness.getLessonById(id);
                    if (newLesson != null)
                    {
                        responseLessonModel = new ResponseLessonModel
                        {
                            id = int.Parse(newLesson.Id.ToString()),
                            name = newLesson.Name,
                            code = newLesson.Code,
                            description = newLesson.Description
                        };
                        LessonAssignment newAssignment = _lessonAssignmentBusiness.GetAssignmentByLesson(newLesson.Id);
                        if (newAssignment != null)
                        {
                            ResponseLessionAssignmentDTO responseLessionAssignmentDTO = new ResponseLessionAssignmentDTO
                            {
                                id = int.Parse(newAssignment.Id.ToString()),
                                name = newAssignment.Name,
                                code = newAssignment.Code,
                                description = newAssignment.Description,
                                assignmentfiles = _lessonAssignmentBusiness.GetAssignmentFilesByAssignmentId(newAssignment.Id),
                            };
                            responseLessonModel.assignment = responseLessionAssignmentDTO;
                        }
                    }
                    Chapter chapter = new Chapter();
                    responseLessonModel.chapter = null;
                    if (newLesson.ChapterId.ToString() != null)
                    {
                        chapter = chapterBusiness.getChapterById(int.Parse(newLesson.ChapterId.ToString()));
                        ResponseChapterModel ResponseChapterModel = new ResponseChapterModel
                        {
                            Code = chapter.Code,
                            Courseid = chapter.CourseId,
                            Id = int.Parse(chapter.Id.ToString()),
                            Name = chapter.Name
                        };
                        responseLessonModel.chapter = ResponseChapterModel;
                    }
                    responseLessonModel.lessonfiles = LessonBusiness.GetLessionFilesByLessionId(newLesson.Id);
                    successResponse.data = responseLessonModel;
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

        [HttpGet("GetLessonByCourseId/{id}")]
        public IActionResult GetLessonByCourseId(int id, int type)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    List<Lesson> LessonList = LessonBusiness.GetLessonByCourseId(id);
                    //List<ResponseLessonModelByChapter> responseLessonModel = new List<ResponseLessonModelByChapter>();

                    //foreach (var lesson in newLesson)
                    //{
                    //    ResponseLessonModelByChapter rlModel = new ResponseLessonModelByChapter
                    //    {
                    //        id = int.Parse(lesson.Id.ToString()),
                    //        name = lesson.Name,
                    //        code = lesson.Code,
                    //        description = lesson.Description
                    //    };
                    //    Chapter chapter = new Chapter();

                    //    if (lesson.ChapterId.ToString() != null)
                    //    {
                    //        chapter = chapterBusiness.getChapterById(int.Parse(lesson.ChapterId.ToString()));
                    //        ResponseChapterModel ResponseChapterModel = new ResponseChapterModel
                    //        {
                    //            Code = chapter.Code,
                    //            Courseid = chapter.CourseId,
                    //            Id = int.Parse(chapter.Id.ToString()),
                    //            Name = chapter.Name
                    //        };
                    //    }



                    //    responseLessonModel.Add(rlModel);
                    //}

                    List<ResponseLessonModel> LessonResponseList = new List<ResponseLessonModel>();
                    List<ResponseLessonFileModel> ResponselessionFile = new List<ResponseLessonFileModel>();
                    foreach (Lesson newLesson in LessonList)
                    {
                        if (type != 0)
                        {
                            List<ResponseLessonFileModel> responseLessonFileModels = LessonBusiness.GetLessionFilesByLessionId(newLesson.Id).Where(b => b.files.filetypeid == type).ToList();
                            if (responseLessonFileModels.Count != 0)
                            {
                                ResponseLessonModel responseLessonModel = new ResponseLessonModel
                                {
                                    id = int.Parse(newLesson.Id.ToString()),
                                    name = newLesson.Name,
                                    code = newLesson.Code,
                                    description = newLesson.Description
                                };
                                Chapter chapter = new Chapter();
                                responseLessonModel.chapter = null;
                                LessonResponseList.Add(responseLessonModel);
                            }
                        }
                        else
                        {
                            ResponseLessonModel responseLessonModel = new ResponseLessonModel
                            {
                                id = int.Parse(newLesson.Id.ToString()),
                                name = newLesson.Name,
                                code = newLesson.Code,
                                description = newLesson.Description
                            };
                            Chapter chapter = new Chapter();
                            responseLessonModel.chapter = null;
                            LessonResponseList.Add(responseLessonModel);
                        }
                    }

                    successResponse.data = LessonResponseList;
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

        [HttpGet]
        public IActionResult Get(int pagenumber, int perpagerecord, string search)
        {
            PaginationModel paginationModel = new PaginationModel
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord,
                search = search
            };


            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                List<Lesson> LessonList = new List<Lesson>();
                List<ResponseLessonModel> LessonResponseList = new List<ResponseLessonModel>();
                LessonList = LessonBusiness.LessonList(paginationModel, out int total);
                List<ResponseLessonFileModel> ResponselessionFile = new List<ResponseLessonFileModel>();

                foreach (Lesson newLesson in LessonList)
                {
                    ResponseLessonModel responseLessonModel = new ResponseLessonModel
                    {
                        id = int.Parse(newLesson.Id.ToString()),
                        name = newLesson.Name,
                        code = newLesson.Code,
                        description = newLesson.Description
                    };
                    Chapter chapter = new Chapter();
                    responseLessonModel.chapter = null;

                    if (newLesson.ChapterId.ToString() != null)
                    {
                        chapter = chapterBusiness.getChapterById(int.Parse(newLesson.ChapterId.ToString()));
                        ResponseChapterModel ResponseChapterModel = new ResponseChapterModel
                        {
                            Code = chapter.Code,
                            Courseid = chapter.CourseId,
                            Id = int.Parse(chapter.Id.ToString()),
                            Name = chapter.Name
                        };
                        responseLessonModel.chapter = ResponseChapterModel;
                    }
                    responseLessonModel.lessonfiles = ResponselessionFile;
                    responseLessonModel.lessonfiles = LessonBusiness.GetLessionFilesByLessionId(newLesson.Id);
                    LessonResponseList.Add(responseLessonModel);
                }

                successResponse.data = LessonResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Lesson Details";
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

        [HttpDelete("DeleteAssignmentFile/{id}")]
        public IActionResult DeleteAssignmentFile(long id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            try
            {
                var deletefile = _lessonAssignmentBusiness.DeleteAssignmentFileSingle(id);
                successResponse.response_code = 0;
                successResponse.message = "File deleted";
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

        #region Assignment Submission 
        [Authorize]
        [HttpPost]
        [Route("AssginmentSubmission")]
        public IActionResult AssignmentSubmission(AssignmentSubmissionDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                int userId = Convert.ToInt32(tc.Id);
                LessonAssignmentSubmission assignmentSubmission = new LessonAssignmentSubmission
                {
                    AssignmentId = dto.assignmentid,
                    UserId = userId,
                    IsSubmission = true
                };
                var submission = _lessonAssignmentSubmissionBusinees.Create(assignmentSubmission);
                List<SubmissionFileDTO> submissionFileDTO = new List<SubmissionFileDTO>();
                if (dto.files.Count > 0)
                {
                    List<LessonAssignmentSubmissionFile> submissionFileList = new List<LessonAssignmentSubmissionFile>();
                    foreach (var file in dto.files)
                    {
                        LessonAssignmentSubmissionFile submissionFile = new LessonAssignmentSubmissionFile
                        {
                            SubmissionId = submission.Id,
                            CreationTime = DateTime.Now.ToString().ToString(),
                            CreatorUserId = int.Parse(tc.Id),
                            IsDeleted = false,
                            FileId = file
                        };
                        submissionFileList.Add(submissionFile);
                    }
                    submissionFileDTO = _lessonAssignmentSubmissionBusinees.CreateSubmissionFile(submissionFileList);
                }
                ResponseAssignmentSubmissionDTO responseAssignmentSubmissionDTO = new ResponseAssignmentSubmissionDTO();
                responseAssignmentSubmissionDTO.id = int.Parse(submission.Id.ToString());
                responseAssignmentSubmissionDTO.assignmentid = submission.AssignmentId;
                responseAssignmentSubmissionDTO.userid = submission.UserId;
                responseAssignmentSubmissionDTO.score = submission.Score;
                responseAssignmentSubmissionDTO.issubmission = submission.IsSubmission;
                responseAssignmentSubmissionDTO.comment = submission.Comment;
                responseAssignmentSubmissionDTO.isapproved = submission.IsApproved;
                responseAssignmentSubmissionDTO.datecreated = submission.CreationTime;
                responseAssignmentSubmissionDTO.submissionfiles = submissionFileDTO;
                successResponse.data = responseAssignmentSubmissionDTO;
                successResponse.response_code = 0;
                successResponse.message = "submission Created";
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

        [Authorize]
        [HttpPost]
        [Route("AddAssignmentComment")]
        public IActionResult AddAssignmentComment(AssignmentSubmissionCommentDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                LessonAssignmentSubmission assignmentSubmission = new LessonAssignmentSubmission
                {
                    AssignmentId = dto.assignmentid,
                    UserId = dto.userid,
                    Comment = dto.comment,
                    TeacherId = dto.teacherid
                };
                if (assignmentSubmission.UserId != 0)
                {
                    var submission = _lessonAssignmentSubmissionBusinees.Create(assignmentSubmission);
                    ResponseAssignmentSubmissionDTO responseAssignmentSubmissionDTO = new ResponseAssignmentSubmissionDTO();
                    responseAssignmentSubmissionDTO.id = int.Parse(submission.Id.ToString());
                    responseAssignmentSubmissionDTO.assignmentid = submission.AssignmentId;
                    responseAssignmentSubmissionDTO.userid = submission.UserId;
                    responseAssignmentSubmissionDTO.score = submission.Score;
                    responseAssignmentSubmissionDTO.issubmission = submission.IsSubmission;
                    responseAssignmentSubmissionDTO.comment = submission.Comment;
                    responseAssignmentSubmissionDTO.isapproved = submission.IsApproved;
                    responseAssignmentSubmissionDTO.datecreated = submission.CreationTime;
                    successResponse.data = responseAssignmentSubmissionDTO;
                }
                successResponse.response_code = 0;
                successResponse.message = "comment Created";
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

        [Authorize]
        [HttpPost]
        [Route("ApprovedSubmission")]
        public IActionResult ApprovedSubmission(ApprovedAssignmentSubmissionDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                LessonAssignmentSubmission assignmentSubmission = new LessonAssignmentSubmission
                {
                    AssignmentId = dto.assignmentid,
                    UserId = dto.userid,
                    TeacherId = dto.teacherid,
                    IsApproved = dto.isapproved,
                    Score = dto.score,
                    Remark = dto.remark
                };
                var submission = _lessonAssignmentSubmissionBusinees.Create(assignmentSubmission);
                ResponseAssignmentSubmissionDTO responseAssignmentSubmissionDTO = new ResponseAssignmentSubmissionDTO();
                responseAssignmentSubmissionDTO.id = int.Parse(submission.Id.ToString());
                responseAssignmentSubmissionDTO.assignmentid = submission.AssignmentId;
                responseAssignmentSubmissionDTO.userid = submission.UserId;
                responseAssignmentSubmissionDTO.score = submission.Score;
                responseAssignmentSubmissionDTO.issubmission = submission.IsSubmission;
                responseAssignmentSubmissionDTO.comment = submission.Comment;
                responseAssignmentSubmissionDTO.isapproved = submission.IsApproved;
                responseAssignmentSubmissionDTO.datecreated = submission.CreationTime;
                successResponse.data = responseAssignmentSubmissionDTO;
                successResponse.response_code = 0;
                successResponse.message = "submission approved";
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

        [Authorize]
        [HttpGet]
        [Route("GetSubmissionDetails")]
        public IActionResult GetSubmissionDetails(int pagenumber, int perpagerecord, int assignmentid, int studentid)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                PaginationModel paginationModel = new PaginationModel
                {
                    pagenumber = pagenumber,
                    perpagerecord = perpagerecord
                };
                AssignmentSubmissionDetailsDTO assignmentSubmissionDTO = new AssignmentSubmissionDetailsDTO();
                var submission = _lessonAssignmentSubmissionBusinees.GetAssignmentDetails(paginationModel, assignmentid, studentid);
                if (submission != null)
                {
                    if (submission.Any(p => p.IsApproved))
                    {
                        assignmentSubmissionDTO.isapproved = true;
                    }
                    assignmentSubmissionDTO.details = (from x in submission
                                                       select new AssignmentSubmissionDetailDTO
                                                       {
                                                           id = x.Id,
                                                           comment = x.Comment,
                                                           isapproved = x.IsApproved,
                                                           assignmentid = x.AssignmentId,
                                                           issubmission = x.IsSubmission,
                                                           userid = x.UserId,
                                                           remark = x.Remark,
                                                           score = x.Score,
                                                           submissionfiles = _lessonAssignmentSubmissionBusinees.GetSubmissionFilesById(x.Id, Certificate),
                                                           user = usersBusiness.GetUserBasicDetails(x.TeacherId, x.UserId, Certificate),
                                                           datecreated = x.CreationTime
                                                       }).ToList();
                }
                successResponse.data = assignmentSubmissionDTO;
                successResponse.response_code = 0;
                successResponse.message = "submission details get";
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

        [Authorize]
        [HttpGet]
        [Route("GetStudentDetails")]
        public IActionResult GetStudentDetails(int assignmentid)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                var studentdetails = _lessonAssignmentSubmissionBusinees.GetStudentDetails(assignmentid);
                successResponse.data = studentdetails;
                successResponse.response_code = 0;
                successResponse.message = "student get";
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

        [Authorize]
        [HttpGet("GetLessonByCourse/{id}")]
        public IActionResult GetLessonByCourse(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                List<Lesson> LessonList = LessonBusiness.GetLessonByCourseId(id);
                List<LessonDTO> lessonDTOs = new List<LessonDTO>();
                foreach (Lesson lesson in LessonList)
                {
                    LessonDTO lessonDTO = new LessonDTO();
                    lessonDTO.id = lesson.Id;
                    lessonDTO.name = lesson.Name;
                    lessonDTO.code = lesson.Code;
                    lessonDTO.description = lesson.Description;
                    lessonDTOs.Add(lessonDTO);
                }
                successResponse.data = lessonDTOs;
                successResponse.response_code = 0;
                successResponse.message = "Lesson Detail";
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
        #endregion 
    }
}