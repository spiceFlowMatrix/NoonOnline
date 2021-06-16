using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.CourseDefination;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class CourseDefinationController : ControllerBase
    {
        private readonly CourseDefinationBusiness CourseDefinationBusiness;
        private readonly CourseBusiness CourseBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly GradeBusiness GradeBusiness;
        private readonly IMapper _mapper;

        public CourseDefinationController
        (
            IMapper mapper,
            CourseDefinationBusiness CourseDefinationBusiness,
            CourseBusiness CourseBusiness,
            LessonBusiness LessonBusiness,
            GradeBusiness GradeBusiness
        )
        {
            this.LessonBusiness = LessonBusiness;
            this.CourseDefinationBusiness = CourseDefinationBusiness;
            _mapper = mapper;
            this.CourseBusiness = CourseBusiness;
            this.GradeBusiness = GradeBusiness;
        }

        [HttpPost]
        public IActionResult Post([FromBody] CourseDefination CourseDefination)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            //get claims after decoding id_token 
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("1")) || tc.RoleName.Contains(General.getRoleType("13")))
                    {
                        CourseDefination courseDefination = new CourseDefination();
                        ResponseCourseDefination responseCourseDefination = new ResponseCourseDefination();

                        if (CourseDefination.Id == 0)
                        {
                            courseDefination = CourseDefinationBusiness.Create(CourseDefination, int.Parse(tc.Id));
                            if (courseDefination == null)
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "Duplicate entry.";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(422, unsuccessResponse);
                            }
                        }
                        else
                        {
                            courseDefination = CourseDefinationBusiness.Update(CourseDefination, int.Parse(tc.Id));

                            if (courseDefination == null)
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "Duplicate entry.";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(422, unsuccessResponse);
                            }
                        }

                        responseCourseDefination.Id = courseDefination.Id;
                        responseCourseDefination.BasePrice = courseDefination.BasePrice;
                        responseCourseDefination.CourseId = courseDefination.CourseId;
                        responseCourseDefination.CourseName = CourseBusiness.getCourseById(courseDefination.CourseId).Name;
                        responseCourseDefination.GradeId = courseDefination.GradeId;
                        responseCourseDefination.GradeName = GradeBusiness.getGradeById(courseDefination.GradeId).Name;
                        responseCourseDefination.Subject = courseDefination.Subject;
                        successResponse.data = responseCourseDefination;
                        successResponse.response_code = 0;
                        successResponse.message = "CourseDefination Created";
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
            //get claims after decoding id_token 
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("1")) || tc.RoleName.Contains(General.getRoleType("13")))
                    {
                        //CourseDefinationBusiness.Delete(id, int.Parse(tc.Id));

                        successResponse.data = _mapper.Map<CourseDefination, ResponseCourseDefination1>(CourseDefinationBusiness.Delete(id, int.Parse(tc.Id)));
                        successResponse.response_code = 0;
                        successResponse.message = "CourseDefination Deleted";
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
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            //get claims after decoding id_token 
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (ModelState.IsValid)
                {
                    CourseDefination courseDefination = CourseDefinationBusiness.getCourseDefinationById(id);
                    ResponseCourseDefination responseCourseDefination = new ResponseCourseDefination();
                    responseCourseDefination.Id = courseDefination.Id;
                    responseCourseDefination.BasePrice = courseDefination.BasePrice;
                    responseCourseDefination.CourseId = courseDefination.CourseId;
                    responseCourseDefination.CourseName = CourseBusiness.getCourseById(courseDefination.CourseId).Name;
                    responseCourseDefination.GradeId = courseDefination.GradeId;
                    Grade grade = GradeBusiness.getGradeById(courseDefination.GradeId);
                    if (grade != null)
                        responseCourseDefination.GradeName = grade.Name;
                    responseCourseDefination.Subject = courseDefination.Subject;
                    successResponse.data = responseCourseDefination;
                    successResponse.response_code = 0;
                    successResponse.message = "CourseDefination Detail";
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
                List<CourseDefination> CourseDefinationList = CourseDefinationBusiness.CourseDefinationList(paginationModel, out int total);
                List<ResponseCourseDefination> ResponseCourseDefinationList = new List<ResponseCourseDefination>();

                foreach (var courseDefination in CourseDefinationList)
                {
                    ResponseCourseDefination responseCourseDefination = new ResponseCourseDefination();
                    responseCourseDefination.Id = courseDefination.Id;
                    responseCourseDefination.BasePrice = courseDefination.BasePrice;
                    Course course = CourseBusiness.getCourseById(courseDefination.CourseId);
                    responseCourseDefination.CourseId = courseDefination.CourseId;
                    if(course != null)
                    {
                        responseCourseDefination.CourseName = course.Name;
                    }
                    responseCourseDefination.GradeId = courseDefination.GradeId;
                    Grade grade = GradeBusiness.getGradeById(courseDefination.GradeId);
                    if(grade != null)
                        responseCourseDefination.GradeName = grade.Name;
                    responseCourseDefination.Subject = courseDefination.Subject;
                    ResponseCourseDefinationList.Add(responseCourseDefination);
                }

                successResponse.data = ResponseCourseDefinationList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "CourseDefination Details";
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

        [HttpGet("GetSubject")]
        public IActionResult GetSubject(string search)
        {
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                List<CourseDefination> CourseDefinationList = CourseDefinationBusiness.GetSubjectList(search);
                List<ResponseCourseDefination> ResponseCourseDefinationList = new List<ResponseCourseDefination>();

                foreach (var courseDefination in CourseDefinationList)
                {
                    ResponseCourseDefination responseCourseDefination = new ResponseCourseDefination();
                    responseCourseDefination.Id = courseDefination.Id;
                    responseCourseDefination.BasePrice = courseDefination.BasePrice;
                    Course course = CourseBusiness.getCourseById(courseDefination.CourseId);
                    responseCourseDefination.CourseId = courseDefination.CourseId;
                    if (course != null)
                    {
                        responseCourseDefination.CourseName = course.Name;
                    }
                    responseCourseDefination.GradeId = courseDefination.GradeId;
                    Grade grade = GradeBusiness.getGradeById(courseDefination.GradeId);
                    if (grade != null)
                        responseCourseDefination.GradeName = grade.Name;
                    responseCourseDefination.Subject = courseDefination.Subject;
                    ResponseCourseDefinationList.Add(responseCourseDefination);
                }

                successResponse.data = ResponseCourseDefinationList;
                successResponse.response_code = 0;
                successResponse.message = "Subject list details";
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