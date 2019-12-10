using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.SalesTax;
using Trainning24.BL.ViewModels.TimeInterval;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SalesTaxController : ControllerBase
    {
        private readonly SalesTaxBusiness _salesTaxBusiness;
        private readonly LessonBusiness _lessonBusiness;

        public SalesTaxController(
            SalesTaxBusiness salesTaxBusiness,
            LessonBusiness lessonBusiness
            )
        {
            _salesTaxBusiness = salesTaxBusiness;
            _lessonBusiness = lessonBusiness;
        }

        #region SalesTax Services
        [Authorize]
        [HttpGet("GetTax")]
        public IActionResult GetTax()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            SalesTaxDTO obj = new SalesTaxDTO();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessonBusiness.getUserId(tc.sub);
            try
            {
                if (tc.RoleName.Contains(General.getRoleType("1")))
                {
                    var data = _salesTaxBusiness.Get();
                    if (data != null)
                    {
                        obj.id = data.Id;
                        obj.tax = data.Tax;
                        successResponse.data = obj;
                        successResponse.response_code = 0;
                        successResponse.message = "tax get";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "No tax found";
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
        [HttpPut("UpdateTax")]
        public IActionResult UpdateTax(SalesTaxDTO obj)
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
                    SalesTax salesTax = new SalesTax();
                    salesTax.Id = obj.id;
                    salesTax.Tax = obj.tax;
                    salesTax.LastModifierUserId = int.Parse(tc.Id);
                    salesTax.LastModificationTime = DateTime.Now.ToString();
                    var data = _salesTaxBusiness.UpdateTax(salesTax);
                    obj.id = data.Id;
                    obj.tax = data.Tax;
                    successResponse.data = obj;
                    successResponse.response_code = 0;
                    successResponse.message = "tax updated";
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

        [HttpGet("GetTaxPub")]
        public IActionResult GetIntervalPub()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            SalesTaxDTO2 obj = new SalesTaxDTO2();
            try
            {
                var data = _salesTaxBusiness.Get();
                if (data != null)
                {
                    obj.tax = data.Tax;
                    successResponse.data = obj;
                    successResponse.response_code = 0;
                    successResponse.message = "tax get";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 2;
                    unsuccessResponse.message = "No tax found";
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
        #endregion
    }
}