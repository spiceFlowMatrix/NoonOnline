using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.FeedBackActivity;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeedBackActivityController : ControllerBase
    {
        private readonly FeedBackActivityBusiness FeedBackActivityBusiness;
        private readonly LessonBusiness LessonBusiness;
        private IHostingEnvironment hostingEnvironment;

        public FeedBackActivityController
        (
            FeedBackActivityBusiness FeedBackActivityBusiness,
            LessonBusiness LessonBusiness,
            IHostingEnvironment hostingEnvironment
        )
        {
            this.FeedBackActivityBusiness = FeedBackActivityBusiness;
            this.LessonBusiness = LessonBusiness;
            this.hostingEnvironment = hostingEnvironment;
        }

        [HttpPost]
        public IActionResult Post([FromBody] AddFeedBackActivityModel FeedBackActivityModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
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
                    FeedBackActivity newFeedBackActivity = FeedBackActivityBusiness.Create(FeedBackActivityModel, int.Parse(tc.Id));
                    successResponse.data = FeedBackActivityBusiness.getFeedBackActivityById(newFeedBackActivity.Id, Certificate);
                    successResponse.response_code = 0;
                    successResponse.message = "FeedBackActivity Created";
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
        public IActionResult Put(int id, UpdateFeedBackActivityModel updateFeedBackActivityModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
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
                    updateFeedBackActivityModel.id = id;
                    FeedBackActivity newFeedBackActivity = FeedBackActivityBusiness.Update(updateFeedBackActivityModel, int.Parse(tc.Id));
                    successResponse.data = FeedBackActivityBusiness.getFeedBackActivityById(newFeedBackActivity.Id, Certificate);
                    successResponse.response_code = 0;
                    successResponse.message = "FeedBackActivity updated";
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
                    string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                    FeedBackActivity newFeedBackActivity = FeedBackActivityBusiness.Delete(id, int.Parse(tc.Id));
                    successResponse.data = FeedBackActivityBusiness.getFeedBackActivityById(newFeedBackActivity.Id, Certificate); ;
                    successResponse.response_code = 0;
                    successResponse.message = "FeedBackActivity Deleted";
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
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                if (ModelState.IsValid)
                {
                    successResponse.data = FeedBackActivityBusiness.getFeedBackActivityById(id,Certificate);
                    successResponse.response_code = 0;
                    successResponse.message = "FeedBackActivity Detail";
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
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                List<FeedBackActivity> FeedBackActivityList = new List<FeedBackActivity>();
                List<ResponseFeedBackActivityModel> FeedBackActivityResponseList = new List<ResponseFeedBackActivityModel>();
                FeedBackActivityList = FeedBackActivityBusiness.FeedBackActivityList(paginationModel, out int total);

                foreach (FeedBackActivity newFeedBackActivity in FeedBackActivityList)
                {
                    FeedBackActivityResponseList.Add(FeedBackActivityBusiness.getFeedBackActivityById(newFeedBackActivity.Id, Certificate));
                }

                successResponse.data = FeedBackActivityResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "FeedBackActivity Details";
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