using System;
using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Subscriptions;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscriptionController : ControllerBase
    {
        private readonly SubscriptionsBusiness SubscriptionsBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly UsersBusiness usersBusiness;
        private readonly IMapper _mapper;
        private readonly SalesAgentBusiness _salesAgentBusiness;
        private readonly EFSubscriptions EFSubscriptions;
        private readonly EFSubscriptionMetadata EFSubscriptionMetadata;
        private readonly EFStudParentRepository _eFStudParentRepository;
        public SubscriptionController
        (
            IMapper mapper,
            SubscriptionsBusiness SubscriptionsBusiness,
            LessonBusiness LessonBusiness,
            UsersBusiness usersBusiness,
            SalesAgentBusiness salesAgentBusiness,
            EFSubscriptions EFSubscriptions,
            EFSubscriptionMetadata EFSubscriptionMetadata,
            EFStudParentRepository eFStudParentRepository
        )
        {
            this.SubscriptionsBusiness = SubscriptionsBusiness;
            this.LessonBusiness = LessonBusiness;
            _mapper = mapper;
            this.usersBusiness = usersBusiness;
            this._salesAgentBusiness = salesAgentBusiness;
            this.EFSubscriptions = EFSubscriptions;
            this.EFSubscriptionMetadata = EFSubscriptionMetadata;
            _eFStudParentRepository = eFStudParentRepository;
        }

        [HttpPost]
        public IActionResult Post([FromBody]NewSubscriptionsModel value)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            var salesAgent = _salesAgentBusiness.GetAgentByUserId(int.Parse(tc.Id));
            if (salesAgent == null)
            {
                unsuccessResponse.response_code = 1;
                unsuccessResponse.message = "No sales agent found";
                unsuccessResponse.status = "Unsuccess";
                return StatusCode(406, unsuccessResponse);
            }
            if (!salesAgent.IsActive)
            {
                unsuccessResponse.response_code = 1;
                unsuccessResponse.message = "Sales agent not active";
                unsuccessResponse.status = "Unsuccess";
                return StatusCode(406, unsuccessResponse);
            }
            try
            {
                if (ModelState.IsValid)
                {
                    if (
                        tc.RoleName.Contains(General.getRoleType("1")) ||
                        tc.RoleName.Contains(General.getRoleType("13")) || tc.RoleName.Contains(General.getRoleType("17"))
                    )
                    {
                        if (tc.RoleName.Contains(General.getRoleType("17")))
                        {
                            Subscriptions subscriptions = new Subscriptions();
                            foreach (int cid in value.userids)
                            {
                                if (string.IsNullOrEmpty(subscriptions.UserId))
                                    subscriptions.UserId += cid;
                                else
                                    subscriptions.UserId += "," + cid;
                            }
                            Subscriptions existSubscription = EFSubscriptions.GetById(b => b.SubscriptionMetadataId == value.subscriptionmetadataid);
                            subscriptions.SubscriptionMetadataId = value.subscriptionmetadataid;
                            Subscriptions Subscriptions = new Subscriptions();
                            if (existSubscription == null)
                            {
                                Subscriptions = SubscriptionsBusiness.Create(subscriptions, int.Parse(tc.Id.ToString()));
                            }
                            else
                            {
                                SubscriptionMetadata subscriptionMetadata = EFSubscriptionMetadata.GetById(b => b.Id == value.subscriptionmetadataid);
                                if (subscriptionMetadata != null)
                                {
                                    if (subscriptionMetadata.SalesAgentId != salesAgent.Id)
                                    {
                                        unsuccessResponse.response_code = 1;
                                        unsuccessResponse.message = "This purchase is not editable";
                                        unsuccessResponse.status = "Unsuccess";
                                        return StatusCode(406, unsuccessResponse);
                                    }
                                    else
                                    {
                                        Subscriptions = SubscriptionsBusiness.Update(subscriptions, int.Parse(tc.Id.ToString()));
                                    }
                                }
                                else
                                {
                                    unsuccessResponse.response_code = 1;
                                    unsuccessResponse.message = "No Subscription found";
                                    unsuccessResponse.status = "Unsuccess";
                                    return StatusCode(406, unsuccessResponse);
                                }
                            }

                            SubscriptionsModel subscriptionsModel = new SubscriptionsModel();
                            subscriptionsModel.id = Subscriptions.Id;
                            subscriptionsModel.subscriptionmetadataid = Subscriptions.SubscriptionMetadataId;
                            string[] uids = Subscriptions.UserId.Split(",");

                            List<UserDetails> userDetails = new List<UserDetails>();
                            for (int i = 0; i < uids.Length; i++)
                            {
                                User user = usersBusiness.GetUserbyId(long.Parse(uids[i]));
                                UserDetails userDto = new UserDetails
                                {
                                    Id = user.Id,
                                    Username = user.Username,
                                    FullName = user.FullName,
                                    Email = user.Email,
                                    Bio = user.Bio,
                                    profilepicurl = user.ProfilePicUrl
                                };
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
                                userDetails.Add(userDto);
                            }

                            subscriptionsModel.userids = userDetails;
                            successResponse.data = subscriptionsModel;


                            successResponse.response_code = 0;
                            successResponse.message = "Subscriptions Created";
                            successResponse.status = "Success";
                            return StatusCode(200, successResponse);
                        }
                        else
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "No sales agent found";
                            unsuccessResponse.status = "Unsuccess";
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

        [HttpPost("SaveParent")]
        public IActionResult SaveParent([FromBody]StudParentDTO dto)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                foreach (var p in dto.parentsids)
                {
                    var exist = _eFStudParentRepository.GetById(b => b.StudentId == dto.studentids && b.ParentId == p && b.IsDeleted != true);
                    if (exist == null)
                    {
                        StudParent studParent = new StudParent();
                        studParent.StudentId = dto.studentids;
                        studParent.ParentId = p;
                        studParent.CreatorUserId = int.Parse(tc.Id.ToString());
                        studParent.CreationTime = DateTime.UtcNow.ToString();
                        _eFStudParentRepository.Insert(studParent);
                    }
                }
                successResponse.response_code = 0;
                successResponse.message = "Parent Assigned";
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