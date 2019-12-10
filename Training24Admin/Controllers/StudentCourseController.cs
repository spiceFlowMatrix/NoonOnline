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
using Trainning24.BL.ViewModels.BundleCourse;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Training24Admin.Controllers
{

    [Route("api/v1/UserCourse")]
    [ApiController]
    [Authorize]
    public class StudentCourseController : ControllerBase
    {
        private readonly StudentCourseBusiness StudentCourseBusiness;
        private readonly EFCourseGradeRepository EFCourseGradeRepository;
        private readonly CourseBusiness courseBusiness;
        private readonly CourseGradeBusiness CourseGradeBusiness;
        private readonly GradeBusiness GradeBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly UsersBusiness usersBusiness;
        private static Training24Context _training24Context;
        private IHostingEnvironment hostingEnvironment;

        public StudentCourseController
        (
            IHostingEnvironment hostingEnvironment,
            StudentCourseBusiness StudentCourseBusiness,
            UsersBusiness usersBusiness,
            CourseBusiness courseBusiness,
            LessonBusiness LessonBusiness,
            Training24Context training24Context,
            EFCourseGradeRepository EFCourseGradeRepository,
            CourseGradeBusiness CourseGradeBusiness,
            GradeBusiness GradeBusiness
        )
        {
            this.hostingEnvironment = hostingEnvironment;
            this.StudentCourseBusiness = StudentCourseBusiness;
            this.usersBusiness = usersBusiness;
            this.LessonBusiness = LessonBusiness;
            this.courseBusiness = courseBusiness;
            _training24Context = training24Context;
            this.EFCourseGradeRepository = EFCourseGradeRepository;
            this.CourseGradeBusiness = CourseGradeBusiness;
            this.GradeBusiness = GradeBusiness;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddStudentCourseModel StudentCourseModel)
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
                    if (tc.RoleName.Contains(General.getRoleType("1")))
                    {
                        User user = usersBusiness.GetUserbyId(StudentCourseModel.userid);

                        List<Role> userallroles = (from allrole in _training24Context.Role
                                                   join userrole in _training24Context.UserRole on allrole.Id equals userrole.RoleId
                                                   where userrole.UserId == user.Id
                                                   select allrole).ToList();

                        if (userallroles.Where(b => b.Id == 4).SingleOrDefault() != null)
                        {
                            if (StudentCourseModel.startdate == null || StudentCourseModel.enddate == null)
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "Start date or End date can not be null";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(422, unsuccessResponse);
                            }
                        }

                        CourseGrade courseGrade = EFCourseGradeRepository.ListQuery(b => b.CourseId == StudentCourseModel.courseid).SingleOrDefault();

                        if (courseGrade == null)
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Course has no grade";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(422, unsuccessResponse);
                        }

                        UserCourse newStudentCourse = StudentCourseBusiness.Create(StudentCourseModel, int.Parse(tc.Id));

                        if (newStudentCourse == null)
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Duplicate entry.";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(422, unsuccessResponse);
                        }

                        ResponseStudentCourseModel responseStudentCourseModel = new ResponseStudentCourseModel
                        {
                            id = newStudentCourse.Id,
                            userid = newStudentCourse.UserId,
                            courseid = newStudentCourse.CourseId,
                            startdate = newStudentCourse.StartDate.ToString(),
                            enddate = newStudentCourse.EndDate.ToString()
                        };

                        successResponse.data = responseStudentCourseModel;
                        successResponse.response_code = 0;
                        successResponse.message = "StudentCourse Created";
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
        public IActionResult Put(long id, UpdateStudentCourseModel updateStudentCourseModel)
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
                    if (tc.RoleName.Contains(General.getRoleType("1")))
                    {
                        updateStudentCourseModel.id = id;

                        User user = usersBusiness.GetUserbyId(updateStudentCourseModel.userid);

                        List<Role> userallroles = (from allrole in _training24Context.Role
                                                   join userrole in _training24Context.UserRole on allrole.Id equals userrole.RoleId
                                                   where userrole.UserId == user.Id
                                                   select allrole).ToList();

                        if (userallroles.Where(b => b.Id == 4).SingleOrDefault() != null)
                        {
                            if (updateStudentCourseModel.startdate == null || updateStudentCourseModel.enddate == null)
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "Start date or End date can not be null";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(422, unsuccessResponse);
                            }
                        }

                        UserCourse newStudentCourse = StudentCourseBusiness.Update(updateStudentCourseModel, int.Parse(tc.Id));
                        ResponseStudentCourseModel responseStudentCourseModel = new ResponseStudentCourseModel
                        {
                            id = newStudentCourse.Id,
                            userid = newStudentCourse.UserId,
                            courseid = newStudentCourse.CourseId,
                            startdate = newStudentCourse.StartDate,
                            enddate = newStudentCourse.EndDate
                        };

                        successResponse.data = responseStudentCourseModel;
                        successResponse.response_code = 0;
                        successResponse.message = "StudentCourse updated";
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


            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            try
            {

                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("1")))
                    {
                        UserCourse newStudentCourse = StudentCourseBusiness.Delete(id, int.Parse(tc.Id));

                        ResponseStudentCourseModel responseStudentCourseModel = new ResponseStudentCourseModel
                        {
                            id = newStudentCourse.Id,
                            userid = newStudentCourse.UserId,
                            courseid = newStudentCourse.CourseId,
                            startdate = newStudentCourse.StartDate,
                            enddate = newStudentCourse.EndDate
                        };

                        successResponse.data = responseStudentCourseModel;
                        successResponse.response_code = 0;
                        successResponse.message = "StudentCourse Deleted";
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
        [Route("GetStudentCourse")]
        public IActionResult Get(int pagenumber, int perpagerecord)
        {
            PaginationModel paginationModel = new PaginationModel
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord
            };

            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            try
            {

                if (ModelState.IsValid)
                {
                    if (
                        tc.RoleName.Contains(General.getRoleType("1")) ||
                        tc.RoleName.Contains(General.getRoleType("3")) ||
                        tc.RoleName.Contains(General.getRoleType("4"))
                    )
                    {
                        List<UserCourse> studentList = new List<UserCourse>();
                        List<ResponseStudentCourseModel> studentResponseList = new List<ResponseStudentCourseModel>();
                        studentList = StudentCourseBusiness.StudentCourseList(paginationModel, out int total);

                        foreach (UserCourse student in studentList)
                        {
                            ResponseStudentCourseModel ResponseStudentCourseModel = new ResponseStudentCourseModel
                            {
                                id = student.Id,
                                userid = student.UserId,
                                courseid = student.CourseId,
                                startdate = student.StartDate,
                                enddate = student.EndDate
                            };

                            studentResponseList.Add(ResponseStudentCourseModel);
                        }

                        successResponse.data = studentResponseList;
                        successResponse.totalcount = total;
                        successResponse.response_code = 0;
                        successResponse.message = "StudentCourseCourse Details";
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
        [Route("GetCoursesByUserId/{id}")]
        public IActionResult GetCoursesByStudentId(int id, int pagenumber, int perpagerecord)
        {
            CourseStudentPaginationModel paginationModel = new CourseStudentPaginationModel
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord,
                studentid = id
            };
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            try
            {
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx

                if (ModelState.IsValid)
                {
                    if (
                        tc.RoleName.Contains(General.getRoleType("1")) ||
                        tc.RoleName.Contains(General.getRoleType("3")) ||
                        tc.RoleName.Contains(General.getRoleType("4"))
                    )
                    {
                        List<UserCourse> StudentCourseCourseList = StudentCourseBusiness.GetCoursesByStudentId(paginationModel, out int total);
                        List<ResponseCourseByStudent> StudentCourseResponseList = new List<ResponseCourseByStudent>();

                        foreach (UserCourse student in StudentCourseCourseList)
                        {
                            ResponseCourseByStudent ResponseStudentCourseModel = new ResponseCourseByStudent();
                            Course course = courseBusiness.getCourseById(int.Parse(student.CourseId.ToString()));
                            if (course != null)
                            {
                                CourseGrade courseGrade = CourseGradeBusiness.GetGradeByCourseId(course.Id);
                                Grade grade = GradeBusiness.getSchoolByGrade(courseGrade.Gradeid);
                                ResponseStudentCourseModel.id = student.Id;
                                ResponseStudentCourseModel.studentid = student.UserId;
                                ResponseStudentCourseModel.courseid = student.CourseId;
                                ResponseStudentCourseModel.name = course.Name;
                                ResponseStudentCourseModel.code = course.Code;
                                ResponseStudentCourseModel.istrial = course.istrial;
                                ResponseStudentCourseModel.description = course.Description;
                                //ResponseStudentCourseModel.image = course.Image;
                                if (!string.IsNullOrEmpty(course.Image))
                                    ResponseStudentCourseModel.image = LessonBusiness.geturl(course.Image, Certificate);
                                //ResponseStudentCourseModel.gradeid = courseGrade.Id;
                                ResponseStudentCourseModel.gradeid = courseGrade.Gradeid;
                                ResponseStudentCourseModel.schoolid = grade.SchoolId;
                                ResponseStudentCourseModel.StartDate = student.StartDate.ToString();
                                ResponseStudentCourseModel.EndDate = student.EndDate.ToString();
                                StudentCourseResponseList.Add(ResponseStudentCourseModel);
                            }
                        }

                        successResponse.data = StudentCourseResponseList;
                        successResponse.totalcount = total;
                        successResponse.response_code = 0;
                        successResponse.message = "StudentCourse Detail";
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
        [Route("GetUsersByCourseId/{id}")]
        public IActionResult GetStudentsByCourseId(int id, int pagenumber, int perpagerecord)
        {
            StudentCoursePaginationModel paginationModel = new StudentCoursePaginationModel
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord,
                courseid = id
            };
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            try
            {

                if (ModelState.IsValid)
                {
                    if (
                        tc.RoleName.Contains(General.getRoleType("1")) ||
                        tc.RoleName.Contains(General.getRoleType("3")) ||
                        tc.RoleName.Contains(General.getRoleType("4"))
                    )
                    {
                        List<UserCourse> StudentCourseCourseList = StudentCourseBusiness.GetStudentsByCourseId(paginationModel, out int total);
                        List<ResponseStudentByCourse> StudentCourseResponseList = new List<ResponseStudentByCourse>();

                        foreach (UserCourse student in StudentCourseCourseList)
                        {
                            ResponseStudentByCourse ResponseStudentCourseModel = new ResponseStudentByCourse();
                            User user = usersBusiness.GetUserbyId(student.UserId);

                            ResponseStudentCourseModel.id = student.Id;
                            ResponseStudentCourseModel.studentid = student.UserId;
                            ResponseStudentCourseModel.courseid = student.CourseId;
                            ResponseStudentCourseModel.name = user.FullName;
                            //ResponseStudentCourseModel.RoleId = user.RoleId;
                            ResponseStudentCourseModel.StartDate = student.StartDate;
                            ResponseStudentCourseModel.EndDate = student.EndDate;

                            //var role = usersBusiness.getRole(user.RoleId);
                            //ResponseStudentCourseModel.RoleName = role.Name;

                            StudentCourseResponseList.Add(ResponseStudentCourseModel);
                        }

                        successResponse.data = StudentCourseResponseList;
                        successResponse.totalcount = total;
                        successResponse.response_code = 0;
                        successResponse.message = "StudentCourse Detail";
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
        [Route("GetAllCourseByUser")]
        public IActionResult GetAllCourseByUser()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            List<UserCourseDTO> userCourseDTOs = new List<UserCourseDTO>();
            try
            {
                var courseList = StudentCourseBusiness.GetAllCourseByUserId(long.Parse(tc.Id));
                if (courseList.Count > 0)
                {
                    foreach (var course in courseList)
                    {
                        Course getCourse = courseBusiness.getCourseById(course.CourseId);
                        if (course != null)
                        {
                            UserCourseDTO userCourseDTO = new UserCourseDTO();
                            userCourseDTO.id = getCourse.Id;
                            userCourseDTO.name = getCourse.Name;
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