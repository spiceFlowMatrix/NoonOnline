using System;
using System.IO;
using System.Threading.Tasks;
using DinkToPdf;
using DinkToPdf.Contracts;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.Files;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CreatePdfController : ControllerBase
    {
        private IHostingEnvironment hostingEnvironment;
        private IConverter _converter;
        private readonly LessonBusiness LessonBusiness;
        private readonly FilesBusiness FilesBusiness;

        public CreatePdfController(
            IHostingEnvironment hostingEnvironment, 
            IConverter converter,
            LessonBusiness LessonBusiness,
            FilesBusiness FilesBusiness
        )
        {
            this.hostingEnvironment = hostingEnvironment;
            this.LessonBusiness = LessonBusiness;
            this.FilesBusiness = FilesBusiness;
            _converter = converter;
        }

        [HttpPost]
        public async Task<IActionResult> CreatePdfAsync(ReceiptModel receiptModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                var credential = GoogleCredential.FromFile(jsonPath);
                var storage = StorageClient.Create(credential);
                var globalSettings = new GlobalSettings
                {
                    ColorMode = ColorMode.Color,
                    Orientation = Orientation.Portrait,
                    PaperSize = PaperKind.A4,
                    Margins = new MarginSettings { Top = 10 },
                    DocumentTitle = "PDF Report"
                };
                var objectSettings = new ObjectSettings
                {
                    HtmlContent = TemplateGenerator.GetHTMLString(receiptModel),
                    WebSettings = { DefaultEncoding = "utf-8", UserStyleSheet = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "styles.css") },
                    HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Report Footer" }
                };
                var pdf = new HtmlToPdfDocument()
                {
                    GlobalSettings = globalSettings,
                    Objects = { objectSettings }
                };

                Guid imageGuid = Guid.NewGuid();
                string fileName = "Receipt-" + imageGuid.ToString() + ".pdf";

                string mediaLink = "";
                byte[] ftest = _converter.Convert(pdf);
                Stream stream = new MemoryStream(_converter.Convert(pdf));
                var imageAcl = PredefinedObjectAcl.PublicRead;               
                var imageObject = await storage.UploadObjectAsync(
                                    bucket: "t24-primary-pdf-storage",
                                    objectName: fileName,
                                    contentType: "application/pdf",
                                    source: stream,
                                    options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                                );
                mediaLink = imageObject.MediaLink;

                AddFilesModel FilesModel = new AddFilesModel();
                FilesModel.Url = mediaLink;
                FilesModel.Name = fileName;
                FilesModel.FileName = fileName;
                FilesModel.FileTypeId = 1;
                Files newFiles = FilesBusiness.Create(FilesModel, int.Parse(tc.Id));
                string imageurl = "";
                if (!string.IsNullOrEmpty(mediaLink))
                {
                    imageurl = LessonBusiness.geturl(mediaLink, Certificate);
                }

                FilesModel.Url = imageurl;
                successResponse.data = FilesModel;
                successResponse.response_code = 0;
                successResponse.message = "Receipt Created";
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
    }
}