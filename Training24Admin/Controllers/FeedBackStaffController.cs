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
using Trainning24.BL.ViewModels.Feedback;
using Trainning24.BL.ViewModels.FeedbackStaff;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeedBackStaffController : ControllerBase
    {
        private readonly FeedbackStaffBusiness FeedbackStaffBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly FeedbackBusiness FeedbackBusiness;
        private IHostingEnvironment hostingEnvironment;

        public FeedBackStaffController
        (
            FeedbackStaffBusiness FeedbackStaffBusiness,
            LessonBusiness LessonBusiness,
            FeedbackBusiness FeedbackBusiness,
            IHostingEnvironment hostingEnvironment
        )
        {
            this.FeedbackStaffBusiness = FeedbackStaffBusiness;
            this.LessonBusiness = LessonBusiness;
            this.FeedbackBusiness = FeedbackBusiness;
            this.hostingEnvironment = hostingEnvironment;
        }


        [HttpPost]
        public async Task<IActionResult> Post([FromBody]AddFeedBackStaffModel FeedBackStaffModel)
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
                ResponseFeedbackStaff newFeedBackStaff = new ResponseFeedbackStaff();
                if (ModelState.IsValid)
                {
                    if (FeedBackStaffModel.type == 4)  //QA
                    {
                        Feedback feedback = FeedbackBusiness.CheckRecordExists(FeedBackStaffModel.feedbackid, FeedBackStaffModel.userid, FeedBackStaffModel.type);
                        if (feedback == null)
                        {
                            newFeedBackStaff = FeedbackBusiness.UpdateFeedBackManager(FeedBackStaffModel.feedbackid, FeedBackStaffModel.userid, FeedBackStaffModel.type, int.Parse(tc.Id), Certificate);
                            successResponse.data = newFeedBackStaff;
                            successResponse.response_code = 0;
                            successResponse.message = "FeedBackStaff Created";
                            successResponse.status = "Success";
                        }
                        else
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "User already exists.";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(422, unsuccessResponse);
                        }
                    }
                    else if (FeedBackStaffModel.type == 5) //Co-ordinator
                    {
                        Feedback feedback = FeedbackBusiness.CheckRecordExists(FeedBackStaffModel.feedbackid, FeedBackStaffModel.userid, FeedBackStaffModel.type);
                        if (feedback == null)
                        {
                            newFeedBackStaff = FeedbackBusiness.UpdateFeedBackManager(FeedBackStaffModel.feedbackid, FeedBackStaffModel.userid, FeedBackStaffModel.type, int.Parse(tc.Id), Certificate);
                            successResponse.data = newFeedBackStaff;
                            successResponse.response_code = 0;
                            successResponse.message = "FeedBackStaff Created";
                            successResponse.status = "Success";
                        }
                        else
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "User already exists.";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(422, unsuccessResponse);
                        }
                    }
                    else
                    {
                        if (FeedBackStaffModel.ismanager)
                        {
                            FeedBackStaff feedBackManager = FeedbackStaffBusiness.CheckManagerUserExists(FeedBackStaffModel.feedbackid, FeedBackStaffModel.type, FeedBackStaffModel.ismanager);
                            if(feedBackManager != null)
                            {
                                FeedbackStaffBusiness.Delete(int.Parse(feedBackManager.Id.ToString()), int.Parse(tc.Id));
                            }
                        }

                        FeedBackStaff feedBackStaff = FeedbackStaffBusiness.CheckUserExists(FeedBackStaffModel.feedbackid, FeedBackStaffModel.userid, FeedBackStaffModel.ismanager);
                        if (feedBackStaff == null)
                        {                            
                            newFeedBackStaff = await FeedbackStaffBusiness.Create(FeedBackStaffModel, int.Parse(tc.Id), Certificate);
                            successResponse.data = newFeedBackStaff;
                            successResponse.response_code = 0;
                            successResponse.message = "FeedBackStaff Created";
                            successResponse.status = "Success";
                        }
                        else
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "User already exists.";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(422, unsuccessResponse);
                        }
                    }
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

        [HttpPost("RemoveStaff")]
        public IActionResult RemoveStaff(AddFeedBackStaffModel FeedBackStaffModel)
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
                    FeedBackStaff newFeedBackStaff = FeedbackStaffBusiness.Removed(FeedBackStaffModel, int.Parse(tc.Id));

                    if(newFeedBackStaff != null)
                    {
                        ResponseFeedBackStaffModel responseFeedBackStaffModel = new ResponseFeedBackStaffModel
                        {
                            id = newFeedBackStaff.Id,
                            feedbackid = newFeedBackStaff.FeedBackId,
                            userid = newFeedBackStaff.UserId,
                            type = newFeedBackStaff.Type
                        };

                        successResponse.data = responseFeedBackStaffModel;
                        successResponse.response_code = 0;
                        successResponse.message = "FeedBackStaff Removed";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Staffnotfound";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(422, unsuccessResponse);
                    }
                }
                else
                {
                    return StatusCode(406, ModelState);
                }
            }
            catch(Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateFeedBackStaffModel updateFeedBackStaffModel)
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
                    updateFeedBackStaffModel.id = id;
                    FeedBackStaff newFeedBackStaff = FeedbackStaffBusiness.Update(updateFeedBackStaffModel, int.Parse(tc.Id));
                    ResponseFeedBackStaffModel responseFeedBackStaffModel = new ResponseFeedBackStaffModel
                    {
                        id = newFeedBackStaff.Id,
                        feedbackid = newFeedBackStaff.FeedBackId,
                        userid = newFeedBackStaff.UserId,
                        type = newFeedBackStaff.Type
                    };

                    successResponse.data = responseFeedBackStaffModel;
                    successResponse.response_code = 0;
                    successResponse.message = "FeedBackStaff updated";
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
                    FeedBackStaff newFeedBackStaff = FeedbackStaffBusiness.Delete(id, int.Parse(tc.Id));

                    ResponseFeedBackStaffModel responseFeedBackStaffModel = new ResponseFeedBackStaffModel
                    {
                        id = newFeedBackStaff.Id,
                        feedbackid = newFeedBackStaff.FeedBackId,
                        userid = newFeedBackStaff.UserId,
                        type = newFeedBackStaff.Type
                    };

                    successResponse.data = responseFeedBackStaffModel;
                    successResponse.response_code = 0;
                    successResponse.message = "FeedBackStaff Deleted";
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

                    FeedBackStaff newFeedBackStaff = FeedbackStaffBusiness.getFeedBackStaffById(id);
                    ResponseFeedBackStaffModel responseFeedBackStaffModel = new ResponseFeedBackStaffModel
                    {
                        id = newFeedBackStaff.Id,
                        feedbackid = newFeedBackStaff.FeedBackId,
                        userid = newFeedBackStaff.UserId,
                        type = newFeedBackStaff.Type
                    };

                    successResponse.data = responseFeedBackStaffModel;
                    successResponse.response_code = 0;
                    successResponse.message = "FeedBackStaff Detail";
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
                List<FeedBackStaff> FeedBackStaffList = new List<FeedBackStaff>();
                List<ResponseFeedBackStaffModel> FeedBackStaffResponseList = new List<ResponseFeedBackStaffModel>();
                FeedBackStaffList = FeedbackStaffBusiness.FeedBackStaffList(paginationModel, out int total);

                foreach (FeedBackStaff newFeedBackStaff in FeedBackStaffList)
                {
                    ResponseFeedBackStaffModel responseFeedBackStaffModel = new ResponseFeedBackStaffModel
                    {
                        id = newFeedBackStaff.Id,
                        feedbackid = newFeedBackStaff.FeedBackId,
                        userid = newFeedBackStaff.UserId,
                        type = newFeedBackStaff.Type
                    };

                    FeedBackStaffResponseList.Add(responseFeedBackStaffModel);
                }

                successResponse.data = FeedBackStaffResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "FeedBackStaff Details";
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