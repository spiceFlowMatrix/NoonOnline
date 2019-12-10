using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.FeedBackTask;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeedBackTaskController : ControllerBase
    {
        private readonly FeedBackTaskBusiness FeedBackTaskBusiness;
        private readonly LessonBusiness LessonBusiness;
        private IHostingEnvironment hostingEnvironment;

        public FeedBackTaskController
        (
            FeedBackTaskBusiness FeedBackTaskBusiness,
            LessonBusiness LessonBusiness,
            IHostingEnvironment hostingEnvironment
        )
        {
            this.FeedBackTaskBusiness = FeedBackTaskBusiness;
            this.LessonBusiness = LessonBusiness;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpPost("ChangeTaskStatus/{id}")]
        public IActionResult ChangeTaskStatus(long id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (ModelState.IsValid)
                {
                    string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");

                    if (tc.RoleName.Contains(General.getRoleType("11")) || tc.RoleName.Contains(General.getRoleType("1")))
                    {
                        int i = FeedBackTaskBusiness.UpdateTaskStatus(2, id, int.Parse(tc.Id));
                        if (i == 0)
                        {
                            unsuccessResponse.response_code = 2;
                            unsuccessResponse.message = "Already approved";
                            unsuccessResponse.status = "Failure";
                            return StatusCode(422, unsuccessResponse);
                        }
                    }
                    if (tc.RoleName.Contains(General.getRoleType("6")) || tc.RoleName.Contains(General.getRoleType("1")))
                    {
                        int i = FeedBackTaskBusiness.UpdateTaskStatus(3, id, int.Parse(tc.Id));
                        if (i == 0)
                        {
                            unsuccessResponse.response_code = 2;
                            unsuccessResponse.message = "Already completed";
                            unsuccessResponse.status = "Failure";
                            return StatusCode(422, unsuccessResponse);
                        }
                    }

                    successResponse.data = FeedBackTaskBusiness.getFeedBackTaskById(id, Certificate);
                    successResponse.response_code = 0;
                    successResponse.message = "FeedBackTask status changed";
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


        [HttpPost]
        public IActionResult Post([FromBody] AddFeedBackTaskModel FeedBackTaskModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");

                if (ModelState.IsValid)
                {
                    FeedBackTask newFeedBackTask = FeedBackTaskBusiness.Create(FeedBackTaskModel, int.Parse(tc.Id));

                    successResponse.data = FeedBackTaskBusiness.getFeedBackTaskById(newFeedBackTask.Id, Certificate);
                    successResponse.response_code = 0;
                    successResponse.message = "FeedBackTask Created";
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
        public IActionResult Put(int id, UpdateFeedBackTaskModel updateFeedBackTaskModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");

                if (ModelState.IsValid)
                {
                    updateFeedBackTaskModel.id = id;
                    FeedBackTask newFeedBackTask = FeedBackTaskBusiness.Update(updateFeedBackTaskModel, int.Parse(tc.Id));

                    successResponse.data = FeedBackTaskBusiness.getFeedBackTaskById(newFeedBackTask.Id, Certificate);
                    successResponse.response_code = 0;
                    successResponse.message = "FeedBackTask updated";
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
                    FeedBackTask newFeedBackTask = FeedBackTaskBusiness.Delete(id, int.Parse(tc.Id));
                    ResponseFeedBackTaskModel responseFeedBackTaskModel = new ResponseFeedBackTaskModel
                    {
                        id = newFeedBackTask.Id,
                        description = newFeedBackTask.Description
                    };

                    successResponse.data = responseFeedBackTaskModel;
                    successResponse.response_code = 0;
                    successResponse.message = "FeedBackTask Deleted";
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
                    string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                    successResponse.data = FeedBackTaskBusiness.getFeedBackTaskById(id, Certificate);
                    successResponse.response_code = 0;
                    successResponse.message = "FeedBackTask Detail";
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
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");

                List<FeedBackTask> FeedBackTaskList = new List<FeedBackTask>();
                List<ResponseFeedBackTaskModel> FeedBackTaskResponseList = new List<ResponseFeedBackTaskModel>();
                FeedBackTaskList = FeedBackTaskBusiness.FeedBackTaskList(paginationModel, out int total);

                foreach (FeedBackTask newFeedBackTask in FeedBackTaskList)
                {
                    FeedBackTaskResponseList.Add(FeedBackTaskBusiness.getFeedBackTaskById(newFeedBackTask.Id, Certificate));
                }

                successResponse.data = FeedBackTaskResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "FeedBackTask Details";
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