using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.AddtionalServices;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class AddtionalServicesController : ControllerBase
    {
        private readonly AddtionalServicesBusiness _addtionalServicesBusiness;
        private readonly LessonBusiness _lessonBusiness;


        public AddtionalServicesController(
            AddtionalServicesBusiness addtionalServicesBusiness,
            LessonBusiness lessonBusiness
            )
        {
            _addtionalServicesBusiness = addtionalServicesBusiness;
            _lessonBusiness = lessonBusiness;
        }

        #region Addtional Services
        [Authorize]
        [HttpGet("GetService/{Id}")]
        public IActionResult GetService(long Id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            AddtionalServicesDto obj = new AddtionalServicesDto();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    var data = _addtionalServicesBusiness.GetById(Id);
                    if (data != null)
                    {
                        obj.id = data.Id;
                        obj.name = data.Name;
                        obj.price = data.Price;
                        obj.createdBy = data.CreatorUserId ?? 0;
                        obj.createdDate = data.CreationTime;
                        successResponse.data = obj;
                        successResponse.response_code = 0;
                        successResponse.message = "Service get";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "No service found";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
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
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [Authorize]
        [HttpGet("GetServices")]
        public IActionResult GetServices(int pagenumber, int perpagerecord)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            List<AddtionalServicesDto> objlst = new List<AddtionalServicesDto>();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            PaginationModel paginationModel = new PaginationModel
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord
            };
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")) || tc.RoleName.Contains(General.getRoleType("13")) || tc.RoleName.Contains(General.getRoleType("17")))
                {
                    var data = _addtionalServicesBusiness.GetServicesList(paginationModel);
                    if (data != null)
                    {
                        foreach (var service in data)
                        {
                            AddtionalServicesDto obj = new AddtionalServicesDto();
                            obj.id = service.Id;
                            obj.name = service.Name;
                            obj.price = service.Price;
                            obj.createdBy = service.CreatorUserId ?? 0;
                            obj.createdDate = service.CreationTime;
                            objlst.Add(obj);
                        }
                        successResponse.data = objlst;
                        successResponse.response_code = 0;
                        successResponse.message = "Service list get";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "No service found";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
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
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [Authorize]
        [HttpPost("AddService")]
        public IActionResult AddService(AddtionalServicesDto obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            if (string.IsNullOrEmpty(obj.name))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "name is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            if (string.IsNullOrEmpty(obj.price))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "price is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    AddtionalServices addtionalServices = new AddtionalServices();
                    addtionalServices.Name = obj.name;
                    addtionalServices.Price = obj.price;
                    addtionalServices.CreatorUserId = int.Parse(tc.Id);
                    addtionalServices.CreationTime = DateTime.Now.ToString();
                    var data = _addtionalServicesBusiness.AddService(addtionalServices);
                    obj.id = data.Id;
                    obj.createdBy = data.CreatorUserId ?? 0;
                    obj.createdDate = data.CreationTime;
                    successResponse.data = obj;
                    successResponse.response_code = 0;
                    successResponse.message = "Service added";
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
        [HttpPut("UpdateService")]
        public IActionResult UpdateService(AddtionalServicesDto obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            if (string.IsNullOrEmpty(obj.name))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "name is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            if (string.IsNullOrEmpty(obj.price))
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = "price is required";
                unsuccessResponse.status = "Failure";
                return StatusCode(406, unsuccessResponse);
            }
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    AddtionalServices addtionalServices = new AddtionalServices();
                    addtionalServices.Id = obj.id;
                    addtionalServices.Name = obj.name;
                    addtionalServices.Price = obj.price;
                    addtionalServices.LastModifierUserId = int.Parse(tc.Id);
                    addtionalServices.LastModificationTime = DateTime.Now.ToString();
                    var data = _addtionalServicesBusiness.UpdateService(addtionalServices);
                    obj.id = data.Id;
                    obj.createdBy = data.CreatorUserId ?? 0;
                    obj.createdDate = data.CreationTime;
                    successResponse.data = obj;
                    successResponse.response_code = 0;
                    successResponse.message = "Service updated";
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
        [HttpDelete("DeleteService/{Id}")]
        public IActionResult DeleteService(long Id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    AddtionalServices addtionalServices = new AddtionalServices();
                    addtionalServices.Id = Id;
                    addtionalServices.DeleterUserId = int.Parse(tc.Id);
                    addtionalServices.DeletionTime = DateTime.Now.ToString();
                    var data = _addtionalServicesBusiness.DeleteService(addtionalServices);
                    if (data == 1)
                    {
                        successResponse.response_code = 0;
                        successResponse.message = "Service deleted";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "No service found";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
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
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }
        #endregion
    }
}