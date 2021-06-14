using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.Domain.Entity;
using Trainning24.BL.ViewModels.Users;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;
using Trainning24.BL.ViewModels.Roles;

namespace Training24Admin.Controllers
{
    
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class RolesController : ControllerBase
    {
        private readonly RoleBusiness rolesBusiness;
        private readonly LessonBusiness LessonBusiness;
         
        public RolesController(
            LessonBusiness LessonBusiness,
            RoleBusiness rolesBusiness
            )
        {
            this.rolesBusiness = rolesBusiness;
            this.LessonBusiness = LessonBusiness;
        }

        [HttpGet]
        public IActionResult Get(int pagenumber, int perpagerecord)
        {
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            PaginationModel paginationModel = new PaginationModel();
            try
            {
                paginationModel.pagenumber = pagenumber;
                paginationModel.perpagerecord = perpagerecord;
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                List<Role> roleList = new List<Role>();
                List<RoleDTO> roleDTOList = new List<RoleDTO>();
                if (
                    tc.RoleName.Contains(General.getRoleType("1")) ||
                    tc.RoleName.Contains(General.getRoleType("3")) || 
                    tc.RoleName.Contains(General.getRoleType("4"))
                   )
                {
                    int totalCount = rolesBusiness.TotalRoleCount();
                    roleList = rolesBusiness.RoleList(paginationModel);
                    foreach (Role role in roleList)
                    {
                        RoleDTO roleDTO = new RoleDTO();
                        roleDTO.Id = role.Id;
                        roleDTO.Name = role.Name;
                        roleDTOList.Add(roleDTO);
                    }

                    successResponse.data = roleDTOList;
                    successResponse.totalcount = totalCount;
                    successResponse.response_code = 0;
                    successResponse.message = "Roles detail";
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

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (
                    tc.RoleName.Contains(General.getRoleType("1")) ||
                    tc.RoleName.Contains(General.getRoleType("3")) ||
                    tc.RoleName.Contains(General.getRoleType("4"))
                   )
                {
                    Role role = rolesBusiness.RoleExistanceById(id);
                    if (role == null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Role not found.";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(404, unsuccessResponse);
                    }
                    else
                    {
                        successResponse.data = role;
                        successResponse.response_code = 0;
                        successResponse.message = "Role detail";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
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

        [HttpPost]
        public IActionResult Post([FromBody]CreateRoleViewModel createRoleViewModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                Role role = rolesBusiness.RoleExistance(createRoleViewModel);
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (ModelState.IsValid)
                {
                    if (role != null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Role already exist";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(401, unsuccessResponse);
                    }
                    else
                    {
                        if (tc.RoleName.Contains(General.getRoleType("1")))
                        {
                            Role newrole = rolesBusiness.Create(createRoleViewModel, tc.Id);

                            successResponse.response_code = 0;
                            successResponse.message = "Role added";
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
        public IActionResult Put(int id, [FromBody]CreateRoleViewModel createRoleViewModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                createRoleViewModel.Id = id;
                Role role = rolesBusiness.RoleExistanceById(id);
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    if (role == null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Role not found.";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(404, unsuccessResponse);
                    }
                    else
                    {
                        if (tc.RoleName.Contains(General.getRoleType("1")))
                        {
                            Role newrole = rolesBusiness.Update(createRoleViewModel, tc.Id);
                          
                            successResponse.response_code = 0;
                            successResponse.message = "Role updated";
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
            try
            {
                Role role = rolesBusiness.RoleExistanceById(id);
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (role == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Role not found.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
                }
                else
                {
                    if (tc.RoleName.Contains(General.getRoleType("1")))
                    {
                        rolesBusiness.Delete(role, tc.Id);
                       
                        successResponse.response_code = 0;
                        successResponse.message = "Role Deleted";
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
