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
using Trainning24.BL.ViewModels.AppVersion;
using Trainning24.BL.ViewModels.Device;


namespace Training24Admin.Controllers
{
    /// <summary>
    /// Everything about your App Version
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = nameof(SwaggerGrouping.AppVersion))]
    public class AppVersionController : ControllerBase
    {
        private readonly AppVersionBusiness appVersionBusiness;
        public AppVersionController(AppVersionBusiness appVersionBusiness)
        {
            this.appVersionBusiness = appVersionBusiness;
        }
        /// <summary>
        ///  Get my all device profile by user.
        /// </summary>
        /// <returns></returns>
        [HttpGet()]
        public IActionResult Get()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    var appVersion = appVersionBusiness.GetappVersion();
                    if (appVersion != null)
                    {
                        successResponse.data = appVersion;
                        successResponse.response_code = 0;
                        successResponse.message = "App Veersion Detail";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "App version not found";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(405, unsuccessResponse);
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
        [HttpPut()]
        public IActionResult Put([FromBody]AppVersionResponseModel appVersionResponseModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    var appVersion = appVersionBusiness.updateAppVersion(appVersionResponseModel);
                    if (appVersion != null)
                    {
                        successResponse.data = appVersion;
                        successResponse.response_code = 0;
                        successResponse.message = "App Veersion updated.";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "App version not found";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(405, unsuccessResponse);
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