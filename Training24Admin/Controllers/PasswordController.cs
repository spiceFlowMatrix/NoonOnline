using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using User = Trainning24.Domain.Entity.User;

namespace Training24Admin.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    public class PasswordController : ControllerBase
    {

        private readonly PasswordBusiness PasswordBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly UsersBusiness UsersBusiness;

        public PasswordController(
            PasswordBusiness PasswordBusiness,
            LessonBusiness LessonBusiness,
            UsersBusiness UsersBusiness
            )
        {
            this.PasswordBusiness = PasswordBusiness;
            this.LessonBusiness = LessonBusiness;
            this.UsersBusiness = UsersBusiness;
        }

        [HttpPut]
        [Authorize]
        public IActionResult ChangePassword([FromBody]ChangePasswordModel ChangePasswordModel)
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
                        bool isValidateOldPassword = PasswordBusiness.isValidateOldPassword(user.Email, ChangePasswordModel.old_password);
                        if (!isValidateOldPassword)
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Old password is not valid.";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(401, unsuccessResponse);
                        }
                        if (ChangePasswordModel.new_password != ChangePasswordModel.confirm_password)
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "NewPassword doest not match with confirm password.";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(401, unsuccessResponse);
                        }
                        bool isPasswordChanged = PasswordBusiness.isPasswordChanged(user.Email, ChangePasswordModel.new_password);


                        //getting ManagementAPIClient authority for Auth0 implementation this is used for calling Auth Management API
                        ManagementApiClient mac = General.getAuthManagementApiToken();

                        UserUpdateRequest userUpdateRequest = new UserUpdateRequest();
                        userUpdateRequest.Password = ChangePasswordModel.new_password;
                        userUpdateRequest.Connection = "Username-Password-Authentication";

                        userUpdateRequest.UserMetadata = JsonConvert.DeserializeObject("{}");

                        try
                        {
                            Auth0.ManagementApi.Models.User a0User = mac.Users.UpdateAsync(user.AuthId, userUpdateRequest).Result;
                        }
                        catch (Exception ex)
                        {
                            unsuccessResponse.response_code = 2;
                            unsuccessResponse.message = ex.Message;
                            unsuccessResponse.status = "Failure";
                            return StatusCode(500, unsuccessResponse);
                        }


                        if (isPasswordChanged)
                        {
                            PasswordDTO passwordDTO = new PasswordDTO();

                            successResponse.data = null;
                            successResponse.response_code = 0;
                            successResponse.message = "Password Changed";
                            successResponse.status = "Success";
                            return StatusCode(200, successResponse);
                        }
                        else
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Password not changed";
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


        [HttpGet("ForgotPassword/{email}")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                User user = UsersBusiness.UserExistsByEmail(email);
                if (user != null)
                {
                    Random generator = new Random();
                    String ramdomN = generator.Next(0, 999999).ToString("D6");
                    user.isforgot = true;
                    user.forgotkey = ramdomN;
                    string subject = "Forgot password noon";
                    string body = "use 6 digit unique code to forgot password:" + "<b>" + ramdomN + "</b>";
                    await UsersBusiness.SendEmailAsync(user.Email, subject, body);
                    UsersBusiness.updateForgotCode(user);
                    successResponse.data = null;
                    successResponse.response_code = 0;
                    successResponse.message = "Mail Sent";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.message = "User not exists";
                    unsuccessResponse.response_code = 2;
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(500, unsuccessResponse);
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

        [HttpGet("CheckForgetKey/{key}")]
        public IActionResult CheckForgetKey(string key)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                User user = UsersBusiness.CheckForgotKey(key);
                if (user != null)
                {
                    ForgotPasswordModel forgotPasswordModel = new ForgotPasswordModel();
                    forgotPasswordModel.Email = user.Email;
                    forgotPasswordModel.AuthId = user.AuthId;
                    forgotPasswordModel.forgotkey = user.forgotkey;
                    forgotPasswordModel.isforgot = user.isforgot;
                    successResponse.data = forgotPasswordModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Sucess";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.message = "Invaild Request";
                    unsuccessResponse.response_code = 2;
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(500, unsuccessResponse);
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
        [HttpPost("UpdatePassword")]
        public IActionResult UpdatePassword(ForgotPasswordModel user)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                User getUser = UsersBusiness.UserExistsByEmail(user.Email);
                if(getUser !=null)
                {
                    if (user.Password != user.ConfirmPassword)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "NewPassword doest not match with confirm password.";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(401, unsuccessResponse);
                    }
                    bool isPasswordChanged = PasswordBusiness.ForgotPassword(user.Email, user.Password);

                    //getting ManagementAPIClient authority for Auth0 implementation this is used for calling Auth Management API
                    ManagementApiClient mac = General.getAuthManagementApiToken();

                    UserUpdateRequest userUpdateRequest = new UserUpdateRequest();
                    userUpdateRequest.Password = user.Password;
                    userUpdateRequest.Connection = "Username-Password-Authentication";

                    userUpdateRequest.UserMetadata = JsonConvert.DeserializeObject("{}");

                    try
                    {
                        Auth0.ManagementApi.Models.User a0User = mac.Users.UpdateAsync(user.AuthId, userUpdateRequest).Result;
                    }
                    catch (Exception ex)
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = ex.Message;
                        unsuccessResponse.status = "Failure";
                        return StatusCode(500, unsuccessResponse);
                    }
                    if (isPasswordChanged)
                    {
                        successResponse.data = null;
                        successResponse.response_code = 0;
                        successResponse.message = "Password Changed";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Password not changed";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(401, unsuccessResponse);
                    }
                }
                else
                {
                    unsuccessResponse.message = "User not exists";
                    unsuccessResponse.response_code = 2;
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(500, unsuccessResponse);
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
