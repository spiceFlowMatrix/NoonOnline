using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.ErpAccounts;
using Trainning24.BL.ViewModels.SalesTax;
using Trainning24.BL.ViewModels.TimeInterval;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ErpAccountsController : ControllerBase
    {
        private readonly ErpAccountsBusiness _erpAccountsBusiness;
        private readonly LessonBusiness _lessonBusiness;

        public ErpAccountsController(
            ErpAccountsBusiness erpAccountsBusiness,
            LessonBusiness lessonBusiness
            )
        {
            _erpAccountsBusiness = erpAccountsBusiness;
            _lessonBusiness = lessonBusiness;
        }

        //#region SalesTax Services method
        [HttpPut("AddUpdateErpAccounts")]
        public IActionResult AddUpdateErpAccounts([FromBody]List<ErpAccountsDTO> obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                if (obj.Count > 0)
                {
                    foreach (var ac in obj)
                    {
                        ERPAccounts erpAc = new ERPAccounts();
                        erpAc.Type = ac.type;
                        erpAc.AccountCode = ac.accountcode;
                        erpAc.LastModifierUserId = 1;
                        erpAc.LastModificationTime = DateTime.Now.ToString();
                        var data = _erpAccountsBusiness.UpdateAccounts(erpAc);
                    }
                }
                successResponse.data = obj;
                successResponse.response_code = 0;
                successResponse.message = "Accounts updated";
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

        [HttpGet("GetAccountsPub")]
        public IActionResult GetIntervalPub()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            List<ErpAccountsDTO> lstobj = new List<ErpAccountsDTO>();
            try
            {
                var data = _erpAccountsBusiness.GetAll();
                if (data.Count > 0)
                {
                    foreach (var ac in data)
                    {
                        ErpAccountsDTO obj = new ErpAccountsDTO();
                        obj.type = ac.Type;
                        obj.accountcode = ac.AccountCode;
                        lstobj.Add(obj);
                    }
                    successResponse.data = lstobj;
                    successResponse.response_code = 0;
                    successResponse.message = "Accounts get";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 2;
                    unsuccessResponse.message = "No accounts found";
                    unsuccessResponse.status = "Failure";
                    return StatusCode(406, unsuccessResponse);
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
        //#endregion
    }
}