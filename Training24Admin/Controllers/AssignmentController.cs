using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Assignment;
using Trainning24.BL.ViewModels.AssignmentFile;
using Trainning24.BL.ViewModels.AssignmentStudent;
using Trainning24.BL.ViewModels.AssignmentSubmission;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AssignmentController : ControllerBase
    {
        private readonly AssignmentBusiness AssignmentBusiness;
        private readonly ChapterBusiness ChapterBusiness;
        private readonly UsersBusiness usersBusiness;
        private readonly LessonBusiness LessonBusiness;
        private IHostingEnvironment hostingEnvironment;
        private readonly AssignmentSubmissionBusinees _assignmentSubmissionBusinees;

        public AssignmentController
        (
            IHostingEnvironment hostingEnvironment,
            LessonBusiness LessonBusiness,
            AssignmentBusiness AssignmentBusiness,
            UsersBusiness usersBusiness,
            ChapterBusiness ChapterBusiness,
            AssignmentSubmissionBusinees assignmentSubmissionBusinees
        )
        {
            this.hostingEnvironment = hostingEnvironment;
            this.LessonBusiness = LessonBusiness;
            this.AssignmentBusiness = AssignmentBusiness;
            this.usersBusiness = usersBusiness;
            this.ChapterBusiness = ChapterBusiness;
            _assignmentSubmissionBusinees = assignmentSubmissionBusinees;
        }

        [HttpPost]
        public IActionResult Post(AddAssignmentModel dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //get claims after decoding id_token 
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                if (ModelState.IsValid)
                {
                    Assignment newAssignment = AssignmentBusiness.Create(dto, int.Parse(tc.Id));
                    List<ResponseAssignmentFileModel> ResponselessionFile = new List<ResponseAssignmentFileModel>();
                    if (dto.assignmentfiles.Count != 0)
                    {
                        List<AssignmentFile> lessionFileList = new List<AssignmentFile>();
                        foreach (var file in dto.assignmentfiles)
                        {
                            AssignmentFile AssignmentFile = new AssignmentFile
                            {
                                AssignmentId = newAssignment.Id,
                                CreationTime = DateTime.Now.ToString().ToString(),
                                CreatorUserId = int.Parse(tc.Id),
                                IsDeleted = false,
                                FileId = file.Id
                            };
                            lessionFileList.Add(AssignmentFile);
                        }
                        ResponselessionFile = AssignmentBusiness.CreateAssignmentFile(lessionFileList, Certificate);
                    }
                    Chapter newChapter = ChapterBusiness.getChapterById(int.Parse(newAssignment.ChapterId.ToString()));
                    ResponseChapterModel chapter = new ResponseChapterModel();
                    chapter.Id = int.Parse(newChapter.Id.ToString());
                    chapter.Name = newChapter.Name;
                    chapter.Code = newChapter.Code;
                    chapter.Courseid = newChapter.CourseId;
                    chapter.quizid = newChapter.QuizId;
                    ResponseAssignmentModel responseAssignmentModel = new ResponseAssignmentModel();
                    responseAssignmentModel.id = int.Parse(newAssignment.Id.ToString());
                    responseAssignmentModel.name = newAssignment.Name;
                    responseAssignmentModel.description = newAssignment.Description;
                    responseAssignmentModel.code = newAssignment.Code;
                    responseAssignmentModel.assignmentfiles = ResponselessionFile;
                    responseAssignmentModel.chapter = chapter;
                    successResponse.data = responseAssignmentModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Assignment Created";
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
        public IActionResult Put(AddAssignmentModel dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //get claims after decoding id_token 
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                if (ModelState.IsValid)
                {
                    Assignment newAssignment = AssignmentBusiness.Update(dto, int.Parse(tc.Id));
                    List<ResponseAssignmentFileModel> ResponselessionFile = new List<ResponseAssignmentFileModel>();
                    if (dto.assignmentfiles.Count != 0)
                    {
                        List<AssignmentFile> lessionFileList = new List<AssignmentFile>();
                        foreach (var file in dto.assignmentfiles)
                        {
                            UpdateAssignmentFileModel updateAssignmentFileModel = new UpdateAssignmentFileModel();
                            AssignmentFile AssignmentFile = new AssignmentFile
                            {
                                LastModificationTime = DateTime.Now.ToString(),
                                LastModifierUserId = int.Parse(tc.Id),
                                IsDeleted = false,
                                AssignmentId = newAssignment.Id,
                                FileId = file.Id
                            };
                            lessionFileList.Add(AssignmentFile);
                        }
                        ResponselessionFile = AssignmentBusiness.UpdateAssignmentFile(lessionFileList, Certificate);
                    }
                    Chapter newChapter = ChapterBusiness.getChapterById(int.Parse(newAssignment.ChapterId.ToString()));
                    ResponseChapterModel chapter = new ResponseChapterModel();
                    chapter.Id = int.Parse(newChapter.Id.ToString());
                    chapter.Name = newChapter.Name;
                    chapter.Code = newChapter.Code;
                    chapter.Courseid = newChapter.CourseId;
                    chapter.quizid = newChapter.QuizId;
                    ResponseAssignmentModel responseAssignmentModel = new ResponseAssignmentModel();
                    responseAssignmentModel.id = int.Parse(newAssignment.Id.ToString());
                    responseAssignmentModel.name = newAssignment.Name;
                    responseAssignmentModel.description = newAssignment.Description;
                    responseAssignmentModel.code = newAssignment.Code;
                    responseAssignmentModel.assignmentfiles = ResponselessionFile;
                    responseAssignmentModel.chapter = chapter;
                    successResponse.data = responseAssignmentModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Assignment updated";
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
            try
            {
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                string Authorization = Request.Headers["id_token"];
                //get claims after decoding id_token 
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    Assignment newAssignment = AssignmentBusiness.Delete(id, int.Parse(tc.Id));
                    successResponse.response_code = 0;
                    successResponse.message = "Assignment Deleted";
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
                //get claims after decoding id_token 
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                if (ModelState.IsValid)
                {
                    Assignment newAssignment = AssignmentBusiness.GetAssignmentById(id);
                    Chapter newChapter = ChapterBusiness.getChapterById(int.Parse(newAssignment.ChapterId.ToString()));
                    ResponseChapterModel chapter = new ResponseChapterModel();
                    chapter.Id = int.Parse(newChapter.Id.ToString());
                    chapter.Name = newChapter.Name;
                    chapter.Code = newChapter.Code;
                    chapter.Courseid = newChapter.CourseId;
                    chapter.quizid = newChapter.QuizId;
                    ResponseAssignmentModel responseAssignmentModel = new ResponseAssignmentModel();
                    responseAssignmentModel.id = int.Parse(newAssignment.Id.ToString());
                    responseAssignmentModel.name = newAssignment.Name;
                    responseAssignmentModel.description = newAssignment.Description;
                    responseAssignmentModel.code = newAssignment.Code;
                    responseAssignmentModel.assignmentfiles = AssignmentBusiness.GetAssignmentFilesByAssignmentId(newAssignment.Id, Certificate);
                    responseAssignmentModel.chapter = chapter;
                    successResponse.data = responseAssignmentModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Assignment Detail";
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
            PaginationModel paginationModel = new PaginationModel();
            paginationModel.pagenumber = pagenumber;
            paginationModel.perpagerecord = perpagerecord;
            paginationModel.search = search;
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //get claims after decoding id_token 
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                //if (tc.RoleId == "1" || tc.RoleId == "3")
                //{
                List<Assignment> AssignmentList = new List<Assignment>();
                List<ResponseAssignmentModel> AssignmentResponseList = new List<ResponseAssignmentModel>();
                AssignmentList = AssignmentBusiness.AssignmentList(paginationModel, out int total);
                List<ResponseAssignmentFileModel> ResponselessionFile = new List<ResponseAssignmentFileModel>();
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                foreach (Assignment newAssignment in AssignmentList)
                {
                    Chapter newChapter = ChapterBusiness.getChapterById(int.Parse(newAssignment.ChapterId.ToString()));
                    ResponseChapterModel chapter = new ResponseChapterModel();
                    chapter.Id = int.Parse(newChapter.Id.ToString());
                    chapter.Name = newChapter.Name;
                    chapter.Code = newChapter.Code;
                    chapter.Courseid = newChapter.CourseId;
                    chapter.quizid = newChapter.QuizId;
                    ResponseAssignmentModel responseAssignmentModel = new ResponseAssignmentModel();
                    responseAssignmentModel.id = int.Parse(newAssignment.Id.ToString());
                    responseAssignmentModel.name = newAssignment.Name;
                    responseAssignmentModel.description = newAssignment.Description;
                    responseAssignmentModel.code = newAssignment.Code;
                    responseAssignmentModel.assignmentfiles = AssignmentBusiness.GetAssignmentFilesByAssignmentId(newAssignment.Id, Certificate);
                    responseAssignmentModel.chapter = chapter;
                    AssignmentResponseList.Add(responseAssignmentModel);
                }
                successResponse.data = AssignmentResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Assignment Details";
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
        [Route("AddAssignmentStudent")]
        public IActionResult AddAssignmentStudent([FromBody] AddAssignmentStudentModel AddAssignmentStudentModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //get claims after decoding id_token 
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    AddAssignmentStudentModel.teacherid = long.Parse(tc.Id);
                    AssignmentStudent newAssignmentStudent = AssignmentBusiness.CreateAssignmentStudent(AddAssignmentStudentModel, int.Parse(tc.Id));
                    if (newAssignmentStudent == null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Duplicate entry.";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(422, unsuccessResponse);
                    }
                    ResponseAssignmentStudentModel responseBundleCourse = new ResponseAssignmentStudentModel();
                    responseBundleCourse.assignmentid = newAssignmentStudent.AssignmentId;
                    responseBundleCourse.studentid = newAssignmentStudent.StudentId;
                    responseBundleCourse.id = newAssignmentStudent.Id;
                    successResponse.data = responseBundleCourse;
                    successResponse.response_code = 0;
                    successResponse.message = "AssignmentStudent Created";
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

        [HttpDelete]
        [Route("DeleteAssignmentStudent/{id}")]
        public IActionResult DeleteAssignmentStudent(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            try
            {
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    AssignmentStudent newBundle = AssignmentBusiness.DeleteAssignmentStudent(id, int.Parse(tc.Id));
                    ResponseAssignmentStudentModel responseAssignmentStudent = new ResponseAssignmentStudentModel();
                    responseAssignmentStudent.assignmentid = newBundle.AssignmentId;
                    responseAssignmentStudent.studentid = newBundle.StudentId;
                    responseAssignmentStudent.id = newBundle.Id;
                    successResponse.data = responseAssignmentStudent;
                    successResponse.response_code = 0;
                    successResponse.message = "AssignmentStudent Deleted";
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
        [Route("GetAssignmentStudent")]
        public IActionResult GetAssignmentStudent(int pagenumber, int perpagerecord)
        {
            PaginationModel paginationModel = new PaginationModel();
            paginationModel.pagenumber = pagenumber;
            paginationModel.perpagerecord = perpagerecord;
            //get claims after decoding id_token 
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                tc.Id = LessonBusiness.getUserId(tc.sub);
                List<AssignmentStudent> bundleList = new List<AssignmentStudent>();
                List<ResponseAssignmentStudentModel> bundleResponseList = new List<ResponseAssignmentStudentModel>();
                bundleList = AssignmentBusiness.AssignmentStudentList(paginationModel, out int total);
                foreach (AssignmentStudent bundle in bundleList)
                {
                    ResponseAssignmentStudentModel ResponseBundleModel = new ResponseAssignmentStudentModel();
                    ResponseBundleModel.id = bundle.Id;
                    ResponseBundleModel.studentid = bundle.StudentId;
                    ResponseBundleModel.assignmentid = bundle.AssignmentId;
                    bundleResponseList.Add(ResponseBundleModel);
                }
                successResponse.data = bundleResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "AssignmentStudent Details";
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

        [HttpGet]
        [Route("GetAssignmentsByStudentId/{id}")]
        public IActionResult GetAssignmentsByStudentId(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    List<Assignment> assignmentList = AssignmentBusiness.GetAssignmentStudentById(id);
                    List<ResponseAssignmentModel> responseAssignmentModels = new List<ResponseAssignmentModel>();
                    foreach (var assignemnt in assignmentList)
                    {
                        ResponseAssignmentModel responseAssignmentModel = new ResponseAssignmentModel();
                        responseAssignmentModel.id = int.Parse(assignemnt.Id.ToString());
                        responseAssignmentModel.name = assignemnt.Name;
                        responseAssignmentModel.description = assignemnt.Description;
                        responseAssignmentModel.code = assignemnt.Code;
                        responseAssignmentModel.assignmentfiles = AssignmentBusiness.GetAssignmentFilesByAssignmentId(assignemnt.Id, Certificate);
                        responseAssignmentModels.Add(responseAssignmentModel);
                    }
                    successResponse.data = responseAssignmentModels;
                    successResponse.response_code = 0;
                    successResponse.message = "Assignment Detail";
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

        //To do
        [HttpGet]
        [Route("GetStudentsByAssignmentId/{id}")]
        public IActionResult GetStudentsByAssignmentId(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //get claims after decoding id_token 
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                if (ModelState.IsValid)
                {
                    List<DetailUser> studentList = AssignmentBusiness.GetStudentsByAssignmentId(id, Certificate);

                    successResponse.data = studentList;
                    successResponse.response_code = 0;
                    successResponse.message = "Assignment Detail";
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
        [HttpGet]
        [Route("AssignmentByCourseId/{id}")]
        public IActionResult AssignmentByCourseId(int Id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //get claims after decoding id_token 
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                int userId = Convert.ToInt32(tc.Id);
                List<CourseAssignmentModel> assignmentList = new List<CourseAssignmentModel>();
                if (tc.RoleName.Contains(LessonBusiness.getRoleType("4")))
                {
                    bool courseAssginToUser = AssignmentBusiness.CheckCourseAssginToUser(userId, Id);
                    if (courseAssginToUser)
                    {
                        assignmentList = AssignmentBusiness.GetAssignmentsByCourse(Id);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Course not assigned to user.";
                        unsuccessResponse.status = "unsuccess";
                        return StatusCode(422, unsuccessResponse);
                    }
                }
                successResponse.data = assignmentList;
                successResponse.response_code = 0;
                successResponse.message = "Assignment Detail";
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
        [HttpDelete]
        [Route("DeleteAssignmentFile/{id}")]
        public IActionResult DeleteAssignmentFile(long id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            try
            {
                var deletefile = AssignmentBusiness.DeleteAssignmentFileSingle(id);
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
                AssignmentSubmission assignmentSubmission = new AssignmentSubmission
                {
                    AssignmentId = dto.assignmentid,
                    UserId = userId,
                    IsSubmission = true
                };
                var submission = _assignmentSubmissionBusinees.Create(assignmentSubmission);
                List<SubmissionFileDTO> submissionFileDTO = new List<SubmissionFileDTO>();
                if (dto.files.Count > 0)
                {
                    List<AssignmentSubmissionFile> submissionFileList = new List<AssignmentSubmissionFile>();
                    foreach (var file in dto.files)
                    {
                        AssignmentSubmissionFile submissionFile = new AssignmentSubmissionFile
                        {
                            SubmissionId = submission.Id,
                            CreationTime = DateTime.Now.ToString().ToString(),
                            CreatorUserId = int.Parse(tc.Id),
                            IsDeleted = false,
                            FileId = file
                        };
                        submissionFileList.Add(submissionFile);
                    }
                    submissionFileDTO = _assignmentSubmissionBusinees.CreateSubmissionFile(submissionFileList);
                }
                ResponseAssignmentSubmissionDTO responseAssignmentSubmissionDTO = new ResponseAssignmentSubmissionDTO();
                responseAssignmentSubmissionDTO.id = int.Parse(submission.Id.ToString());
                responseAssignmentSubmissionDTO.assignmentid = submission.AssignmentId;
                responseAssignmentSubmissionDTO.userid = submission.UserId;
                responseAssignmentSubmissionDTO.score = submission.Score;
                responseAssignmentSubmissionDTO.issubmission = submission.IsSubmission;
                responseAssignmentSubmissionDTO.comment = submission.Comment;
                responseAssignmentSubmissionDTO.isapproved = submission.IsApproved;
                responseAssignmentSubmissionDTO.submissionfiles = submissionFileDTO;
                responseAssignmentSubmissionDTO.datecreated = submission.CreationTime;
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
                AssignmentSubmission assignmentSubmission = new AssignmentSubmission
                {
                    AssignmentId = dto.assignmentid,
                    UserId = dto.userid,
                    Comment = dto.comment,
                    TeacherId = dto.teacherid
                };
                if (assignmentSubmission.UserId != 0)
                {
                    var submission = _assignmentSubmissionBusinees.Create(assignmentSubmission);
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
                AssignmentSubmission assignmentSubmission = new AssignmentSubmission
                {
                    AssignmentId = dto.assignmentid,
                    UserId = dto.userid,
                    TeacherId = dto.teacherid,
                    IsApproved = dto.isapproved,
                    Score = dto.score,
                    Remark = dto.remark
                };
                var submission = _assignmentSubmissionBusinees.Create(assignmentSubmission);
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
                var submission = _assignmentSubmissionBusinees.GetAssignmentDetails(paginationModel, assignmentid, studentid);
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
                                                           submissionfiles = _assignmentSubmissionBusinees.GetSubmissionFilesById(x.Id, Certificate),
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
                var studentdetails = _assignmentSubmissionBusinees.GetStudentDetails(assignmentid);
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

        #endregion 
    }
}