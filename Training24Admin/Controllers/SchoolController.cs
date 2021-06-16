using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.School;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]    
    public class SchoolController : ControllerBase
    {
        private readonly SchoolBusiness SchoolBusiness;
        private readonly UsersBusiness UsersBusiness;
        private readonly LessonBusiness LessonBusiness;

        public SchoolController
        (
            SchoolBusiness SchoolBusiness,
            LessonBusiness LessonBusiness,
            UsersBusiness UsersBusiness
        )
        {
            this.SchoolBusiness = SchoolBusiness;
            this.LessonBusiness = LessonBusiness;
            this.UsersBusiness = UsersBusiness;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddSchoolModel SchoolModel)
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
                    School newSchool = SchoolBusiness.Create(SchoolModel, int.Parse(tc.Id));
                    ResponseSchoolModel responseSchoolModel = new ResponseSchoolModel
                    {
                        id = newSchool.Id,
                        name = newSchool.Name,
                        code = newSchool.Code
                    };

                    successResponse.data = responseSchoolModel;
                    successResponse.response_code = 0;
                    successResponse.message = "School Created";
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

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateSchoolModel updateSchoolModel)
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
                    updateSchoolModel.id = id;
                    School newSchool = SchoolBusiness.Update(updateSchoolModel, int.Parse(tc.Id));
                    ResponseSchoolModel responseSchoolModel = new ResponseSchoolModel
                    {
                        id = newSchool.Id,
                        name = newSchool.Name,
                        code = newSchool.Code
                    };

                    successResponse.data = responseSchoolModel;
                    successResponse.response_code = 0;
                    successResponse.message = "School updated";
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
                    School newSchool = SchoolBusiness.Delete(id, int.Parse(tc.Id));
                    ResponseSchoolModel responseSchoolModel = new ResponseSchoolModel
                    {
                        id = newSchool.Id,
                        name = newSchool.Name,
                        code = newSchool.Code
                    };

                    successResponse.data = responseSchoolModel;
                    successResponse.response_code = 0;
                    successResponse.message = "School Deleted";
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
                if (ModelState.IsValid)
                {
                    School newSchool = SchoolBusiness.getSchoolById(id);
                    ResponseSchoolModel responseSchoolModel = new ResponseSchoolModel
                    {
                        id = newSchool.Id,
                        name = newSchool.Name,
                        code = newSchool.Code
                    };

                    successResponse.data = responseSchoolModel;
                    successResponse.response_code = 0;
                    successResponse.message = "School Detail";
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
                List<School> SchoolList = new List<School>();
                List<ResponseSchoolModel> SchoolResponseList = new List<ResponseSchoolModel>();
                SchoolList = SchoolBusiness.SchoolList(paginationModel, out int total);

                foreach (School newSchool in SchoolList)
                {
                    ResponseSchoolModel responseSchoolModel = new ResponseSchoolModel
                    {
                        id = newSchool.Id,
                        name = newSchool.Name,
                        code = newSchool.Code,
                        creationtime = newSchool.CreationTime,                        
                        lastmodificationtime = newSchool.LastModificationTime                        
                    };

                    User user = new User();
                    AllFeedBackUser all = new AllFeedBackUser();

                    if (newSchool.CreatorUserId != null)
                    {
                        user = UsersBusiness.GetUserbyId(long.Parse(newSchool.CreatorUserId.Value.ToString()));
                        all.Id = user.Id;
                        all.Username = user.Username;
                        responseSchoolModel.creatoruserid = all;
                    }

                    if (newSchool.LastModifierUserId != null)
                    {
                        user = UsersBusiness.GetUserbyId(long.Parse(newSchool.LastModifierUserId.Value.ToString()));
                        all.Id = user.Id;
                        all.Username = user.Username;
                        responseSchoolModel.lastmodifieruserid = all;
                    }

                    SchoolResponseList.Add(responseSchoolModel);
                }

                successResponse.data = SchoolResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "School Details";
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