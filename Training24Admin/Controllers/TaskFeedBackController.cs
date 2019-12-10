using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Subscriptions;
using Trainning24.BL.ViewModels.TaskFeedBack;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class TaskFeedBackController : ControllerBase
    {
        private readonly TaskFeedbackBusiness _taskFeedBackBusiness;
        private IHostingEnvironment _hostingEnvironment;
        private readonly LessonBusiness _lessonBusiness;
        private readonly CourseGradeBusiness _courseGradeBusiness;
        private readonly UsersBusiness _usersBusiness;
        public TaskFeedBackController(TaskFeedbackBusiness taskFeedBackBusiness,
            LessonBusiness lessonBusiness,
            CourseGradeBusiness courseGradeBusiness,
            UsersBusiness usersBusiness,
            IHostingEnvironment hostingEnvironment)
        {
            _taskFeedBackBusiness = taskFeedBackBusiness;
            _lessonBusiness = lessonBusiness;
            _courseGradeBusiness = courseGradeBusiness;
            _hostingEnvironment = hostingEnvironment;
            _usersBusiness = usersBusiness;
        }

        [Authorize]
        [HttpPost("AddFeedback")]
        public IActionResult AddFeedback(CreateFeedBackDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                string Certificate = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx

                TaskFeedBack taskFeedBack = new TaskFeedBack();
                taskFeedBack.Title = dto.title;
                taskFeedBack.Description = dto.description;
                taskFeedBack.CategoryId = dto.categoryid;
                if (dto.courseid != null && dto.courseid != 0)
                {
                    taskFeedBack.GradeId = _courseGradeBusiness.GetGradeByCourseId(dto.courseid ?? 0).Gradeid;
                }
                taskFeedBack.CourseId = dto.courseid;
                taskFeedBack.ChapterId = dto.chapterid;
                taskFeedBack.LessonId = dto.lessonid;
                taskFeedBack.Time = dto.time;
                taskFeedBack.Device = dto.device;
                taskFeedBack.Version = dto.version;
                taskFeedBack.Status = 1;
                taskFeedBack.AppVersion = dto.appversion;
                taskFeedBack.OperatingSystem = dto.operatingsystem;
                taskFeedBack.CreatorUserId = int.Parse(tc.Id);
                taskFeedBack.CreationTime = DateTime.UtcNow.ToString();
                TaskFeedBack newFeedback = _taskFeedBackBusiness.AddFeedback(taskFeedBack);
                if (dto.filesids.Count > 0)
                {
                    foreach (var file in dto.filesids)
                    {
                        TaskFileFeedBack taskFileFeedBack = new TaskFileFeedBack();
                        taskFileFeedBack.TaskId = newFeedback.Id;
                        taskFileFeedBack.FileId = file;
                        taskFileFeedBack.CreationTime = DateTime.UtcNow.ToString();
                        taskFileFeedBack.CreatorUserId = int.Parse(tc.Id);
                        _taskFeedBackBusiness.AddFeedbackFiles(taskFileFeedBack);
                    }
                }
                successResponse.response_code = 0;
                successResponse.message = "Feedback added";
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
        [HttpGet("GetInQueueListApp")]
        public IActionResult GetInQueueListApp(int pagenumber, int perpagerecord)
        {
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            PaginationModel paginationModel = new PaginationModel
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord
            };
            try
            {
                string Certificate = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                var getTaskFeedBackList = _taskFeedBackBusiness.GetInQueueTaskListApp(paginationModel, Certificate, out int total, int.Parse(tc.Id));
                successResponse.data = getTaskFeedBackList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Course detail";
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
        [HttpGet("GetInProcessListApp")]
        public IActionResult GetInProcessListApp(int pagenumber, int perpagerecord)
        {
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            PaginationModel paginationModel = new PaginationModel
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord
            };
            try
            {
                string Certificate = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                var getTaskFeedBackList = _taskFeedBackBusiness.GetInProcessTaskListApp(paginationModel, Certificate, out int total, int.Parse(tc.Id));
                successResponse.data = getTaskFeedBackList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Course detail";
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
        [HttpGet("GetCompletedListApp")]
        public IActionResult GetCompletedListApp(int pagenumber, int perpagerecord)
        {
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            PaginationModel paginationModel = new PaginationModel
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord
            };
            try
            {
                string Certificate = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                var getTaskFeedBackList = _taskFeedBackBusiness.GetComplatedTaskListApp(paginationModel, Certificate, out int total, int.Parse(tc.Id));
                successResponse.data = getTaskFeedBackList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Course detail";
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
        [HttpGet("GetTaskFeedBackDetailsById")]
        public IActionResult GetTaskFeedBackDetailsById(long Id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);

            try
            {
                string Certificate = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                var getTaskFeedBackDetails = _taskFeedBackBusiness.GetTaskFeedBackDetailsById(Certificate, Id);
                if (getTaskFeedBackDetails != null)
                {
                    successResponse.data = getTaskFeedBackDetails;
                    successResponse.response_code = 0;
                    successResponse.message = "TaskFeedBack  detail";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 2;
                    unsuccessResponse.message = "TaskFeedBack Details not found";
                    unsuccessResponse.status = "Failure";
                    return StatusCode(602, unsuccessResponse);
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

        #region Admin Feedback
        [Authorize]
        [HttpPost("GetInQueueList")]
        public IActionResult GetInQueueList(FeedbackPaginationModel dto)
        {
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                string Certificate = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                var getTaskFeedBackList = _taskFeedBackBusiness.GetInQueueTaskList(dto, Certificate, out int total);
                successResponse.data = getTaskFeedBackList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Course detail";
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
        [HttpPost("GetInProcessList")]
        public IActionResult GetInProcessList(FeedbackPaginationModel dto)
        {
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                string Certificate = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                var getTaskFeedBackList = _taskFeedBackBusiness.GetInProcessTaskList(dto, Certificate, out int total);
                successResponse.data = getTaskFeedBackList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Course detail";
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
        [HttpPost("GetCompletedList")]
        public IActionResult GetCompletedList(FeedbackPaginationModel dto)
        {
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                string Certificate = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                var getTaskFeedBackList = _taskFeedBackBusiness.GetComplatedTaskList(dto, Certificate, out int total);
                successResponse.data = getTaskFeedBackList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Course detail";
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
        [HttpPost("GetArchivedList")]
        public IActionResult GetArchivedList(FeedbackPaginationModel dto)
        {
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                string Certificate = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                var getTaskFeedBackList = _taskFeedBackBusiness.GetArchivedTaskList(dto, Certificate, out int total);
                successResponse.data = getTaskFeedBackList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Course detail";
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
        [HttpPost("MoveInProgress")]
        public IActionResult MoveInProgress([FromBody]List<long> ids)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                if (ids.Count > 0)
                {
                    foreach (var id in ids)
                    {
                        _taskFeedBackBusiness.MoveInProgress(id, int.Parse(tc.Id));
                    }
                }
                successResponse.response_code = 0;
                successResponse.message = "Moved In Progress";
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
        [HttpPost("MoveInCompleted")]
        public IActionResult MoveInCompleted([FromBody]List<long> ids)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                if (ids.Count > 0)
                {
                    foreach (var id in ids)
                    {
                        _taskFeedBackBusiness.MoveInCompleted(id, int.Parse(tc.Id));
                    }
                }
                successResponse.response_code = 0;
                successResponse.message = "Moved In Completed";
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
        [HttpPost("MoveInArchived")]
        public IActionResult MoveInArchived([FromBody]List<long> ids)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                if (ids.Count > 0)
                {
                    foreach (var id in ids)
                    {
                        _taskFeedBackBusiness.MoveInArchived(id, int.Parse(tc.Id));
                    }
                }
                successResponse.response_code = 0;
                successResponse.message = "Moved In Completed";
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

        [Authorize]
        [HttpGet("GetFeedbackCategoryList")]
        public IActionResult GetFeedbackCategoryList()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                var data = _taskFeedBackBusiness.GetFeedBackCategoryList();
                successResponse.data = data;
                successResponse.response_code = 0;
                successResponse.message = "Task feedback category";
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
        [HttpGet("GetFeedbackUsersList")]
        public IActionResult GetFeedbackUsersList()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                List<UserDetailsDTO> userDetails = new List<UserDetailsDTO>();
                var data = _taskFeedBackBusiness.GetFeedbackUsersIds();
                foreach (var userid in data)
                {
                    User studentUser = _usersBusiness.GetUserbyId(userid);
                    if (studentUser != null)
                    {
                        UserDetailsDTO userDetailsDTO = new UserDetailsDTO
                        {
                            Id = studentUser.Id,
                            Username = studentUser.Username,
                            FullName = studentUser.FullName,
                            Email = studentUser.Email,
                            Bio = studentUser.Bio,
                            profilepicurl = studentUser.ProfilePicUrl
                        };

                        List<Role> userroles = _usersBusiness.Role(studentUser);
                        if (userroles != null)
                        {
                            List<long> roleids = new List<long>();
                            List<string> rolenames = new List<string>();
                            foreach (var role in userroles)
                            {
                                roleids.Add(role.Id);
                                rolenames.Add(role.Name);
                            }
                            userDetailsDTO.Roles = roleids;
                            userDetailsDTO.RoleName = rolenames;
                        }
                        userDetails.Add(userDetailsDTO);
                    }
                }
                successResponse.data = userDetails;
                successResponse.response_code = 0;
                successResponse.message = "Task feedback category";
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
        [HttpGet("AssignUserTask")]
        public IActionResult AssignUserTask(long taskid, long userid)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                TaskFeedBack newFeedback = _taskFeedBackBusiness.AssignUserTask(taskid, userid, long.Parse(tc.Id));
                successResponse.response_code = 0;
                successResponse.message = "User Assigned";
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