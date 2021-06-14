using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.AddtionalServices;
using Trainning24.BL.ViewModels.BundleCourse;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.CourseDefination;
using Trainning24.BL.ViewModels.Deposit;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.IndividualDetails;
using Trainning24.BL.ViewModels.SubscriptionMetadata;
using Trainning24.BL.ViewModels.Subscriptions;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class PurchaseDetailsController : ControllerBase
    {
        private readonly IndividualDetailsBusiness IndividualDetailsBusiness;
        private readonly SubscriptionMetadataBusiness SubscriptionMetadataBusiness;
        private readonly FilesBusiness FilesBusiness;
        private readonly SalesAgentBusiness SalesAgentBusiness;
        private readonly CourseDefinationBusiness CourseDefinationBusiness;
        private readonly SubscriptionsBusiness SubscriptionsBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly DepositBusiness DepositBusiness;
        private readonly DiscountBusiness DiscountBusiness;
        private readonly StudentCourseBusiness StudentCourseBusiness;
        private readonly GradeBusiness GradeBusiness;
        private readonly CourseBusiness CourseBusiness;
        private IHostingEnvironment hostingEnvironment;
        private readonly UsersBusiness usersBusiness;
        private readonly PackageBusiness _packageBusiness;
        private readonly AddtionalServicesBusiness _addtionalServicesBusiness;
        private readonly PackageCourseBusiness _packageCourseBusiness;
        private readonly SalesAgentBusiness _salesAgentBusiness;
        private readonly SalesTaxBusiness _salesTaxBusiness;

        //private IConverter _converter;

        public PurchaseDetailsController
        (
            IHostingEnvironment hostingEnvironment,
            SubscriptionMetadataBusiness SubscriptionMetadataBusiness,
            FilesBusiness FilesBusiness,
            SubscriptionsBusiness SubscriptionsBusiness,
            SalesAgentBusiness SalesAgentBusiness,
            StudentCourseBusiness StudentCourseBusiness,
            LessonBusiness LessonBusiness,
            DiscountBusiness DiscountBusiness,
            GradeBusiness GradeBusiness,
            CourseBusiness CourseBusiness,
            DepositBusiness DepositBusiness,
            CourseDefinationBusiness CourseDefinationBusiness,
            IndividualDetailsBusiness IndividualDetailsBusiness,
            UsersBusiness usersBusiness,
            PackageBusiness packageBusiness,
            AddtionalServicesBusiness addtionalServicesBusiness,
            PackageCourseBusiness packageCourseBusiness,
            SalesAgentBusiness salesAgentBusiness,
            SalesTaxBusiness salesTaxBusiness
        //,
        //IConverter converter
        )
        {
            this.DiscountBusiness = DiscountBusiness;
            this.SalesAgentBusiness = SalesAgentBusiness;
            this.FilesBusiness = FilesBusiness;
            this.StudentCourseBusiness = StudentCourseBusiness;
            this.LessonBusiness = LessonBusiness;
            this.hostingEnvironment = hostingEnvironment;
            this.SubscriptionMetadataBusiness = SubscriptionMetadataBusiness;
            this.SubscriptionsBusiness = SubscriptionsBusiness;
            this.GradeBusiness = GradeBusiness;
            this.DepositBusiness = DepositBusiness;
            this.CourseBusiness = CourseBusiness;
            this.CourseDefinationBusiness = CourseDefinationBusiness;
            this.IndividualDetailsBusiness = IndividualDetailsBusiness;
            this.usersBusiness = usersBusiness;
            _packageBusiness = packageBusiness;
            _addtionalServicesBusiness = addtionalServicesBusiness;
            _packageCourseBusiness = packageCourseBusiness;
            _salesAgentBusiness = salesAgentBusiness;
            _salesTaxBusiness = salesTaxBusiness;
            //_converter = converter;
        }


        [HttpPost("UploadSignedReceipt")]
        public async Task<IActionResult> UploadSignedReceiptAsync()
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
                if (tc.RoleName.Contains(General.getRoleType("17")))
                {
                    UploadSignedReceiptModel usurm = new UploadSignedReceiptModel();
                    usurm.metadataid = long.Parse(Request.Form["metadataid"]);

                    SubscriptionMetadata subscriptionMetadata = SubscriptionMetadataBusiness.GetMetadataByid(usurm.metadataid);
                    if (subscriptionMetadata.SalesAgentId != salesAgent.Id)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "This purchase is not editable";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(406, unsuccessResponse);
                    }

                    string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                    var credential = GoogleCredential.FromFile(jsonPath);
                    var storage = StorageClient.Create(credential);
                    string mediaLink = "";

                    //string fileName = "";
                    //IFormFile file = null;
                    //if (Request.Form.Files.Count != 0)
                    //    file = Request.Form.Files[0];

                    //var imageAcl = PredefinedObjectAcl.PublicRead;

                    //fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    //var ext = fileName.Substring(fileName.LastIndexOf("."));
                    //var extension = ext.ToLower();
                    //fileName = fileName.Split(".")[0] + "_" + IndividualDetailsBusiness.ReturnCode() + extension;


                    //var imageObject = await storage.UploadObjectAsync(
                    //    bucket: "t24-primary-pdf-storage",
                    //    objectName: fileName,
                    //    contentType: file.ContentType,
                    //    source: file.OpenReadStream(),
                    //    options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                    //);
                    string BucketName = General.getBucketName("1");
                    if (!string.IsNullOrEmpty(Request.Form["filename"].ToString()))
                    {
                        var storageObject = storage.GetObject(BucketName, Request.Form["filename"].ToString());
                        mediaLink = storageObject.MediaLink;
                    }

                    AddFilesModel FilesModel = new AddFilesModel();
                    FilesModel.Url = mediaLink;
                    FilesModel.Name = Request.Form["filename"].ToString();
                    FilesModel.FileName = Request.Form["filename"].ToString();
                    FilesModel.FileTypeId = 1;
                    Files newFiles = FilesBusiness.Create(FilesModel, int.Parse(tc.Id));
                    IndividualDetailsBusiness.CreatePurchageUpload(new PurchageUpload { PdfFile = newFiles.Id, PurchageId = subscriptionMetadata.PurchageId }, int.Parse(tc.Id));

                    subscriptionMetadata.Status = "Completed";
                    subscriptionMetadata.Tax = _salesTaxBusiness.Get().Tax;
                    subscriptionMetadata = SubscriptionMetadataBusiness.Update(subscriptionMetadata, int.Parse(tc.Id.ToString()));
                    string[] cids = new string[] { };
                    if (subscriptionMetadata.CourseId != null)
                    {
                        cids = subscriptionMetadata.CourseId.Split(",");
                    }
                    Subscriptions Subscriptions = SubscriptionsBusiness.GetSubscriptionsByMetadataId(usurm.metadataid);
                    string[] uids = Subscriptions.UserId.Split(",");
                    for (int i = 0; i < cids.Length; i++)
                    {
                        for (int j = 0; j < uids.Length; j++)
                        {
                            CourseDefination getById = CourseDefinationBusiness.getCourseDefinationById(long.Parse(cids[i]));
                            if (getById != null)
                            {
                                AddStudentCourseModel StudentCourseModel = new AddStudentCourseModel
                                {
                                    courseid = getById.CourseId,
                                    userid = long.Parse(uids[j]),
                                    startdate = DateTime.Parse(subscriptionMetadata.EnrollmentFromDate),
                                    enddate = DateTime.Parse(subscriptionMetadata.EnrollmentToDate)
                                };
                                UserCourse newStudentCourse = StudentCourseBusiness.Create(StudentCourseModel, int.Parse(tc.Id));
                            }
                        }
                    }
                    string[] sids = new string[] { };
                    if (!string.IsNullOrEmpty(subscriptionMetadata.ServiceId))
                    {
                        for (int j = 0; j < uids.Length; j++)
                        {
                            UpdateUserServiceDTO userAdditionalService = new UpdateUserServiceDTO();
                            userAdditionalService.Id = long.Parse(uids[j]);
                            sids = subscriptionMetadata.ServiceId.Split(",");
                            for (int i = 0; i < sids.Length; i++)
                            {
                                if (sids[i] == "1")
                                {
                                    userAdditionalService.is_assignment_authorized = true;
                                }
                                if (sids[i] == "2")
                                {
                                    userAdditionalService.is_discussion_authorized = true;
                                }
                                if (sids[i] == "3")
                                {
                                    userAdditionalService.is_library_authorized = true;
                                }
                            }
                            User updateUserService = usersBusiness.UpdateAdditionalService(userAdditionalService);
                        }
                    }

                    if (subscriptionMetadata.PackageId != 0)
                    {
                        List<PackageCourse> packageCourses = _packageCourseBusiness.GetPackageCourseListByPackageId(subscriptionMetadata.PackageId);
                        if (packageCourses.Count > 0)
                        {
                            foreach (var pc in packageCourses)
                            {
                                for (int j = 0; j < uids.Length; j++)
                                {
                                    AddStudentCourseModel StudentCourseModel = new AddStudentCourseModel
                                    {
                                        courseid = pc.CourseId,
                                        userid = long.Parse(uids[j]),
                                        startdate = DateTime.Parse(subscriptionMetadata.EnrollmentFromDate),
                                        enddate = DateTime.Parse(subscriptionMetadata.EnrollmentToDate),
                                    };
                                    UserCourse newStudentCourse = StudentCourseBusiness.Create(StudentCourseModel, int.Parse(tc.Id));
                                }
                            }
                        }
                    }

                    usurm.status = "Completed";
                    successResponse.data = usurm;
                    successResponse.response_code = 0;
                    successResponse.message = "UploadSignedReceipt";
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
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("GetgenerateSignableReceipt")]
        public IActionResult GetgenerateSignableReceipt(PriceSummaryModel priceSummaryModel)
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
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);

            try
            {
                if (tc.RoleName.Contains(General.getRoleType("17")))
                {
                    decimal totalbaseprice = 0;
                    Subscriptions Subscriptions = SubscriptionsBusiness.GetSubscriptionsByMetadataId(priceSummaryModel.metadataid);
                    SubscriptionMetadata subscriptionMetadata = SubscriptionMetadataBusiness.GetMetadataByid(priceSummaryModel.metadataid);
                    if (subscriptionMetadata.SalesAgentId != salesAgent.Id)
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "This purchase is not editable";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(406, unsuccessResponse);
                    }
                    string[] uids = Subscriptions.UserId.Split(",");
                    string totalsubscriptions = uids.Length.ToString();

                    string students = "";

                    if (!totalsubscriptions.Equals(priceSummaryModel.totalsubscriptions))
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "something worng with totalsubscriptions";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(422, unsuccessResponse);
                    }


                    for (int i = 0; i < uids.Length; i++)
                    {
                        User user = usersBusiness.GetUserbyId(long.Parse(uids[i]));


                        if (user != null)
                        {
                            if (string.IsNullOrEmpty(students))
                            {
                                students = user.FullName;
                            }
                            else
                            {
                                students = "," + user.FullName;
                            }
                        }
                    }

                    if (subscriptionMetadata != null)
                    {
                        AgentAccountSummary agentAccountSummary = new AgentAccountSummary();
                        agentAccountSummary = DepositBusiness.GetAgentSummaryDetails(subscriptionMetadata.SalesAgentId);
                        if (agentAccountSummary.currentbalance < decimal.Parse(priceSummaryModel.finalprice))
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Selected agent has not enough balance.";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(422, unsuccessResponse);
                        }
                    }
                    string[] coursedefinationids = new string[] { };
                    string[] additionalserviceids = new string[] { };
                    if (!string.IsNullOrEmpty(subscriptionMetadata.CourseId))
                    {
                        coursedefinationids = subscriptionMetadata.CourseId.Split(",");
                    }

                    if (!string.IsNullOrEmpty(subscriptionMetadata.ServiceId))
                    {
                        additionalserviceids = subscriptionMetadata.ServiceId.Split(",");
                    }
                    List<ResponseCourseDefination> ResponseCourseDefinationList = new List<ResponseCourseDefination>();
                    Grade grade = new Grade();
                    ResponseCourseDefination responseCourseDefination = new ResponseCourseDefination();
                    string coursename = "";
                    string coursegradename = "";
                    for (int i = 0; i < coursedefinationids.Length; i++)
                    {
                        CourseDefination courseDefination = CourseDefinationBusiness.getCourseDefinationById(int.Parse(coursedefinationids[i]));
                        responseCourseDefination.Id = courseDefination.Id;
                        responseCourseDefination.BasePrice = courseDefination.BasePrice;
                        totalbaseprice += decimal.Parse(responseCourseDefination.BasePrice);
                        responseCourseDefination.CourseId = courseDefination.CourseId;
                        responseCourseDefination.CourseName = CourseBusiness.getCourseById(courseDefination.CourseId).Name;
                        if (string.IsNullOrEmpty(coursename))
                        {
                            coursename = responseCourseDefination.CourseName;
                        }
                        else
                        {
                            coursename = coursename + "," + responseCourseDefination.CourseName;
                        }
                        responseCourseDefination.GradeId = courseDefination.GradeId;
                        grade = GradeBusiness.getGradeById(courseDefination.GradeId);
                        if (grade != null)
                        {
                            responseCourseDefination.GradeName = grade.Name;
                            if (string.IsNullOrEmpty(coursegradename))
                            {
                                coursegradename = responseCourseDefination.GradeName;
                            }
                            else
                            {
                                coursegradename = coursegradename + "," + responseCourseDefination.GradeName;
                            }
                        }
                        responseCourseDefination.Subject = courseDefination.Subject;
                        ResponseCourseDefinationList.Add(responseCourseDefination);
                    }

                    if (subscriptionMetadata.PackageId != 0)
                    {
                        Package package = _packageBusiness.GetById(subscriptionMetadata.PackageId);
                        if (package != null)
                        {
                            totalbaseprice += decimal.Parse(package.Price);
                        }
                    }

                    for (int i = 0; i < additionalserviceids.Length; i++)
                    {
                        AddtionalServices addtionalServices = _addtionalServicesBusiness.GetById(int.Parse(additionalserviceids[i]));
                        if (addtionalServices != null)
                        {
                            totalbaseprice += decimal.Parse(addtionalServices.Price);
                        }
                    }

                    decimal ftotalbaseprice = totalbaseprice * decimal.Parse(totalsubscriptions);
                    int ftotalsubscription = 0;

                    if (subscriptionMetadata.NoOfMonths > 0)
                    {
                        ftotalbaseprice = ftotalbaseprice * subscriptionMetadata.NoOfMonths;
                        totalbaseprice = ftotalbaseprice / decimal.Parse(totalsubscriptions);
                    }

                    if (!ftotalbaseprice.ToString().Equals(priceSummaryModel.totalbaseprice))
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "something worng with totalbaseprice";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(422, unsuccessResponse);
                    }

                    decimal finalprice = 0;

                    Discount discount = new Discount();
                    if (subscriptionMetadata.DiscountPackageId != 0)
                    {
                        discount = DiscountBusiness.getDiscountById(subscriptionMetadata.DiscountPackageId);
                        ftotalsubscription = (int.Parse(totalsubscriptions) - discount.FreeSubscriptions);

                        if (ftotalsubscription != 0)
                        {
                            ftotalbaseprice = totalbaseprice * ftotalsubscription;

                            finalprice = ftotalbaseprice - ftotalbaseprice * (ftotalsubscription * discount.OffSubscriptions) / 100; // this is discount for discounted off subscriptions

                            finalprice = finalprice - (finalprice * discount.OffTotalPrice) / 100; // this is discount for discounted off total price 

                        }
                        else
                        {
                            finalprice = totalbaseprice * (ftotalsubscription * discount.OffTotalPrice) / 100;
                        }
                    }
                    else
                    {
                        finalprice = ftotalbaseprice;
                    }

                    if (finalprice != decimal.Parse(priceSummaryModel.finalprice))
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "something worng with finalprice";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(422, unsuccessResponse);
                    }

                    subscriptionMetadata.Status = "Waiting";
                    SubscriptionMetadata updatedsubscriptionMetadata = SubscriptionMetadataBusiness.Update(subscriptionMetadata, int.Parse(tc.Id.ToString()));
                    priceSummaryModel.status = "Waiting";

                    //PDF Generation
                    //ReceiptModel receiptModel = new ReceiptModel();
                    //receiptModel.Grade = coursegradename;
                    //receiptModel.Course = coursename;

                    //if (!string.IsNullOrEmpty(discount.PackageName))
                    //{
                    //    receiptModel.Package = discount.PackageName;
                    //    receiptModel.PackageDiscount = (ftotalbaseprice * (discount.OffSubscriptions * ftotalsubscription) / 100).ToString();
                    //}
                    //else
                    //{
                    //    receiptModel.Package = "NA";
                    //    receiptModel.PackageDiscount = "NA";
                    //}

                    //receiptModel.SalesPartner = !string.IsNullOrEmpty(agentAccountSummary.agentname) ? agentAccountSummary.agentname : "NA";

                    //receiptModel.StartDate = Convert.ToDateTime(subscriptionMetadata.EnrollmentFromDate, CultureInfo.InvariantCulture).Date.ToString("dd-MM-yyyy");
                    //receiptModel.EndDate = Convert.ToDateTime(subscriptionMetadata.EnrollmentToDate, CultureInfo.InvariantCulture).ToString("dd-MM-yyyy");

                    //receiptModel.AdditionalGradePurchased = "NA";
                    //receiptModel.Tax = "NA";
                    //receiptModel.Paid = "NA";
                    //receiptModel.Total = finalprice.ToString();
                    //receiptModel.students = students;

                    ////var htmlToPdf = new IronPdf.HtmlToPdf();
                    ////var html = TemplateGenerator.GetHTMLString(receiptModel);
                    ////var pdf = htmlToPdf.RenderHtmlAsPdf(html);
                    ////pdf.SaveAs(Path.Combine(Directory.GetCurrentDirectory(), "Receipt.Pdf"));

                    //PdfGenerateConfig config = new PdfGenerateConfig();
                    //config.PageSize = PageSize.A4;
                    //config.SetMargins(0);
                    //config.MarginTop = 30;
                    //config.MarginBottom = 30;


                    //string html = TemplateGenerator.GetHTMLString(receiptModel);
                    //PdfDocument pdf = PdfGenerator.GeneratePdf(html, config);
                    //pdf.Save(Path.Combine(Directory.GetCurrentDirectory(), "Receipt.Pdf"));

                    //HtmlConverter.ConvertToPdf(
                    //                              new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "invoice.html")),
                    //                              new FileInfo(Path.Combine(Directory.GetCurrentDirectory(), "Receipt.Pdf"))
                    //                          );

                    //PdfCreator.CreateFromHtml(html);

                    //var imageAcl = PredefinedObjectAcl.PublicRead;
                    //FileStream fileStream = null;
                    //fileStream = new FileStream(Path.Combine(Directory.GetCurrentDirectory(), "Receipt.Pdf"), FileMode.Open);

                    //string fileName = fileStream.Name.Split("\\").Last();
                    //var ext = fileName.Substring(fileName.LastIndexOf("."));
                    //var extension = ext.ToLower();
                    //fileName = fileName.Split(".")[0] + "_" + IndividualDetailsBusiness.ReturnCode() + extension;

                    //var imageObject = await storage.UploadObjectAsync(
                    //                    bucket: "t24-primary-pdf-storage",
                    //                    objectName: fileName,
                    //                    contentType: "application/pdf",
                    //                    source: fileStream,
                    //                    options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                    //                );

                    //string mediaLink = imageObject.MediaLink;

                    //AddFilesModel FilesModel = new AddFilesModel();
                    //FilesModel.Url = mediaLink;
                    //FilesModel.Name = fileName;
                    //FilesModel.FileName = fileName;
                    //FilesModel.FileTypeId = 1;
                    //FilesModel.FileSize = fileStream.Length;
                    //Files newFiles = FilesBusiness.Create(FilesModel, int.Parse(tc.Id));
                    //priceSummaryModel.receipturl = newFiles.Url;
                    //fileStream.Dispose();
                    //fileStream.Close();

                    //IndividualDetailsBusiness.CreatePurchagePdf(new PurchagePdf { PdfFile = newFiles.Id, PurchageId = updatedsubscriptionMetadata.PurchageId }, int.Parse(tc.Id));

                    successResponse.data = priceSummaryModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Everything good";
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
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpGet("GetPurchaseListTest")]
        public IActionResult GetPurchaseList(string search)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                List<SubscriptionMetadata> subscriptionMetadatas = SubscriptionMetadataBusiness.GetPurchaseList(search);
                List<PurchaseList> purchaseList = new List<PurchaseList>();

                foreach (var singlepurchase in subscriptionMetadatas)
                {
                    PurchaseList purchase = new PurchaseList();
                    purchase.id = singlepurchase.Id;
                    purchase.purchasedate = singlepurchase.CreationTime;
                    purchase.purchaseid = singlepurchase.PurchageId;
                    purchase.status = singlepurchase.Status;
                    purchaseList.Add(purchase);
                }

                successResponse.data = purchaseList;
                successResponse.response_code = 0;
                successResponse.message = "PurchaseList";
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


        [HttpPost("GetPurchaseList")]
        public async Task<IActionResult> GetPurchaseListTest(PurchaseListFilterModel purchaseListFilterModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                List<SubscriptionMetadata> subscriptionMetadatas = new List<SubscriptionMetadata>();
                if (tc.RoleName.Contains(General.getRoleType("1")) || tc.RoleName.Contains(General.getRoleType("13")))
                {
                    subscriptionMetadatas = await SubscriptionMetadataBusiness.GetPurchaseListTest(purchaseListFilterModel);
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
                    subscriptionMetadatas = await SubscriptionMetadataBusiness.GetPurchaseListSalesAgent(purchaseListFilterModel, salesAgent.Id);
                }

                List<PurchaseList> purchaseList = new List<PurchaseList>();

                foreach (var singlepurchase in subscriptionMetadatas)
                {
                    PurchaseList purchase = new PurchaseList();
                    purchase.id = singlepurchase.Id;
                    purchase.purchasedate = singlepurchase.CreationTime;
                    purchase.purchaseid = singlepurchase.PurchageId;
                    purchase.status = singlepurchase.Status;
                    purchase.lastmodifieddate = singlepurchase.LastModificationTime;
                    purchaseList.Add(purchase);
                }

                successResponse.data = purchaseList;
                successResponse.response_code = 0;
                successResponse.message = "PurchaseList";
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



        [HttpGet("GetPurchaseDetail")]
        public IActionResult GetPurchaseDetail(long id)
        {

            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                PurchaseDetailResponse purchaseDetailResponse = new PurchaseDetailResponse();
                SubscriptionMetadataModel subscriptionMetadataModel = new SubscriptionMetadataModel();
                List<IndividualDetailsResponse> individualDetailsResponses = new List<IndividualDetailsResponse>();
                List<SchoolDetailsResponse> schoolDetailsResponses = new List<SchoolDetailsResponse>();

                SubscriptionMetadata subscriptionMetadata = SubscriptionMetadataBusiness.GetMetadataByid(id);


                if (subscriptionMetadata == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Record not found";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(422, unsuccessResponse);
                }

                subscriptionMetadataModel.id = subscriptionMetadata.Id;
                string[] cids = new string[] { };
                if (subscriptionMetadata.CourseId != null)
                {
                    cids = subscriptionMetadata.CourseId.Split(",");
                }

                if (subscriptionMetadata.PackageId != 0)
                {
                    subscriptionMetadataModel.packageid = subscriptionMetadata.PackageId;
                }
                List<int> sids = new List<int>();
                if (subscriptionMetadata.ServiceId != null)
                {
                    sids = subscriptionMetadata.ServiceId.Split(",").Select(int.Parse).ToList();
                    subscriptionMetadataModel.serviceids = sids;
                }

                List<ResponseCourseDefination> ResponseCourseDefinationList = new List<ResponseCourseDefination>();
                for (int i = 0; i < cids.Length; i++)
                {
                    ResponseCourseDefination responseCourseDefination = new ResponseCourseDefination();
                    CourseDefination courseDefination = CourseDefinationBusiness.getCourseDefinationById(int.Parse(cids[i]));
                    if (courseDefination != null)
                    {
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
                }
                subscriptionMetadataModel.courseids = ResponseCourseDefinationList;
                subscriptionMetadataModel.discountpackageid = subscriptionMetadata.DiscountPackageId;
                subscriptionMetadataModel.discountpackagename = DiscountBusiness.getDiscountById(subscriptionMetadata.DiscountPackageId) != null ? DiscountBusiness.getDiscountById(subscriptionMetadata.DiscountPackageId).PackageName : "";
                subscriptionMetadataModel.enrollmentfromdate = subscriptionMetadata.EnrollmentFromDate;
                subscriptionMetadataModel.enrollmenttodate = subscriptionMetadata.EnrollmentToDate;
                subscriptionMetadataModel.salesagentid = subscriptionMetadata.SalesAgentId;
                subscriptionMetadataModel.saleagentname = SalesAgentBusiness.getSalesAgentById(subscriptionMetadata.SalesAgentId) != null ? SalesAgentBusiness.getSalesAgentById(subscriptionMetadata.SalesAgentId).AgentName : "";
                subscriptionMetadataModel.subscriptiontypeid = subscriptionMetadata.SubscriptionTypeId;
                subscriptionMetadataModel.noofmonths = subscriptionMetadata.NoOfMonths;
                subscriptionMetadataModel.purchasedate = subscriptionMetadata.CreationTime;
                subscriptionMetadataModel.lastmodifieddate = subscriptionMetadata.LastModificationTime;
                purchaseDetailResponse.metadatadetails = subscriptionMetadataModel;


                Subscriptions Subscriptions = SubscriptionsBusiness.GetSubscriptionsByMetadataId(id);

                if (Subscriptions != null)
                {
                    SubscriptionsModel subscriptionsModel = new SubscriptionsModel();
                    subscriptionsModel.id = Subscriptions.Id;
                    subscriptionsModel.subscriptionmetadataid = Subscriptions.SubscriptionMetadataId;
                    string[] uids = new string[] { };

                    if (!string.IsNullOrEmpty(Subscriptions.UserId))
                    {
                        uids = Subscriptions.UserId.Split(",");
                    }

                    List<UserDetails> userDetails = new List<UserDetails>();
                    for (int i = 0; i < uids.Length; i++)
                    {
                        User user = usersBusiness.GetUserbyId(long.Parse(uids[i]));
                        List<StudParent> studParent = usersBusiness.GetStudParentById(long.Parse(uids[i]));
                        if (user != null)
                        {
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
                            if (studParent.Count > 0)
                            {
                                List<ParentUser> parentDetails = new List<ParentUser>();
                                foreach (var parent in studParent)
                                {
                                    User parentUser = usersBusiness.GetUserbyId(parent.ParentId);
                                    if (parentUser != null)
                                    {
                                        ParentUser parentUserDTO = new ParentUser
                                        {
                                            Id = parentUser.Id,
                                            Username = parentUser.Username,
                                            FullName = parentUser.FullName,
                                            Email = parentUser.Email,
                                            Bio = parentUser.Bio,
                                            profilepicurl = parentUser.ProfilePicUrl
                                        };

                                        List<Role> parentroles = usersBusiness.Role(parentUser);
                                        if (parentroles != null)
                                        {
                                            List<long> proleids = new List<long>();
                                            List<string> prolenames = new List<string>();
                                            foreach (var prole in parentroles)
                                            {
                                                proleids.Add(prole.Id);
                                                prolenames.Add(prole.Name);
                                            }
                                            parentUserDTO.Roles = proleids;
                                            parentUserDTO.RoleName = prolenames;
                                        }
                                        parentDetails.Add(parentUserDTO);
                                    }
                                }
                                userDto.parents = parentDetails;
                            }
                            userDetails.Add(userDto);
                        }
                    }

                    subscriptionsModel.userids = userDetails;
                    purchaseDetailResponse.Subscriptions = subscriptionsModel;
                }



                List<MetaDataDetail> metaDataDetailList = new List<MetaDataDetail>();
                metaDataDetailList = IndividualDetailsBusiness.GetMetadataDetail(id);
                foreach (var metadata in metaDataDetailList)
                {
                    if (metadata.DetailTypeId == 1)
                    {
                        IndividualDetails individual = IndividualDetailsBusiness.GetIndividualDetailById(metadata.DetailId);
                        if (individual != null)
                        {
                            IndividualDetailsResponse individualDetailsResponse = new IndividualDetailsResponse
                            {
                                id = individual.Id,
                                studentcode = individual.StudentCode,
                                studentname = individual.StudentName,
                                schoolname = individual.SchoolName,
                                fathernumber = individual.FatherNumber,
                                idcardnumber = individual.IdCardNumber,
                                permanentaddress = individual.PermanentAddress,
                                village = individual.Village,
                                provinceid = individual.ProvinceId,
                                cityid = individual.CityId,
                                phone = individual.Phone,
                                countryid = individual.CountryId,
                                refferedby = individual.RefferedBy,
                                email = individual.Email,
                                passportnumber = individual.PassportNumber,
                                nationality = individual.Nationality,
                                sexid = individual.SexId,
                                dateofbirth = individual.DateOfBirth,
                                placeofbirth = individual.PlaceOfBirth,
                                currentaddress = individual.CurrentAddress,
                                maritalstatusid = individual.MaritalStatusId,
                                remarks = individual.Remarks
                            };

                            if (!string.IsNullOrEmpty(individual.StudentTazkira))
                            {
                                List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                                string[] docid = individual.StudentTazkira.Split(",");
                                for (int i = 0; i < docid.Length; i++)
                                {
                                    DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                                    DocumentDetailsResponse document = new DocumentDetailsResponse
                                    {
                                        id = documentDetails.Id,
                                        documenturl = documentDetails.DocumentUrl,
                                        name = documentDetails.name
                                    };
                                    documentlist.Add(document);
                                }
                                individualDetailsResponse.studenttazkira = documentlist;
                            }

                            if (!string.IsNullOrEmpty(individual.ParentTazrika))
                            {
                                List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                                string[] docid = individual.ParentTazrika.Split(",");
                                for (int i = 0; i < docid.Length; i++)
                                {
                                    DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                                    DocumentDetailsResponse document = new DocumentDetailsResponse
                                    {
                                        id = documentDetails.Id,
                                        documenturl = documentDetails.DocumentUrl,
                                        name = documentDetails.name
                                    };
                                    documentlist.Add(document);
                                }
                                individualDetailsResponse.parenttazrika = documentlist;
                            }

                            if (!string.IsNullOrEmpty(individual.PreviousMarksheets))
                            {
                                List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                                string[] docid = individual.PreviousMarksheets.Split(",");
                                for (int i = 0; i < docid.Length; i++)
                                {
                                    DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                                    DocumentDetailsResponse document = new DocumentDetailsResponse
                                    {
                                        id = documentDetails.Id,
                                        documenturl = documentDetails.DocumentUrl,
                                        name = documentDetails.name
                                    };
                                    documentlist.Add(document);
                                }
                                individualDetailsResponse.previousmarksheets = documentlist;
                            }
                            individualDetailsResponses.Add(individualDetailsResponse);
                        }

                    }
                    else
                    {
                        SchoolDetails schoolDetails = IndividualDetailsBusiness.GetSchoolDetailById(metadata.DetailId);

                        if (schoolDetails != null)
                        {
                            SchoolDetailsResponse schoolDetailsResponse = new SchoolDetailsResponse
                            {
                                id = schoolDetails.Id,
                                registernumber = schoolDetails.RegisterNumber,
                                schooltypeid = schoolDetails.SchoolTypeId,
                                schoolname = schoolDetails.SchoolName,
                                schooladdress = schoolDetails.SchoolAddress,
                                sectiontypeid = schoolDetails.SectionTypeId,
                                numberofteachermale = schoolDetails.NumberOfTeacherMale,
                                numberofteacherfemale = schoolDetails.NumberOfTeacherFemale,
                                numberofstudentmale = schoolDetails.NumberOfStudentMale,
                                numberofstudentfemale = schoolDetails.NumberOfStudentFemale,
                                numberofstaffmale = schoolDetails.NumberOfStaffMale,
                                numberofstafffemale = schoolDetails.NumberOfStaffFemale,
                                poweraddressid = schoolDetails.PowerAddressId,
                                internetaccessid = schoolDetails.InternetAccessId,
                                buildingownershipid = schoolDetails.BuildingOwnershipId,
                                computers = schoolDetails.Computers,
                                monitors = schoolDetails.Monitors,
                                routers = schoolDetails.Routers,
                                dongles = schoolDetails.Dongles
                            };

                            if (!string.IsNullOrEmpty(schoolDetails.SchoolLicense))
                            {
                                List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                                string[] docid = schoolDetails.SchoolLicense.Split(",");
                                for (int i = 0; i < docid.Length; i++)
                                {
                                    DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                                    DocumentDetailsResponse document = new DocumentDetailsResponse
                                    {
                                        id = documentDetails.Id,
                                        documenturl = documentDetails.DocumentUrl,
                                        name = documentDetails.name
                                    };
                                    documentlist.Add(document);
                                }
                                schoolDetailsResponse.schoollicense = documentlist;
                            }

                            if (!string.IsNullOrEmpty(schoolDetails.RegisterationPaper))
                            {
                                List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                                string[] docid = schoolDetails.RegisterationPaper.Split(",");
                                for (int i = 0; i < docid.Length; i++)
                                {
                                    DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                                    DocumentDetailsResponse document = new DocumentDetailsResponse
                                    {
                                        id = documentDetails.Id,
                                        documenturl = documentDetails.DocumentUrl,
                                        name = documentDetails.name
                                    };
                                    documentlist.Add(document);
                                }
                                schoolDetailsResponse.registerationpaper = documentlist;
                            }
                            purchaseDetailResponse.schooldetails = schoolDetailsResponse;
                        }

                    }
                }

                purchaseDetailResponse.individualdetails = individualDetailsResponses;

                successResponse.data = purchaseDetailResponse;
                successResponse.response_code = 0;
                successResponse.message = "SchoolDetails";
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



        [HttpPost("AddIndividualDetails")]
        public async Task<IActionResult> AddIndividualDetails()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);

            try
            {
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
                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("17")))
                    {
                        if (string.IsNullOrEmpty(Request.Form["metadataid"]))
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Please provide metadataid.";
                            unsuccessResponse.status = "Failure";
                            return StatusCode(422, unsuccessResponse);
                        }

                        IndividualDetails IndividualDetails = new IndividualDetails();

                        if (string.IsNullOrEmpty(Request.Form["studentcode"]))
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "studentcode is required";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(422, unsuccessResponse);
                        }
                        IndividualDetails.StudentCode = Request.Form["studentcode"];
                        if (string.IsNullOrEmpty(Request.Form["studentname"]))
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "studentname is required";
                            unsuccessResponse.status = "Unsuccess";
                            return StatusCode(422, unsuccessResponse);
                        }
                        IndividualDetails.StudentName = Request.Form["studentname"];
                        IndividualDetails.SchoolName = Request.Form["schoolname"];
                        IndividualDetails.FatherNumber = Request.Form["fathernumber"];
                        IndividualDetails.IdCardNumber = Request.Form["idcardnumber"];
                        IndividualDetails.PermanentAddress = Request.Form["permanentaddress"];
                        IndividualDetails.Village = Request.Form["village"];
                        IndividualDetails.ProvinceId = Request.Form["provinceid"];
                        IndividualDetails.CityId = Request.Form["cityid"];
                        IndividualDetails.Phone = Request.Form["phone"];
                        IndividualDetails.CountryId = Request.Form["countryid"];
                        IndividualDetails.RefferedBy = Request.Form["refferedby"];
                        IndividualDetails.Email = Request.Form["email"];
                        IndividualDetails.PassportNumber = Request.Form["passportnumber"];
                        IndividualDetails.Nationality = Request.Form["nationality"];
                        IndividualDetails.SexId = Request.Form["sexid"];
                        IndividualDetails.DateOfBirth = Request.Form["dateofbirth"];
                        IndividualDetails.PlaceOfBirth = Request.Form["placeofbirth"];
                        IndividualDetails.CurrentAddress = Request.Form["currentaddress"];
                        IndividualDetails.MaritalStatusId = Request.Form["maritalstatusid"];
                        IndividualDetails.Remarks = Request.Form["remarks"];

                        var studentfile = Request.Form["studenttazrikafile"].ToString();
                        string[] studenttazrikafiles = null;
                        if (!string.IsNullOrWhiteSpace(studentfile))
                            studenttazrikafiles = studentfile.Split(',');

                        if (studenttazrikafiles != null)
                        {
                            foreach (var studenttazrikafile in studenttazrikafiles)
                            {
                                string mediaLink = "";
                                string BucketName = General.getBucketName("1");
                                if (!string.IsNullOrEmpty(studenttazrikafile))
                                {
                                    var storageObject = storage.GetObject(BucketName, studenttazrikafile);
                                    mediaLink = storageObject.MediaLink;
                                }

                                DocumentDetails documentDetails = IndividualDetailsBusiness.CreateDocumentDetails(new DocumentDetails { DocumentUrl = mediaLink, name = studenttazrikafile }, int.Parse(tc.Id));

                                if (string.IsNullOrEmpty(IndividualDetails.StudentTazkira))
                                {
                                    IndividualDetails.StudentTazkira = documentDetails.Id.ToString();
                                }
                                else
                                {
                                    IndividualDetails.StudentTazkira += "," + documentDetails.Id;
                                }
                            }
                        }

                        var parentfile = Request.Form["parenttazrikafile"].ToString();
                        string[] parenttazrikafiles = null;
                        if (!string.IsNullOrWhiteSpace(parentfile))
                            parenttazrikafiles = parentfile.Split(',');

                        if (parenttazrikafiles != null)
                        {
                            foreach (var parenttazrikafile in parenttazrikafiles)
                            {
                                string mediaLink = "";
                                string BucketName = General.getBucketName("1");
                                if (!string.IsNullOrEmpty(parenttazrikafile))
                                {
                                    var storageObject = storage.GetObject(BucketName, parenttazrikafile);
                                    mediaLink = storageObject.MediaLink;
                                }

                                DocumentDetails documentDetails = IndividualDetailsBusiness.CreateDocumentDetails(new DocumentDetails { DocumentUrl = mediaLink, name = parenttazrikafile }, int.Parse(tc.Id));

                                if (string.IsNullOrEmpty(IndividualDetails.ParentTazrika))
                                {
                                    IndividualDetails.ParentTazrika = documentDetails.Id.ToString();
                                }
                                else
                                {
                                    IndividualDetails.ParentTazrika += "," + documentDetails.Id;
                                }
                            }
                        }

                        var marksheetfile = Request.Form["previousmarksheetsfile"].ToString();
                        string[] previousmarksheetsfiles = null;
                        if (!string.IsNullOrWhiteSpace(marksheetfile))
                            previousmarksheetsfiles = marksheetfile.Split(',');

                        if (previousmarksheetsfiles != null)
                        {
                            foreach (var previousmarksheetsfile in previousmarksheetsfiles)
                            {
                                string mediaLink = "";
                                string BucketName = General.getBucketName("1");
                                if (!string.IsNullOrEmpty(previousmarksheetsfile))
                                {
                                    var storageObject = storage.GetObject(BucketName, previousmarksheetsfile);
                                    mediaLink = storageObject.MediaLink;
                                }

                                DocumentDetails documentDetails = IndividualDetailsBusiness.CreateDocumentDetails(new DocumentDetails { DocumentUrl = mediaLink, name = previousmarksheetsfile }, int.Parse(tc.Id));

                                if (string.IsNullOrEmpty(IndividualDetails.PreviousMarksheets))
                                {
                                    IndividualDetails.PreviousMarksheets = documentDetails.Id.ToString();
                                }
                                else
                                {
                                    IndividualDetails.PreviousMarksheets += "," + documentDetails.Id;
                                }
                            }
                        }
                        //var allFiles = Request.Form.Files.ToList();
                        //for (int k = 0; k < allFiles.Count; k++)
                        //{
                        //    string fileName = "";
                        //    IFormFile file = null;
                        //    if (Request.Form.Files.Count != 0)
                        //        file = Request.Form.Files[k];
                        //    var imageAcl = PredefinedObjectAcl.PublicRead;
                        //    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        //    var ext = fileName.Substring(fileName.LastIndexOf("."));
                        //    var extension = ext.ToLower();
                        //    Guid imageGuid = Guid.NewGuid();
                        //    fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;
                        //    string mediaLink = "";
                        //    var imageObject = await storage.UploadObjectAsync(
                        //        bucket: "t24-primary-pdf-storage",
                        //        objectName: fileName,
                        //        contentType: file.ContentType,
                        //        source: file.OpenReadStream(),
                        //        options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        //    );
                        //    mediaLink = imageObject.MediaLink;

                        //    DocumentDetails documentDetails = IndividualDetailsBusiness.CreateDocumentDetails(new DocumentDetails { DocumentUrl = mediaLink, name = fileName }, int.Parse(tc.Id));

                        //    var singleFile = allFiles[k];
                        //    switch (singleFile.Name)
                        //    {
                        //        case "studenttazrika":
                        //            if (string.IsNullOrEmpty(IndividualDetails.StudentTazkira))
                        //            {
                        //                IndividualDetails.StudentTazkira = documentDetails.Id.ToString();
                        //            }
                        //            else
                        //            {
                        //                IndividualDetails.StudentTazkira = "," + documentDetails.Id;
                        //            }
                        //            break;
                        //        case "parenttazrika":
                        //            if (string.IsNullOrEmpty(IndividualDetails.ParentTazrika))
                        //            {
                        //                IndividualDetails.ParentTazrika = documentDetails.Id.ToString();
                        //            }
                        //            else
                        //            {
                        //                IndividualDetails.ParentTazrika += "," + documentDetails.Id;
                        //            }
                        //            break;
                        //        case "previousmarksheets":
                        //            if (string.IsNullOrEmpty(IndividualDetails.PreviousMarksheets))
                        //            {
                        //                IndividualDetails.PreviousMarksheets = documentDetails.Id.ToString();
                        //            }
                        //            else
                        //            {
                        //                IndividualDetails.PreviousMarksheets += "," + documentDetails.Id;
                        //            }
                        //            break;
                        //    }

                        //}

                        IndividualDetails individualDetails = IndividualDetailsBusiness.Create(IndividualDetails, int.Parse(tc.Id));

                        MetaDataDetail MetaDataDetail = IndividualDetailsBusiness.CreateMetaDataDetail(new MetaDataDetail
                        {
                            DetailId = individualDetails.Id,
                            DetailTypeId = 1,
                            MetadataId = long.Parse(Request.Form["metadataid"])
                        }, int.Parse(tc.Id));

                        IndividualDetailsResponse individualDetailsResponse = new IndividualDetailsResponse
                        {
                            studentcode = individualDetails.StudentCode,
                            studentname = individualDetails.StudentName,
                            schoolname = individualDetails.SchoolName,
                            fathernumber = individualDetails.FatherNumber,
                            idcardnumber = individualDetails.IdCardNumber,
                            permanentaddress = individualDetails.PermanentAddress,
                            village = individualDetails.Village,
                            provinceid = individualDetails.ProvinceId,
                            cityid = individualDetails.CityId,
                            phone = individualDetails.Phone,
                            countryid = individualDetails.CountryId,
                            refferedby = individualDetails.RefferedBy,
                            email = individualDetails.Email,
                            passportnumber = individualDetails.PassportNumber,
                            nationality = individualDetails.Nationality,
                            sexid = individualDetails.SexId,
                            dateofbirth = individualDetails.DateOfBirth,
                            placeofbirth = individualDetails.PlaceOfBirth,
                            currentaddress = individualDetails.CurrentAddress,
                            maritalstatusid = individualDetails.MaritalStatusId,
                            remarks = individualDetails.Remarks
                        };

                        if (!string.IsNullOrEmpty(individualDetails.StudentTazkira))
                        {
                            List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                            string[] docid = individualDetails.StudentTazkira.Split(",");
                            for (int i = 0; i < docid.Length; i++)
                            {
                                DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                                DocumentDetailsResponse document = new DocumentDetailsResponse
                                {
                                    id = documentDetails.Id,
                                    documenturl = documentDetails.DocumentUrl,
                                    name = documentDetails.name
                                };
                                documentlist.Add(document);
                            }
                            individualDetailsResponse.studenttazkira = documentlist;
                        }

                        if (!string.IsNullOrEmpty(individualDetails.ParentTazrika))
                        {
                            List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                            string[] docid = individualDetails.ParentTazrika.Split(",");
                            for (int i = 0; i < docid.Length; i++)
                            {
                                DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                                DocumentDetailsResponse document = new DocumentDetailsResponse
                                {
                                    id = documentDetails.Id,
                                    documenturl = documentDetails.DocumentUrl,
                                    name = documentDetails.name
                                };
                                documentlist.Add(document);
                            }
                            individualDetailsResponse.parenttazrika = documentlist;
                        }

                        if (!string.IsNullOrEmpty(individualDetails.PreviousMarksheets))
                        {
                            List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                            string[] docid = individualDetails.PreviousMarksheets.Split(",");
                            for (int i = 0; i < docid.Length; i++)
                            {
                                DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                                DocumentDetailsResponse document = new DocumentDetailsResponse
                                {
                                    id = documentDetails.Id,
                                    documenturl = documentDetails.DocumentUrl,
                                    name = documentDetails.name
                                };
                                documentlist.Add(document);
                            }
                            individualDetailsResponse.previousmarksheets = documentlist;
                        }

                        successResponse.data = individualDetailsResponse;
                        successResponse.response_code = 0;
                        successResponse.message = "IndividualDetails Inserted";
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

        [HttpPut("UpdateIndividualDetails/{id}")]
        public async Task<IActionResult> UpdateIndividualDetails(long id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);

            try
            {
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
                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("17")))
                    {
                        SubscriptionMetadata subscriptionMetadata = SubscriptionMetadataBusiness.GetMetadataByid(long.Parse(Request.Form["metadataid"]));
                        if (subscriptionMetadata != null)
                        {
                            if (subscriptionMetadata.SalesAgentId != salesAgent.Id)
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "This purchase is not editable";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(406, unsuccessResponse);
                            }
                        }
                        IndividualDetails IndividualDetails = new IndividualDetails();
                        IndividualDetails.StudentCode = Request.Form["studentcode"];
                        IndividualDetails.StudentName = Request.Form["studentname"];
                        IndividualDetails.SchoolName = Request.Form["schoolname"];
                        IndividualDetails.FatherNumber = Request.Form["fathernumber"];
                        IndividualDetails.IdCardNumber = Request.Form["idcardnumber"];
                        IndividualDetails.PermanentAddress = Request.Form["permanentaddress"];
                        IndividualDetails.Village = Request.Form["village"];
                        IndividualDetails.ProvinceId = Request.Form["provinceid"];
                        IndividualDetails.CityId = Request.Form["cityid"];
                        IndividualDetails.Phone = Request.Form["phone"];
                        IndividualDetails.CountryId = Request.Form["countryid"];
                        IndividualDetails.RefferedBy = Request.Form["refferedby"];
                        IndividualDetails.Email = Request.Form["email"];
                        IndividualDetails.PassportNumber = Request.Form["passportnumber"];
                        IndividualDetails.Nationality = Request.Form["nationality"];
                        IndividualDetails.SexId = Request.Form["sexid"];
                        IndividualDetails.DateOfBirth = Request.Form["dateofbirth"];
                        IndividualDetails.PlaceOfBirth = Request.Form["placeofbirth"];
                        IndividualDetails.CurrentAddress = Request.Form["currentaddress"];
                        IndividualDetails.MaritalStatusId = Request.Form["maritalstatusid"];
                        IndividualDetails.Remarks = Request.Form["remarks"];


                        var studentfile = Request.Form["studenttazrikafile"].ToString();
                        string[] studenttazrikafiles = null;
                        if (!string.IsNullOrWhiteSpace(studentfile))
                            studenttazrikafiles = studentfile.Split(',');

                        if (studenttazrikafiles != null)
                        {
                            foreach (var studenttazrikafile in studenttazrikafiles)
                            {
                                string mediaLink = "";
                                string BucketName = General.getBucketName("1");
                                if (!string.IsNullOrEmpty(studenttazrikafile))
                                {
                                    var storageObject = storage.GetObject(BucketName, studenttazrikafile);
                                    mediaLink = storageObject.MediaLink;
                                }

                                DocumentDetails documentDetails = IndividualDetailsBusiness.CreateDocumentDetails(new DocumentDetails { DocumentUrl = mediaLink, name = studenttazrikafile }, int.Parse(tc.Id));

                                if (string.IsNullOrEmpty(IndividualDetails.StudentTazkira))
                                {
                                    IndividualDetails.StudentTazkira = documentDetails.Id.ToString();
                                }
                                else
                                {
                                    IndividualDetails.StudentTazkira += "," + documentDetails.Id;
                                }
                            }
                        }

                        var parentfile = Request.Form["parenttazrikafile"].ToString();
                        string[] parenttazrikafiles = null;
                        if (!string.IsNullOrWhiteSpace(parentfile))
                            parenttazrikafiles = parentfile.Split(',');

                        if (parenttazrikafiles != null)
                        {
                            foreach (var parenttazrikafile in parenttazrikafiles)
                            {
                                string mediaLink = "";
                                string BucketName = General.getBucketName("1");
                                if (!string.IsNullOrEmpty(parenttazrikafile))
                                {
                                    var storageObject = storage.GetObject(BucketName, parenttazrikafile);
                                    mediaLink = storageObject.MediaLink;
                                }

                                DocumentDetails documentDetails = IndividualDetailsBusiness.CreateDocumentDetails(new DocumentDetails { DocumentUrl = mediaLink, name = parenttazrikafile }, int.Parse(tc.Id));

                                if (string.IsNullOrEmpty(IndividualDetails.ParentTazrika))
                                {
                                    IndividualDetails.ParentTazrika = documentDetails.Id.ToString();
                                }
                                else
                                {
                                    IndividualDetails.ParentTazrika += "," + documentDetails.Id;
                                }
                            }
                        }

                        var marksheetfile = Request.Form["previousmarksheetsfile"].ToString();
                        string[] previousmarksheetsfiles = null;
                        if (!string.IsNullOrWhiteSpace(marksheetfile))
                            previousmarksheetsfiles = marksheetfile.Split(',');

                        if (previousmarksheetsfiles != null)
                        {
                            foreach (var previousmarksheetsfile in previousmarksheetsfiles)
                            {
                                string mediaLink = "";
                                string BucketName = General.getBucketName("1");
                                if (!string.IsNullOrEmpty(previousmarksheetsfile))
                                {
                                    var storageObject = storage.GetObject(BucketName, previousmarksheetsfile);
                                    mediaLink = storageObject.MediaLink;
                                }

                                DocumentDetails documentDetails = IndividualDetailsBusiness.CreateDocumentDetails(new DocumentDetails { DocumentUrl = mediaLink, name = previousmarksheetsfile }, int.Parse(tc.Id));

                                if (string.IsNullOrEmpty(IndividualDetails.PreviousMarksheets))
                                {
                                    IndividualDetails.PreviousMarksheets = documentDetails.Id.ToString();
                                }
                                else
                                {
                                    IndividualDetails.PreviousMarksheets += "," + documentDetails.Id;
                                }
                            }
                        }

                        //var allFiles = Request.Form.Files.ToList();
                        //for (int k = 0; k < allFiles.Count; k++)
                        //{
                        //    string fileName = "";
                        //    IFormFile file = null;
                        //    if (Request.Form.Files.Count != 0)
                        //        file = Request.Form.Files[k];
                        //    var imageAcl = PredefinedObjectAcl.PublicRead;
                        //    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        //    var ext = fileName.Substring(fileName.LastIndexOf("."));
                        //    var extension = ext.ToLower();
                        //    Guid imageGuid = Guid.NewGuid();
                        //    fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;
                        //    string mediaLink = "";
                        //    var imageObject = await storage.UploadObjectAsync(
                        //        bucket: "t24-primary-pdf-storage",
                        //        objectName: fileName,
                        //        contentType: file.ContentType,
                        //        source: file.OpenReadStream(),
                        //        options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        //    );
                        //    mediaLink = imageObject.MediaLink;

                        //    DocumentDetails documentDetails = IndividualDetailsBusiness.CreateDocumentDetails(new DocumentDetails { DocumentUrl = mediaLink, name = fileName }, int.Parse(tc.Id));

                        //    var singleFile = allFiles[k];
                        //    switch (singleFile.Name)
                        //    {
                        //        case "studenttazrika":
                        //            if (string.IsNullOrEmpty(IndividualDetails.StudentTazkira))
                        //            {
                        //                IndividualDetails.StudentTazkira = documentDetails.Id.ToString();
                        //            }
                        //            else
                        //            {
                        //                IndividualDetails.StudentTazkira = "," + documentDetails.Id;
                        //            }
                        //            break;
                        //        case "parenttazrika":
                        //            if (string.IsNullOrEmpty(IndividualDetails.ParentTazrika))
                        //            {
                        //                IndividualDetails.ParentTazrika = documentDetails.Id.ToString();
                        //            }
                        //            else
                        //            {
                        //                IndividualDetails.ParentTazrika += "," + documentDetails.Id;
                        //            }
                        //            break;
                        //        case "previousmarksheets":
                        //            if (string.IsNullOrEmpty(IndividualDetails.PreviousMarksheets))
                        //            {
                        //                IndividualDetails.PreviousMarksheets = documentDetails.Id.ToString();
                        //            }
                        //            else
                        //            {
                        //                IndividualDetails.PreviousMarksheets += "," + documentDetails.Id;
                        //            }
                        //            break;
                        //    }

                        //}

                        IndividualDetails.Id = id;

                        IndividualDetails individualDetails = IndividualDetailsBusiness.Update(IndividualDetails, int.Parse(tc.Id));

                        MetaDataDetail getDetails = IndividualDetailsBusiness.GetMetadataById(long.Parse(Request.Form["metadataid"]));
                        if (getDetails == null)
                        {
                            MetaDataDetail MetaDataDetail = IndividualDetailsBusiness.CreateMetaDataDetail(new MetaDataDetail
                            {
                                DetailId = individualDetails.Id,
                                DetailTypeId = 1,
                                MetadataId = long.Parse(Request.Form["metadataid"])
                            }, int.Parse(tc.Id));
                        }
                        IndividualDetailsResponse individualDetailsResponse = new IndividualDetailsResponse
                        {
                            id = individualDetails.Id,
                            studentcode = individualDetails.StudentCode,
                            studentname = individualDetails.StudentName,
                            schoolname = individualDetails.SchoolName,
                            fathernumber = individualDetails.FatherNumber,
                            idcardnumber = individualDetails.IdCardNumber,
                            permanentaddress = individualDetails.PermanentAddress,
                            village = individualDetails.Village,
                            provinceid = individualDetails.ProvinceId,
                            cityid = individualDetails.CityId,
                            phone = individualDetails.Phone,
                            countryid = individualDetails.CountryId,
                            refferedby = individualDetails.RefferedBy,
                            email = individualDetails.Email,
                            passportnumber = individualDetails.PassportNumber,
                            nationality = individualDetails.Nationality,
                            sexid = individualDetails.SexId,
                            dateofbirth = individualDetails.DateOfBirth,
                            placeofbirth = individualDetails.PlaceOfBirth,
                            currentaddress = individualDetails.CurrentAddress,
                            maritalstatusid = individualDetails.MaritalStatusId,
                            remarks = individualDetails.Remarks
                        };

                        if (!string.IsNullOrEmpty(individualDetails.StudentTazkira))
                        {
                            List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                            string[] docid = individualDetails.StudentTazkira.Split(",");
                            for (int i = 0; i < docid.Length; i++)
                            {
                                DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                                DocumentDetailsResponse document = new DocumentDetailsResponse
                                {
                                    id = documentDetails.Id,
                                    documenturl = documentDetails.DocumentUrl,
                                    name = documentDetails.name
                                };
                                documentlist.Add(document);
                            }
                            individualDetailsResponse.studenttazkira = documentlist;
                        }

                        if (!string.IsNullOrEmpty(individualDetails.ParentTazrika))
                        {
                            List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                            string[] docid = individualDetails.ParentTazrika.Split(",");
                            for (int i = 0; i < docid.Length; i++)
                            {
                                DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                                DocumentDetailsResponse document = new DocumentDetailsResponse
                                {
                                    id = documentDetails.Id,
                                    documenturl = documentDetails.DocumentUrl,
                                    name = documentDetails.name
                                };
                                documentlist.Add(document);
                            }
                            individualDetailsResponse.parenttazrika = documentlist;
                        }

                        if (!string.IsNullOrEmpty(individualDetails.PreviousMarksheets))
                        {
                            List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                            string[] docid = individualDetails.PreviousMarksheets.Split(",");
                            for (int i = 0; i < docid.Length; i++)
                            {
                                DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                                DocumentDetailsResponse document = new DocumentDetailsResponse
                                {
                                    id = documentDetails.Id,
                                    documenturl = documentDetails.DocumentUrl,
                                    name = documentDetails.name
                                };
                                documentlist.Add(document);
                            }
                            individualDetailsResponse.previousmarksheets = documentlist;
                        }

                        successResponse.data = individualDetailsResponse;
                        successResponse.response_code = 0;
                        successResponse.message = "IndividualDetails Updated";
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

        [HttpGet("GetIndividualDetails")]
        public IActionResult GetIndividualDetails(string search)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                List<IndividualDetails> individualDetails = IndividualDetailsBusiness.GetIndiviualDetails(search);

                if (individualDetails.Count == 0)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Record not found";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(422, unsuccessResponse);
                }

                List<IndividualDetailsResponse> IndividualDetailsResponseList = new List<IndividualDetailsResponse>();

                foreach (var individual in individualDetails)
                {
                    IndividualDetailsResponse individualDetailsResponse = new IndividualDetailsResponse
                    {
                        id = individual.Id,
                        studentcode = individual.StudentCode,
                        studentname = individual.StudentName,
                        schoolname = individual.SchoolName,
                        fathernumber = individual.FatherNumber,
                        idcardnumber = individual.IdCardNumber,
                        permanentaddress = individual.PermanentAddress,
                        village = individual.Village,
                        provinceid = individual.ProvinceId,
                        cityid = individual.CityId,
                        phone = individual.Phone,
                        countryid = individual.CountryId,
                        refferedby = individual.RefferedBy,
                        email = individual.Email,
                        passportnumber = individual.PassportNumber,
                        nationality = individual.Nationality,
                        sexid = individual.SexId,
                        dateofbirth = individual.DateOfBirth,
                        placeofbirth = individual.PlaceOfBirth,
                        currentaddress = individual.CurrentAddress,
                        maritalstatusid = individual.MaritalStatusId,
                        remarks = individual.Remarks
                    };


                    if (!string.IsNullOrEmpty(individual.StudentTazkira))
                    {
                        List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                        string[] docid = individual.StudentTazkira.Split(",");
                        for (int i = 0; i < docid.Length; i++)
                        {
                            DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                            DocumentDetailsResponse document = new DocumentDetailsResponse
                            {
                                id = documentDetails.Id,
                                documenturl = documentDetails.DocumentUrl,
                                name = documentDetails.name
                            };
                            documentlist.Add(document);
                        }
                        individualDetailsResponse.studenttazkira = documentlist;
                    }

                    if (!string.IsNullOrEmpty(individual.ParentTazrika))
                    {
                        List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                        string[] docid = individual.ParentTazrika.Split(",");
                        for (int i = 0; i < docid.Length; i++)
                        {
                            DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                            DocumentDetailsResponse document = new DocumentDetailsResponse
                            {
                                id = documentDetails.Id,
                                documenturl = documentDetails.DocumentUrl,
                                name = documentDetails.name
                            };
                            documentlist.Add(document);
                        }
                        individualDetailsResponse.parenttazrika = documentlist;
                    }

                    if (!string.IsNullOrEmpty(individual.PreviousMarksheets))
                    {
                        List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                        string[] docid = individual.PreviousMarksheets.Split(",");
                        for (int i = 0; i < docid.Length; i++)
                        {
                            DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                            DocumentDetailsResponse document = new DocumentDetailsResponse
                            {
                                id = documentDetails.Id,
                                documenturl = documentDetails.DocumentUrl,
                                name = documentDetails.name
                            };
                            documentlist.Add(document);
                        }
                        individualDetailsResponse.previousmarksheets = documentlist;
                    }


                    IndividualDetailsResponseList.Add(individualDetailsResponse);
                }

                successResponse.data = IndividualDetailsResponseList;
                successResponse.response_code = 0;
                successResponse.message = "IndividualDetails";
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


        [HttpGet("GetIndividualList")]
        public IActionResult GetIndividualList(string search)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                var individualDetails = IndividualDetailsBusiness.GetIndividualList(search);
                successResponse.data = individualDetails;
                successResponse.response_code = 0;
                successResponse.message = "IndividualDetails";
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

        [HttpGet("GetIndividualDetailById")]
        public IActionResult GetIndividualDetailById(long id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                IndividualDetails individual = IndividualDetailsBusiness.GetIndividualDetailById(id);

                if (individual == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Record not found";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(422, unsuccessResponse);
                }

                IndividualDetailsResponse individualDetailsResponse = new IndividualDetailsResponse
                {
                    id = individual.Id,
                    studentcode = individual.StudentCode,
                    studentname = individual.StudentName,
                    schoolname = individual.SchoolName,
                    fathernumber = individual.FatherNumber,
                    idcardnumber = individual.IdCardNumber,
                    permanentaddress = individual.PermanentAddress,
                    village = individual.Village,
                    provinceid = individual.ProvinceId,
                    cityid = individual.CityId,
                    phone = individual.Phone,
                    countryid = individual.CountryId,
                    refferedby = individual.RefferedBy,
                    email = individual.Email,
                    passportnumber = individual.PassportNumber,
                    nationality = individual.Nationality,
                    sexid = individual.SexId,
                    dateofbirth = individual.DateOfBirth,
                    placeofbirth = individual.PlaceOfBirth,
                    currentaddress = individual.CurrentAddress,
                    maritalstatusid = individual.MaritalStatusId,
                    remarks = individual.Remarks
                };

                if (!string.IsNullOrEmpty(individual.StudentTazkira))
                {
                    List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                    string[] docid = individual.StudentTazkira.Split(",");
                    for (int i = 0; i < docid.Length; i++)
                    {
                        DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                        DocumentDetailsResponse document = new DocumentDetailsResponse
                        {
                            id = documentDetails.Id,
                            documenturl = documentDetails.DocumentUrl,
                            name = documentDetails.name
                        };
                        documentlist.Add(document);
                    }
                    individualDetailsResponse.studenttazkira = documentlist;
                }

                if (!string.IsNullOrEmpty(individual.ParentTazrika))
                {
                    List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                    string[] docid = individual.ParentTazrika.Split(",");
                    for (int i = 0; i < docid.Length; i++)
                    {
                        DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                        DocumentDetailsResponse document = new DocumentDetailsResponse
                        {
                            id = documentDetails.Id,
                            documenturl = documentDetails.DocumentUrl,
                            name = documentDetails.name
                        };
                        documentlist.Add(document);
                    }
                    individualDetailsResponse.parenttazrika = documentlist;
                }

                if (!string.IsNullOrEmpty(individual.PreviousMarksheets))
                {
                    List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                    string[] docid = individual.PreviousMarksheets.Split(",");
                    for (int i = 0; i < docid.Length; i++)
                    {
                        DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                        DocumentDetailsResponse document = new DocumentDetailsResponse
                        {
                            id = documentDetails.Id,
                            documenturl = documentDetails.DocumentUrl,
                            name = documentDetails.name
                        };
                        documentlist.Add(document);
                    }
                    individualDetailsResponse.previousmarksheets = documentlist;
                }


                successResponse.data = individualDetailsResponse;
                successResponse.response_code = 0;
                successResponse.message = "IndividualDetails";
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


        [HttpPost("AddSchoolDetails")]
        public async Task<IActionResult> AddSchoolDetails()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);

            try
            {
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
                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("17")))
                    {
                        if (string.IsNullOrEmpty(Request.Form["metadataid"]))
                        {
                            unsuccessResponse.response_code = 1;
                            unsuccessResponse.message = "Please provide metadataid.";
                            unsuccessResponse.status = "Failure";
                            return StatusCode(422, unsuccessResponse);
                        }

                        SchoolDetails SchoolDetails = new SchoolDetails();
                        SchoolDetails.RegisterNumber = Request.Form["registernumber"];
                        SchoolDetails.SchoolTypeId = Request.Form["schooltypeid"];
                        SchoolDetails.SchoolName = Request.Form["schoolname"];
                        SchoolDetails.SchoolAddress = Request.Form["schooladdress"];
                        SchoolDetails.SectionTypeId = Request.Form["sectiontypeid"];
                        SchoolDetails.NumberOfTeacherMale = !string.IsNullOrEmpty(Request.Form["numberofteachermale"]) ? int.Parse(Request.Form["numberofteachermale"]) : 0;
                        SchoolDetails.NumberOfTeacherFemale = !string.IsNullOrEmpty(Request.Form["numberofteacherfemale"]) ? int.Parse(Request.Form["numberofteacherfemale"]) : 0;
                        SchoolDetails.NumberOfStudentMale = !string.IsNullOrEmpty(Request.Form["numberofstudentmale"]) ? int.Parse(Request.Form["numberofstudentmale"]) : 0;
                        SchoolDetails.NumberOfStudentFemale = !string.IsNullOrEmpty(Request.Form["numberofstudentfemale"]) ? int.Parse(Request.Form["numberofstudentfemale"]) : 0;
                        SchoolDetails.NumberOfStaffMale = !string.IsNullOrEmpty(Request.Form["numberofstaffmale"]) ? int.Parse(Request.Form["numberofstaffmale"]) : 0;
                        SchoolDetails.NumberOfStaffFemale = !string.IsNullOrEmpty(Request.Form["numberofstafffemale"]) ? int.Parse(Request.Form["numberofstafffemale"]) : 0;
                        SchoolDetails.PowerAddressId = Request.Form["poweraddressid"];
                        SchoolDetails.InternetAccessId = Request.Form["internetaccessid"];
                        SchoolDetails.BuildingOwnershipId = Request.Form["buildingownershipid"];
                        SchoolDetails.Computers = Request.Form["computers"];
                        SchoolDetails.Monitors = Request.Form["monitors"];
                        SchoolDetails.Routers = Request.Form["routers"];
                        SchoolDetails.Dongles = Request.Form["dongles"];

                        var schoollicense = Request.Form["schoollicensefile"].ToString();
                        string[] schoollicensefiles = null;
                        if (!string.IsNullOrWhiteSpace(schoollicense))
                            schoollicensefiles = schoollicense.Split(',');

                        if (schoollicensefiles != null)
                        {
                            foreach (var schoollicensefile in schoollicensefiles)
                            {
                                string mediaLink = "";
                                string BucketName = General.getBucketName("1");
                                if (!string.IsNullOrEmpty(schoollicensefile))
                                {
                                    var storageObject = storage.GetObject(BucketName, schoollicensefile);
                                    mediaLink = storageObject.MediaLink;
                                }

                                DocumentDetails documentDetails = IndividualDetailsBusiness.CreateDocumentDetails(new DocumentDetails { DocumentUrl = mediaLink, name = schoollicensefile }, int.Parse(tc.Id));

                                if (string.IsNullOrEmpty(SchoolDetails.SchoolLicense))
                                {
                                    SchoolDetails.SchoolLicense = documentDetails.Id.ToString();
                                }
                                else
                                {
                                    SchoolDetails.SchoolLicense += "," + documentDetails.Id;
                                }
                            }
                        }

                        var registerationpaper = Request.Form["registerationpaperfile"].ToString();
                        string[] registerationpaperfiles = null;
                        if (!string.IsNullOrWhiteSpace(registerationpaper))
                            registerationpaperfiles = registerationpaper.Split(',');

                        if (schoollicensefiles != null)
                        {
                            foreach (var registerationpaperfile in registerationpaperfiles)
                            {
                                string mediaLink = "";
                                string BucketName = General.getBucketName("1");
                                if (!string.IsNullOrEmpty(registerationpaperfile))
                                {
                                    var storageObject = storage.GetObject(BucketName, registerationpaperfile);
                                    mediaLink = storageObject.MediaLink;
                                }

                                DocumentDetails documentDetails = IndividualDetailsBusiness.CreateDocumentDetails(new DocumentDetails { DocumentUrl = mediaLink, name = registerationpaperfile }, int.Parse(tc.Id));

                                if (string.IsNullOrEmpty(SchoolDetails.RegisterationPaper))
                                {
                                    SchoolDetails.RegisterationPaper = documentDetails.Id.ToString();
                                }
                                else
                                {
                                    SchoolDetails.RegisterationPaper += "," + documentDetails.Id;
                                }
                            }
                        }

                        //var allFiles = Request.Form.Files.ToList();
                        //for (int k = 0; k < allFiles.Count; k++)
                        //{
                        //    string fileName = "";
                        //    IFormFile file = null;
                        //    if (Request.Form.Files.Count != 0)
                        //        file = Request.Form.Files[k];
                        //    var imageAcl = PredefinedObjectAcl.PublicRead;
                        //    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        //    var ext = fileName.Substring(fileName.LastIndexOf("."));
                        //    var extension = ext.ToLower();
                        //    Guid imageGuid = Guid.NewGuid();
                        //    fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;
                        //    string mediaLink = "";
                        //    var imageObject = await storage.UploadObjectAsync(
                        //        bucket: "t24-primary-pdf-storage",
                        //        objectName: fileName,
                        //        contentType: file.ContentType,
                        //        source: file.OpenReadStream(),
                        //        options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        //    );
                        //    mediaLink = imageObject.MediaLink;

                        //    DocumentDetails documentDetails = IndividualDetailsBusiness.CreateDocumentDetails(new DocumentDetails { DocumentUrl = mediaLink, name = fileName }, int.Parse(tc.Id));

                        //    var singleFile = allFiles[k];
                        //    switch (singleFile.Name)
                        //    {
                        //        case "schoollicense":
                        //            if (string.IsNullOrEmpty(SchoolDetails.SchoolLicense))
                        //            {
                        //                SchoolDetails.SchoolLicense = documentDetails.Id.ToString();
                        //            }
                        //            else
                        //            {
                        //                SchoolDetails.SchoolLicense += "," + documentDetails.Id;
                        //            }

                        //            break;
                        //        case "registerationpaper":
                        //            if (string.IsNullOrEmpty(SchoolDetails.RegisterationPaper))
                        //            {
                        //                SchoolDetails.RegisterationPaper = documentDetails.Id.ToString();
                        //            }
                        //            else
                        //            {
                        //                SchoolDetails.RegisterationPaper += "," + documentDetails.Id;
                        //            }
                        //            break;
                        //    }
                        //}

                        SchoolDetails schoolDetails = IndividualDetailsBusiness.CreateSchoolDetails(SchoolDetails, int.Parse(tc.Id));

                        MetaDataDetail MetaDataDetail = IndividualDetailsBusiness.CreateMetaDataDetail(new MetaDataDetail
                        {
                            DetailId = schoolDetails.Id,
                            DetailTypeId = 2,
                            MetadataId = long.Parse(Request.Form["metadataid"])
                        }, int.Parse(tc.Id));

                        SchoolDetailsResponse schoolDetailsResponse = new SchoolDetailsResponse
                        {
                            registernumber = schoolDetails.RegisterNumber,
                            schooltypeid = schoolDetails.SchoolTypeId,
                            schoolname = schoolDetails.SchoolName,
                            schooladdress = schoolDetails.SchoolAddress,
                            sectiontypeid = schoolDetails.SectionTypeId,
                            numberofteachermale = schoolDetails.NumberOfTeacherMale,
                            numberofteacherfemale = schoolDetails.NumberOfTeacherFemale,
                            numberofstudentmale = schoolDetails.NumberOfStudentMale,
                            numberofstudentfemale = schoolDetails.NumberOfStudentFemale,
                            numberofstaffmale = schoolDetails.NumberOfStaffMale,
                            numberofstafffemale = schoolDetails.NumberOfStaffFemale,
                            poweraddressid = schoolDetails.PowerAddressId,
                            internetaccessid = schoolDetails.InternetAccessId,
                            buildingownershipid = schoolDetails.BuildingOwnershipId,
                            computers = schoolDetails.Computers,
                            monitors = schoolDetails.Monitors,
                            routers = schoolDetails.Routers,
                            dongles = schoolDetails.Dongles
                        };

                        successResponse.data = schoolDetailsResponse;
                        successResponse.response_code = 0;
                        successResponse.message = "SchoolDetails Inserted";
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

        [HttpPut("UpdateSchoolDetails/{id}")]
        public async Task<IActionResult> UpdateSchoolDetails(long id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);

            try
            {
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
                if (ModelState.IsValid)
                {
                    if (tc.RoleName.Contains(General.getRoleType("17")))
                    {
                        SubscriptionMetadata subscriptionMetadata = SubscriptionMetadataBusiness.GetMetadataByid(long.Parse(Request.Form["metadataid"]));
                        if (subscriptionMetadata != null)
                        {
                            if (subscriptionMetadata.SalesAgentId != salesAgent.Id)
                            {
                                unsuccessResponse.response_code = 1;
                                unsuccessResponse.message = "This purchase is not editable";
                                unsuccessResponse.status = "Unsuccess";
                                return StatusCode(406, unsuccessResponse);
                            }
                        }
                        SchoolDetails SchoolDetails = new SchoolDetails();
                        SchoolDetails.RegisterNumber = Request.Form["registernumber"];
                        SchoolDetails.SchoolTypeId = Request.Form["schooltypeid"];
                        SchoolDetails.SchoolName = Request.Form["schoolname"];
                        SchoolDetails.SchoolAddress = Request.Form["schooladdress"];
                        SchoolDetails.SectionTypeId = Request.Form["sectiontypeid"];
                        SchoolDetails.NumberOfTeacherMale = int.Parse(Request.Form["numberofteachermale"]);
                        SchoolDetails.NumberOfTeacherFemale = int.Parse(Request.Form["numberofteacherfemale"]);
                        SchoolDetails.NumberOfStudentMale = int.Parse(Request.Form["numberofstudentmale"]);
                        SchoolDetails.NumberOfStudentFemale = int.Parse(Request.Form["numberofstudentfemale"]);
                        SchoolDetails.NumberOfStaffMale = int.Parse(Request.Form["numberofstaffmale"]);
                        SchoolDetails.NumberOfStaffFemale = int.Parse(Request.Form["numberofstafffemale"]);
                        SchoolDetails.PowerAddressId = Request.Form["poweraddressid"];
                        SchoolDetails.InternetAccessId = Request.Form["internetaccessid"];
                        SchoolDetails.BuildingOwnershipId = Request.Form["buildingownershipid"];
                        SchoolDetails.Computers = Request.Form["computers"];
                        SchoolDetails.Monitors = Request.Form["monitors"];
                        SchoolDetails.Routers = Request.Form["routers"];
                        SchoolDetails.Dongles = Request.Form["dongles"];

                        var schoollicense = Request.Form["schoollicensefile"].ToString();
                        string[] schoollicensefiles = null;
                        if (!string.IsNullOrWhiteSpace(schoollicense))
                            schoollicensefiles = schoollicense.Split(',');

                        if (schoollicensefiles != null)
                        {
                            foreach (var schoollicensefile in schoollicensefiles)
                            {
                                string mediaLink = "";
                                string BucketName = General.getBucketName("1");
                                if (!string.IsNullOrEmpty(schoollicensefile))
                                {
                                    var storageObject = storage.GetObject(BucketName, schoollicensefile);
                                    mediaLink = storageObject.MediaLink;
                                }

                                DocumentDetails documentDetails = IndividualDetailsBusiness.CreateDocumentDetails(new DocumentDetails { DocumentUrl = mediaLink, name = schoollicensefile }, int.Parse(tc.Id));

                                if (string.IsNullOrEmpty(SchoolDetails.SchoolLicense))
                                {
                                    SchoolDetails.SchoolLicense = documentDetails.Id.ToString();
                                }
                                else
                                {
                                    SchoolDetails.SchoolLicense += "," + documentDetails.Id;
                                }
                            }
                        }

                        var registerationpaper = Request.Form["registerationpaperfile"].ToString();
                        string[] registerationpaperfiles = null;
                        if (!string.IsNullOrWhiteSpace(registerationpaper))
                            registerationpaperfiles = registerationpaper.Split(',');

                        if (schoollicensefiles != null)
                        {
                            foreach (var registerationpaperfile in registerationpaperfiles)
                            {
                                string mediaLink = "";
                                string BucketName = General.getBucketName("1");
                                if (!string.IsNullOrEmpty(registerationpaperfile))
                                {
                                    var storageObject = storage.GetObject(BucketName, registerationpaperfile);
                                    mediaLink = storageObject.MediaLink;
                                }

                                DocumentDetails documentDetails = IndividualDetailsBusiness.CreateDocumentDetails(new DocumentDetails { DocumentUrl = mediaLink, name = registerationpaperfile }, int.Parse(tc.Id));

                                if (string.IsNullOrEmpty(SchoolDetails.RegisterationPaper))
                                {
                                    SchoolDetails.RegisterationPaper = documentDetails.Id.ToString();
                                }
                                else
                                {
                                    SchoolDetails.RegisterationPaper += "," + documentDetails.Id;
                                }
                            }
                        }

                        //var allFiles = Request.Form.Files.ToList();
                        //for (int k = 0; k < allFiles.Count; k++)
                        //{
                        //    string fileName = "";
                        //    IFormFile file = null;
                        //    if (Request.Form.Files.Count != 0)
                        //        file = Request.Form.Files[k];
                        //    var imageAcl = PredefinedObjectAcl.PublicRead;
                        //    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        //    var ext = fileName.Substring(fileName.LastIndexOf("."));
                        //    var extension = ext.ToLower();
                        //    Guid imageGuid = Guid.NewGuid();
                        //    fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;
                        //    string mediaLink = "";
                        //    var imageObject = await storage.UploadObjectAsync(
                        //        bucket: "t24-primary-pdf-storage",
                        //        objectName: fileName,
                        //        contentType: file.ContentType,
                        //        source: file.OpenReadStream(),
                        //        options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        //    );
                        //    mediaLink = imageObject.MediaLink;

                        //    DocumentDetails documentDetails = IndividualDetailsBusiness.CreateDocumentDetails(new DocumentDetails { DocumentUrl = mediaLink, name = fileName }, int.Parse(tc.Id));

                        //    var singleFile = allFiles[k];
                        //    switch (singleFile.Name)
                        //    {
                        //        case "schoollicense":
                        //            if (string.IsNullOrEmpty(SchoolDetails.SchoolLicense))
                        //            {
                        //                SchoolDetails.SchoolLicense = documentDetails.Id.ToString();
                        //            }
                        //            else
                        //            {
                        //                SchoolDetails.SchoolLicense += "," + documentDetails.Id;
                        //            }

                        //            break;
                        //        case "registerationpaper":
                        //            if (string.IsNullOrEmpty(SchoolDetails.RegisterationPaper))
                        //            {
                        //                SchoolDetails.RegisterationPaper = documentDetails.Id.ToString();
                        //            }
                        //            else
                        //            {
                        //                SchoolDetails.RegisterationPaper += "," + documentDetails.Id;
                        //            }
                        //            break;
                        //    }
                        //}


                        SchoolDetails.Id = id;
                        SchoolDetails schoolDetails = IndividualDetailsBusiness.UpdateSchoolDetails(SchoolDetails, int.Parse(tc.Id));

                        MetaDataDetail MetaDataDetail = IndividualDetailsBusiness.CreateMetaDataDetail(new MetaDataDetail
                        {
                            DetailId = schoolDetails.Id,
                            DetailTypeId = 2,
                            MetadataId = long.Parse(Request.Form["metadataid"])
                        }, int.Parse(tc.Id));

                        SchoolDetailsResponse schoolDetailsResponse = new SchoolDetailsResponse
                        {
                            registernumber = schoolDetails.RegisterNumber,
                            schooltypeid = schoolDetails.SchoolTypeId,
                            schoolname = schoolDetails.SchoolName,
                            schooladdress = schoolDetails.SchoolAddress,
                            sectiontypeid = schoolDetails.SectionTypeId,
                            numberofteachermale = schoolDetails.NumberOfTeacherMale,
                            numberofteacherfemale = schoolDetails.NumberOfTeacherFemale,
                            numberofstudentmale = schoolDetails.NumberOfStudentMale,
                            numberofstudentfemale = schoolDetails.NumberOfStudentFemale,
                            numberofstaffmale = schoolDetails.NumberOfStaffMale,
                            numberofstafffemale = schoolDetails.NumberOfStaffFemale,
                            poweraddressid = schoolDetails.PowerAddressId,
                            internetaccessid = schoolDetails.InternetAccessId,
                            buildingownershipid = schoolDetails.BuildingOwnershipId,
                            computers = schoolDetails.Computers,
                            monitors = schoolDetails.Monitors,
                            routers = schoolDetails.Routers,
                            dongles = schoolDetails.Dongles
                        };

                        successResponse.data = schoolDetailsResponse;
                        successResponse.response_code = 0;
                        successResponse.message = "SchoolDetails Inserted";
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

        [HttpGet("GetSchoolDetails")]
        public IActionResult GetSchoolDetails(string search)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                List<SchoolDetails> SchoolDetails = IndividualDetailsBusiness.GetSchoolDetails(search);

                if (SchoolDetails.Count == 0)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Record not found";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(422, unsuccessResponse);
                }

                List<SchoolDetailsResponse> SchoolDetailsResponseList = new List<SchoolDetailsResponse>();

                foreach (var schoolDetails in SchoolDetails)
                {
                    SchoolDetailsResponse schoolDetailsResponse = new SchoolDetailsResponse
                    {
                        id = schoolDetails.Id,
                        registernumber = schoolDetails.RegisterNumber,
                        schooltypeid = schoolDetails.SchoolTypeId,
                        schoolname = schoolDetails.SchoolName,
                        schooladdress = schoolDetails.SchoolAddress,
                        sectiontypeid = schoolDetails.SectionTypeId,
                        numberofteachermale = schoolDetails.NumberOfTeacherMale,
                        numberofteacherfemale = schoolDetails.NumberOfTeacherFemale,
                        numberofstudentmale = schoolDetails.NumberOfStudentMale,
                        numberofstudentfemale = schoolDetails.NumberOfStudentFemale,
                        numberofstaffmale = schoolDetails.NumberOfStaffMale,
                        numberofstafffemale = schoolDetails.NumberOfStaffFemale,
                        poweraddressid = schoolDetails.PowerAddressId,
                        internetaccessid = schoolDetails.InternetAccessId,
                        buildingownershipid = schoolDetails.BuildingOwnershipId,
                        computers = schoolDetails.Computers,
                        monitors = schoolDetails.Monitors,
                        routers = schoolDetails.Routers,
                        dongles = schoolDetails.Dongles
                    };

                    if (!string.IsNullOrEmpty(schoolDetails.SchoolLicense))
                    {
                        List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                        string[] docid = schoolDetails.SchoolLicense.Split(",");
                        for (int i = 0; i < docid.Length; i++)
                        {
                            DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                            DocumentDetailsResponse document = new DocumentDetailsResponse
                            {
                                id = documentDetails.Id,
                                documenturl = documentDetails.DocumentUrl,
                                name = documentDetails.name
                            };
                            documentlist.Add(document);
                        }
                        schoolDetailsResponse.schoollicense = documentlist;
                    }

                    if (!string.IsNullOrEmpty(schoolDetails.RegisterationPaper))
                    {
                        List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                        string[] docid = schoolDetails.RegisterationPaper.Split(",");
                        for (int i = 0; i < docid.Length; i++)
                        {
                            DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                            DocumentDetailsResponse document = new DocumentDetailsResponse
                            {
                                id = documentDetails.Id,
                                documenturl = documentDetails.DocumentUrl,
                                name = documentDetails.name
                            };
                            documentlist.Add(document);
                        }
                        schoolDetailsResponse.registerationpaper = documentlist;
                    }

                    SchoolDetailsResponseList.Add(schoolDetailsResponse);
                }

                successResponse.data = SchoolDetailsResponseList;
                successResponse.response_code = 0;
                successResponse.message = "schoolDetails";
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

        [HttpGet("GetSchoolList")]
        public IActionResult GetSchoolList(string search)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                var SchoolDetails = IndividualDetailsBusiness.GetSchoolList(search);
                successResponse.data = SchoolDetails;
                successResponse.response_code = 0;
                successResponse.message = "schoolDetails";
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

        [HttpGet("GetSchoolDetailById")]
        public IActionResult GetSchoolDetailById(long id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                SchoolDetails schoolDetails = IndividualDetailsBusiness.GetSchoolDetailById(id);

                if (schoolDetails == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Record not found";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(422, unsuccessResponse);
                }
                SchoolDetailsResponse schoolDetailsResponse = new SchoolDetailsResponse
                {
                    id = schoolDetails.Id,
                    registernumber = schoolDetails.RegisterNumber,
                    schooltypeid = schoolDetails.SchoolTypeId,
                    schoolname = schoolDetails.SchoolName,
                    schooladdress = schoolDetails.SchoolAddress,
                    sectiontypeid = schoolDetails.SectionTypeId,
                    numberofteachermale = schoolDetails.NumberOfTeacherMale,
                    numberofteacherfemale = schoolDetails.NumberOfTeacherFemale,
                    numberofstudentmale = schoolDetails.NumberOfStudentMale,
                    numberofstudentfemale = schoolDetails.NumberOfStudentFemale,
                    numberofstaffmale = schoolDetails.NumberOfStaffMale,
                    numberofstafffemale = schoolDetails.NumberOfStaffFemale,
                    poweraddressid = schoolDetails.PowerAddressId,
                    internetaccessid = schoolDetails.InternetAccessId,
                    buildingownershipid = schoolDetails.BuildingOwnershipId,
                    computers = schoolDetails.Computers,
                    monitors = schoolDetails.Monitors,
                    routers = schoolDetails.Routers,
                    dongles = schoolDetails.Dongles
                };

                if (!string.IsNullOrEmpty(schoolDetails.SchoolLicense))
                {
                    List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                    string[] docid = schoolDetails.SchoolLicense.Split(",");
                    for (int i = 0; i < docid.Length; i++)
                    {
                        DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                        DocumentDetailsResponse document = new DocumentDetailsResponse
                        {
                            id = documentDetails.Id,
                            documenturl = documentDetails.DocumentUrl,
                            name = documentDetails.name
                        };
                        documentlist.Add(document);
                    }
                    schoolDetailsResponse.schoollicense = documentlist;
                }

                if (!string.IsNullOrEmpty(schoolDetails.RegisterationPaper))
                {
                    List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                    string[] docid = schoolDetails.RegisterationPaper.Split(",");
                    for (int i = 0; i < docid.Length; i++)
                    {
                        DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                        DocumentDetailsResponse document = new DocumentDetailsResponse
                        {
                            id = documentDetails.Id,
                            documenturl = documentDetails.DocumentUrl,
                            name = documentDetails.name
                        };
                        documentlist.Add(document);
                    }
                    schoolDetailsResponse.registerationpaper = documentlist;
                }

                successResponse.data = schoolDetailsResponse;
                successResponse.response_code = 0;
                successResponse.message = "SchoolDetails";
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

        [HttpGet("GetParentByStudentId/{Id}")]
        public IActionResult GetParentByStudentId(long Id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                List<ParentUser> parentDetails = new List<ParentUser>();
                List<StudParent> studParent = usersBusiness.GetStudParentById(Id);
                if (studParent.Count > 0)
                {
                    foreach (var parent in studParent)
                    {
                        User parentUser = usersBusiness.GetUserbyId(parent.ParentId);
                        if (parentUser != null)
                        {
                            ParentUser parentUserDTO = new ParentUser
                            {
                                Id = parentUser.Id,
                                Username = parentUser.Username,
                                FullName = parentUser.FullName,
                                Email = parentUser.Email,
                                Bio = parentUser.Bio,
                                profilepicurl = parentUser.ProfilePicUrl
                            };

                            List<Role> parentroles = usersBusiness.Role(parentUser);
                            if (parentroles != null)
                            {
                                List<long> proleids = new List<long>();
                                List<string> prolenames = new List<string>();
                                foreach (var prole in parentroles)
                                {
                                    proleids.Add(prole.Id);
                                    prolenames.Add(prole.Name);
                                }
                                parentUserDTO.Roles = proleids;
                                parentUserDTO.RoleName = prolenames;
                            }
                            parentDetails.Add(parentUserDTO);
                        }
                    }
                }
                successResponse.data = parentDetails;
                successResponse.response_code = 0;
                successResponse.message = "parent details";
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

        [HttpDelete("RemoveParentUser")]
        public IActionResult RemoveParentUser(long studentid, long parentid)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                var studParent = usersBusiness.RemoveParent(studentid, parentid);
                successResponse.response_code = 0;
                successResponse.message = "parent removed";
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