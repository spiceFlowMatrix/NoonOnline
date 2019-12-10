using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.DefaultValues;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DefaultValuesController : ControllerBase
    {
        private readonly DefaultValuesBusiness DefaultValuesBusiness;
        private readonly LessonBusiness LessonBusiness;


        public DefaultValuesController
        (
            DefaultValuesBusiness DefaultValuesBusiness,
            LessonBusiness LessonBusiness
        )
        {
            this.DefaultValuesBusiness = DefaultValuesBusiness;
            this.LessonBusiness = LessonBusiness;
        }

        [HttpPost]
        public IActionResult Post(DefaultValuesModel DefaultValuesModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                //get claims after decoding id_token 
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (ModelState.IsValid)
                {
                    if (!tc.RoleName.Contains(General.getRoleType("1")))
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "You are not authorized.";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(401, unsuccessResponse);
                    }

                    DefaultValuesModel.id = 6;
                    DefaultValues newDefaultValues = DefaultValuesBusiness.Update(DefaultValuesModel, int.Parse(tc.Id));
                    DefaultValuesModel responseBundleModel = new DefaultValuesModel();

                    responseBundleModel.timeout = newDefaultValues.timeout;
                    responseBundleModel.reminder = newDefaultValues.reminder;
                    responseBundleModel.istimeouton = newDefaultValues.istimeouton;
                    responseBundleModel.intervals = newDefaultValues.intervals;
                    responseBundleModel.id = int.Parse(newDefaultValues.Id.ToString());

                    successResponse.data = responseBundleModel;
                    successResponse.response_code = 0;
                    successResponse.message = "DefaultValues Updated";
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
        public IActionResult Get()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                //get claims after decoding id_token 
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);


                if (!tc.RoleName.Contains(General.getRoleType("1")))
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "You are not authorized.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(401, unsuccessResponse);
                }

                DefaultValues DefaultValues = DefaultValuesBusiness.GetById(6);

                DefaultValuesModel responseBundleModel = new DefaultValuesModel
                {
                    id = DefaultValues.Id,
                    timeout = DefaultValues.timeout,
                    reminder = DefaultValues.reminder,
                    intervals = DefaultValues.intervals,
                    istimeouton = DefaultValues.istimeouton
                };

                successResponse.data = responseBundleModel;
                successResponse.response_code = 0;
                successResponse.message = "DefaultValues details";
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