using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Helpers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.AddtionalServices;
using Trainning24.BL.ViewModels.CourseDefination;
using Trainning24.BL.ViewModels.Deposit;
using Trainning24.BL.ViewModels.SubscriptionMetadata;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class SubscriptionMetadaController : ControllerBase
    {
        private readonly SubscriptionMetadataBusiness SubscriptionMetadataBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly CourseDefinationBusiness CourseDefinationBusiness;
        private readonly GradeBusiness GradeBusiness;
        private readonly CourseBusiness CourseBusiness;
        private readonly DepositBusiness DepositBusiness;
        private readonly AddtionalServicesBusiness _addtionalServicesBusiness;
        private readonly SalesAgentBusiness _salesAgentBusiness;

        private readonly IMapper _mapper;

        public SubscriptionMetadaController
        (
            IMapper mapper,
            SubscriptionMetadataBusiness SubscriptionMetadataBusiness,
            LessonBusiness LessonBusiness,
            GradeBusiness GradeBusiness,
            CourseBusiness CourseBusiness,
            DepositBusiness DepositBusiness,
            CourseDefinationBusiness CourseDefinationBusiness,
            AddtionalServicesBusiness addtionalServicesBusiness,
            SalesAgentBusiness salesAgentBusiness
        )
        {
            this.SubscriptionMetadataBusiness = SubscriptionMetadataBusiness;
            this.LessonBusiness = LessonBusiness;
            this.CourseBusiness = CourseBusiness;
            this.GradeBusiness = GradeBusiness;
            this.DepositBusiness = DepositBusiness;
            this.CourseDefinationBusiness = CourseDefinationBusiness;
            _addtionalServicesBusiness = addtionalServicesBusiness;
            _mapper = mapper;
            this._salesAgentBusiness = salesAgentBusiness;
        }

        [HttpPost]
        public IActionResult Post([FromBody]InputSubscriptionMetadata value)
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
                            SubscriptionMetadata metadata = new SubscriptionMetadata();

                            if (value.courseids != null)
                            {
                                foreach (int cid in value.courseids)
                                {
                                    if (string.IsNullOrEmpty(metadata.CourseId))
                                        metadata.CourseId += cid;
                                    else
                                        metadata.CourseId += "," + cid;
                                }
                            }

                            if (value.serviceids != null)
                            {
                                foreach (int sid in value.serviceids)
                                {
                                    if (string.IsNullOrEmpty(metadata.ServiceId))
                                        metadata.ServiceId += sid;
                                    else
                                        metadata.ServiceId += "," + sid;
                                }
                            }
                            metadata.DiscountPackageId = value.discountpackageid;
                            metadata.EnrollmentFromDate = value.enrollmentfromdate;
                            metadata.EnrollmentToDate = value.enrollmenttodate;
                            metadata.SalesAgentId = salesAgent.Id;
                            metadata.SubscriptionTypeId = value.subscriptiontypeid;
                            metadata.NoOfMonths = value.noofmonths;
                            metadata.PackageId = value.packageid;

                            metadata.Status = "Pending";

                            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                            string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
                            string numbers = "1234567890";

                            string characters = numbers;
                            if ("1" == "1")
                            {
                                characters += alphabets + small_alphabets + numbers;
                            }

                            string otp = string.Empty;
                            for (int i = 0; i < 5; i++)
                            {
                                string character = string.Empty;
                                do
                                {
                                    int index = new Random().Next(0, characters.Length);
                                    character = characters.ToCharArray()[index].ToString();
                                } while (otp.IndexOf(character) != -1);
                                otp += character;
                            }



                            metadata.PurchageId = Crypto.Hash(otp, "MD5");

                            if (value.id == 0)
                            {
                                SubscriptionMetadata subscriptionMetadata = SubscriptionMetadataBusiness.Create(metadata, int.Parse(tc.Id.ToString()));

                                SubscriptionMetadataModel subscriptionMetadataModel = new SubscriptionMetadataModel();
                                subscriptionMetadataModel.id = subscriptionMetadata.Id;
                                string[] cids = new string[] { };
                                if (subscriptionMetadata.CourseId != null)
                                {
                                    cids = subscriptionMetadata.CourseId.Split(",");
                                }

                                string[] sids = new string[] { };
                                if (subscriptionMetadata.ServiceId != null)
                                {
                                    sids = subscriptionMetadata.ServiceId.Split(",");
                                }

                                List<ResponseCourseDefination> ResponseCourseDefinationList = new List<ResponseCourseDefination>();

                                for (int i = 0; i < cids.Length; i++)
                                {
                                    CourseDefination courseDefination = CourseDefinationBusiness.getCourseDefinationById(int.Parse(cids[i]));
                                    ResponseCourseDefination responseCourseDefination = new ResponseCourseDefination();
                                    responseCourseDefination.Id = courseDefination.Id;
                                    responseCourseDefination.BasePrice = courseDefination.BasePrice;
                                    responseCourseDefination.CourseId = courseDefination.CourseId;
                                    responseCourseDefination.CourseName = CourseBusiness.getCourseById(courseDefination.CourseId).Name;
                                    responseCourseDefination.GradeId = courseDefination.GradeId;
                                    Grade grade = GradeBusiness.getGradeById(courseDefination.GradeId);
                                    if (grade != null)
                                        responseCourseDefination.GradeName = grade.Name;
                                    responseCourseDefination.Subject = courseDefination.Subject;
                                    ResponseCourseDefinationList.Add(responseCourseDefination);
                                }
                                List<int> serviceslst = new List<int>();
                                for (int i = 0; i < sids.Length; i++)
                                {
                                    var data = int.Parse(sids[i]);
                                    serviceslst.Add(data);
                                }
                                //List<AddtionalServicesDto> serviceslst = new List<AddtionalServicesDto>();
                                //for (int i = 0; i < sids.Length; i++)
                                //{
                                //    AddtionalServices services = _addtionalServicesBusiness.GetById(int.Parse(sids[i]));
                                //    AddtionalServicesDto serviceobj = new AddtionalServicesDto();
                                //    serviceobj.id = services.Id;
                                //    serviceobj.name = services.Name;
                                //    serviceobj.price = services.Price;
                                //    serviceobj.createdDate = services.CreationTime;
                                //    serviceobj.createdBy = services.CreatorUserId ?? 0;
                                //    serviceslst.Add(serviceobj);
                                //}

                                subscriptionMetadataModel.courseids = ResponseCourseDefinationList;
                                subscriptionMetadataModel.discountpackageid = subscriptionMetadata.DiscountPackageId;
                                subscriptionMetadataModel.enrollmentfromdate = subscriptionMetadata.EnrollmentFromDate;
                                subscriptionMetadataModel.enrollmenttodate = subscriptionMetadata.EnrollmentToDate;
                                subscriptionMetadataModel.salesagentid = subscriptionMetadata.SalesAgentId;
                                subscriptionMetadataModel.subscriptiontypeid = subscriptionMetadata.SubscriptionTypeId;
                                subscriptionMetadataModel.noofmonths = subscriptionMetadata.NoOfMonths;
                                subscriptionMetadataModel.packageid = subscriptionMetadata.PackageId;
                                subscriptionMetadataModel.serviceids = serviceslst;
                                successResponse.data = subscriptionMetadataModel;
                            }
                            else
                            {
                                SubscriptionMetadata existSubscriptionMetadata = SubscriptionMetadataBusiness.GetMetadataByid(value.id);
                                if (existSubscriptionMetadata != null)
                                {
                                    if (existSubscriptionMetadata.SalesAgentId != salesAgent.Id)
                                    {
                                        unsuccessResponse.response_code = 1;
                                        unsuccessResponse.message = "This purchase is not editable";
                                        unsuccessResponse.status = "Unsuccess";
                                        return StatusCode(406, unsuccessResponse);
                                    }
                                }
                                metadata.Id = value.id;
                                SubscriptionMetadata subscriptionMetadata = SubscriptionMetadataBusiness.Update(metadata, int.Parse(tc.Id.ToString()));

                                SubscriptionMetadataModel subscriptionMetadataModel = new SubscriptionMetadataModel();

                                subscriptionMetadataModel.id = subscriptionMetadata.Id;

                                string[] cids = new string[] { };
                                if (subscriptionMetadata.CourseId != null)
                                {
                                    cids = subscriptionMetadata.CourseId.Split(",");
                                }

                                string[] sids = new string[] { };
                                if (subscriptionMetadata.ServiceId != null)
                                {
                                    sids = subscriptionMetadata.ServiceId.Split(",");
                                }

                                List<ResponseCourseDefination> ResponseCourseDefinationList = new List<ResponseCourseDefination>();

                                for (int i = 0; i < cids.Length; i++)
                                {
                                    CourseDefination courseDefination = CourseDefinationBusiness.getCourseDefinationById(int.Parse(cids[i]));
                                    ResponseCourseDefination responseCourseDefination = new ResponseCourseDefination();
                                    responseCourseDefination.Id = courseDefination.Id;
                                    responseCourseDefination.BasePrice = courseDefination.BasePrice;
                                    responseCourseDefination.CourseId = courseDefination.CourseId;
                                    responseCourseDefination.CourseName = CourseBusiness.getCourseById(courseDefination.CourseId).Name;
                                    responseCourseDefination.GradeId = courseDefination.GradeId;
                                    Grade grade = GradeBusiness.getGradeById(courseDefination.GradeId);
                                    if (grade != null)
                                        responseCourseDefination.GradeName = grade.Name;
                                    responseCourseDefination.Subject = courseDefination.Subject;
                                    ResponseCourseDefinationList.Add(responseCourseDefination);
                                }

                                List<int> serviceslst = new List<int>();
                                for (int i = 0; i < sids.Length; i++)
                                {
                                    var data = int.Parse(sids[i]);
                                    serviceslst.Add(data);
                                }
                                //List<AddtionalServicesDto> serviceslst = new List<AddtionalServicesDto>();
                                //for (int i = 0; i < sids.Length; i++)
                                //{
                                //    AddtionalServices services = _addtionalServicesBusiness.GetById(int.Parse(sids[i]));
                                //    AddtionalServicesDto serviceobj = new AddtionalServicesDto();
                                //    serviceobj.id = services.Id;
                                //    serviceobj.name = services.Name;
                                //    serviceobj.price = services.Price;
                                //    serviceobj.createdDate = services.CreationTime;
                                //    serviceobj.createdBy = services.CreatorUserId ?? 0;
                                //    serviceslst.Add(serviceobj);
                                //}

                                subscriptionMetadataModel.courseids = ResponseCourseDefinationList;
                                subscriptionMetadataModel.discountpackageid = subscriptionMetadata.DiscountPackageId;
                                subscriptionMetadataModel.enrollmentfromdate = subscriptionMetadata.EnrollmentFromDate;
                                subscriptionMetadataModel.enrollmenttodate = subscriptionMetadata.EnrollmentToDate;
                                subscriptionMetadataModel.salesagentid = subscriptionMetadata.SalesAgentId;
                                subscriptionMetadataModel.subscriptiontypeid = subscriptionMetadata.SubscriptionTypeId;
                                subscriptionMetadataModel.noofmonths = subscriptionMetadata.NoOfMonths;
                                subscriptionMetadataModel.packageid = subscriptionMetadata.PackageId;
                                subscriptionMetadataModel.serviceids = serviceslst;

                                successResponse.data = subscriptionMetadataModel;
                            }

                            successResponse.response_code = 0;
                            successResponse.message = "SubscriptionMetadata Created";
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


        [HttpPost("PurchaseSummary")]
        public async Task<IActionResult> PurchaseSummary(PurchaseListFilterModel purchaseListFilterModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                AboutAllPurchase allPurchase = new AboutAllPurchase();
                if (tc.RoleName.Contains(General.getRoleType("1")) || tc.RoleName.Contains(General.getRoleType("13")))
                {
                    allPurchase = await SubscriptionMetadataBusiness.PurchaseSummary(purchaseListFilterModel);
                }
                else if (tc.RoleName.Contains(General.getRoleType("17")))
                {
                    var salesAgent = _salesAgentBusiness.GetAgentByUserId(int.Parse(tc.Id));
                    if (salesAgent == null)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "No sales agent found";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(401, unsuccessResponse);
                    }
                    allPurchase = await SubscriptionMetadataBusiness.PurchaseSummarySalesAgent(purchaseListFilterModel, salesAgent.Id);
                }
                successResponse.data = allPurchase;
                successResponse.response_code = 0;
                successResponse.message = "Purchase summary";
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
