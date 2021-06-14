using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Students;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class ProfileController : ControllerBase
    {
        private readonly PasswordBusiness PasswordBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly UsersBusiness UsersBusiness;
        private readonly ProfileBusiness ProfileBusiness;

        public ProfileController(
            PasswordBusiness PasswordBusiness,
            LessonBusiness LessonBusiness,
            UsersBusiness UsersBusiness,
            ProfileBusiness ProfileBusiness
        )
        {
            this.PasswordBusiness = PasswordBusiness;
            this.LessonBusiness = LessonBusiness;
            this.UsersBusiness = UsersBusiness;
            this.ProfileBusiness = ProfileBusiness;
        }

        [HttpPut]
        public IActionResult UpdateProfile([FromBody]UpdateProfileModel UpdateProfileModel)
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
                    User user = UsersBusiness.AuthenticationByAuth(tc.sub); 
                    if (user == null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "User not found.";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(404, unsuccessResponse);
                    }
                    else
                    {                        

                        bool isProfileUpdate = ProfileBusiness.isProfileUpdate(user.Email, UpdateProfileModel,int.Parse(tc.Id), user.Id);
                        if (isProfileUpdate)
                        {
                            PasswordDTO passwordDTO = new PasswordDTO();

                            successResponse.data = null;
                            successResponse.response_code = 0;
                            successResponse.message = "Profile updated";
                            successResponse.status = "Success";
                            return StatusCode(200, successResponse);
                        }
                        else
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Username already exist";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(422, unsuccessResponse);
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

        [HttpGet]
        public bool IsValidUserName(string Username,long userid)
        {
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            return UsersBusiness.IsvalidUsername(Username, int.Parse(tc.Id),userid);         
        }
    }
}