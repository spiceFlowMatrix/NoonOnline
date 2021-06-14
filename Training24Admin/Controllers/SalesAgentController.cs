using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Auth0.ManagementApi;
using Auth0.ManagementApi.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.SalesAgent;
using Trainning24.BL.ViewModels.UserRole;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class SalesAgentController : ControllerBase
    {
        private readonly SalesAgentBusiness SalesAgentBusiness;
        private readonly UsersBusiness usersBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly AgentCategoryBusiness AgentCategoryBusiness;
        private readonly IMapper _mapper;
        private readonly DefaultValuesBusiness DefaultValuesBusiness;

        public SalesAgentController
        (
            IMapper mapper,
            SalesAgentBusiness SalesAgentBusiness,
            LessonBusiness LessonBusiness,
            AgentCategoryBusiness AgentCategoryBusiness,
            UsersBusiness usersBusiness,
            DefaultValuesBusiness DefaultValuesBusiness
        )
        {
            this.SalesAgentBusiness = SalesAgentBusiness;
            this.LessonBusiness = LessonBusiness;
            this.AgentCategoryBusiness = AgentCategoryBusiness;
            this.usersBusiness = usersBusiness;
            _mapper = mapper;
            this.DefaultValuesBusiness = DefaultValuesBusiness;
        }


        //[HttpPost("AddCourrency")]
        //public IActionResult AddCourrency([FromBody] List<Currency> currency)
        //{
        //    SalesAgentBusiness.AddCurrency(currency);
        //    return null;
        //}



        [HttpGet("GetCurrencyList")]
        public IActionResult GetCurrencyList()
        {
            SuccessResponse successResponse = new SuccessResponse();

            List<CurrencyResponse> currencyResponses = new List<CurrencyResponse>();
            List<Currency> currencies = SalesAgentBusiness.getAllCurrency();

            foreach (var currency in currencies)
            {
                CurrencyResponse currencyResponse = new CurrencyResponse();
                currencyResponse.id = currency.Id;
                currencyResponse.currency = currency.currency;
                currencyResponse.abbreviation = currency.abbreviation;
                currencyResponse.symbol = currency.symbol;
                currencyResponses.Add(currencyResponse);
            }

            successResponse.data = currencyResponses;
            successResponse.response_code = 0;
            successResponse.message = "Currencylist";
            successResponse.status = "Success";
            return StatusCode(200, successResponse);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Post([FromBody] CreateUserViewModel dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            Trainning24.Domain.Entity.User newuser = new Trainning24.Domain.Entity.User();
            Trainning24.Domain.Entity.User user = usersBusiness.UserExistance(dto);
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (ModelState.IsValid)
                {
                    if (
                        tc.RoleName.Contains(General.getRoleType("1")) ||
                        tc.RoleName.Contains(General.getRoleType("13"))
                       )
                    {
                        SalesAgent IfAgentExist = SalesAgentBusiness.GetAgentByEmail(dto.Email);
                        if (IfAgentExist != null)
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Duplicate entry.";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(422, unsuccessResponse);
                        }
                        else
                        {
                            if (user != null)
                            {
                                if (!string.IsNullOrEmpty(dto.Password))
                                {
                                    if (dto.Password != dto.ConfirmPassword)
                                    {
                                        unsuccessResponse.response_code = 1;
                                        unsuccessResponse.message = "Password doest not match with confirm password.";
                                        unsuccessResponse.status = "Unsuccess";
                                        return StatusCode(401, unsuccessResponse);
                                    }
                                }
                                if (user != null)
                                {
                                    if (string.IsNullOrEmpty(dto.istimeouton.ToString()))
                                        dto.istimeouton = user.istimeouton;
                                    if (dto.timeout == 0)
                                        dto.timeout = user.timeout;
                                    if (dto.reminder == 0)
                                        dto.reminder = user.reminder;
                                    if (dto.intervals == 0)
                                        dto.reminder = user.intervals;
                                }
                                dto.Id = user.Id;
                                newuser = usersBusiness.UpdateSalesAgentUser(dto, tc.Id);
                                //getting ManagementAPIClient authority for Auth0 implementation this is used for calling Auth Management API
                                ManagementApiClient mac = General.getAuthManagementApiToken();
                                UserUpdateRequest userUpdateRequest = new UserUpdateRequest();

                                userUpdateRequest.Password = dto.Password;
                                userUpdateRequest.Connection = "Username-Password-Authentication";

                                List<Role> roles = usersBusiness.Role(user);
                                if (roles != null)
                                {
                                    List<AddUserRoleModel> userRoles = new List<AddUserRoleModel>();
                                    foreach (var role in roles)
                                    {
                                        AddUserRoleModel addRole = new AddUserRoleModel();
                                        addRole.roleid = role.Id;
                                        if (dto.Roles.Any(x => x.roleid != role.Id))
                                        {
                                            userRoles.Add(addRole);
                                        }
                                    }
                                    dto.Roles.AddRange(userRoles);
                                }

                                string roledetail = usersBusiness.getrolelist(dto.Roles);
                                string metadata = "{" +
                                                    "\"authorization\": {    \"roles\": [" + roledetail + "],\"username\": \"" + newuser.Username + "\",\"uid\": \"" + newuser.Id + "\"}," +
                                                  "}";

                                userUpdateRequest.AppMetadata = JsonConvert.DeserializeObject(metadata);
                                userUpdateRequest.UserMetadata = JsonConvert.DeserializeObject("{}");
                                try
                                {
                                    Auth0.ManagementApi.Models.User a0User = mac.Users.UpdateAsync(user.AuthId, userUpdateRequest).Result;
                                    dto.authid = a0User.UserId;
                                }
                                catch (Exception ex)
                                {
                                    unsuccessResponse.response_code = 2;
                                    unsuccessResponse.message = ex.Message;
                                    unsuccessResponse.status = "Failure";
                                    return StatusCode(500, unsuccessResponse);
                                }
                            }
                            else
                            {
                                if (dto.Password != dto.ConfirmPassword)
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
                                //getting ManagementAPIClient authority for Auth0 implementation this is used for calling Auth Management API
                                ManagementApiClient mac = General.getAuthManagementApiToken();
                                newuser = await usersBusiness.Create(dto, tc.Id);
                                UserCreateRequest userCreateRequest = new UserCreateRequest();
                                //userCreateRequest.UserName = createUserViewModel.Username;
                                userCreateRequest.Email = dto.Email;
                                userCreateRequest.EmailVerified = true;
                                userCreateRequest.FullName = dto.FullName;
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
                                }
                                catch (Exception ex)
                                {
                                    unsuccessResponse.response_code = 2;
                                    unsuccessResponse.message = ex.Message;
                                    unsuccessResponse.status = "Failure";
                                    return StatusCode(500, unsuccessResponse);
                                }
                            }
                            SalesAgent salesAgent = new SalesAgent();
                            salesAgent.AgentName = dto.Username;
                            salesAgent.AgentCategoryId = dto.agentCategoryId;
                            salesAgent.Phone = dto.phonenumber;
                            salesAgent.Email = dto.Email;
                            salesAgent.PartnerBackgroud = dto.partnerBackgroud;
                            salesAgent.FocalPoint = dto.focalPoint;
                            salesAgent.Position = dto.position;
                            salesAgent.PhysicalAddress = dto.physicalAddress;
                            salesAgent.CurrencyCode = dto.currencyCode;
                            salesAgent.AgreedMonthlyDeposit = dto.agreedMonthlyDeposit;
                            salesAgent.IsActive = dto.isactive;
                            salesAgent.UserId = newuser.Id;
                            var _createSalesAgent = SalesAgentBusiness.Create(salesAgent, int.Parse(tc.Id));
                            successResponse.response_code = 0;
                            successResponse.message = "SalesAgent Created";
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
        [Authorize]
        public async Task<IActionResult> Put(int id, [FromBody] CreateUserViewModel dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            Trainning24.Domain.Entity.User newuser = new Trainning24.Domain.Entity.User();
            Trainning24.Domain.Entity.User user = usersBusiness.UserExistance(dto);
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (ModelState.IsValid)
                {
                    if (
                        tc.RoleName.Contains(General.getRoleType("1")) ||
                        tc.RoleName.Contains(General.getRoleType("13"))
                       )
                    {
                        SalesAgent IfAgentExist = SalesAgentBusiness.GetAgentByEmail(dto.Email);
                        if (IfAgentExist == null)
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "User does not exist.";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(422, unsuccessResponse);
                        }
                        else
                        {
                            if (user != null)
                            {
                                if (!string.IsNullOrEmpty(dto.Password))
                                {
                                    if (dto.Password != dto.ConfirmPassword)
                                    {
                                        unsuccessResponse.response_code = 1;
                                        unsuccessResponse.message = "Password doest not match with confirm password.";
                                        unsuccessResponse.status = "Unsuccess";
                                        return StatusCode(401, unsuccessResponse);
                                    }
                                }
                                if (user != null)
                                {
                                    if (string.IsNullOrEmpty(dto.istimeouton.ToString()))
                                        dto.istimeouton = user.istimeouton;
                                    if (dto.timeout == 0)
                                        dto.timeout = user.timeout;
                                    if (dto.reminder == 0)
                                        dto.reminder = user.reminder;
                                    if (dto.intervals == 0)
                                        dto.reminder = user.intervals;
                                }
                                dto.Id = user.Id;
                                newuser = usersBusiness.UpdateSalesAgentUser(dto, tc.Id);
                                //getting ManagementAPIClient authority for Auth0 implementation this is used for calling Auth Management API
                                ManagementApiClient mac = General.getAuthManagementApiToken();
                                UserUpdateRequest userUpdateRequest = new UserUpdateRequest();

                                userUpdateRequest.Password = dto.Password;
                                userUpdateRequest.Connection = "Username-Password-Authentication";

                                List<Role> roles = usersBusiness.Role(user);
                                if (roles != null)
                                {
                                    List<AddUserRoleModel> userRoles = new List<AddUserRoleModel>();
                                    foreach (var role in roles)
                                    {
                                        AddUserRoleModel addRole = new AddUserRoleModel();
                                        addRole.roleid = role.Id;
                                        if (dto.Roles.Any(x => x.roleid != role.Id))
                                        {
                                            userRoles.Add(addRole);
                                        }
                                    }
                                    dto.Roles.AddRange(userRoles);
                                }

                                string roledetail = usersBusiness.getrolelist(dto.Roles);
                                string metadata = "{" +
                                                    "\"authorization\": {    \"roles\": [" + roledetail + "],\"username\": \"" + newuser.Username + "\",\"uid\": \"" + newuser.Id + "\"}," +
                                                  "}";

                                userUpdateRequest.AppMetadata = JsonConvert.DeserializeObject(metadata);
                                userUpdateRequest.UserMetadata = JsonConvert.DeserializeObject("{}");
                                try
                                {
                                    Auth0.ManagementApi.Models.User a0User = mac.Users.UpdateAsync(user.AuthId, userUpdateRequest).Result;
                                    dto.authid = a0User.UserId;
                                }
                                catch (Exception ex)
                                {
                                    unsuccessResponse.response_code = 2;
                                    unsuccessResponse.message = ex.Message;
                                    unsuccessResponse.status = "Failure";
                                    return StatusCode(500, unsuccessResponse);
                                }
                            }
                            else
                            {
                                if (dto.Password != dto.ConfirmPassword)
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
                                //getting ManagementAPIClient authority for Auth0 implementation this is used for calling Auth Management API
                                ManagementApiClient mac = General.getAuthManagementApiToken();
                                newuser = await usersBusiness.Create(dto, tc.Id);
                                UserCreateRequest userCreateRequest = new UserCreateRequest();
                                //userCreateRequest.UserName = createUserViewModel.Username;
                                userCreateRequest.Email = dto.Email;
                                userCreateRequest.EmailVerified = true;
                                userCreateRequest.FullName = dto.FullName;
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
                                }
                                catch (Exception ex)
                                {
                                    unsuccessResponse.response_code = 2;
                                    unsuccessResponse.message = ex.Message;
                                    unsuccessResponse.status = "Failure";
                                    return StatusCode(500, unsuccessResponse);
                                }
                            }
                            SalesAgent salesAgent = new SalesAgent();
                            salesAgent.AgentName = dto.Username;
                            salesAgent.AgentCategoryId = dto.agentCategoryId;
                            salesAgent.Phone = dto.phonenumber;
                            salesAgent.Email = dto.Email;
                            salesAgent.PartnerBackgroud = dto.partnerBackgroud;
                            salesAgent.FocalPoint = dto.focalPoint;
                            salesAgent.Position = dto.position;
                            salesAgent.PhysicalAddress = dto.physicalAddress;
                            salesAgent.CurrencyCode = dto.currencyCode;
                            salesAgent.AgreedMonthlyDeposit = dto.agreedMonthlyDeposit;
                            salesAgent.IsActive = dto.isactive;
                            salesAgent.UserId = newuser.Id;
                            var _createSalesAgent = SalesAgentBusiness.Update(salesAgent, int.Parse(tc.Id));
                            successResponse.response_code = 0;
                            successResponse.message = "SalesAgent Created";
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
        [Authorize]
        public IActionResult Delete(int id)
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
                    if (
                        tc.RoleName.Contains(General.getRoleType("1")) ||
                        tc.RoleName.Contains(General.getRoleType("13"))
                    )
                    {

                        SalesAgent salesAgent = SalesAgentBusiness.GetAgentById(id);
                        if (salesAgent != null)
                        {
                            Trainning24.Domain.Entity.User user = usersBusiness.UserExistsByEmail(salesAgent.Email);
                            if (user != null)
                            {
                                var removeRole = usersBusiness.RemoveUserRole(user.Id, 17);
                                List<Role> roles = usersBusiness.Role(user);
                                if (roles.Count > 0)
                                {
                                    //getting ManagementAPIClient authority for Auth0 implementation this is used for calling Auth Management API
                                    ManagementApiClient mac = General.getAuthManagementApiToken();
                                    UserUpdateRequest userUpdateRequest = new UserUpdateRequest();
                                    userUpdateRequest.Connection = "Username-Password-Authentication";
                                    List<AddUserRoleModel> userRoles = new List<AddUserRoleModel>();
                                    if (roles != null)
                                    {
                                        foreach (var role in roles)
                                        {
                                            AddUserRoleModel addRole = new AddUserRoleModel();
                                            addRole.roleid = role.Id;
                                            userRoles.Add(addRole);
                                        }
                                    }

                                    string roledetail = usersBusiness.getrolelist(userRoles);
                                    string metadata = "{" +
                                                        "\"authorization\": {    \"roles\": [" + roledetail + "],\"username\": \"" + user.Username + "\",\"uid\": \"" + user.Id + "\"}," +
                                                      "}";

                                    userUpdateRequest.AppMetadata = JsonConvert.DeserializeObject(metadata);
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
                                }
                                else
                                {
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
                                    usersBusiness.Delete(user, tc.Id);
                                }
                            }
                            SalesAgentBusiness.Delete(id, int.Parse(tc.Id));
                            successResponse.response_code = 0;
                            successResponse.message = "SalesAgent Deleted";
                            successResponse.status = "Success";
                            return StatusCode(200, successResponse);
                        }
                        else
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "User not found.";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(401, unsuccessResponse);
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
        [Authorize]
        public IActionResult Get(int id)
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
                    SalesAgent salesAgent = SalesAgentBusiness.getSalesAgentById(id);

                    ResponseSalesAgent responseSalesAgent = new ResponseSalesAgent();
                    responseSalesAgent.AgentCategoryId = salesAgent.AgentCategoryId;
                    responseSalesAgent.AgentName = salesAgent.AgentName;
                    if (salesAgent.AgentCategoryId != 0)
                        responseSalesAgent.CategoryName = AgentCategoryBusiness.getAgentCategoryById(salesAgent.AgentCategoryId).CategoryName;
                    responseSalesAgent.Email = salesAgent.Email;
                    responseSalesAgent.SalesAgentId = salesAgent.Id;
                    responseSalesAgent.Phone = salesAgent.Phone;
                    responseSalesAgent.PartnerBackgroud = salesAgent.PartnerBackgroud;
                    responseSalesAgent.FocalPoint = salesAgent.FocalPoint;
                    responseSalesAgent.Position = salesAgent.Position;
                    responseSalesAgent.PhysicalAddress = salesAgent.PhysicalAddress;
                    responseSalesAgent.CurrencyCode = salesAgent.CurrencyCode;
                    responseSalesAgent.AgreedMonthlyDeposit = salesAgent.AgreedMonthlyDeposit;
                    responseSalesAgent.IsActive = salesAgent.IsActive;
                    responseSalesAgent.username = salesAgent.AgentName;
                    responseSalesAgent.phonenumber = salesAgent.Phone;
                    successResponse.data = responseSalesAgent;
                    successResponse.response_code = 0;
                    successResponse.message = "SalesAgent Detail";
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
                List<SalesAgent> salesAgents = SalesAgentBusiness.SalesAgentList(paginationModel, out int total);
                List<ResponseSalesAgent> rSalesAgents = new List<ResponseSalesAgent>();

                foreach (var salesAgent in salesAgents)
                {
                    ResponseSalesAgent responseSalesAgent = new ResponseSalesAgent();
                    responseSalesAgent.AgentCategoryId = salesAgent.AgentCategoryId;
                    responseSalesAgent.AgentName = salesAgent.AgentName;
                    if (salesAgent.AgentCategoryId != 0)
                        responseSalesAgent.CategoryName = AgentCategoryBusiness.getAgentCategoryById(salesAgent.AgentCategoryId).CategoryName;
                    responseSalesAgent.Email = salesAgent.Email;
                    responseSalesAgent.SalesAgentId = salesAgent.Id;
                    responseSalesAgent.Phone = salesAgent.Phone;

                    responseSalesAgent.PartnerBackgroud = salesAgent.PartnerBackgroud;
                    responseSalesAgent.FocalPoint = salesAgent.FocalPoint;
                    responseSalesAgent.Position = salesAgent.Position;
                    responseSalesAgent.PhysicalAddress = salesAgent.PhysicalAddress;
                    //responseSalesAgent.CurrencyId = salesAgent.CurrencyId;
                    responseSalesAgent.CurrencyCode = salesAgent.CurrencyCode;
                    responseSalesAgent.AgreedMonthlyDeposit = salesAgent.AgreedMonthlyDeposit;
                    responseSalesAgent.IsActive = salesAgent.IsActive;

                    rSalesAgents.Add(responseSalesAgent);
                }

                successResponse.data = rSalesAgents;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "SalesAgent Details";
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

        [HttpGet("GetAgentComission")]
        [Authorize]
        public IActionResult GetAgentComission()
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
                    SalesAgent salesAgent = SalesAgentBusiness.getSalesAgentByUserId(int.Parse(tc.Id));
                    AgentComissionDTO comissionDTO = new AgentComissionDTO();
                    if (salesAgent != null)
                    {
                        comissionDTO.commission = AgentCategoryBusiness.getAgentCategoryById(salesAgent.AgentCategoryId).Commission;
                        comissionDTO.currency = salesAgent.CurrencyCode;
                        successResponse.data = comissionDTO;
                        successResponse.response_code = 0;
                        successResponse.message = "Commission get success.";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 0;
                        unsuccessResponse.message = "No agent found.";
                        successResponse.status = "Unsuccess";
                        return StatusCode(406, unsuccessResponse);
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
    }
}