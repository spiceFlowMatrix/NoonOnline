using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.Business.Device;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.Device;
using Trainning24.BL.ViewModels.Users;

namespace Training24Admin.Controllers
{
    /// <summary>
    /// Everything about your device quotas
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = nameof(SwaggerGrouping.DeviceQuotas))]
    public class DeviceQuotasController : ControllerBase
    {
        private readonly LessonBusiness LessonBusiness;
        private readonly DeviceQuotasBusiness DeviceQuotasBusiness;
        public DeviceQuotasController(LessonBusiness lessonBusiness, DeviceQuotasBusiness deviceQuotasBusiness)
        {
            this.LessonBusiness = lessonBusiness;
            this.DeviceQuotasBusiness = deviceQuotasBusiness;
        }
  

        [HttpGet("getAllDeviceQuotaExtensionRequest")]
        public IActionResult GetAllDeviceQuotaExtensionRequest(int pagenumber, int perpagerecord, string search, string fromdate, string todate)
        {
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            DeviceQuotaExtensionFilterModel deviceQuotaExtensionFilterModel = new DeviceQuotaExtensionFilterModel()
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord,
                search = search,
                fromdate = fromdate,
                todate = todate,
            };
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    List<ResponceDeviceQuotaExtension> QuotaExtensionList = DeviceQuotasBusiness.QuotaExtensionList(deviceQuotaExtensionFilterModel, out int total);
                    successResponse.totalcount = total;
                    successResponse.data = QuotaExtensionList;
                    successResponse.response_code = 0;
                    successResponse.message = "QuotaExtensionList";
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

        /// <summary>
        /// Request a device quota exension
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="requestedLimit">Quota extension request Limit </param>
        /// <returns></returns>
        [HttpPost("{requestedLimit}")]
        public IActionResult Post(int requestedLimit)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("4")))
                    {
                        var penddingExtensionRequest = DeviceQuotasBusiness.GetbyUserId(int.Parse(tc.Id));
                        if (penddingExtensionRequest == null)
                        {
                            var deviceDetail = DeviceQuotasBusiness.ExtensionRequest(int.Parse(tc.Id), requestedLimit);
                            if (deviceDetail != 0)
                            {
                                successResponse.data = deviceDetail;
                                successResponse.response_code = 0;
                                successResponse.message = "successful operation";
                                successResponse.status = "Success";
                                return StatusCode(200, successResponse);
                            }
                            else
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "Not valid input";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(405, unsuccessResponse);
                            }
                        }
                        else
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "PENDING extension request already exists";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(405, unsuccessResponse);
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

        /// <summary>
        /// Approve/Reject existing quota extension requests
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="extensionRequestId">Requested device extention id</param>
        /// <param name="IsAccepted">true will approve and false will reject.</param>
        /// <returns></returns>
        [HttpPut]
        public IActionResult Put(int extensionRequestId, bool IsAccepted)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("1")))
                    {
                        var penddingExtensionRequest = DeviceQuotasBusiness.GetbyId(extensionRequestId);
                        if (penddingExtensionRequest != null)
                        {
                            var deviceDetail = DeviceQuotasBusiness.ExtensionRequestChangeStatus(penddingExtensionRequest, IsAccepted, int.Parse(tc.Id));
                            if (deviceDetail != 0)
                            {
                                successResponse.data = deviceDetail;
                                successResponse.response_code = 0;
                                successResponse.message = "successful operation";
                                successResponse.status = "Success";
                                return StatusCode(200, successResponse);
                            }
                            else
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "Not valid input";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(405, unsuccessResponse);
                            }
                        }
                        else
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Extension request not exists";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(405, unsuccessResponse);
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