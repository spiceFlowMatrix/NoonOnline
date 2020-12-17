using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.AppTimeTrack;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.ProgressSyncDTO;
using Trainning24.BL.ViewModels.Subscriptions;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [Authorize]
    public class ParentControlController : ControllerBase
    {
        private readonly LessonBusiness _lessonBusiness;
        private readonly StudParentBusiness _studParentBusiness;
        private readonly UsersBusiness _usersBusiness;
        private readonly AppTimeTrackBusiness _appTimeTrackBusiness;
        private readonly UserQuizResultBusiness _userQuizResultBusiness;
        private readonly StudentCourseBusiness _studentCourseBusiness;
        private readonly CourseBusiness _courseBusiness;
        private readonly AssignmentSubmissionBusinees _assignmentSubmissionBusinees;
        private readonly LessonProgressBusiness _lessonProgressBusiness;
        private readonly AssignmentBusiness _assignmentBusiness;
        private readonly ChapterQuizBusiness _chapterQuizBusiness;
        public ParentControlController(
            LessonBusiness lessonBusiness,
            StudParentBusiness studParentBusiness,
            UsersBusiness usersBusiness,
            AppTimeTrackBusiness appTimeTrackBusiness,
            UserQuizResultBusiness userQuizResultBusiness,
            StudentCourseBusiness studentCourseBusiness,
            CourseBusiness courseBusiness,
            AssignmentSubmissionBusinees assignmentSubmissionBusinees,
            LessonProgressBusiness lessonProgressBusiness,
            AssignmentBusiness assignmentBusiness,
            ChapterQuizBusiness chapterQuizBusiness
            )
        {
            _lessonBusiness = lessonBusiness;
            _studParentBusiness = studParentBusiness;
            _usersBusiness = usersBusiness;
            _appTimeTrackBusiness = appTimeTrackBusiness;
            _userQuizResultBusiness = userQuizResultBusiness;
            _studentCourseBusiness = studentCourseBusiness;
            _courseBusiness = courseBusiness;
            _assignmentSubmissionBusinees = assignmentSubmissionBusinees;
            _lessonProgressBusiness = lessonProgressBusiness;
            _assignmentBusiness = assignmentBusiness;
            _chapterQuizBusiness = chapterQuizBusiness;
        }

        [HttpGet]
        [Route("GetStudentList/{Id}")]
        public IActionResult GetStudentList(int Id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = _lessonBusiness.getUserId(tc.sub);
                if (tc.RoleName.Contains(General.getRoleType("18")))
                {
                    List<UserDetailsDTO> userDetails = new List<UserDetailsDTO>();
                    List<StudParent> studParent = _studParentBusiness.GetStudentListByParentId(long.Parse(tc.Id));
                    if (studParent.Count > 0)
                    {
                        foreach (var stud in studParent)
                        {
                            User studentUser = _usersBusiness.GetUserbyId(stud.StudentId);
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
                    }
                    successResponse.data = userDetails;
                    successResponse.response_code = 0;
                    successResponse.message = "student details";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "you are not authorized.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(401, unsuccessResponse);
                }
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost]
        [Route("GetStudentCourse")]
        public IActionResult GetStudentCourse([FromBody]ParentPostDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = _lessonBusiness.getUserId(tc.sub);
                List<UserCourseDTO> userCourseDTOs = new List<UserCourseDTO>();
                if (tc.RoleName.Contains(General.getRoleType("18")))
                {
                    var courseList = _studentCourseBusiness.GetAllCourseByUserId(dto.id);
                    if (courseList.Count > 0)
                    {
                        foreach (var courseid in courseList)
                        {
                            Course course = _courseBusiness.getCourseById(courseid);
                            if (course != null)
                            {
                                UserCourseDTO userCourseDTO = new UserCourseDTO();
                                userCourseDTO.id = course.Id;
                                userCourseDTO.name = course.Name;
                                userCourseDTOs.Add(userCourseDTO);
                            }
                        }
                    }
                    successResponse.data = userCourseDTOs;
                    successResponse.response_code = 0;
                    successResponse.message = "course details";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "you are not authorized.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(401, unsuccessResponse);
                }
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost]
        [Route("GetTimeActivity")]
        public IActionResult GetTimeActivity([FromBody]ParentPostDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                List<AppTimeTrack> appTimeTracks = _appTimeTrackBusiness.GetTimeTrackingByUserId(dto.id);
                if (appTimeTracks.Count > 0)
                {
                    List<TimeActivityDTO> timeActivityDTOs = (from x in appTimeTracks
                                                              select new TimeActivityDTO
                                                              {
                                                                  activitytime = Convert.ToDateTime(x.ActivityTime),
                                                                  outtime = Convert.ToDateTime(x.Outtime)
                                                              }).ToList();
                    var activityGroup = timeActivityDTOs.Where(x => x.activitytime > DateTime.UtcNow.AddMonths(-dto.month)).GroupBy(x => new { x.activitytime.Month, x.activitytime.Year }).ToList();
                    AllTimeActivityDTO allTimeActivityDTO = new AllTimeActivityDTO();
                    if (activityGroup.Count > 0)
                    {
                        List<TimeChartDTO> timeChartDTO = new List<TimeChartDTO>();
                        foreach (var dt in activityGroup)
                        {
                            TimeChartDTO timeChart = new TimeChartDTO();
                            timeChart.x = dt.Key.Month;
                            double timeCount = 0;
                            foreach (var time in dt)
                            {
                                TimeSpan diff = time.outtime - time.activitytime;
                                double hours = diff.TotalHours;
                                timeCount += hours;
                            }
                            if (timeCount >= 0)
                                timeChart.y = timeCount;
                            timeChartDTO.Add(timeChart);
                        }
                        allTimeActivityDTO.mo = timeChartDTO.OrderBy(x => x.x).ToList();
                    }
                    var activityGroupDaliy = timeActivityDTOs.Where(x => x.activitytime > DateTime.UtcNow.AddMonths(-dto.month))
                        .GroupBy(x => new { x.activitytime.Date }).ToList();

                    if (activityGroupDaliy.Count > 0)
                    {
                        List<TimeChartDayDTO> timeChartDTO = new List<TimeChartDayDTO>();
                        foreach (var dt in activityGroupDaliy)
                        {
                            TimeChartDayDTO timeChart = new TimeChartDayDTO();
                            timeChart.x = dt.Key.Date;
                            double timeCount = 0;
                            foreach (var time in dt)
                            {
                                TimeSpan diff = time.outtime - time.activitytime;
                                double hours = diff.TotalHours;
                                timeCount += hours;
                            }
                            if (timeCount >= 0)
                                timeChart.y = timeCount;
                            timeChartDTO.Add(timeChart);
                        }
                        allTimeActivityDTO.da = timeChartDTO.OrderBy(x => x.x.Date).ToList();
                    }
                    successResponse.data = allTimeActivityDTO;
                }
                successResponse.response_code = 0;
                successResponse.message = "chart details";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost]
        [Route("GetQuizActivityPoint")]
        public IActionResult GetQuizActivityPoint([FromBody]ParentPostDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                List<UserQuizResult> quizProgresses = _userQuizResultBusiness.GetQuizProgressByCourseId(dto.id, dto.courseid);
                if (quizProgresses.Count > 0)
                {
                    List<QuizChartDTO> quizChartDTOs = (from x in quizProgresses
                                                        select new QuizChartDTO
                                                        {
                                                            progress = x.Score,
                                                            datetime = Convert.ToDateTime(x.CreationTime)
                                                        }).ToList();
                    var activityGroup = quizChartDTOs.GroupBy(x => new { x.datetime.Month, x.datetime.Year }).ToList();
                    AllTimeActivityDTO allTimeActivityDTO = new AllTimeActivityDTO();
                    if (activityGroup.Count > 0)
                    {
                        List<TimeChartDTO> timeChartDTO = new List<TimeChartDTO>();
                        foreach (var dt in activityGroup)
                        {
                            TimeChartDTO timeChart = new TimeChartDTO();
                            timeChart.x = dt.Key.Month;
                            double progress = 0;
                            foreach (var time in dt)
                            {
                                progress += time.progress;
                            }
                            var percentCount = progress / dt.Count();
                            //var pointCount = percentCount / 10;
                            timeChart.y = percentCount;
                            timeChartDTO.Add(timeChart);
                        }
                        allTimeActivityDTO.mo = timeChartDTO.OrderBy(x => x.x).ToList();
                    }

                    var activityGroupDaliy = quizChartDTOs.GroupBy(x => new { x.datetime.Date }).ToList();

                    if (activityGroupDaliy.Count > 0)
                    {
                        List<TimeChartDayDTO> timeChartDTO = new List<TimeChartDayDTO>();
                        foreach (var dt in activityGroupDaliy)
                        {
                            TimeChartDayDTO timeChart = new TimeChartDayDTO();
                            timeChart.x = dt.Key.Date;
                            double progress = 0;
                            foreach (var time in dt)
                            {
                                progress += time.progress;
                            }
                            var percentCount = progress / dt.Count();
                            //var pointCount = percentCount / 10;
                            if (percentCount >= 0)
                                timeChart.y = percentCount;
                            timeChartDTO.Add(timeChart);
                        }
                        allTimeActivityDTO.da = timeChartDTO.OrderBy(x => x.x.Date).ToList();
                    }
                    successResponse.data = allTimeActivityDTO;
                }
                successResponse.response_code = 0;
                successResponse.message = "chart details";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("GetQuizProgress")]
        public IActionResult GetQuizProgress([FromBody]ParentPostDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                List<UserQuizResult> quizProgresses = _userQuizResultBusiness.GetQuizProgressByCourseId(dto.id, dto.courseid);
                if (quizProgresses.Count > 0)
                {
                    List<QuizPieChartDTO> quizChartDTOs = (from x in quizProgresses
                                                           select new QuizPieChartDTO
                                                           {
                                                               id = x.Id,
                                                               quizid = x.QuizId,
                                                               progress = x.Score,
                                                               passmark = x.PassingScore,
                                                               datetime = Convert.ToDateTime(x.CreationTime)
                                                           }).ToList();

                    var getPassedQuiz = quizChartDTOs
                        .Where(b => b.passmark <= b.progress)
                        .GroupBy(x => x.quizid)
                        .Select(x => x.OrderByDescending(y => y.id)
                        .FirstOrDefault())
                        .ToList();

                    var activityGroup = quizChartDTOs
                        .Where(x => !getPassedQuiz.Any(d => d.quizid == x.quizid))
                        .GroupBy(x => x.quizid)
                        .Select(x => x.OrderByDescending(y => y.id)
                        .FirstOrDefault())
                        .ToList();

                    activityGroup.AddRange(getPassedQuiz);

                    var totalTries = activityGroup.Count();

                    PieChartDTO pieChartDTO = new PieChartDTO();
                    pieChartDTO.totaltries = totalTries;
                    if (activityGroup.Count > 0)
                    {
                        double progress = 0;
                        foreach (var dt in activityGroup)
                        {
                            progress += dt.progress;
                        }
                        var avarage = progress / activityGroup.Count();
                        pieChartDTO.overallscore = avarage;
                    }

                    if (getPassedQuiz.Count > 0)
                    {
                        var avarage = getPassedQuiz.Count() * 100 / totalTries;
                        pieChartDTO.score = avarage;
                    }
                    successResponse.data = pieChartDTO;
                }
                successResponse.response_code = 0;
                successResponse.message = "chart details";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost]
        [Route("GetOverAllQuizProgress")]
        public IActionResult GetOverAllQuizProgress([FromBody]ParentPostDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                List<UserQuizResult> quizProgresses = _userQuizResultBusiness.GetQuizProgressByCourseId(dto.id, dto.courseid);
                if (quizProgresses.Count > 0)
                {
                    List<QuizPieChartDTO> quizChartDTOs = (from x in quizProgresses
                                                           select new QuizPieChartDTO
                                                           {
                                                               progress = x.Score,
                                                               datetime = Convert.ToDateTime(x.CreationTime)
                                                           }).ToList();
                    PieChartDTO pieChartDTO = new PieChartDTO();
                    if (quizChartDTOs.Count > 0)
                    {
                        double progress = 0;
                        foreach (var dt in quizChartDTOs)
                        {
                            progress += dt.progress;
                        }
                        var percentCount = progress / quizChartDTOs.Count();
                        pieChartDTO.score = percentCount;
                    }
                    successResponse.data = pieChartDTO;
                }
                successResponse.response_code = 0;
                successResponse.message = "chart details";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("GetAssignmentActivityPoint")]
        public IActionResult GetAssignmentActivityPoint([FromBody]ParentPostDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                List<AssignmentSubmission> assignmentProgresses = _assignmentSubmissionBusinees.GetAssignmentProgressByCourseId(dto.id, dto.courseid);
                if (assignmentProgresses.Count > 0)
                {
                    List<QuizChartDTO> assignmentChartDTOs = (from x in assignmentProgresses
                                                              select new QuizChartDTO
                                                              {
                                                                  progress = x.Score,
                                                                  datetime = Convert.ToDateTime(x.CreationTime)
                                                              }).ToList();
                    var activityGroup = assignmentChartDTOs.GroupBy(x => new { x.datetime.Month, x.datetime.Year }).ToList();
                    AllTimeActivityDTO allTimeActivityDTO = new AllTimeActivityDTO();
                    if (activityGroup.Count > 0)
                    {
                        List<TimeChartDTO> timeChartDTO = new List<TimeChartDTO>();
                        foreach (var dt in activityGroup)
                        {
                            TimeChartDTO timeChart = new TimeChartDTO();
                            timeChart.x = dt.Key.Month;
                            double progress = 0;
                            foreach (var time in dt)
                            {
                                progress += time.progress;
                            }
                            var percentCount = progress / dt.Count();
                            //var pointCount = percentCount / 10;
                            if (percentCount >= 0)
                                timeChart.y = percentCount;
                            timeChartDTO.Add(timeChart);
                        }
                        allTimeActivityDTO.mo = timeChartDTO.OrderBy(x => x.x).ToList();
                    }
                    var activityGroupDaliy = assignmentChartDTOs.GroupBy(x => new { x.datetime.Date }).ToList();
                    if (activityGroupDaliy.Count > 0)
                    {
                        List<TimeChartDayDTO> timeChartDTO = new List<TimeChartDayDTO>();
                        foreach (var dt in activityGroupDaliy)
                        {
                            TimeChartDayDTO timeChart = new TimeChartDayDTO();
                            timeChart.x = dt.Key.Date;
                            double progress = 0;
                            foreach (var time in dt)
                            {
                                progress += time.progress;
                            }
                            var percentCount = progress / dt.Count();
                            //var pointCount = percentCount / 10;
                            if (percentCount >= 0)
                                timeChart.y = percentCount;
                            timeChartDTO.Add(timeChart);
                        }
                        allTimeActivityDTO.da = timeChartDTO.OrderBy(x => x.x.Date).ToList();
                    }
                    successResponse.data = allTimeActivityDTO;
                }
                successResponse.response_code = 0;
                successResponse.message = "chart details";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("GetAssignmentProgress")]
        public IActionResult GetAssignmentProgress([FromBody]ParentPostDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                List<AssignmentSubmission> assignmentProgresses = _assignmentSubmissionBusinees.GetAssignmentProgressByCourseId(dto.id, dto.courseid);
                if (assignmentProgresses.Count > 0)
                {
                    List<AssinmentPieChartDTO> assignmentChartDTOs = (from x in assignmentProgresses
                                                                      select new AssinmentPieChartDTO
                                                                      {
                                                                          id = x.Id,
                                                                          assignmentid = x.AssignmentId,
                                                                          progress = x.Score,
                                                                          datetime = Convert.ToDateTime(x.CreationTime)
                                                                      }).ToList();

                    var getPassedAssignment = assignmentChartDTOs
                        .Where(b => 90 <= b.progress)
                        .GroupBy(x => x.assignmentid)
                        .Select(x => x.OrderByDescending(y => y.id).FirstOrDefault()).ToList();

                    var activityGroup = assignmentChartDTOs
                        .Where(x => !getPassedAssignment.Any(d => d.assignmentid == x.assignmentid))
                        .GroupBy(x => x.assignmentid)
                        .Select(x => x.OrderByDescending(y => y.id).FirstOrDefault()).ToList();

                    activityGroup.AddRange(getPassedAssignment);

                    var totalTries = activityGroup.Count();

                    PieChartDTO pieChartDTO = new PieChartDTO();
                    pieChartDTO.totaltries = totalTries;
                    if (activityGroup.Count > 0)
                    {
                        double progress = 0;
                        foreach (var dt in activityGroup)
                        {
                            progress += dt.progress;
                        }
                        var avarage = progress / activityGroup.Count();
                        pieChartDTO.overallscore = avarage;
                    }
                    if (getPassedAssignment.Count > 0)
                    {
                        var avarage = getPassedAssignment.Count() * 100 / totalTries;
                        pieChartDTO.score = avarage;
                    }

                    successResponse.data = pieChartDTO;
                }
                successResponse.response_code = 0;
                successResponse.message = "chart details";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost]
        [Route("GetOverAllAssignmentProgress")]
        public IActionResult GetOverAllAssignmentProgress([FromBody]ParentPostDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                List<AssignmentSubmission> assignmentProgresses = _assignmentSubmissionBusinees.GetAssignmentProgressByCourseId(dto.id, dto.courseid);
                if (assignmentProgresses.Count > 0)
                {
                    List<AssinmentPieChartDTO> assignmentChartDTOs = (from x in assignmentProgresses
                                                                      select new AssinmentPieChartDTO
                                                                      {
                                                                          progress = x.Score,
                                                                          datetime = Convert.ToDateTime(x.CreationTime)
                                                                      }).ToList();
                    PieChartDTO pieChartDTO = new PieChartDTO();
                    if (assignmentChartDTOs.Count > 0)
                    {
                        double progress = 0;
                        foreach (var dt in assignmentChartDTOs)
                        {
                            progress += dt.progress;
                        }
                        var percentCount = progress / assignmentChartDTOs.Count();
                        pieChartDTO.score = percentCount;
                    }
                    successResponse.data = pieChartDTO;
                }
                successResponse.response_code = 0;
                successResponse.message = "chart details";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("GetCurrentGPA")]
        public IActionResult GetCurrentGPA([FromBody]ParentPostDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                List<AssignmentSubmission> assignmentProgresses = _assignmentSubmissionBusinees.GetAssignmentProgressByCourseId(dto.id, dto.courseid);
                List<UserQuizResult> quizProgresses = _userQuizResultBusiness.GetQuizProgressByCourseId(dto.id, dto.courseid);
                double assignmentScore = 0.0;
                double quizScore = 0.0;
                if (assignmentProgresses.Count > 0)
                {
                    List<AssinmentPieChartDTO> assignmentChartDTOs = (from x in assignmentProgresses
                                                                      select new AssinmentPieChartDTO
                                                                      {
                                                                          id = x.Id,
                                                                          assignmentid = x.AssignmentId,
                                                                          progress = x.Score,
                                                                          datetime = Convert.ToDateTime(x.CreationTime)
                                                                      }).ToList();
                    var activityGroup = assignmentChartDTOs.GroupBy(x => x.assignmentid)
                      .Select(x => x.OrderByDescending(y => y.id).FirstOrDefault()).ToList();
                    if (activityGroup.Count > 0)
                    {
                        double progress = 0;
                        foreach (var dt in activityGroup)
                        {
                            progress += dt.progress;
                        }
                        var avarage = progress / activityGroup.Count();
                        assignmentScore = avarage / 2;
                    }
                }

                if (quizProgresses.Count > 0)
                {
                    List<QuizPieChartDTO> quizChartDTOs = (from x in quizProgresses
                                                           select new QuizPieChartDTO
                                                           {
                                                               id = x.Id,
                                                               quizid = x.QuizId,
                                                               progress = x.Score,
                                                               datetime = Convert.ToDateTime(x.CreationTime)
                                                           }).ToList();
                    var activityGroup = quizChartDTOs.GroupBy(x => x.quizid)
                      .Select(x => x.OrderByDescending(y => y.id).FirstOrDefault()).ToList();
                    PieChartDTO pieChartDTO = new PieChartDTO();
                    if (activityGroup.Count > 0)
                    {
                        double progress = 0;
                        foreach (var dt in activityGroup)
                        {
                            progress += dt.progress;
                        }
                        var avarage = progress / activityGroup.Count();
                        quizScore = avarage / 2;
                    }
                }
                var getcurrentgpa = _assignmentSubmissionBusinees.CalculateGAP(assignmentScore + quizScore);
                CurrentGPA currentGPA = new CurrentGPA();
                if (!string.IsNullOrEmpty(getcurrentgpa))
                {
                    currentGPA.grade = getcurrentgpa;
                }
                successResponse.data = currentGPA;
                successResponse.response_code = 0;
                successResponse.message = "GPA details";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("GetCurrentPoint")]
        public IActionResult GetCurrentPoint([FromBody]ParentPostDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                List<AssignmentSubmission> assignmentProgresses = _assignmentSubmissionBusinees.GetAssignmentProgressByCourseId(dto.id, 0);
                List<UserQuizResult> quizProgresses = _userQuizResultBusiness.GetQuizProgressByCourseId(dto.id, 0);
                List<QuizChartDTO> assignmentChartDTOs = new List<QuizChartDTO>();
                List<QuizChartDTO> quizChartDTOs = new List<QuizChartDTO>();
                if (assignmentProgresses.Count > 0)
                {
                    assignmentChartDTOs = (from x in assignmentProgresses
                                           select new QuizChartDTO
                                           {
                                               id = x.Id,
                                               progress = x.Score,
                                               datetime = Convert.ToDateTime(x.CreationTime)
                                           }).ToList();
                }
                if (quizProgresses.Count > 0)
                {
                    quizChartDTOs = (from x in quizProgresses
                                     select new QuizChartDTO
                                     {
                                         id = x.Id,
                                         progress = x.Score,
                                         datetime = Convert.ToDateTime(x.CreationTime)
                                     }).ToList();
                }
                assignmentChartDTOs.AddRange(quizChartDTOs);

                var activityGroup = assignmentChartDTOs.Where(x => x.datetime > DateTime.UtcNow.AddMonths(-dto.month)).GroupBy(x => new { x.datetime.Month, x.datetime.Year }).ToList();

                List<TimeChartDTO> lsttimeChartDTO = new List<TimeChartDTO>();
                if (activityGroup.Count > 0)
                {
                    foreach (var dt in activityGroup)
                    {
                        TimeChartDTO timeChartDTO = new TimeChartDTO();
                        timeChartDTO.x = dt.Key.Month;
                        double progress = 0;
                        foreach (var time in dt)
                        {
                            progress += time.progress;
                        }
                        var percentCount = progress / dt.Count();
                        var divideByItem = percentCount / 2;
                        //var pointCount = divideByItem / 10;
                        timeChartDTO.y = divideByItem;
                        lsttimeChartDTO.Add(timeChartDTO);
                    }
                }
                successResponse.data = lsttimeChartDTO.OrderBy(x => x.x);
                successResponse.response_code = 0;
                successResponse.message = "points details";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost]
        [Route("GetLastOnline")]
        public IActionResult GetLastOnline([FromBody]ParentPostDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                List<AppTimeTrack> appTimeTracks = _appTimeTrackBusiness.GetTimeTrackingByUserId(dto.id);
                if (appTimeTracks.Count > 0)
                {
                    List<TimeActivityDTO> timeActivityDTOs = (from x in appTimeTracks
                                                              select new TimeActivityDTO
                                                              {
                                                                  activitytime = Convert.ToDateTime(x.ActivityTime),
                                                                  outtime = Convert.ToDateTime(x.Outtime),
                                                                  id = x.Id
                                                              }).ToList();
                    var activityGroup = timeActivityDTOs.OrderByDescending(x => x.id).Select(x => x.activitytime).FirstOrDefault();
                    successResponse.data = activityGroup;
                }
                successResponse.response_code = 0;
                successResponse.message = "last online details";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost]
        [Route("GetAllProgress")]
        public IActionResult GetAllProgress([FromBody]ParentPostDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //Assignment variable
                int totalAssignment = 0;
                double assignmentAvarage = 0;
                //Quiz variable
                int totalQuiz = 0;
                double quizAvarage = 0;
                //Lesson variable
                int totalLesson = 0;
                double lessonAvarage = 0;

                List<AssignmentSubmission> assignmentProgresses = new List<AssignmentSubmission>();
                List<QuizProgress> quizProgresses = new List<QuizProgress>();
                List<LessonProgress> lessonProgresses = new List<LessonProgress>();

                if (dto.content == 3)
                {
                    assignmentProgresses = _assignmentSubmissionBusinees.GetAssignmentProgressByCourseId(dto.id, dto.courseid);
                }
                else if (dto.content == 2)
                {
                    quizProgresses = _userQuizResultBusiness.GetQuizProgressesByStdCouserId(dto.id, dto.courseid);
                }
                else if (dto.content == 1)
                {
                    lessonProgresses = _lessonProgressBusiness.GetLessonProgresses(dto.id, dto.courseid);
                }
                else
                {
                    assignmentProgresses = _assignmentSubmissionBusinees.GetAssignmentProgressByCourseId(dto.id, dto.courseid);
                    quizProgresses = _userQuizResultBusiness.GetQuizProgressesByStdCouserId(dto.id, dto.courseid);
                    lessonProgresses = _lessonProgressBusiness.GetLessonProgresses(dto.id, dto.courseid);
                }

                if (assignmentProgresses.Count > 0)
                {
                    List<AssinmentPieChartDTO> assignmentChartDTOs = (from x in assignmentProgresses
                                                                      select new AssinmentPieChartDTO
                                                                      {
                                                                          id = x.Id,
                                                                          assignmentid = x.AssignmentId,
                                                                          progress = x.Score,
                                                                          datetime = Convert.ToDateTime(x.CreationTime)
                                                                      }).ToList();
                    var filteredRecords = assignmentChartDTOs.Where(x => x.datetime > DateTime.UtcNow.AddMonths(-dto.month)).ToList();
                    totalAssignment = filteredRecords.Count();
                    var getPassedAssignment = filteredRecords
                         .Where(b => 90 <= b.progress)
                         .GroupBy(x => x.assignmentid)
                         .Select(x => x.OrderByDescending(y => y.id).FirstOrDefault()).ToList();

                    var activityGroup = filteredRecords
                        .Where(x => !getPassedAssignment.Any(d => d.assignmentid == x.assignmentid))
                        .GroupBy(x => x.assignmentid)
                        .Select(x => x.OrderByDescending(y => y.id).FirstOrDefault()).ToList();

                    if (activityGroup.Count > 0)
                    {
                        double progress = 0;
                        foreach (var dt in activityGroup)
                        {
                            progress += dt.progress;
                        }
                        var avarage = progress / 100;
                        assignmentAvarage += avarage;
                    }
                    if (getPassedAssignment.Count > 0)
                    {
                        var avarage = 1 * getPassedAssignment.Count();
                        assignmentAvarage += avarage;
                    }
                }

                if (quizProgresses.Count > 0)
                {
                    List<QuizPieChartDTO> quizChartDTOs = (from x in quizProgresses
                                                           select new QuizPieChartDTO
                                                           {
                                                               id = x.Id,
                                                               quizid = x.QuizId,
                                                               progress = x.Progress,
                                                               datetime = Convert.ToDateTime(x.CreationTime)
                                                           }).ToList();

                    var filteredRecords = quizChartDTOs.Where(x => x.datetime > DateTime.UtcNow.AddMonths(-dto.month)).ToList();
                    totalQuiz = filteredRecords.Count();

                    var activityGroup = filteredRecords
                        .GroupBy(x => x.quizid)
                        .Select(x => x.OrderByDescending(y => y.id)
                        .FirstOrDefault())
                        .ToList();

                    if (activityGroup.Count > 0)
                    {
                        double progress = 0;
                        foreach (var dt in activityGroup)
                        {
                            progress += dt.progress;
                        }
                        var avarage = progress / 100;
                        quizAvarage += avarage;
                    }
                }

                if (lessonProgresses.Count > 0)
                {
                    List<LessonPieChartDTO> lessonPieChartDTOs = (from x in lessonProgresses
                                                                  select new LessonPieChartDTO
                                                                  {
                                                                      id = x.Id,
                                                                      lessonid = x.LessonId,
                                                                      progress = x.Progress,
                                                                      datetime = Convert.ToDateTime(x.CreationTime)
                                                                  }).ToList();

                    var filteredRecords = lessonPieChartDTOs.Where(x => x.datetime > DateTime.UtcNow.AddMonths(-dto.month)).ToList();
                    totalLesson = filteredRecords.Count();
                    var activityGroup = filteredRecords
                        .GroupBy(x => x.lessonid)
                        .Select(x => x.OrderByDescending(y => y.id)
                        .FirstOrDefault())
                        .ToList();

                    if (activityGroup.Count > 0)
                    {
                        double progress = 0;
                        foreach (var dt in activityGroup)
                        {
                            progress += dt.progress;
                        }
                        var avarage = progress / 100;
                        lessonAvarage += avarage;
                    }
                }

                PieChartDTO pieChartDTO = new PieChartDTO();
                if (dto.content == 3)
                {
                    var percentCount = assignmentAvarage * 100 / totalAssignment;
                    pieChartDTO.score = percentCount;
                    successResponse.data = pieChartDTO;
                }
                else if (dto.content == 2)
                {
                    var percentCount = quizAvarage * 100 / totalQuiz;
                    pieChartDTO.score = percentCount;
                    successResponse.data = pieChartDTO;
                }
                else if (dto.content == 1)
                {
                    var percentCount = lessonAvarage * 100 / totalLesson;
                    pieChartDTO.score = percentCount;
                    successResponse.data = pieChartDTO;
                }
                else
                {
                    int totalCount = totalAssignment + totalQuiz + totalLesson;
                    double totalAvarage = assignmentAvarage + quizAvarage + lessonAvarage;
                    double percentCount = totalAvarage * 100 / totalCount;
                    pieChartDTO.score = percentCount;
                    successResponse.data = pieChartDTO;
                }

                successResponse.response_code = 0;
                successResponse.message = "chart details";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost]
        [Route("GetUsersLocations")]
        public IActionResult GetUsersLocations([FromBody]ParentPostDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = _lessonBusiness.getUserId(tc.sub);
                if (tc.RoleName.Contains(General.getRoleType("18")))
                {
                    List<UserLocationActivityDTO> userDetails = new List<UserLocationActivityDTO>();
                    List<AppTimeTrack> appTimeTracks = _appTimeTrackBusiness.GetTimeTrackingByUserId(dto.id);
                    if (appTimeTracks.Count > 0)
                    {
                        List<UserLocationDTO> UserLocationDTO = (from x in appTimeTracks
                                                                 select new UserLocationDTO
                                                                 {
                                                                     id = x.Id,
                                                                     userid = x.UserId,
                                                                     lat = x.Latitude,
                                                                     lng = x.Longitude,
                                                                     datetime = Convert.ToDateTime(x.CreationTime)
                                                                 }).ToList();
                        var latestLocation = UserLocationDTO
                                           .Where(b => !string.IsNullOrEmpty(b.lat) && !string.IsNullOrEmpty(b.lng) && !b.lat.Contains("0.0") && !b.lng.Contains("0.0"))
                                           .GroupBy(b => b.userid)
                                           .Select(y => y.OrderByDescending(x => x.id).FirstOrDefault())
                                           .ToList();

                        if (latestLocation != null)
                        {
                            foreach (var loc in latestLocation)
                            {
                                UserLocationActivityDTO userLocationActivityDTO = new UserLocationActivityDTO();
                                userLocationActivityDTO.lat = loc.lat;
                                userLocationActivityDTO.lng = loc.lng;
                                User studentUser = _usersBusiness.GetUserbyId(loc.userid);
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
                                    userLocationActivityDTO.user = userDetailsDTO;
                                    userDetails.Add(userLocationActivityDTO);
                                }
                            }
                        }
                    }
                    successResponse.data = userDetails;
                }
                successResponse.response_code = 0;
                successResponse.message = "chart details";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "failure";
                return StatusCode(500, unsuccessResponse);
            }
        }


        [HttpGet]
        [Route("ViewChildActivity")]
        public IActionResult ViewChildActivity(string date, int pagenumber, int perpagerecord)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                long total;
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = _lessonBusiness.getUserId(tc.sub);
                if (tc.RoleName.Contains(General.getRoleType("18")))
                {
                    List<ParentChildActivityDTO> parentChildActivityDTOs = new List<ParentChildActivityDTO>();
                    List<StudParent> studParent = _studParentBusiness.GetStudentListByParentId(long.Parse(tc.Id));
                    if (pagenumber != 0 && perpagerecord != 0)
                    {
                        total = studParent.Count();

                        studParent = studParent.OrderByDescending(b => b.Id).
                                Skip(perpagerecord * (pagenumber - 1)).
                                Take(perpagerecord).
                                ToList();
                    }
                    if (studParent.Count > 0)
                    {
                        foreach (var stud in studParent)
                        {
                            User studentUser = _usersBusiness.GetUserbyId(stud.StudentId);
                            if (studentUser != null)
                            {
                                ParentChildActivityDTO parentChildActivityDTO = new ParentChildActivityDTO();
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
                                parentChildActivityDTO.userDetails = userDetailsDTO;
                                List<AppTimeTrack> appTimeTracks = _appTimeTrackBusiness.GetTimeTrackingByUserId(new List<int> { Convert.ToInt32(studentUser.Id) });
                                if (appTimeTracks.Count > 0)
                                {
                                    List<TimeActivityDTO> timeActivityDTOs = (from x in appTimeTracks
                                                                              select new TimeActivityDTO
                                                                              {
                                                                                  activitytime = Convert.ToDateTime(x.ActivityTime),
                                                                                  outtime = Convert.ToDateTime(x.Outtime)
                                                                              }).ToList();

                                    var activityDaliy = timeActivityDTOs.Where(x => DateTime.Compare(x.activitytime.Date, Convert.ToDateTime(date).Date) == 0).ToList();
                                    if (activityDaliy.Count > 0)
                                    {
                                        double timeCount = 0;
                                        foreach (var time in activityDaliy)
                                        {
                                            TimeSpan diff = time.outtime - time.activitytime;
                                            double hours = diff.TotalHours;
                                            timeCount += hours;
                                        }
                                        if (timeCount >= 0)
                                            parentChildActivityDTO.hours = timeCount;
                                    }
                                }
                                parentChildActivityDTOs.Add(parentChildActivityDTO);
                            }
                        }
                        successResponse.data = parentChildActivityDTOs;
                    }
                    successResponse.response_code = 0;
                    successResponse.message = "Child Activity details";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "you are not authorized.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(401, unsuccessResponse);
                }
            }
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "failure";
                return StatusCode(500, unsuccessResponse);
            }
        }
    }
}