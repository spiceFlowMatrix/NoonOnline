using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Package;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class PackageCourseController : ControllerBase
    {
        private readonly PackageCourseBusiness _packageCourseBusiness;
        private readonly CourseBusiness _courseBusiness;
        private readonly LessonBusiness _lessonBusiness;
        public PackageCourseController(
          PackageCourseBusiness packageCourseBusiness,
          LessonBusiness lessonBusiness,
          CourseBusiness courseBusiness
          )
        {
            _lessonBusiness = lessonBusiness;
            _packageCourseBusiness = packageCourseBusiness;
            _courseBusiness = courseBusiness;
        }

        #region Package Course

        [Authorize]
        [HttpGet("GetPackageCourses/{id}")]
        public IActionResult GetPackageCourses(int id, int pagenumber, int perpagerecord)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            List<PackageCourseDTO> objlst = new List<PackageCourseDTO>();
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
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    var data = _packageCourseBusiness.GetPackageCourseList(id, paginationModel);
                    if (data != null)
                    {
                        foreach (var service in data)
                        {
                            PackageCourseDTO packageCourse = new PackageCourseDTO();
                            packageCourse.id = service.Id;
                            packageCourse.package_id = service.PackageId;
                            packageCourse.course_id = service.CourseId;
                            var course = _courseBusiness.getCourseById(service.CourseId);
                            if (course != null)
                            {
                                packageCourse.name = course.Name;
                                packageCourse.code = course.Code;
                            }
                            objlst.Add(packageCourse);
                        }
                        successResponse.data = objlst;
                        successResponse.response_code = 0;
                        successResponse.message = "package course list get";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "No package course found";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
                    }
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "You are not authorized.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(401, unsuccessResponse);
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
        [HttpPost("AddPackageCourse")]
        public IActionResult AddPackageCourse(PackageCourseDTO obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            if (obj.course_id == 0)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "course is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    var exist = _packageCourseBusiness.CheckDuplicateEntry(obj.package_id, obj.course_id);
                    if (!exist)
                    {
                        PackageCourse packageCourse = new PackageCourse();
                        packageCourse.PackageId = obj.package_id;
                        packageCourse.CourseId = obj.course_id;
                        packageCourse.CreatorUserId = int.Parse(tc.Id);
                        packageCourse.CreationTime = DateTime.Now.ToString();
                        var data = _packageCourseBusiness.AddPackageCourse(packageCourse);
                        obj.id = data.Id;
                        var course = _courseBusiness.getCourseById(obj.course_id);
                        obj.name = course.Name;
                        obj.code = course.Code;
                        successResponse.data = obj;
                        successResponse.response_code = 0;
                        successResponse.message = "Package course added";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "Duplicate Record";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
                    }
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "You are not authorized.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(401, unsuccessResponse);
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
        [HttpDelete("DeletePackageCourse/{Id}")]
        public IActionResult DeletePackageCourse(long Id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    PackageCourse packageCourse = new PackageCourse();
                    packageCourse.Id = Id;
                    packageCourse.DeleterUserId = int.Parse(tc.Id);
                    packageCourse.DeletionTime = DateTime.Now.ToString();
                    var data = _packageCourseBusiness.DeletePackageCourse(packageCourse);
                    if (data == 1)
                    {
                        successResponse.response_code = 0;
                        successResponse.message = "Package course deleted";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "No package course found";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
                    }
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "You are not authorized.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(401, unsuccessResponse);
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
        #endregion
    }
}