using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.AgentCategory;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class AgentCategoryController : ControllerBase
    {
        private readonly AgentCategoryBusiness AgentCategoryBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly IMapper _mapper;

        public AgentCategoryController
        (
            IMapper mapper,
            AgentCategoryBusiness AgentCategoryBusiness,
            LessonBusiness LessonBusiness
        )
        {
            this.AgentCategoryBusiness = AgentCategoryBusiness;
            this.LessonBusiness = LessonBusiness;
            _mapper = mapper;

        }

        [HttpPost]
        public IActionResult Post([FromBody] AgentCategory AgentCategory)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
           
            string Authorization = Request.Headers["id_token"];

            //get claims after decoding id_token 
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            try
            {
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (ModelState.IsValid)
                {                    

                    if (tc.RoleName.Contains(General.getRoleType("1")) || tc.RoleName.Contains(General.getRoleType("13")))
                    {
                        if (AgentCategory.Id == 0)
                        {
                            successResponse.data = _mapper.Map<AgentCategory, ResponseAgentCategory>(AgentCategoryBusiness.Create(AgentCategory, int.Parse(tc.Id)));
                            if(successResponse.data == null)
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "Duplicate entry.";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(422, unsuccessResponse);
                            }
                        }
                        else
                        {
                            successResponse.data = _mapper.Map<AgentCategory, ResponseAgentCategory>(AgentCategoryBusiness.Update(AgentCategory, int.Parse(tc.Id)));
                        }

                        successResponse.response_code = 0;
                        successResponse.message = "AgentCategory Created";
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

            //get claims after decoding id_token 
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);
            try
            {
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("1")) || tc.RoleName.Contains(General.getRoleType("13")))
                    {
                        successResponse.data = _mapper.Map<AgentCategory, ResponseAgentCategory>(AgentCategoryBusiness.Delete(id, int.Parse(tc.Id)));
                        successResponse.response_code = 0;
                        successResponse.message = "AgentCategory Deleted";
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


            //get claims after decoding id_token 
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);

            try
            {
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {


                    successResponse.data = _mapper.Map<AgentCategory, ResponseAgentCategory>(AgentCategoryBusiness.getAgentCategoryById(id));
                    successResponse.response_code = 0;
                    successResponse.message = "AgentCategory Detail";
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

                successResponse.data = _mapper.Map<List<AgentCategory>, List<ResponseAgentCategory>>(AgentCategoryBusiness.AgentCategoryList(paginationModel, out int total));
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "AgentCategory Details";
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