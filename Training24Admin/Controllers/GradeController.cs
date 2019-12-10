using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Grade;
using Trainning24.BL.ViewModels.School;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    //[Authorize]
    public class GradeController : ControllerBase
    {
        private readonly GradeBusiness GradeBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly SchoolBusiness SchoolBusiness;

        public GradeController
        (
            GradeBusiness GradeBusiness,
            LessonBusiness LessonBusiness,
            SchoolBusiness SchoolBusiness
        )
        {
            this.GradeBusiness = GradeBusiness;
            this.LessonBusiness = LessonBusiness;
            this.SchoolBusiness = SchoolBusiness;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddGradeModel GradeModel)
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
                    //if (tc.RoleId == "1")
                    //{
                    Grade newGrade = GradeBusiness.Create(GradeModel, int.Parse(tc.Id));
                    ResponseGradeModel1 responseGradeModel = new ResponseGradeModel1
                    {
                        id = newGrade.Id,
                        name = newGrade.Name,
                        description = newGrade.Description
                    };

                    School newSchool = SchoolBusiness.getSchoolById(newGrade.SchoolId);
                    if (newSchool != null)
                    {
                        ResponseSchoolModel responseSchoolModel = new ResponseSchoolModel
                        {
                            id = newSchool.Id,
                            name = newSchool.Name,
                            code = newSchool.Code
                        };
                        responseGradeModel.school = responseSchoolModel;
                    }


                    successResponse.data = responseGradeModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Grade Created";
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

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateGradeModel updateGradeModel)
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
                    //if (tc.RoleId == "1")
                    //{
                    updateGradeModel.id = id;
                    Grade newGrade = GradeBusiness.Update(updateGradeModel, int.Parse(tc.Id));
                    ResponseGradeModel1 responseGradeModel = new ResponseGradeModel1
                    {
                        id = newGrade.Id,
                        name = newGrade.Name,
                        description = newGrade.Description
                    };

                    School newSchool = SchoolBusiness.getSchoolById(newGrade.SchoolId);
                    if (newSchool != null)
                    {
                        ResponseSchoolModel responseSchoolModel = new ResponseSchoolModel
                        {
                            id = newSchool.Id,
                            name = newSchool.Name,
                            code = newSchool.Code
                        };
                        responseGradeModel.school = responseSchoolModel;
                    }

                    successResponse.data = responseGradeModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Grade updated";
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
                    //if (tc.RoleId == "1")
                    //{
                    Grade newGrade = GradeBusiness.Delete(id, int.Parse(tc.Id));

                    ResponseGradeModel1 responseGradeModel = new ResponseGradeModel1
                    {
                        id = newGrade.Id,
                        name = newGrade.Name,
                        description = newGrade.Description
                    };

                    School newSchool = SchoolBusiness.getSchoolById(newGrade.SchoolId);
                    if (newSchool != null)
                    {
                        ResponseSchoolModel responseSchoolModel = new ResponseSchoolModel
                        {
                            id = newSchool.Id,
                            name = newSchool.Name,
                            code = newSchool.Code
                        };
                        responseGradeModel.school = responseSchoolModel;
                    }

                    successResponse.data = responseGradeModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Grade Deleted";
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

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();


            //string Authorization = Request.Headers["Authorization"];
            //TokenClaims tc = General.GetClaims(Authorization);

            try
            {

                if (ModelState.IsValid)
                {
                    //if (tc.RoleId == "1")
                    //{
                    Grade newGrade = GradeBusiness.getGradeById(id);

                    ResponseGradeModel1 responseGradeModel = new ResponseGradeModel1
                    {
                        id = newGrade.Id,
                        name = newGrade.Name,
                        description = newGrade.Description
                    };

                    School newSchool = SchoolBusiness.getSchoolById(newGrade.SchoolId);
                    if (newSchool != null)
                    {
                        ResponseSchoolModel responseSchoolModel = new ResponseSchoolModel
                        {
                            id = newSchool.Id,
                            name = newSchool.Name,
                            code = newSchool.Code
                        };
                        responseGradeModel.school = responseSchoolModel;
                    }

                    successResponse.data = responseGradeModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Grade Detail";
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
                List<Grade> GradeList = new List<Grade>();
                List<ResponseGradeModel1> GradeResponseList = new List<ResponseGradeModel1>();
                GradeList = GradeBusiness.GradeList(paginationModel, out int total);

                foreach (Grade newGrade in GradeList)
                {
                    ResponseGradeModel1 responseGradeModel = new ResponseGradeModel1
                    {
                        id = newGrade.Id,
                        name = newGrade.Name,
                        description = newGrade.Description
                    };

                    School newSchool = SchoolBusiness.getSchoolById(newGrade.SchoolId);
                    if (newSchool != null)
                    {
                        ResponseSchoolModel responseSchoolModel = new ResponseSchoolModel
                        {
                            id = newSchool.Id,
                            name = newSchool.Name,
                            code = newSchool.Code
                        };
                        responseGradeModel.school = responseSchoolModel;
                    }

                    GradeResponseList.Add(responseGradeModel);
                }

                successResponse.data = GradeResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Grade Details";
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

        [HttpGet("GetGradeList")]
        public IActionResult GetGradeList(int pagenumber, int perpagerecord, string search)
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
                List<Grade> GradeList = new List<Grade>();
                List<ResponseGradeModel> GradeResponseList = new List<ResponseGradeModel>();
                GradeList = GradeBusiness.GradeList(paginationModel, out int total);

                foreach (Grade newGrade in GradeList)
                {
                    ResponseGradeModel responseGradeModel = new ResponseGradeModel
                    {
                        id = newGrade.Id,
                        name = newGrade.Name,
                        description = newGrade.Description
                    };
                    GradeResponseList.Add(responseGradeModel);
                }

                successResponse.data = GradeResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Grade Details";
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