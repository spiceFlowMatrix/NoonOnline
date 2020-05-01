using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.BundleCourse;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.CourseGrade;
using Trainning24.BL.ViewModels.CourseItemProgressSync;
using Trainning24.BL.ViewModels.Grade;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = nameof(SwaggerGrouping.Course))]
    //[Authorize]
    public class CourseController : ControllerBase
    {
        private readonly CourseBusiness CourseBusiness;
        private readonly CourseGradeBusiness CourseGradeBusiness;
        private readonly CourseItemProgressSyncBusiness CourseItemProgressSyncBusiness;
        private IHostingEnvironment hostingEnvironment;
        private readonly FilesBusiness FilesBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly StudentCourseBusiness StudentCourseBusiness;
        private readonly UsersBusiness _usersBusiness;

        public CourseController
        (
            IHostingEnvironment hostingEnvironment,
            CourseBusiness CourseBusiness,
            CourseItemProgressSyncBusiness CourseItemProgressSyncBusiness,
            LessonBusiness LessonBusiness,
            FilesBusiness FilesBusiness,
            CourseGradeBusiness CourseGradeBusiness,
            StudentCourseBusiness StudentCourseBusiness,
            UsersBusiness usersBusiness
        )
        {
            this.hostingEnvironment = hostingEnvironment;
            this.CourseBusiness = CourseBusiness;
            this.LessonBusiness = LessonBusiness;
            this.CourseItemProgressSyncBusiness = CourseItemProgressSyncBusiness;
            this.FilesBusiness = FilesBusiness;
            this.CourseGradeBusiness = CourseGradeBusiness;
            this.StudentCourseBusiness = StudentCourseBusiness;
            this._usersBusiness = usersBusiness;
        }

        [Authorize]
        [HttpPost("CourseItemProgressSync")]
        public IActionResult AddCourseItemProgressSyncBusiness(List<AddCourseItemProgressSync> addCourseItemProgressSyncs)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //get claims after decoding id_token 
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            try
            {
                successResponse.data = CourseItemProgressSyncBusiness.Create(addCourseItemProgressSyncs);
                successResponse.response_code = 0;
                successResponse.message = "CourseItemProgressSync";
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
        public async Task<IActionResult> PostAsync()
        {
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            var credential = GoogleCredential.FromFile(jsonPath);
            var _storageClient = StorageClient.Create(credential);

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
                        string fileName = "";
                        IFormFile file = null;

                        if (Request.Form.Files.Count != 0)
                            file = Request.Form.Files[0];

                        string mediaLink = "";

                        //if (file != null)
                        //{
                        //    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        //    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        //    var ext = fileName.Substring(fileName.LastIndexOf("."));
                        //    var extension = ext.ToLower();
                        //    if (AllowedFileExtensions.Contains(extension))
                        //    {
                        //        Guid imageGuid = Guid.NewGuid();
                        //        fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                        //        var imageAcl = PredefinedObjectAcl.PublicRead;
                        //        var imageObject = await _storageClient.UploadObjectAsync(
                        //            bucket: "edg-primary-course-image-storage",
                        //            objectName: fileName,
                        //            contentType: file.ContentType,
                        //            source: file.OpenReadStream(),
                        //            options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        //        );
                        //        mediaLink = imageObject.MediaLink;
                        //    }
                        //}

                        if (!string.IsNullOrEmpty(Request.Form["filename"].ToString()))
                        {
                            var storageObject = _storageClient.GetObject("edg-primary-course-image-storage", Request.Form["filename"].ToString());
                            mediaLink = storageObject.MediaLink;
                        }

                        AddCourseModel CourseModel = new AddCourseModel
                        {
                            image = mediaLink,
                            name = Request.Form["name"],
                            code = Request.Form["code"],
                            description = Request.Form["description"],
                            gradeid = string.IsNullOrEmpty(Request.Form["gradeid"]) ? 0 : long.Parse(Request.Form["gradeid"]),
                            istrial = string.IsNullOrEmpty(Request.Form["istrial"]) ? false : bool.Parse(Request.Form["istrial"])
                        };

                        Course newCourse = CourseBusiness.Create(CourseModel, int.Parse(tc.Id));

                        ResponseCourseModel responseCourseModel = new ResponseCourseModel
                        {
                            Name = newCourse.Name,
                            Id = int.Parse(newCourse.Id.ToString()),
                            Code = newCourse.Code,
                            Description = newCourse.Description,
                            Image = newCourse.Image,
                            gradeid = CourseBusiness.GetGradeByCourseId(newCourse.Id).FirstOrDefault().id,
                            gradename = CourseBusiness.GetGradeByCourseId(newCourse.Id).FirstOrDefault().name,
                            istrial = newCourse.istrial
                        };

                        successResponse.data = responseCourseModel;
                        successResponse.response_code = 0;
                        successResponse.message = "Course Created";
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
        public async Task<IActionResult> Put(int id)
        {
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            var credential = GoogleCredential.FromFile(jsonPath);
            var _storageClient = StorageClient.Create(credential);

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
                        //string fileName = "";
                        //IFormFile file = null;

                        //if (Request.Form.Files.Count != 0)
                        //    file = Request.Form.Files[0];
                        //string mediaLink = "";
                        UpdateCourseModel updateCourseModel = new UpdateCourseModel();
                        //if (file != null)
                        //{
                        //    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        //    IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        //    var ext = fileName.Substring(fileName.LastIndexOf("."));
                        //    var extension = ext.ToLower();

                        //    if (AllowedFileExtensions.Contains(extension))
                        //    {
                        //        Guid imageGuid = Guid.NewGuid();
                        //        fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                        //        var imageAcl = PredefinedObjectAcl.PublicRead;
                        //        var imageObject = await _storageClient.UploadObjectAsync(
                        //            bucket: "edg-primary-course-image-storage",
                        //            objectName: fileName,
                        //            contentType: file.ContentType,
                        //            source: file.OpenReadStream(),
                        //            options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        //        );
                        //        mediaLink = imageObject.MediaLink;
                        //        updateCourseModel.image = mediaLink;
                        //    }
                        //}

                        //Get Media link for course cover image
                        string mediaLink = "";
                        if(!string.IsNullOrEmpty(Request.Form["filename"].ToString()))
                        {
                            var storageObject = _storageClient.GetObject("edg-primary-course-image-storage", Request.Form["filename"].ToString());
                            mediaLink = storageObject.MediaLink;
                        }

                        updateCourseModel.image = mediaLink;
                        updateCourseModel.name = Request.Form["name"];
                        updateCourseModel.code = Request.Form["code"];
                        updateCourseModel.description = Request.Form["description"];
                        updateCourseModel.gradeid = string.IsNullOrEmpty(Request.Form["gradeid"]) ? 0 : long.Parse(Request.Form["gradeid"]);
                        updateCourseModel.istrial = string.IsNullOrEmpty(Request.Form["istrial"]) ? false : bool.Parse(Request.Form["istrial"]);
                        updateCourseModel.id = id;
                        Course newCourse = CourseBusiness.Update(updateCourseModel, int.Parse(tc.Id));

                        //send push notification
                        CourseBusiness.SendNotificationOnCourseUpdate(newCourse, jsonPath, int.Parse(tc.Id));

                        ResponseCourseModel responseCourseModel = new ResponseCourseModel
                        {
                            Name = newCourse.Name,
                            Id = int.Parse(newCourse.Id.ToString()),
                            Code = newCourse.Code,
                            Description = newCourse.Description,
                            Image = newCourse.Image,
                            gradeid = CourseBusiness.GetGradeByCourseId(newCourse.Id).FirstOrDefault().id,
                            gradename = CourseBusiness.GetGradeByCourseId(newCourse.Id).FirstOrDefault().name
                        };

                        successResponse.data = responseCourseModel;
                        successResponse.response_code = 0;
                        successResponse.message = "Course updated";
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
                        var DeleteCourse = CourseBusiness.Delete(id, int.Parse(tc.Id));
                        successResponse.response_code = 0;
                        successResponse.message = "Course Deleted";
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
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("1")))
                    {
                        Course newCourse = CourseBusiness.getCourseById(id);


                        string imageurl = "";
                        if (!string.IsNullOrEmpty(newCourse.Image))
                        {
                            if (newCourse.Image.Contains("t24-primary-image-storage"))
                                imageurl = newCourse.Image;
                            else
                                imageurl = LessonBusiness.geturl(newCourse.Image, Certificate);
                        }

                        ResponseCourseModel responseCourseModel = new ResponseCourseModel
                        {
                            Name = newCourse.Name,
                            Id = int.Parse(newCourse.Id.ToString()),
                            Code = newCourse.Code,
                            Description = newCourse.Description,
                            Image = imageurl,
                            istrial = newCourse.istrial
                        };

                        List<ResponseGradeModel> responseGradeModels = CourseBusiness.GetGradeByCourseId(newCourse.Id);
                        if (responseGradeModels.Count > 0)
                        {
                            responseCourseModel.gradeid = CourseBusiness.GetGradeByCourseId(newCourse.Id).FirstOrDefault().id;
                            responseCourseModel.gradename = CourseBusiness.GetGradeByCourseId(newCourse.Id).FirstOrDefault().name;
                        }

                        successResponse.data = responseCourseModel;
                        successResponse.response_code = 0;
                        successResponse.message = "Course Detail";
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
            PaginationModel paginationModel = new PaginationModel
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord,
                search = search
            };

            //get claims after decoding id_token 
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //var Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                var Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    List<Course> CourseList = new List<Course>();
                    List<ResponseCourseModel> CourseResponseList = new List<ResponseCourseModel>();
                    CourseList = CourseBusiness.CourseList(paginationModel, out int total);
                    var tem = 0;
                    foreach (Course newCourse in CourseList)
                    {
                        tem++;
                        string imageurl = "";
                        if (!string.IsNullOrEmpty(newCourse.Image))
                        {
                            if (newCourse.Image.Contains("t24-primary-image-storage"))
                                imageurl = newCourse.Image;
                            else
                                imageurl = LessonBusiness.geturl(newCourse.Image, Certificate);
                        }
                        List<ResponseGradeModel> responseGradeModels = CourseBusiness.GetGradeByCourseId(newCourse.Id);
                        ResponseCourseModel responseCourseModel = new ResponseCourseModel
                        {
                            Name = newCourse.Name,
                            Id = int.Parse(newCourse.Id.ToString()),
                            Code = newCourse.Code,
                            Description = newCourse.Description,
                            istrial = newCourse.istrial,
                            Image = imageurl
                        };

                        if (responseGradeModels.Count > 0)
                        {
                            responseCourseModel.gradeid = CourseBusiness.GetGradeByCourseId(newCourse.Id).FirstOrDefault().id;
                            responseCourseModel.gradename = CourseBusiness.GetGradeByCourseId(newCourse.Id).FirstOrDefault().name;
                        }

                        CourseResponseList.Add(responseCourseModel);
                    }
                    successResponse.data = CourseResponseList;
                    successResponse.totalcount = total;
                    successResponse.response_code = 0;
                    successResponse.message = "Course Details";
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
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpGet("GetCourseList")]
        public IActionResult GetCourseList(int pagenumber, int perpagerecord, string search)
        {
            PaginationModel paginationModel = new PaginationModel
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord,
                search = search
            };

            //get claims after decoding id_token 
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    var CourseList = CourseBusiness.GetCourseList(paginationModel, out int total);
                    successResponse.data = CourseList;
                    successResponse.totalcount = total;
                    successResponse.response_code = 0;
                    successResponse.message = "Course Details";
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
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost]
        [Route("CourseGrade")]
        public IActionResult PostCourseGrade([FromBody] AddCourseGradeModel CourseGradeModel)
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
                        CourseGrade newCourseGrade = CourseGradeBusiness.Create(CourseGradeModel, int.Parse(tc.Id));

                        if (newCourseGrade == null)
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Allready assigned";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(422, unsuccessResponse);
                        }

                        ResponseCourseGradeModel responseCourseGradeModel = new ResponseCourseGradeModel
                        {
                            id = newCourseGrade.Id,
                            courseid = newCourseGrade.CourseId,
                            gradeid = newCourseGrade.Gradeid
                        };

                        successResponse.data = responseCourseGradeModel;
                        successResponse.response_code = 0;
                        successResponse.message = "CourseGrade Created";
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

        [HttpPut]
        [Route("CourseGrade/{id}")]
        public IActionResult PutCourseGrade(int id, UpdateCourseGradeModel updateCourseGradeModel)
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
                        updateCourseGradeModel.id = id;
                        CourseGrade newCourseGrade = CourseGradeBusiness.Update(updateCourseGradeModel, int.Parse(tc.Id));
                        ResponseCourseGradeModel responseCourseGradeModel = new ResponseCourseGradeModel
                        {
                            id = newCourseGrade.Id,
                            courseid = newCourseGrade.CourseId,
                            gradeid = newCourseGrade.Gradeid
                        };

                        successResponse.data = responseCourseGradeModel;
                        successResponse.response_code = 0;
                        successResponse.message = "CourseGrade updated";
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

        [HttpDelete]
        [Route("CourseGrade/{id}")]
        public IActionResult DeleteCourseGrade(int id)
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
                        CourseGrade newCourseGrade = CourseGradeBusiness.Delete(id, int.Parse(tc.Id));

                        ResponseCourseGradeModel responseCourseGradeModel = new ResponseCourseGradeModel
                        {
                            id = newCourseGrade.Id,
                            courseid = newCourseGrade.CourseId,
                            gradeid = newCourseGrade.Gradeid
                        };

                        successResponse.data = responseCourseGradeModel;
                        successResponse.response_code = 0;
                        successResponse.message = "CourseGrade Deleted";
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
        [Route("CourseGrade/{id}")]
        public IActionResult GetCourseGrade(int id)
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
                    if (tc.RoleName.Contains(General.getRoleType("1")))
                    {
                        CourseGrade newCourseGrade = CourseGradeBusiness.getCourseGradeById(id);
                        ResponseCourseGradeModel responseCourseGradeModel = new ResponseCourseGradeModel
                        {
                            id = newCourseGrade.Id,
                            courseid = newCourseGrade.CourseId,
                            gradeid = newCourseGrade.Gradeid
                        };

                        successResponse.data = responseCourseGradeModel;
                        successResponse.response_code = 0;
                        successResponse.message = "CourseGrade Detail";
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
        [Route("GetCourseByGradeId/{id}")]
        public IActionResult GetCourseGrade(int id, int pagenumber, int perpagerecord)
        {
            BundleCoursePaginationModel paginationModel = new BundleCoursePaginationModel
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord,
                bundleid = id
            };


            //string Authorization = Request.Headers["Authorization"];
            //TokenClaims tc = General.GetClaims(Authorization);
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                List<CourseGrade> CourseGradeList = new List<CourseGrade>();
                List<ResponseCourseByGrade> CourseGradeResponseList = new List<ResponseCourseByGrade>();
                CourseGradeList = CourseGradeBusiness.CourseGradeList(paginationModel, out int total);

                foreach (CourseGrade newCourseGrade in CourseGradeList)
                {
                    ResponseCourseByGrade ResponseBundleModel = new ResponseCourseByGrade();
                    Course course = CourseBusiness.getCourseById(int.Parse(newCourseGrade.CourseId.ToString()));

                    if (course != null)
                    {
                        ResponseBundleModel.id = newCourseGrade.Id;
                        ResponseBundleModel.gradeid = newCourseGrade.Gradeid;
                        ResponseBundleModel.courseid = newCourseGrade.CourseId;
                        ResponseBundleModel.name = course.Name;
                        ResponseBundleModel.code = course.Code;
                        CourseGradeResponseList.Add(ResponseBundleModel);
                    }
                }

                successResponse.data = CourseGradeResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Courses by GradeId";
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

        [HttpGet("CoursePriview/{id}/{studentid?}")]
        public IActionResult CoursePriview(long id, long studentid)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                //get claims after decoding id_token 
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (tc.RoleName.Contains(General.getRoleType("1")) ||
                    tc.RoleName.Contains(General.getRoleType("3")) ||
                    tc.RoleName.Contains(General.getRoleType("4")))
                {
                    CoursePreviewModel coursePreview = new CoursePreviewModel();
                    if (studentid == 0)
                        coursePreview = CourseBusiness.getCoursePreviewById(id, Certificate);
                    else
                        coursePreview = CourseBusiness.getCoursePreviewById(id, studentid, Certificate);
                    if (coursePreview == null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Course not found";
                        unsuccessResponse.status = "Success";
                        return StatusCode(404, unsuccessResponse);
                    }
                    successResponse.data = coursePreview;
                    successResponse.response_code = 0;
                    successResponse.message = "Course detail";
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
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpGet("CoursePriviewTest/{id}/{studentid?}")]
        public IActionResult CoursePriviewTest(long id, long studentid)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                //get claims after decoding id_token 
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (tc.RoleName.Contains(General.getRoleType("1")) ||
                    tc.RoleName.Contains(General.getRoleType("3")) ||
                    tc.RoleName.Contains(General.getRoleType("4")))
                {
                    CoursePreviewModel coursePreview = new CoursePreviewModel();
                    if (studentid == 0)
                        coursePreview = CourseBusiness.getCoursePreviewByIdTest(id, Certificate);
                    else
                        coursePreview = CourseBusiness.getCoursePreviewByIdTest(id, studentid, Certificate);
                    if (coursePreview == null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Course not found";
                        unsuccessResponse.status = "Success";
                        return StatusCode(404, unsuccessResponse);
                    }
                    successResponse.data = coursePreview;
                    successResponse.response_code = 0;
                    successResponse.message = "Course detail";
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
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpGet("CoursePriviewGradeWise")]
        public IActionResult CoursePriviewGradeWise(int pagenumber, int perpagerecord, string search, int gradeid)
        {
            PaginationModel paginationModel = new PaginationModel();
            paginationModel.pagenumber = pagenumber;
            paginationModel.perpagerecord = perpagerecord;
            paginationModel.search = search;
            paginationModel.roleid = gradeid;
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Authorization = Request.Headers["id_token"];
                //get claims after decoding id_token 
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx

                CourseBusiness.updateTrailCourse(long.Parse(tc.Id));
                var user = _usersBusiness.GetUserbyId(long.Parse(tc.Id));
                object coursePreview = null;
                if (user.istrial)
                {
                    var days = (DateTime.Now.Date - Convert.ToDateTime(user.CreationTime, CultureInfo.InvariantCulture).Date).Days;
                    if (days < 15)
                    {
                        coursePreview = CourseBusiness.getTrialCoursePriviewGradeWise(long.Parse(tc.Id.ToString()), paginationModel, Certificate, out int total);
                        successResponse.totalcount = total;
                    }
                }
                else
                {
                    coursePreview = CourseBusiness.getCoursePriviewGradeWise(long.Parse(tc.Id.ToString()), paginationModel, Certificate, out int total);
                    successResponse.totalcount = total;
                }

                successResponse.data = coursePreview;
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

        [HttpGet("ConvertedGeneratedUrl")]
        public IActionResult ConvertedGeneratedUrl()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                var credential = GoogleCredential.FromFile(jsonPath);
                var storage = StorageClient.Create(credential);

                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (
                tc.RoleName.Contains(General.getRoleType("1")) ||
                tc.RoleName.Contains(General.getRoleType("3"))
                )
                {
                    foreach (var obj in storage.ListObjects("edg-primary-course-image-storage", ""))
                    {
                        if (obj.MediaLink == "https://www.googleapis.com/download/storage/v1/b/edg-primary-course-image-storage/o/splash_b6225b52-ffce-4c6b-ad68-4e03eb31eccd.png?generation=1537353117899874&alt=media")
                        {
                            string str = "";
                        }

                    }

                    successResponse.data = null;
                    successResponse.response_code = 0;
                    successResponse.message = "file converted";
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
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [Authorize]
        [HttpGet("GetAllDetails")]
        public IActionResult GetAllDetails(int pagenumber, int perpagerecord, string filter, string bygrade, string search)
        {
            PaginationModel paginationModel = new PaginationModel();
            paginationModel.pagenumber = pagenumber;
            paginationModel.perpagerecord = perpagerecord;
            paginationModel.search = search;

            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            //get claims after decoding id_token 
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (search == null)
                {
                    successResponse.response_code = 1;
                    successResponse.message = "No data found";
                    successResponse.status = "Success";
                    return StatusCode(422, successResponse);
                }
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                string[] filterArray = null;
                string[] gradeArray = null;
                if (!string.IsNullOrEmpty(filter))
                {
                    filterArray = filter.Split(",");
                }
                if (!string.IsNullOrEmpty(bygrade))
                {
                    gradeArray = bygrade.Split(",");
                }

                var allDetail = CourseBusiness.GetAllDetails(long.Parse(tc.Id.ToString()), search, filterArray, gradeArray, Certificate);

                successResponse.data = allDetail;
                successResponse.response_code = 0;
                successResponse.message = "GetAllDetails";// + Environment.GetEnvironmentVariable("ASPNET_DB_CONNECTIONSTRING");
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
