using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RestSharp;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.Terms;
using Trainning24.BL.ViewModels.UserRole;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    [Produces("application/json")]
    [ApiExplorerSettings(GroupName = nameof(SwaggerGrouping.Users))]
    //[Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UsersBusiness usersBusiness;
        private readonly RoleBusiness RoleBusiness;
        private readonly DefaultValuesBusiness DefaultValuesBusiness;
        private readonly LessonBusiness LessonBusiness;
        private IHostingEnvironment hostingEnvironment;
        private readonly ManagementInfoBusiness managementInfoBusiness;
        private readonly SalesAgentBusiness SalesAgentBusiness;
        private readonly IndividualDetailsBusiness _individualDetailsBusiness;
        private readonly TermsAndCondtionBusiness _termsAndCondtionBusiness;
        public UsersController(
            IHostingEnvironment hostingEnvironment,
            LessonBusiness LessonBusiness,
            UsersBusiness usersBusiness,
            RoleBusiness RoleBusiness,
            DefaultValuesBusiness DefaultValuesBusiness,
            ManagementInfoBusiness ManagementInfoBusiness,
            SalesAgentBusiness SalesAgentBusiness,
            IndividualDetailsBusiness individualDetailsBusiness,
            TermsAndCondtionBusiness termsAndCondtionBusiness
            )
        {
            this.LessonBusiness = LessonBusiness;
            this.hostingEnvironment = hostingEnvironment;
            this.usersBusiness = usersBusiness;
            this.RoleBusiness = RoleBusiness;
            this.DefaultValuesBusiness = DefaultValuesBusiness;
            this.managementInfoBusiness = ManagementInfoBusiness;
            this.SalesAgentBusiness = SalesAgentBusiness;
            this._individualDetailsBusiness = individualDetailsBusiness;
            this._termsAndCondtionBusiness = termsAndCondtionBusiness;
        }

        [HttpPost("Signup")]
        public IActionResult Signup([FromBody]AthSignUp signup)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                //getting ManagementAPIClient authority for Auth0 implementation this is used for calling Auth Management API
                ManagementApiClient mac = General.getAuthManagementApiToken();

                //getting user from auth0 by email
                Auth0.ManagementApi.Models.User user = mac.Users.GetUsersByEmailAsync(signup.Email).Result.SingleOrDefault();

                if (user == null)
                {
                    AddUserRoleModel addUserRoleModel = new AddUserRoleModel { roleid = 4 };
                    List<AddUserRoleModel> rolelist = new List<AddUserRoleModel>();
                    rolelist.Add(addUserRoleModel);
                    signup.Roles = rolelist;
                    UserCreateRequest userCreateRequest = new UserCreateRequest();
                    userCreateRequest.Email = signup.Email;
                    //userCreateRequest.UserName = string.Empty;
                    userCreateRequest.EmailVerified = true;
                    userCreateRequest.Password = signup.Password;
                    userCreateRequest.Connection = "Username-Password-Authentication";
                    string roledetail = usersBusiness.getrolelist(signup.Roles);

                    string metadata = "{" +
                                        "\"authorization\": {\"roles\": [" + roledetail + "]}" +
                                      "}";

                    userCreateRequest.AppMetadata = JsonConvert.DeserializeObject(metadata);
                    userCreateRequest.UserMetadata = JsonConvert.DeserializeObject("{}");

                    //creating user on atuh0 db
                    Auth0.ManagementApi.Models.User newuser = mac.Users.CreateAsync(userCreateRequest).Result;

                    successResponse.data = null;
                    successResponse.response_code = 0;
                    successResponse.message = "User added";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "User already exists";
                    unsuccessResponse.status = "Failure";
                    return StatusCode(422, unsuccessResponse);
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
        public async Task<IActionResult> AddUser([FromBody]CreateUserViewModel createUserViewModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            Trainning24.Domain.Entity.User user = usersBusiness.UserExistance(createUserViewModel);
            string Authorization = Request.Headers["id_token"];
            //getting claim using id_token(authid)
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (ModelState.IsValid)
                {
                    if (user != null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "User already exist";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(422, unsuccessResponse);
                    }
                    else
                    {
                        if (tc.RoleName.Contains(General.getRoleType("1")) || tc.RoleName.Contains(General.getRoleType("17")))
                        {
                            if (createUserViewModel.Password != createUserViewModel.ConfirmPassword)
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "Password does not match with confirm password.";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(401, unsuccessResponse);
                            }
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
                            //getting ManagementAPIClient authority for Auth0 implementation this is used for calling Auth Management API
                            ManagementApiClient mac = General.getAuthManagementApiToken();
                            Trainning24.Domain.Entity.User newuser = await usersBusiness.Create(createUserViewModel, tc.Id);
                            UserCreateRequest userCreateRequest = new UserCreateRequest();
                            //userCreateRequest.UserName = createUserViewModel.Username;
                            userCreateRequest.Email = createUserViewModel.Email;
                            userCreateRequest.EmailVerified = true;
                            userCreateRequest.FullName = createUserViewModel.FullName;
                            userCreateRequest.Password = createUserViewModel.Password;
                            userCreateRequest.Connection = "Username-Password-Authentication";
                            string roledetail = usersBusiness.getrolelist(createUserViewModel.Roles);
                            string metadata = "{" +
                                                "\"authorization\": {    \"roles\": [" + roledetail + "],\"username\": \"" + newuser.Username + "\",\"uid\": \"" + newuser.Id + "\"}," +
                                              "}";

                            userCreateRequest.AppMetadata = JsonConvert.DeserializeObject(metadata);
                            userCreateRequest.UserMetadata = JsonConvert.DeserializeObject("{}");
                            try
                            {
                                //creating user on auth0 db
                                Auth0.ManagementApi.Models.User a0User = mac.Users.CreateAsync(userCreateRequest).Result;
                                UpdateUserViewModel updateUserViewModel = new UpdateUserViewModel();
                                updateUserViewModel.Id = newuser.Id;
                                updateUserViewModel.authid = a0User.UserId;
                                usersBusiness.UpdateAuthId(updateUserViewModel);
                            }
                            catch (Exception ex)
                            {
                                unsuccessResponse.response_code = 2;
                                unsuccessResponse.message = ex.Message;
                                unsuccessResponse.status = "Failure";
                                return StatusCode(500, unsuccessResponse);
                            }
                            if (createUserViewModel.Roles.Any(x => x.roleid.Equals(17)))
                            {
                                SalesAgent IfAgentExist = SalesAgentBusiness.GetAgentByEmail(createUserViewModel.Email);
                                SalesAgent salesAgent = new SalesAgent();
                                salesAgent.AgentName = createUserViewModel.Username;
                                salesAgent.AgentCategoryId = createUserViewModel.agentCategoryId;
                                salesAgent.Phone = createUserViewModel.phonenumber;
                                salesAgent.Email = createUserViewModel.Email;
                                salesAgent.PartnerBackgroud = createUserViewModel.partnerBackgroud;
                                salesAgent.FocalPoint = createUserViewModel.focalPoint;
                                salesAgent.Position = createUserViewModel.position;
                                salesAgent.PhysicalAddress = createUserViewModel.physicalAddress;
                                salesAgent.CurrencyCode = createUserViewModel.currencyCode;
                                salesAgent.AgreedMonthlyDeposit = createUserViewModel.agreedMonthlyDeposit;
                                salesAgent.IsActive = createUserViewModel.isactive;
                                salesAgent.UserId = newuser.Id;
                                if (IfAgentExist != null)
                                {
                                    var _updateSalesAgent = SalesAgentBusiness.Update(salesAgent, int.Parse(tc.Id));
                                }
                                else
                                {
                                    var _createSalesAgent = SalesAgentBusiness.Create(salesAgent, int.Parse(tc.Id));
                                }
                            }
                            DetailUserDTO userDTO = new DetailUserDTO();
                            List<Role> roles = usersBusiness.Role(newuser);
                            if (roles != null)
                            {
                                List<long> roleids = new List<long>();
                                List<string> rolenames = new List<string>();
                                foreach (var role1 in roles)
                                {
                                    roleids.Add(role1.Id);
                                    rolenames.Add(role1.Name);
                                }
                                userDTO.Roles = roleids;
                                userDTO.RoleName = rolenames;
                            }
                            userDTO.Id = newuser.Id;
                            userDTO.Username = newuser.Username;
                            userDTO.FullName = newuser.FullName;
                            userDTO.Email = newuser.Email;
                            userDTO.is_skippable = newuser.is_skippable;
                            userDTO.profilepicurl = newuser.ProfilePicUrl;
                            userDTO.timeout = newuser.timeout;
                            userDTO.reminder = newuser.reminder;
                            userDTO.istimeouton = newuser.istimeouton;
                            userDTO.phonenumber = newuser.phonenumber;
                            successResponse.data = userDTO;
                            successResponse.response_code = 0;
                            successResponse.message = "User added";
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
        public IActionResult UpdateUser(int id, [FromBody]UpdateUserViewModel updateUserViewModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            updateUserViewModel.Id = id;
            Trainning24.Domain.Entity.User user = usersBusiness.UserExistanceUpdate(updateUserViewModel);
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            //getting claims after decoding id_token
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (ModelState.IsValid)
                {
                    if (user == null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "User not found.";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(404, unsuccessResponse);
                    }
                    else
                    {
                        if (tc.RoleName.Contains(General.getRoleType("1")))
                        {
                            if (!string.IsNullOrEmpty(updateUserViewModel.Password))
                            {
                                if (updateUserViewModel.Password != updateUserViewModel.ConfirmPassword)
                                {
                                    unsuccessResponse.response_code = 1;
                                    unsuccessResponse.message = "Password doest not match with confirm password.";
                                    unsuccessResponse.status = "Unsuccess";
                                    return StatusCode(401, unsuccessResponse);
                                }
                            }
                            Trainning24.Domain.Entity.User userExistance = usersBusiness.UserExistanceUpdate(updateUserViewModel);
                            if (user.Id == id || userExistance == null)
                            {
                                DefaultValues newDefaultValuesList = DefaultValuesBusiness.GetById(6);
                                if (newDefaultValuesList != null)
                                {
                                    if (string.IsNullOrEmpty(updateUserViewModel.istimeouton.ToString()))
                                        updateUserViewModel.istimeouton = newDefaultValuesList.istimeouton;
                                    if (updateUserViewModel.timeout == 0)
                                        updateUserViewModel.timeout = newDefaultValuesList.timeout;
                                    if (updateUserViewModel.reminder == 0)
                                        updateUserViewModel.reminder = newDefaultValuesList.reminder;
                                    if (updateUserViewModel.intervals == 0)
                                        updateUserViewModel.reminder = newDefaultValuesList.intervals;
                                }
                                Trainning24.Domain.Entity.User newuser = usersBusiness.Update(updateUserViewModel, tc.Id);
                                //getting ManagementAPIClient authority for Auth0 implementation this is used for calling Auth Management API
                                ManagementApiClient mac = General.getAuthManagementApiToken();
                                UserUpdateRequest userUpdateRequest = new UserUpdateRequest();
                                user = usersBusiness.UserExistanceUpdate(updateUserViewModel);
                                userUpdateRequest.Password = updateUserViewModel.Password;
                                userUpdateRequest.Connection = "Username-Password-Authentication";
                                string roledetail = usersBusiness.getrolelist(updateUserViewModel.Roles);
                                string metadata = "{" +
                                                    "\"authorization\": {    \"roles\": [" + roledetail + "],\"username\": \"" + newuser.Username + "\",\"uid\": \"" + newuser.Id + "\"}," +
                                                  "}";

                                userUpdateRequest.AppMetadata = JsonConvert.DeserializeObject(metadata);
                                userUpdateRequest.UserMetadata = JsonConvert.DeserializeObject("{}");
                                try
                                {
                                    Auth0.ManagementApi.Models.User a0User = mac.Users.UpdateAsync(user.AuthId, userUpdateRequest).Result;
                                    updateUserViewModel.authid = a0User.UserId;
                                }
                                catch (Exception ex)
                                {
                                    unsuccessResponse.response_code = 2;
                                    unsuccessResponse.message = ex.Message;
                                    unsuccessResponse.status = "Failure";
                                    return StatusCode(500, unsuccessResponse);
                                }
                                if (updateUserViewModel.Roles.Any(x => x.roleid.Equals(17)))
                                {
                                    SalesAgent IfAgentExist = SalesAgentBusiness.GetAgentByEmail(updateUserViewModel.Email);
                                    SalesAgent salesAgent = new SalesAgent();
                                    salesAgent.AgentName = updateUserViewModel.Username;
                                    salesAgent.AgentCategoryId = updateUserViewModel.agentCategoryId;
                                    salesAgent.Phone = updateUserViewModel.phonenumber;
                                    salesAgent.Email = updateUserViewModel.Email;
                                    salesAgent.PartnerBackgroud = updateUserViewModel.partnerBackgroud;
                                    salesAgent.FocalPoint = updateUserViewModel.focalPoint;
                                    salesAgent.Position = updateUserViewModel.position;
                                    salesAgent.PhysicalAddress = updateUserViewModel.physicalAddress;
                                    salesAgent.CurrencyCode = updateUserViewModel.currencyCode;
                                    salesAgent.AgreedMonthlyDeposit = updateUserViewModel.agreedMonthlyDeposit;
                                    salesAgent.IsActive = updateUserViewModel.isactive;
                                    salesAgent.UserId = updateUserViewModel.Id;
                                    if (IfAgentExist != null)
                                    {
                                        var _updateSalesAgent = SalesAgentBusiness.Update(salesAgent, int.Parse(tc.Id));
                                    }
                                    else
                                    {
                                        var _createSalesAgent = SalesAgentBusiness.Create(salesAgent, int.Parse(tc.Id));
                                    }
                                }
                                successResponse.response_code = 0;
                                successResponse.message = "User updated";
                                successResponse.status = "Success";
                                return StatusCode(200, successResponse);
                            }
                            else
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "Email already exist";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(422, unsuccessResponse);
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
        public IActionResult DeleteUser(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            CreateUserViewModel createUserViewModel = new CreateUserViewModel
            {
                Id = id
            };
            Trainning24.Domain.Entity.User user = usersBusiness.UserExistanceById(createUserViewModel);

            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (user == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "User not found.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
                }
                else
                {
                    if (tc.RoleName.Contains(General.getRoleType("1")))
                    {
                        //getting ManagementAPIClient authority for Auth0 implementation this is used for calling Auth Management API
                        ManagementApiClient mac = General.getAuthManagementApiToken();
                        try
                        {
                            mac.Users.DeleteAsync(user.AuthId);
                        }
                        catch (Exception ex)
                        {
                            unsuccessResponse.response_code = 2;
                            unsuccessResponse.message = ex.Message;
                            unsuccessResponse.status = "Failure";
                            return StatusCode(500, unsuccessResponse);
                        }
                        SalesAgent salesAgent = SalesAgentBusiness.GetAgentByEmail(user.Email);
                        if (salesAgent != null)
                        {
                            SalesAgentBusiness.DeleteByAdmin(salesAgent.Id, int.Parse(tc.Id));
                        }
                        usersBusiness.Delete(user, tc.Id);
                        successResponse.response_code = 0;
                        successResponse.message = "User Deleted";
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
            finally
            {
                user = null;
            }
        }

        [HttpGet]
        public IActionResult Get(int pagenumber, int perpagerecord, string roleid, string search)
        {
            string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx

            string[] lstroleids = null;
            if (!string.IsNullOrEmpty(roleid))
            {
                lstroleids = roleid.Split(",");
            }
            PaginationModel2 paginationModel = new PaginationModel2
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord,
                roleid = lstroleids,
                search = search
            };
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            List<Trainning24.Domain.Entity.User> userList = new List<Trainning24.Domain.Entity.User>();
            List<DetailUserDTO> userDTOList = new List<DetailUserDTO>();

            if (
                    tc.RoleName.Contains(General.getRoleType("1")) ||
                    tc.RoleName.Contains(General.getRoleType("5")) ||
                    tc.RoleName.Contains(General.getRoleType("6")) ||
                    tc.RoleName.Contains(General.getRoleType("7")) ||
                    tc.RoleName.Contains(General.getRoleType("8")) ||
                    tc.RoleName.Contains(General.getRoleType("9")) ||
                    tc.RoleName.Contains(General.getRoleType("10")) ||
                    tc.RoleName.Contains(General.getRoleType("11")) ||
                    tc.RoleName.Contains(General.getRoleType("12")))
            {
                userList = usersBusiness.UserList(paginationModel, out int total);
                foreach (Trainning24.Domain.Entity.User user in userList)
                {
                    DetailUserDTO userDTO = new DetailUserDTO();
                    List<Role> roles = usersBusiness.Role(user);

                    if (roles != null)
                    {
                        List<long> roleids = new List<long>();
                        List<string> rolenames = new List<string>();
                        foreach (var role in roles)
                        {
                            roleids.Add(role.Id);
                            rolenames.Add(role.Name);
                        }
                        userDTO.Roles = roleids;
                        userDTO.RoleName = rolenames;
                    }
                    userDTO.Id = user.Id;
                    userDTO.Username = user.Username;
                    userDTO.FullName = user.FullName;
                    userDTO.Email = user.Email;
                    userDTO.is_skippable = user.is_skippable;
                    userDTO.timeout = user.timeout;
                    userDTO.reminder = user.reminder;
                    userDTO.istimeouton = user.istimeouton;
                    userDTO.phonenumber = user.phonenumber;
                    //userDTO.profilepicurl = user.ProfilePicUrl;
                    if (!string.IsNullOrEmpty(user.ProfilePicUrl))
                        userDTO.profilepicurl = LessonBusiness.geturl(user.ProfilePicUrl, Certificate);
                    userDTOList.Add(userDTO);
                }

                successResponse.data = userDTOList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Users detail";
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

        [HttpGet("GetUserList")]
        public IActionResult GetUserList(int pagenumber, int perpagerecord, string roleid, string search)
        {
            //PaginationModel paginationModel = new PaginationModel
            //{
            //    pagenumber = pagenumber,
            //    perpagerecord = perpagerecord,
            //    roleid = roleid,
            //    search = search
            //};
            string[] lstroleids = null;
            if (!string.IsNullOrEmpty(roleid))
            {
                lstroleids = roleid.Split(",");
            }
            PaginationModel2 paginationModel = new PaginationModel2
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord,
                roleid = lstroleids,
                search = search
            };
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);

            if (tc.RoleName.Contains(General.getRoleType("1")) || tc.RoleName.Contains(General.getRoleType("13")) || tc.RoleName.Contains(General.getRoleType("17")))
            {
                successResponse.data = usersBusiness.GetUserList(paginationModel, out int total);
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Users detail";
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

        [HttpGet("{id}")]
        public IActionResult GetUserById(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            CreateUserViewModel createUserViewModel = new CreateUserViewModel();

            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];


            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            createUserViewModel.Id = id;
            try
            {
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx

                Trainning24.Domain.Entity.User user = usersBusiness.UserExistanceById(createUserViewModel);
                if (user == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "User not found.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
                }
                else
                {
                    DetailUserDTO userDto = new DetailUserDTO
                    {
                        Id = user.Id,
                        Username = user.Username,
                        FullName = user.FullName,
                        Email = user.Email,
                        is_skippable = user.is_skippable,
                        Bio = user.Bio,
                        //profilepicurl = user.ProfilePicUrl,
                        timeout = user.timeout,
                        intervals = user.intervals,
                        reminder = user.reminder,
                        phonenumber = user.phonenumber,
                        istimeouton = user.istimeouton,
                        is_discussion_authorized = user.is_discussion_authorized,
                        is_assignment_authorized = user.is_assignment_authorized,
                        is_library_authorized = user.is_library_authorized,
                        isallowmap = user.isallowmap
                    };
                    if (!string.IsNullOrEmpty(user.ProfilePicUrl))
                        userDto.profilepicurl = LessonBusiness.geturl(user.ProfilePicUrl, Certificate);


                    List<Role> roles = usersBusiness.Role(user);
                    if (roles != null)
                    {
                        List<long> roleids = new List<long>();
                        List<string> rolenames = new List<string>();
                        foreach (var role in roles)
                        {
                            roleids.Add(role.Id);
                            rolenames.Add(role.Name);
                        }
                        userDto.Roles = roleids;
                        userDto.RoleName = rolenames;
                    }

                    if (userDto.Roles.Contains(17))
                    {
                        SalesAgent salesAgent = SalesAgentBusiness.GetAgentByEmail(userDto.Email);
                        if (salesAgent != null)
                        {
                            userDto.salesagentId = salesAgent.Id;
                            userDto.agentCategoryId = salesAgent.AgentCategoryId;
                            userDto.partnerBackgroud = salesAgent.PartnerBackgroud;
                            userDto.focalPoint = salesAgent.FocalPoint;
                            userDto.position = salesAgent.Position;
                            userDto.physicalAddress = salesAgent.PhysicalAddress;
                            userDto.currencyCode = salesAgent.CurrencyCode;
                            userDto.agreedMonthlyDeposit = salesAgent.AgreedMonthlyDeposit;
                            userDto.isactive = salesAgent.IsActive;
                        }
                    }

                    successResponse.data = userDto;
                    successResponse.response_code = 0;
                    successResponse.message = "User detail";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
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

        [HttpGet("getAssignedPersonDetails")]
        public IActionResult GetAssignedPersonDetails(int pagenumber, int perpagerecord, string search)
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
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                List<AssignedPersonModel> assignedPersonModel = usersBusiness.GetAssignedPersonDetails(
                                                                                                            long.Parse(tc.Id),
                                                                                                            tc.RoleName,
                                                                                                            paginationModel,
                                                                                                            out int total
                                                                                                            );
                successResponse.totalcount = total;
                successResponse.data = assignedPersonModel;
                successResponse.response_code = 0;
                successResponse.message = "AssignedPerson detail";
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

        [HttpPost("getUsersDetailRoleWise")]
        public IActionResult getUsersDetailRoleWise(List<long> roles)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            List<UserDetailRoleWise> userDetailRoleWiseList = new List<UserDetailRoleWise>();

            foreach (long roleid in roles)
            {
                UserDetailRoleWise userDetailRoleWise = new UserDetailRoleWise();
                Role role = RoleBusiness.RoleExistanceById(int.Parse(roleid.ToString()));
                userDetailRoleWise.RoleId = roleid;
                userDetailRoleWise.RoleName = role.Name;

                List<DetailUserRole> detailUserRoles = new List<DetailUserRole>();
                List<UserRole> userRoles = usersBusiness.getUserRole(roleid);
                foreach (var userrole in userRoles)
                {
                    Trainning24.Domain.Entity.User user = usersBusiness.GetUserbyId(userrole.Id);

                    if (user != null)
                    {
                        DetailUserRole userDto = new DetailUserRole
                        {
                            Id = user.Id,
                            Username = user.Username,
                            FullName = user.FullName,
                            is_skippable = user.is_skippable,
                            timeout = user.timeout,
                            reminder = user.reminder,
                            istimeouton = user.istimeouton,
                            Email = user.Email,
                            phonenumber = user.phonenumber
                        };

                        List<Role> userroles = usersBusiness.Role(user);

                        if (userroles != null)
                        {
                            List<long> roleids = new List<long>();
                            List<string> rolenames = new List<string>();
                            foreach (var role1 in userroles)
                            {
                                roleids.Add(role1.Id);
                                rolenames.Add(role1.Name);
                            }
                            userDto.Roles = roleids;
                            userDto.RoleName = rolenames;
                        }

                        detailUserRoles.Add(userDto);
                        userDetailRoleWise.Details = detailUserRoles;
                    }

                }
                userDetailRoleWiseList.Add(userDetailRoleWise);
            }

            successResponse.data = userDetailRoleWiseList;
            successResponse.response_code = 0;
            successResponse.message = "User detail";
            successResponse.status = "Success";
            return StatusCode(200, successResponse);
        }

        [Authorize]
        [HttpGet("GetStatistic")]
        public IActionResult GetStatistic()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            UserStatistics userStatistics = new UserStatistics();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                var userid = Convert.ToInt32(tc.Id);
                //var roleid = Convert.ToInt32(tc.RoleId.FirstOrDefault());
                userStatistics = usersBusiness.GetStatistics(tc.RoleName.FirstOrDefault(), userid);
                if (userStatistics != null)
                {
                    successResponse.data = userStatistics;
                    successResponse.response_code = 0;
                    successResponse.message = "Statistic detail";
                    successResponse.status = "Success";
                }
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
        [HttpGet("GetManagementInfo")]
        public IActionResult GetManagementInfo()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            ManagementInfoViewModel managementInfo = new ManagementInfoViewModel();

            try
            {
                var data = managementInfoBusiness.GetAll();
                if (data != null)
                {
                    managementInfo.individual_receipt_notes = data.individual_receipt_notes;
                    managementInfo.noon_background = data.noon_background;
                    managementInfo.sales_partner_dari = data.sales_partner_dari;
                    managementInfo.sales_partner_eng = data.sales_partner_eng;
                    managementInfo.school_receipt_notes = data.school_receipt_notes;
                    successResponse.data = managementInfo;
                    successResponse.response_code = 0;
                    successResponse.message = "ManagementInfo detail";
                    successResponse.status = "Success";
                }
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
        [HttpPost("ManageManagementInfo")]
        public IActionResult ManageManagementInfo(ManagementInfo obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Authorization = Request.Headers["id_token"];

                //getting claim using id_token(authid)
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                var data = managementInfoBusiness.CreateUpdate(obj, tc.Id);
                if (data != null)
                {
                    successResponse.response_code = 0;
                    successResponse.message = "ManagementInfo Updated";
                    successResponse.status = "Success";
                    successResponse.status = "Success";
                }
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

        [HttpPost("SignupTrialUser")]
        public async Task<IActionResult> SignupTrialUser([FromBody]CreateTrialUser dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            Trainning24.Domain.Entity.User user = usersBusiness.UserExistsByEmail(dto.Email);
            try
            {
                if (ModelState.IsValid)
                {
                    if (user != null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "User already exist";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(422, unsuccessResponse);
                    }
                    else
                    {
                        if (dto.Password != dto.RepeatPassword)
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Password does not match with confirm password.";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(401, unsuccessResponse);
                        }
                        DefaultValues newDefaultValuesList = DefaultValuesBusiness.GetById(6);
                        if (newDefaultValuesList != null)
                        {
                            if (string.IsNullOrEmpty(dto.istimeouton.ToString()))
                                dto.istimeouton = newDefaultValuesList.istimeouton;
                            if (dto.timeout == 0)
                                dto.timeout = newDefaultValuesList.timeout;
                            if (dto.reminder == 0)
                                dto.reminder = newDefaultValuesList.reminder;
                            if (dto.intervals == 0)
                                dto.intervals = newDefaultValuesList.intervals;
                        }
                        //Add default role for student
                        List<AddUserRoleModel> _lstRoles = new List<AddUserRoleModel>();
                        AddUserRoleModel _roles = new AddUserRoleModel
                        {
                            roleid = 4
                        };
                        _lstRoles.Add(_roles);
                        dto.Roles = _lstRoles;
                        //getting ManagementAPIClient authority for Auth0 implementation this is used for calling Auth Management API
                        ManagementApiClient mac = General.getAuthManagementApiToken();
                        Trainning24.Domain.Entity.User newuser = await usersBusiness.CreateTrialUser(dto);
                        UserCreateRequest userCreateRequest = new UserCreateRequest();
                        //userCreateRequest.UserName = createUserViewModel.Username;
                        userCreateRequest.Email = dto.Email;
                        userCreateRequest.EmailVerified = true;
                        userCreateRequest.FullName = dto.StudentName;
                        userCreateRequest.Password = dto.Password;
                        userCreateRequest.Connection = "Username-Password-Authentication";
                        string roledetail = usersBusiness.getrolelist(dto.Roles);
                        string metadata = "{" +
                                            "\"authorization\": {    \"roles\": [" + roledetail + "],\"username\": \"" + newuser.Username + "\",\"uid\": \"" + newuser.Id + "\"}," +
                                          "}";

                        userCreateRequest.AppMetadata = JsonConvert.DeserializeObject(metadata);
                        userCreateRequest.UserMetadata = JsonConvert.DeserializeObject("{}");
                        try
                        {
                            //creating user on auth0 db
                            Auth0.ManagementApi.Models.User a0User = mac.Users.CreateAsync(userCreateRequest).Result;
                            UpdateUserViewModel updateUserViewModel = new UpdateUserViewModel();
                            updateUserViewModel.Id = newuser.Id;
                            updateUserViewModel.authid = a0User.UserId;
                            usersBusiness.UpdateAuthId(updateUserViewModel);
                            dto.UserId = newuser.Id;
                            _individualDetailsBusiness.CreateTrialUser(dto);
                        }
                        catch (Exception ex)
                        {
                            unsuccessResponse.response_code = 2;
                            unsuccessResponse.message = ex.Message;
                            unsuccessResponse.status = "Failure";
                            return StatusCode(500, unsuccessResponse);
                        }
                        DetailUserDTO userDTO = new DetailUserDTO();
                        List<Role> roles = usersBusiness.Role(newuser);
                        if (roles != null)
                        {
                            List<long> roleids = new List<long>();
                            List<string> rolenames = new List<string>();
                            foreach (var role1 in roles)
                            {
                                roleids.Add(role1.Id);
                                rolenames.Add(role1.Name);
                            }
                            userDTO.Roles = roleids;
                            userDTO.RoleName = rolenames;
                        }
                        userDTO.Id = newuser.Id;
                        userDTO.Username = newuser.Username;
                        userDTO.FullName = newuser.FullName;
                        userDTO.Email = newuser.Email;
                        userDTO.is_skippable = newuser.is_skippable;
                        userDTO.profilepicurl = newuser.ProfilePicUrl;
                        userDTO.timeout = newuser.timeout;
                        userDTO.reminder = newuser.reminder;
                        userDTO.istimeouton = newuser.istimeouton;
                        userDTO.phonenumber = newuser.phonenumber;
                        successResponse.data = userDTO;
                        successResponse.response_code = 0;
                        successResponse.message = "User added";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
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

        [HttpGet("GetErpDetails")]
        public IActionResult GetErpDetails()
        {
            SuccessResponse successResponse = new SuccessResponse();
            var getErpSettings = usersBusiness.GetErpDetails();
            successResponse.data = getErpSettings;
            successResponse.response_code = 0;
            successResponse.message = "Detail fetched";
            successResponse.status = "Success";
            return StatusCode(200, successResponse);
        }

        [HttpPost("UpdateTerms")]
        public IActionResult UpdateTerms([FromBody]TermsDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                var getTerms = _termsAndCondtionBusiness.GetTerms();
                if (getTerms == null)
                {
                    TermsAndConditions termsAndConditions = new TermsAndConditions();
                    termsAndConditions.Terms = dto.terms;
                    termsAndConditions.CreatorUserId = int.Parse(tc.Id);
                    termsAndConditions.CreationTime = DateTime.UtcNow.ToString();
                    _termsAndCondtionBusiness.AddRecord(termsAndConditions);
                }
                else
                {
                    getTerms.Terms = dto.terms;
                    getTerms.LastModifierUserId = int.Parse(tc.Id);
                    getTerms.LastModificationTime = DateTime.UtcNow.ToString();
                    _termsAndCondtionBusiness.UpdateRecord(getTerms);
                }
                successResponse.response_code = 0;
                successResponse.message = "Terms Updated";
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

        [AllowAnonymous]
        [HttpGet("GetTerms")]
        public IActionResult GetTerms()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                var getTerms = _termsAndCondtionBusiness.GetTerms();
                TermsDTO termsDTO = new TermsDTO();
                if (getTerms != null)
                {
                    termsDTO.terms = getTerms.Terms;
                    successResponse.data = termsDTO;
                }
                successResponse.response_code = 0;
                successResponse.message = "Terms Updated";
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