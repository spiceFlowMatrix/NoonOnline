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

namespace Training24Admin.Controllers
{
    /// <summary>
    /// Everything about your devices
    /// </summary>
    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = nameof(SwaggerGrouping.Device))]
    public class DeviceController : ControllerBase
    {

        private readonly LessonBusiness LessonBusiness;
        private readonly DeviceBusiness deviceBusiness;
        public static List<string> UniqMacList = new List<string>();


        public DeviceController(LessonBusiness LessonBusiness, DeviceBusiness deviceBusiness
           )
        {
            this.LessonBusiness = LessonBusiness;
            this.deviceBusiness = deviceBusiness;

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

            //get claims after decoding id_token 
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("4")))
                    {
                        var deviceDetail = deviceBusiness.GetAllDeviceByUserId(int.Parse(tc.Id));
                        if (deviceDetail != null)
                        {
                            successResponse.data = deviceDetail;
                            successResponse.response_code = 0;
                            successResponse.message = "Device Detail";
                            successResponse.status = "Success";
                            return StatusCode(200, successResponse);
                        }
                        else
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Device not found";
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
        /// Register device quota for user and activate new device
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="objData">New device that I want to activate</param>
        /// <returns></returns>
        [HttpPost]
        public IActionResult Post([FromBody]DeviceActivate objData)
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
                    if (!String.IsNullOrEmpty(objData.macAddress) && !String.IsNullOrEmpty(objData.ipAddress))
                    {
                        if (!UniqMacList.Contains(objData.macAddress))
                        {
                            UniqMacList.Add(objData.macAddress);
                            var deviceExist = deviceBusiness.GetDevicesByMacAdd(objData.macAddress, int.Parse(tc.Id));
                            if (deviceExist == null)
                            {
                                if (deviceBusiness.CheckDeviceQuota(int.Parse(tc.Id)) > 0)
                                {
                                    UniqMacList.Remove(objData.macAddress);
                                    var deviceDetails = deviceBusiness.Create(objData, int.Parse(tc.Id));
                                    successResponse.data = deviceDetails;
                                    successResponse.response_code = 0;
                                    successResponse.message = "Device registered and activated";
                                    successResponse.status = "Success";
                                    return StatusCode(200, successResponse);
                                }
                                else
                                {
                                    UniqMacList.Remove(objData.macAddress);
                                    unsuccessResponse.response_code = 3;
                                    unsuccessResponse.message = "you are out of device quota";
                                    unsuccessResponse.status = "Unsuccess";
                                    return StatusCode(406, unsuccessResponse);
                                }
                            }
                            else if (deviceExist != null && deviceExist.IsDeleted != true)
                            {
                                UniqMacList.Remove(objData.macAddress);
                                successResponse.data = deviceExist;
                                successResponse.response_code = 0;
                                successResponse.message = "Device registered and activated";
                                successResponse.status = "Success";
                                return StatusCode(200, successResponse);
                            }
                            else
                            {
                                UniqMacList.Remove(objData.macAddress);
                                unsuccessResponse.response_code = 2;
                                unsuccessResponse.message = "This device has been deactivated.";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(406, unsuccessResponse);
                            }
                        }
                        else
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "This user's request already in the queue.";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(406, unsuccessResponse);
                        }
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "Invalid input";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(500, unsuccessResponse);
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
        /// Deactivate an existing device
        /// </summary>
        /// <param name="deviceId">Id of device</param>
        /// <returns></returns>
        [HttpPut("ChaneDeviceStatus/{deviceId}")]
        public IActionResult ChaneDeviceStatus(int deviceId)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                var device = deviceBusiness.GetDevice(int.Parse(tc.Id), deviceId);
                if (device != null)
                {
                    if (deviceBusiness.CheckDeviceQuota(int.Parse(tc.Id)) > 0 || device.IsDeleted == false)
                    {
                        var result = deviceBusiness.activeDeactiveDevice(device, int.Parse(tc.Id));
                        if (result != 0)
                        {
                            successResponse.response_code = 0;
                            successResponse.message = result == 1 ? "Device activated." : "Device deactivated.";
                            successResponse.status = "Success";
                            return StatusCode(200, successResponse);
                        }
                        else
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Device not found";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(404, unsuccessResponse);
                        }
                    }
                    else
                    {
                        unsuccessResponse.response_code = 3;
                        unsuccessResponse.message = "This user out of device quota";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(406, unsuccessResponse);
                    }
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Device not found";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
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
        ///  Get my  all user's device profile .
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetAllUserDeviceList")]
        public IActionResult GetAllUserDeviceList(int pagenumber, int perpagerecord, string search, long userId)
        {
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            DeviceQuotaExtensionFilterModel deviceQuotaExtensionFilterModel = new DeviceQuotaExtensionFilterModel()
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord,
                search = search,
                userId = userId
            };
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
                        var deviceDetail = deviceBusiness.GetAllDeviceUserWise(deviceQuotaExtensionFilterModel, out int total);
                        if (deviceDetail != null)
                        {
                            successResponse.data = deviceDetail;
                            successResponse.totalcount = total;
                            successResponse.response_code = 0;
                            successResponse.message = "User Device list";
                            successResponse.status = "Success";
                            return StatusCode(200, successResponse);
                        }
                        else
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Device not found";
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
        ///  Get my all device profile by userId.
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetUserDeviceListByUserId")]
        public IActionResult GetUserDeviceListByUserId(int userId)
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
                        var deviceDetail = deviceBusiness.GetUserDeviceByUserId(userId);
                        if (deviceDetail != null)
                        {
                            successResponse.data = deviceDetail;
                            successResponse.response_code = 0;
                            successResponse.message = "Device Detail";
                            successResponse.status = "Success";
                            return StatusCode(200, successResponse);
                        }
                        else
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Device not found";
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
        /// Deactivate an existing device for admin
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="deviceId">Id of device</param>
        /// <returns></returns>
        [HttpPut("ChangeuserDeviceStatus/{userId}/{deviceId}")]
        public IActionResult ChangeuserDeviceStatus(int userId, int deviceId)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                var device = deviceBusiness.GetDevice(userId, deviceId);
                if (device != null)
                {
                    if (deviceBusiness.CheckDeviceQuota(userId) > 0 || device.IsDeleted == false)
                    {
                        var result = deviceBusiness.activeDeactiveDevice(device, userId, int.Parse(tc.Id));
                        if (result != 0)
                        {
                            successResponse.response_code = 0;
                            successResponse.message = result == 1 ? "Device activated." : "Device deactivated.";
                            successResponse.status = "Success";
                            return StatusCode(200, successResponse);
                        }
                        else
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Device not found";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(404, unsuccessResponse);
                        }
                    }
                    else
                    {
                        unsuccessResponse.response_code = 3;
                        unsuccessResponse.message = "This user out of device quota";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(406, unsuccessResponse);
                    }
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Device not found";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
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