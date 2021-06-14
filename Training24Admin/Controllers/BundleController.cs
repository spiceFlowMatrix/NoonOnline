using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Bundle;
using Trainning24.BL.ViewModels.BundleCourse;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class BundleController : ControllerBase
    {
        private readonly BundleBusiness bundleBusiness;
        private readonly CourseBusiness courseBusiness;
        private readonly LessonBusiness LessonBusiness;

        public BundleController
        (
            BundleBusiness bundleBusiness,
            CourseBusiness courseBusiness,
            LessonBusiness LessonBusiness
        )
        {
            this.bundleBusiness = bundleBusiness;
            this.courseBusiness = courseBusiness;
            this.LessonBusiness = LessonBusiness;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddBundleModel bundleModel)
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
                        Bundle newBundle= bundleBusiness.Create(bundleModel, int.Parse(tc.Id));

                        ResponseBundleModel responseBundleModel = new ResponseBundleModel();

                        responseBundleModel.Name = newBundle.Name;
                        responseBundleModel.Id = int.Parse(newBundle.Id.ToString());

                        successResponse.data = responseBundleModel;
                        successResponse.response_code = 0;
                        successResponse.message = "Bundle Created";
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
        public IActionResult Put(int id, UpdateBundleModel updateBundleModel)
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
                        updateBundleModel.Id = id;
                        Bundle newBundle = bundleBusiness.Update(updateBundleModel, int.Parse(tc.Id));
                        ResponseBundleModel responseBundleModel = new ResponseBundleModel();
                        responseBundleModel.Name = newBundle.Name;
                        successResponse.data = responseBundleModel;
                        successResponse.response_code = 0;
                        successResponse.message = "Bundle updated";
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
                        Bundle bundle = bundleBusiness.Delete(id,int.Parse(tc.Id));

                        ResponseBundleModel ResponseBundleModel = new ResponseBundleModel();
                        ResponseBundleModel.Id = int.Parse(bundle.Id.ToString());
                        ResponseBundleModel.Name = bundle.Name;

                        successResponse.data = ResponseBundleModel;
                        successResponse.response_code = 0;
                        successResponse.message = "Bundle Deleted";
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
            string Authorization = Request.Headers["Authorization"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            try
            {

                if (ModelState.IsValid)
                {
                    //if (tc.RoleId == "1")
                    //{
                        Bundle bundle = bundleBusiness.getBundleById(id);

                        ResponseBundleModel ResponseBundleModel = new ResponseBundleModel();
                        ResponseBundleModel.Id = int.Parse(bundle.Id.ToString());
                        ResponseBundleModel.Name = bundle.Name;

                        successResponse.data = ResponseBundleModel;
                        successResponse.response_code = 0;
                        successResponse.message = "Bundle Detail";
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
            string Authorization = Request.Headers["Authorization"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                List<Bundle> bundleList = new List<Bundle>();
                List<ResponseBundleModel> bundleResponseList = new List<ResponseBundleModel>();
                bundleList = bundleBusiness.BundleList(paginationModel, out int total);

                foreach (Bundle bundle in bundleList)
                {
                    ResponseBundleModel ResponseBundleModel = new ResponseBundleModel();
                    ResponseBundleModel.Id = int.Parse(bundle.Id.ToString());
                    ResponseBundleModel.Name = bundle.Name;
                    bundleResponseList.Add(ResponseBundleModel);
                }

                successResponse.data = bundleResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Bundle Details";
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
        [Route("AddBundleCourse")]
        public IActionResult AddBundleCourse([FromBody] AddBundleCourseModel BundleCourseModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            //get claims after decoding id_token 
            string Authorization = Request.Headers["Authorization"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            try
            {
                if (ModelState.IsValid)
                {
                    //if (tc.RoleId == "1")
                    //{
                        BundleCourse newBundle = bundleBusiness.CreateBundleCourse(BundleCourseModel, int.Parse(tc.Id));


                        if (newBundle == null) {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Duplicate entry.";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(422, unsuccessResponse);
                        }

                        ResponseBundleCourseModel responseBundleCourse = new ResponseBundleCourseModel();


                        responseBundleCourse.bundleid = newBundle.BundleId;
                        responseBundleCourse.courseid = newBundle.CourseId;                        
                        responseBundleCourse.id = newBundle.Id;                        


                        successResponse.data = responseBundleCourse;
                        successResponse.response_code = 0;
                        successResponse.message = "BundleCourse Created";
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
        [Route("DeleteBundleCourse/{id}")]
        public IActionResult DeleteBundleCourse(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            //get claims after decoding id_token 
            string Authorization = Request.Headers["Authorization"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            try
            {

                if (ModelState.IsValid)
                {
                    //if (tc.RoleId == "1")
                    //{
                        BundleCourse newBundle = bundleBusiness.DeleteBundleCourse(id, int.Parse(tc.Id));

                        ResponseBundleCourseModel responseBundleCourse = new ResponseBundleCourseModel();

                        responseBundleCourse.bundleid = newBundle.BundleId;
                        responseBundleCourse.courseid = newBundle.CourseId;
                        responseBundleCourse.id = newBundle.Id;

                        successResponse.data = responseBundleCourse;
                        successResponse.response_code = 0;
                        successResponse.message = "BundleCourse Deleted";
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
        [Route("GetBundleCourse")]
        public IActionResult GetBundleCourse(int pagenumber, int perpagerecord)
        {
            PaginationModel paginationModel = new PaginationModel();
            paginationModel.pagenumber = pagenumber;
            paginationModel.perpagerecord = perpagerecord;


            //get claims after decoding id_token 
            string Authorization = Request.Headers["Authorization"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                List<BundleCourse> bundleList = new List<BundleCourse>();
                List<ResponseBundleCourseModel> bundleResponseList = new List<ResponseBundleCourseModel>();
                bundleList = bundleBusiness.BundleCourseList(paginationModel, out int total);

                foreach (BundleCourse bundle in bundleList)
                {
                    ResponseBundleCourseModel ResponseBundleModel = new ResponseBundleCourseModel();
                    ResponseBundleModel.id = bundle.Id;
                    ResponseBundleModel.bundleid = bundle.BundleId;
                    ResponseBundleModel.courseid = bundle.CourseId;
                    bundleResponseList.Add(ResponseBundleModel);
                }

                successResponse.data = bundleResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "BundleCourse Details";
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
        [Route("GetBundleCourseById/{id}")]
        public IActionResult GetBundleCourseById(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            //get claims after decoding id_token 
            string Authorization = Request.Headers["Authorization"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            try
            {

                if (ModelState.IsValid)
                {
                    //if (tc.RoleId == "1")
                    //{
                        BundleCourse bundle = bundleBusiness.getBundleCourseById(id);

                        ResponseBundleCourseModel ResponseBundleModel = new ResponseBundleCourseModel();
                        ResponseBundleModel.id = bundle.Id;
                        ResponseBundleModel.bundleid = bundle.BundleId;
                        ResponseBundleModel.courseid = bundle.CourseId;

                        successResponse.data = ResponseBundleModel;
                        successResponse.response_code = 0;
                        successResponse.message = "Bundle Detail";
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
        [Route("GetCourseByBundleId/{id}")]
        public IActionResult GetCourseByBundleId(int id, int pagenumber, int perpagerecord)
        {
            BundleCoursePaginationModel paginationModel = new BundleCoursePaginationModel();
            paginationModel.pagenumber = pagenumber;
            paginationModel.perpagerecord = perpagerecord;
            paginationModel.bundleid = id;

            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            //get claims after decoding id_token 
            string Authorization = Request.Headers["Authorization"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            try
            {

                if (ModelState.IsValid)
                {
                    //if (tc.RoleId == "1")
                    //{
                        List<BundleCourse> BundleCourseList = bundleBusiness.getCourseByBundleId(paginationModel,out int total);
                        List<ResponseCourseByBundle> bundleResponseList = new List<ResponseCourseByBundle>();

                        foreach (BundleCourse bundle in BundleCourseList)
                        {
                            ResponseCourseByBundle ResponseBundleModel = new ResponseCourseByBundle();
                            Course course = courseBusiness.getCourseById(int.Parse(bundle.CourseId.ToString()));
                            ResponseBundleModel.id = bundle.Id;
                            ResponseBundleModel.bundleid = bundle.BundleId;
                            ResponseBundleModel.courseid = bundle.CourseId;
                            ResponseBundleModel.name = course.Name;
                            ResponseBundleModel.code = course.Code;
                            bundleResponseList.Add(ResponseBundleModel);
                        }

                        successResponse.data = bundleResponseList;
                        successResponse.totalcount = total;
                        successResponse.response_code = 0;
                        successResponse.message = "Bundle Detail";
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
    }
}
