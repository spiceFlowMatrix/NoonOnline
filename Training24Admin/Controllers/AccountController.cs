using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using RestSharp;
using Training24Admin.Model;
using Training24Admin.Security.Bearer.Helpers;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.BundleCourse;
using Trainning24.BL.ViewModels.UserRole;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [ApiController]
    [Produces("application/json")]
    [Route("api/v1/[controller]")]
    [ApiExplorerSettings(GroupName = nameof(SwaggerGrouping.Account))]
    public class AccountController : ControllerBase
    {
        private IConfiguration _config;
        private readonly UsersBusiness _usersBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly StudentCourseBusiness StudentCourseBusiness;
        private readonly UserSessionsBusiness _userSessions;
        private readonly CourseBusiness CourseBusiness;
        private readonly DefaultValuesBusiness DefaultValuesBusiness;
        private readonly int _currentUser;

        public AccountController(
            IConfiguration config,
            UsersBusiness usersBusiness,
            StudentCourseBusiness StudentCourseBusiness,
            LessonBusiness LessonBusiness,
            CourseBusiness CourseBusiness,
            IHttpContextAccessor httpContextAccessor,
            UserSessionsBusiness userSessions,
            DefaultValuesBusiness DefaultValuesBusiness
        )
        {
            this.CourseBusiness = CourseBusiness;
            this.LessonBusiness = LessonBusiness;
            this.StudentCourseBusiness = StudentCourseBusiness;
            _config = config;
            _usersBusiness = usersBusiness;
            _currentUser = httpContextAccessor.CurrentUser();
            _userSessions = userSessions;
            this.DefaultValuesBusiness = DefaultValuesBusiness;
        }

        [HttpPost]
        [Authorize]
        public async System.Threading.Tasks.Task<IActionResult> AuthenticateAsync([FromBody]UserAuthenticationModel login)
        {
            AuthLoginResponse responseData = new AuthLoginResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            
            //get claim after decoding id_token 
            TokenClaims tc = General.GetClaims(Authorization);
            
            try
            {

                //user exitance by authid(tc.sub)
                User user = _usersBusiness.AuthenticationByAuth(tc.sub);

                if(user == null)
                {
                    CreateUserViewModel createUserViewModel = new CreateUserViewModel();
                    DefaultValues newDefaultValuesList = DefaultValuesBusiness.GetById(6);

                    if (newDefaultValuesList != null)
                    {
                        if (string.IsNullOrEmpty(createUserViewModel.istimeouton.ToString()))
                            createUserViewModel.istimeouton = newDefaultValuesList.istimeouton;
                        if (createUserViewModel.timeout == 0)
                            createUserViewModel.timeout = newDefaultValuesList.timeout;
                        if (createUserViewModel.reminder == 0)
                            createUserViewModel.reminder = newDefaultValuesList.reminder;
                        if (createUserViewModel.intervals == 0)
                            createUserViewModel.intervals = newDefaultValuesList.intervals;
                    }

                    createUserViewModel.authid = tc.sub;
                    createUserViewModel.Email = login.Email;
                    createUserViewModel.Password = login.Password;
                    AddUserRoleModel addUserRoleModel = new AddUserRoleModel { roleid = 4 };
                    List<AddUserRoleModel> rolelist = new List<AddUserRoleModel>();
                    rolelist.Add(addUserRoleModel);
                    createUserViewModel.Roles = rolelist;
                    createUserViewModel.is_skippable = true;                    
                    User newuser = await _usersBusiness.Create(createUserViewModel, tc.Id);

                    List<Course> courses = new List<Course>();
                    courses = CourseBusiness.getTrailCourses();

                    foreach (var item in courses)
                    {
                        AddStudentCourseModel asm = new AddStudentCourseModel();
                        asm.courseid = item.Id;
                        asm.startdate = DateTime.Now;
                        asm.enddate = asm.startdate.Value.AddDays(14);
                        asm.userid = newuser.Id;
                        UserCourse newStudentCourse = StudentCourseBusiness.Create(asm, int.Parse(newuser.Id.ToString()));
                    }


                    UpdateUserViewModel updateUserViewModel = new UpdateUserViewModel();
                    updateUserViewModel.Id = newuser.Id;
                    updateUserViewModel.isfirstlogin = true;
                    _usersBusiness.UpdateIsfirstlogin(updateUserViewModel);

                    responseData.message = "Logged In";
                    responseData.status = "Success";
                    responseData.userid = newuser.Id.ToString();
                    return StatusCode(200, responseData);
                }
                else
                {
                    if (user.isfirstlogin == false && tc.RoleName.Contains(General.getRoleType("4")))
                    {
                        List<Course> courses = new List<Course>();
                        courses = CourseBusiness.getTrailCourses();

                        foreach (var item in courses)
                        {
                            AddStudentCourseModel asm = new AddStudentCourseModel();
                            asm.courseid = item.Id;
                            asm.startdate = DateTime.Now;
                            asm.enddate = asm.startdate.Value.AddDays(14);
                            asm.userid = long.Parse(user.Id.ToString());
                            UserCourse newStudentCourse = StudentCourseBusiness.Create(asm, int.Parse(user.Id.ToString()));
                        }

                        UpdateUserViewModel updateUserViewModel = new UpdateUserViewModel();
                        updateUserViewModel.Id = user.Id;
                        updateUserViewModel.isfirstlogin = true;
                        _usersBusiness.UpdateIsfirstlogin(updateUserViewModel);
                    }

                    if (user.IsBlocked == true)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "You are blocked, please contact to the admin.";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(401, unsuccessResponse);
                    }

                    UserDTO userDto = new UserDTO();
                    userDto.Id = user.Id;
                    userDto.FullName = user.FullName;
                    userDto.Email = user.Email;
                    userDto.Username = user.Username;
                    userDto.is_skippable = user.is_skippable;
                    userDto.timeout = user.timeout;
                    userDto.reminder = user.reminder;
                    userDto.istimeouton = user.istimeouton;
                    userDto.isallowmap = user.isallowmap;
                    List<Role> roles = _usersBusiness.Role(user);

                    if (roles != null)
                    {
                        List<long> roleids = new List<long>();
                        List<string> rolenames = new List<string>();
                        foreach (var role in roles)
                        {
                            roleids.Add(role.Id);
                            rolenames.Add(role.Name);
                            if ((login.DeviceType.ToLower() == DeviceType.Android || login.DeviceType.ToLower() == DeviceType.IOS) && role.Name.ToLower() != RoleType.Student.ToString().ToLower() && role.Name.ToLower() != RoleType.Teacher.ToString().ToLower() && role.Name.ToLower() != RoleType.Parent.ToString().ToLower())
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "You are not authorised";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(401, unsuccessResponse);
                            }
                        }
                        userDto.RoleId = roleids;
                        userDto.RoleName = rolenames;
                    }

                    _userSessions.Create(new UserSessions
                    {
                        UserId = userDto.Id,
                        AccessToken = Authorization,
                        DeviceToken = login.DeviceToken,
                        DeviceType = login.DeviceType,
                        Version = login.Version,
                    }, userDto.Id.ToString());

                    responseData.message = "Logged In";
                    responseData.status = "Success";
                    responseData.userid = user.Id.ToString();
                    return StatusCode(200, responseData);
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
        [HttpGet]
        public IActionResult Logout()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            string Id = tc.Id;
            //string RoleId = tokenS.Claims.First(claim => claim.Type == "RoleId").Value;

            // Status Delete to UserSession
            _userSessions.DeleteSession(Authorization, Id);
            try
            {
                successResponse.data = null;
                successResponse.response_code = 0;
                successResponse.message = "User logged Out";
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

        [Authorize]
        [HttpGet("Blocked")]
        public IActionResult Blocked()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);

            try
            {
                tc.Id = LessonBusiness.getUserId(tc.sub);

                User user = _usersBusiness.blocked(long.Parse(tc.Id));
                UserDeleteDTO dTO = new UserDeleteDTO();
                dTO.Id = user.Id;

                successResponse.data = dTO;
                successResponse.response_code = 0;
                successResponse.message = "User bocked";
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


        [HttpGet]
        [Route("RefreshAccessToken")]
        [Authorize]
        public IActionResult RefreshAccessToken()
        {
            UserDTO userDto = new UserDTO();
            AthResponse responseData = new AthResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                if (string.IsNullOrEmpty(Request.Headers["id_token"]))
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Token needed";
                    unsuccessResponse.status = "UnAuthorized";
                    return StatusCode(422, unsuccessResponse);
                }

                string Authorization = (Request.Headers["id_token"]).ToString().Split(" ")[1];

                string OldToken = (Request.Headers["old_id_token"]).ToString().Split(" ")[1];
                UserSessions Usession = _userSessions.GetUserSessionByAccessToken(OldToken);

                if (Usession != null)
                {
                    User user = _usersBusiness.GetUserbyId(Usession.UserId);

                    //userDto.Id = user.Id;
                    //userDto.FullName = user.FullName;
                    //userDto.Id = user.Id;
                    //userDto.FullName = user.FullName;
                    //userDto.Email = user.Email;
                    //userDto.Username = user.Username;
                    ////userDto.phonenumber = user.phonenumber;

                    //List<Role> roles = _usersBusiness.Role(user);

                    //if (roles != null)
                    //{
                    //    List<long> roleids = new List<long>();
                    //    List<string> rolenames = new List<string>();
                    //    foreach (var role in roles)
                    //    {
                    //        roleids.Add(role.Id);
                    //        rolenames.Add(role.Name);
                    //    }
                    //    userDto.RoleId = roleids;
                    //    userDto.RoleName = rolenames;
                    //}

                    //var tokenString = new JwtTokenBuilder()
                    //                    .AddSecurityKey(JwtSecurityKey.Create("fiver-secret-key"))
                    //                    .AddSubject("james bond")
                    //                    .AddIssuer("Fiver.Security.Bearer")
                    //                    .AddAudience("Fiver.Security.Bearer")
                    //                    .AddClaim("MembershipId", "111")
                    //                    .AddExpiry(1 * 60 * 24 * 7)
                    //                    .AdduserDTO(userDto)
                    //                    .Build();

                    // Update record to UserSession
                    _userSessions.Update(new UserSessions
                    {
                        Id = Usession.Id,
                        AccessToken = Authorization
                    }, userDto.Id.ToString());

                    //responseData.token = tokenString.Value;
                    responseData.response_code = 0;
                    responseData.message = "Refreshed Successfully";
                    responseData.status = "Success";
                    return StatusCode(200, responseData);

                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Invalid token";
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

        private string BuildToken(UserDTO user)
        {

            List<Claim> claims = new List<Claim>
            {
                        new Claim("Id", user.Id.ToString()),
                        new Claim("FullName", user.FullName),
                        new Claim("Email", user.Email),
            };

            foreach (var rolename in user.RoleName)
            {
                claims.Add(new Claim("RoleName", rolename));
            }

            foreach (var roleid in user.RoleId)
            {
                claims.Add(new Claim("RoleId", roleid.ToString()));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("veryVerySecretKey"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(_config["Tokens:Issuer"],
              _config["Tokens:Issuer"],
              claims,
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }


    public static class IHttpContextAccessorExtension
    {
        public static int CurrentUser(this IHttpContextAccessor httpContextAccessor)
        {
            var stringId = httpContextAccessor?.HttpContext?.User?.FindFirst(JwtRegisteredClaimNames.Jti)?.Value;
            int.TryParse(stringId ?? "0", out int userId);

            return userId;
        }
    }
}
