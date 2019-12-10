using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Deposit;
using Trainning24.BL.ViewModels.IndividualDetails;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DepositController : ControllerBase
    {
        private readonly DepositBusiness DepositBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly SalesAgentBusiness SalesAgentBusiness;
        private IHostingEnvironment hostingEnvironment;
        private readonly IndividualDetailsBusiness IndividualDetailsBusiness;

        public DepositController
        (
            IHostingEnvironment hostingEnvironment,
            DepositBusiness DepositBusiness,
            SalesAgentBusiness SalesAgentBusiness,
            LessonBusiness LessonBusiness,
            IndividualDetailsBusiness IndividualDetailsBusiness
        )
        {
            this.hostingEnvironment = hostingEnvironment;
            this.LessonBusiness = LessonBusiness;
            this.SalesAgentBusiness = SalesAgentBusiness;
            this.DepositBusiness = DepositBusiness;
            this.IndividualDetailsBusiness = IndividualDetailsBusiness;
        }

        [HttpPost]
        public async Task<IActionResult> Post()
        {
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);

            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                //get claims after decoding id_token 
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (ModelState.IsValid)
                {
                    var allFiles = Request.Form.Files.ToList();
                    Deposit Deposit = new Deposit();
                    Deposit.DepositAmount = decimal.Parse(Request.Form["depositamount"]);
                    Deposit.DepositDate = Request.Form["depositdate"];
                    Deposit.SalesAgentId = long.Parse(Request.Form["salesagentid"]);

                    for (int k = 0; k < allFiles.Count; k++)
                    {
                        string fileName = "";
                        IFormFile file = null;
                        if (Request.Form.Files.Count != 0)
                            file = Request.Form.Files[k];
                        var imageAcl = PredefinedObjectAcl.PublicRead;
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        var ext = fileName.Substring(fileName.LastIndexOf("."));
                        var extension = ext.ToLower();
                        Guid imageGuid = Guid.NewGuid();
                        fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;
                        string mediaLink = "";
                        var imageObject = await storage.UploadObjectAsync(
                            bucket: "t24-primary-pdf-storage",
                            objectName: fileName,
                            contentType: file.ContentType,
                            source: file.OpenReadStream(),
                            options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        );
                        mediaLink = imageObject.MediaLink;

                        DocumentDetails documentDetails = IndividualDetailsBusiness.CreateDocumentDetails(new DocumentDetails { DocumentUrl = mediaLink,name = fileName }, int.Parse(tc.Id));

                        if (string.IsNullOrEmpty(Deposit.DocumentIds))
                        {
                            Deposit.DocumentIds = documentDetails.Id.ToString();
                        }
                        else
                        {
                            Deposit.DocumentIds = Deposit.DocumentIds +"," + documentDetails.Id;
                        }
                    }

                    Deposit newDeposit = new Deposit();

                    if (!string.IsNullOrEmpty(Request.Form["id"]))
                    {
                        Deposit.Id = long.Parse(Request.Form["id"]);

                        if (!string.IsNullOrEmpty(Request.Form["isrevoke"]))
                        {
                            Deposit.IsRevoke = bool.Parse(Request.Form["isrevoke"]);
                        }
                        if (!string.IsNullOrEmpty(Request.Form["isconfirm"]))
                        {
                            Deposit.IsConfirm = bool.Parse(Request.Form["isconfirm"]);
                        }

                        newDeposit = DepositBusiness.Update(Deposit, int.Parse(tc.Id));
                    }
                    else
                    {
                        newDeposit = DepositBusiness.Create(Deposit, int.Parse(tc.Id));
                    }

                    ResponseDeposit responseDeposit = new ResponseDeposit
                    {
                        id = newDeposit.Id,
                        depositamount = newDeposit.DepositAmount,
                        salesagentid = newDeposit.SalesAgentId,
                        salesgentname = SalesAgentBusiness.getSalesAgentById(newDeposit.SalesAgentId).AgentName,
                        depositdate = newDeposit.DepositDate,
                        isconfirm = newDeposit.IsConfirm,
                        isrevoke = newDeposit.IsRevoke
                    };

                    string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx

                    if (!string.IsNullOrEmpty(newDeposit.DocumentIds))
                    {
                        List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                        string[] docid = newDeposit.DocumentIds.Split(",");
                        for (int i = 0; i < docid.Length; i++)
                        {
                            DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                            DocumentDetailsResponse document = new DocumentDetailsResponse
                            {
                                id = documentDetails.Id,
                                name = documentDetails.name
                            };
                            if (!string.IsNullOrEmpty(documentDetails.DocumentUrl))
                                document.documenturl = LessonBusiness.geturl(documentDetails.DocumentUrl, Certificate);

                            documentlist.Add(document);
                        }
                        responseDeposit.documentid = documentlist;
                    }

                    successResponse.data = responseDeposit;
                    successResponse.response_code = 0;
                    successResponse.message = "deposit Created";
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

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                //get claims after decoding id_token 
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                Deposit newDeposit = DepositBusiness.getGradeById(id);

                ResponseDeposit responseDeposit = new ResponseDeposit
                {
                    id = newDeposit.Id,
                    depositamount = newDeposit.DepositAmount,
                    salesagentid = newDeposit.SalesAgentId,
                    salesgentname = SalesAgentBusiness.getSalesAgentById(newDeposit.SalesAgentId).AgentName,
                    depositdate = newDeposit.DepositDate,
                    isconfirm = newDeposit.IsConfirm,
                    isrevoke = newDeposit.IsRevoke
                };

                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx

                if (!string.IsNullOrEmpty(newDeposit.DocumentIds))
                {
                    List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                    string[] docid = newDeposit.DocumentIds.Split(",");
                    for (int i = 0; i < docid.Length; i++)
                    {
                        DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                        DocumentDetailsResponse document = new DocumentDetailsResponse
                        {
                            id = documentDetails.Id,
                            name = documentDetails.name
                        };
                        if (!string.IsNullOrEmpty(documentDetails.DocumentUrl))
                            document.documenturl = LessonBusiness.geturl(documentDetails.DocumentUrl, Certificate);
                            documentlist.Add(document);
                    }
                    responseDeposit.documentid = documentlist;
                }

                successResponse.data = responseDeposit;
                successResponse.response_code = 0;
                successResponse.message = "deposit Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch(Exception ex)
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
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                //get claims after decoding id_token 
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                List<Deposit> deposits = DepositBusiness.DepositList(paginationModel,out int total);

                if (deposits.Count == 0)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Record not found";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(422, unsuccessResponse);
                }

                List<ResponseDeposit> responseDeposits = new List<ResponseDeposit>();
                foreach(var newDeposit in deposits)
                {
                    ResponseDeposit responseDeposit = new ResponseDeposit
                    {
                        id = newDeposit.Id,
                        depositamount = newDeposit.DepositAmount,
                        salesagentid = newDeposit.SalesAgentId,
                        depositdate = newDeposit.DepositDate,
                        isconfirm = newDeposit.IsConfirm,
                        isrevoke = newDeposit.IsRevoke
                    };


                    // Here is the change
                    SalesAgent salesAgent = SalesAgentBusiness.getSalesAgentById(newDeposit.SalesAgentId);
                    if (salesAgent != null)
                        responseDeposit.salesgentname = SalesAgentBusiness.getSalesAgentById(newDeposit.SalesAgentId).AgentName;

                    string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx

                    if (!string.IsNullOrEmpty(newDeposit.DocumentIds))
                    {
                        List<DocumentDetailsResponse> documentlist = new List<DocumentDetailsResponse>();
                        string[] docid = newDeposit.DocumentIds.Split(",");
                        for (int i = 0; i < docid.Length; i++)
                        {
                            DocumentDetails documentDetails = IndividualDetailsBusiness.GetDocumentById(long.Parse(docid[i]));
                            DocumentDetailsResponse document = new DocumentDetailsResponse
                            {
                                id = documentDetails.Id,
                                name = documentDetails.name                              
                            };
                            if (!string.IsNullOrEmpty(documentDetails.DocumentUrl))
                                document.documenturl = LessonBusiness.geturl(documentDetails.DocumentUrl, Certificate);
                                documentlist.Add(document);
                        }
                        responseDeposit.documentid = documentlist;
                    }
                    responseDeposits.Add(responseDeposit);
                }

                successResponse.totalcount = total;
                successResponse.data = responseDeposits;
                successResponse.response_code = 0;
                successResponse.message = "responseDeposits";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch(Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpGet("GetAccountSummary")]
        public IActionResult GetAccountSummary()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                //TokenClaims tc = General.GetClaims(Authorization);

                List<AgentAccountSummary> agentAccountSummary = DepositBusiness.GetAccountSummary();

                successResponse.data = agentAccountSummary;
                successResponse.response_code = 0;
                successResponse.message = "AccountSummary";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch(Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }


        [HttpGet("GetAgentSummaryDetails/{id}")]
        public IActionResult GetAgentSummaryDetails(long id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                AgentAccountSummary agentAccountSummary = DepositBusiness.GetAgentSummaryDetails(id);
                successResponse.data = agentAccountSummary;
                successResponse.response_code = 0;
                successResponse.message = "AccountSummary";
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