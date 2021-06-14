using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.Contact;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.Feedback;
using Trainning24.BL.ViewModels.FeedBackCategory;
using Trainning24.BL.ViewModels.FeedbackTime;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Grade;
using Trainning24.BL.ViewModels.QuestionFile;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly EFContact EFContact;
        private readonly FeedbackBusiness FeedbackBusiness;
        private IHostingEnvironment hostingEnvironment;
        private readonly FilesBusiness FilesBusiness;
        private readonly EFFeedBackCategory EFFeedBackCategory;
        private readonly IMapper _mapper;
        private readonly EFGradeRepository EFGradeRepository;
        private readonly EFFeedBackTask EFFeedBackTask;
        private readonly EFChapterRepository EFChapterRepository;
        private readonly UsersBusiness UsersBusiness;
        private readonly EFCourseRepository EFCourseRepository;
        private readonly LessonBusiness LessonBusiness;

        public FeedbackController(
            EFContact EFContact,
            FeedbackBusiness FeedbackBusiness,
            IHostingEnvironment hostingEnvironment,
            IMapper mapper,
            EFGradeRepository EFGradeRepository,
            EFFeedBackCategory EFFeedBackCategory,
            UsersBusiness UsersBusiness,
            EFCourseRepository EFCourseRepository,
            EFChapterRepository EFChapterRepository,
            LessonBusiness LessonBusiness,
            EFFeedBackTask EFFeedBackTask,
            FilesBusiness FilesBusiness
            )
        {
            this.UsersBusiness = UsersBusiness;
            this.FeedbackBusiness = FeedbackBusiness;
            this.EFContact = EFContact;
            this.hostingEnvironment = hostingEnvironment;
            this.EFFeedBackTask = EFFeedBackTask;
            this.EFFeedBackCategory = EFFeedBackCategory;
            this.EFGradeRepository = EFGradeRepository;
            this.EFChapterRepository = EFChapterRepository;
            this.LessonBusiness = LessonBusiness;
            this.EFCourseRepository = EFCourseRepository;
            this.FilesBusiness = FilesBusiness;
            _mapper = mapper;
        }

        [HttpPost("AddContact")]
        public IActionResult AddContact(Contact contact)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {

                if (ModelState.IsValid)
                {
                    contact.CreatorUserId = 0;
                    Contact newContact = FeedbackBusiness.AddContact(contact);
                    successResponse.data = newContact;
                    successResponse.response_code = 0;
                    successResponse.message = "Contact added";
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

        [HttpPost("FeedbackStatusChanged")]
        public IActionResult FeedbackStatusChanged(FeedbackStatusChangedModel model)
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
                    FeedbackBusiness.FeedbackStatusChangedToCompleted(model, int.Parse(tc.Id));

                    successResponse.data = null;
                    successResponse.response_code = 0;
                    successResponse.message = "FeedbackStatusChanged";
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

        [HttpPost("AddFeedback")]
        public IActionResult AddFeedback(Feedback feedback)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = "";
            TokenClaims tc = new TokenClaims();

            if (!string.IsNullOrEmpty(Request.Headers["id_token"]))
            {
                //string Authorization = Request.Headers["Authorization"];
                Authorization = Request.Headers["id_token"];

                //tc = General.GetClaims(Authorization);
                //tc.Id = LessonBusiness.getUserId(tc.sub);
            }
            try
            {
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                if (ModelState.IsValid)
                {
                    int creatorid = 0;
                    if (tc.Id != null)
                        creatorid = int.Parse(tc.Id);
                    feedback.CreatorUserId = 0;
                    Feedback newFeedback = FeedbackBusiness.AddFeedback(feedback, creatorid);
                    successResponse.data = FeedbackBusiness.GetFeedback(newFeedback.Id, Certificate);
                    successResponse.response_code = 0;
                    successResponse.message = "Feedback added";
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

        [HttpPost("AddFeedbackTime")]
        public async Task<IActionResult> AddFeedbackTimeAsync()
        {
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);

            List<string> audiomimetypelist = new List<string>();
            audiomimetypelist.Add("audio/basic");
            audiomimetypelist.Add("audio/mpeg");
            audiomimetypelist.Add("audio/mp3");
            audiomimetypelist.Add("audio/mpeg");
            audiomimetypelist.Add("audio/x-aiff");
            audiomimetypelist.Add("audio/x-mpegurl");
            audiomimetypelist.Add("audio/x-pn-realaudio");
            audiomimetypelist.Add("audio/x-wav");
            audiomimetypelist.Add("audio/wav");

            List<string> imagemimetypelist = new List<string>();
            imagemimetypelist.Add("image/bmp");
            imagemimetypelist.Add("image/cis-cod");
            imagemimetypelist.Add("image/gif");
            imagemimetypelist.Add("image/ief");
            imagemimetypelist.Add("image/jpeg");
            imagemimetypelist.Add("image/pipeg");
            imagemimetypelist.Add("image/svg+xml");
            imagemimetypelist.Add("image/tiff");
            imagemimetypelist.Add("image/x-cmu-raster");
            imagemimetypelist.Add("image/x-cmx");
            imagemimetypelist.Add("image/x-icon");
            imagemimetypelist.Add("image/x-portable-anymap");
            imagemimetypelist.Add("image/x-portable-bitmap");
            imagemimetypelist.Add("image/x-portable-graymap");
            imagemimetypelist.Add("image/x-portable-pixmap");
            imagemimetypelist.Add("image/x-rgb");
            imagemimetypelist.Add("image/x-xbitmap");
            imagemimetypelist.Add("image/x-xpixmap");
            imagemimetypelist.Add("image/x-xwindowdump");
            imagemimetypelist.Add("image/png");

            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    string desc = Request.Form["Description"];
                    string tym = Request.Form["Time"];
                    string fdbackid = Request.Form["FeedbackId"];

                    FeebackTime feebackTime = new FeebackTime
                    {
                        Description = string.IsNullOrEmpty(desc) ? "" : desc,
                        Time = string.IsNullOrEmpty(tym) ? "" : tym,
                        FeedbackId = long.Parse(fdbackid),
                        CreatorUserId = 0
                    };

                    ResponseFeedbackTime responseFeedbackTime = new ResponseFeedbackTime();
                    FeebackTime newFeebackTime = FeedbackBusiness.AddFeedbackTime(feebackTime);
                    responseFeedbackTime.Id = newFeebackTime.Id;
                    List<UpdateQuestionFileModel> feedbackTimeFiles = new List<UpdateQuestionFileModel>();

                    var allFiles = Request.Form.Files.ToList();

                    List<DurationModel> durationList = JsonConvert.DeserializeObject<List<DurationModel>>(Request.Form["duration"]);//1219
                
                    for (int k = 0; k < allFiles.Count; k++)
                    {
                        string tmpfileName = "";
                        string fileName = "";
                        IFormFile file = null;
                        if (Request.Form.Files.Count != 0)
                            file = Request.Form.Files[k];
                        var imageAcl = PredefinedObjectAcl.PublicRead;
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        tmpfileName = fileName;                      

                        var ext = fileName.Substring(fileName.LastIndexOf("."));
                        var extension = ext.ToLower();
                        Guid imageGuid = Guid.NewGuid();
                        fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;
                        string mediaLink = "";

                        DurationModel durationsingle = new DurationModel();
                        string bucket = "";
                        if (audiomimetypelist.Contains(file.ContentType))
                        {                            
                            bucket = "t24-primary-audio-storage";
                            durationsingle = durationList.Where(b => b.name == tmpfileName).Single();
                        }
                        if(imagemimetypelist.Contains(file.ContentType))
                        {
                            bucket = "t24-primary-image-storage";
                        }

                        var imageObject = await storage.UploadObjectAsync(
                                            bucket: bucket,
                                            objectName: fileName,
                                            contentType: file.ContentType,
                                            source: file.OpenReadStream(),
                                            options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                                        );

                        mediaLink = imageObject.MediaLink;

                        AddFilesModel FilesModel = new AddFilesModel();
                        FilesModel.Url = mediaLink;
                        FilesModel.Name = fileName;
                        FilesModel.FileName = fileName;

                        if(durationsingle !=null )
                        FilesModel.Duration = durationsingle.duration;

                        if (audiomimetypelist.Contains(file.ContentType))
                        {
                            FilesModel.FileTypeId = 5;
                        }
                        if (imagemimetypelist.Contains(file.ContentType))
                        {
                            FilesModel.FileTypeId = 3;
                        }

                        FilesModel.FileSize = file.Length;
                        Files newFiles = FilesBusiness.Create(FilesModel, 0);

                        UpdateQuestionFileModel singleanswerFile = new UpdateQuestionFileModel();
                        singleanswerFile.fileid = newFiles.Id;
                        singleanswerFile.Url = newFiles.Url;
                        singleanswerFile.duration = newFiles.Duration;
                        feedbackTimeFiles.Add(singleanswerFile);

                        FeedbackFile feedbackFile = new FeedbackFile
                        {
                            FileId = newFiles.Id,
                            FeedtimeId = newFeebackTime.Id
                        };

                        FeedbackFile newFeedbackFile = FeedbackBusiness.AddFeedbackFile(feedbackFile);
                    }

                    responseFeedbackTime.feedbackTimeFiles = feedbackTimeFiles;
                    successResponse.data = responseFeedbackTime;
                    successResponse.response_code = 0;
                    successResponse.message = "FeedbackTime added";
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

        [HttpGet("GetFeedBackCategory")]
        public IActionResult GetFeedBackCategory()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                if (ModelState.IsValid)
                {
                    List<FeedBackCategory> FeedBackCategory = FeedbackBusiness.GetFeedBackCategory();

                    successResponse.data = FeedBackCategory;
                    successResponse.response_code = 0;
                    successResponse.message = "FeedBackCategory Detail";
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

        [Authorize]
        [HttpGet("GetFeedBackById/{id}")]
        public IActionResult GetFeedBackById(long id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx

                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (
                    tc.RoleName.Contains(General.getRoleType("1")) ||
                    tc.RoleName.Contains(General.getRoleType("5")) ||
                    tc.RoleName.Contains(General.getRoleType("6")) ||
                    tc.RoleName.Contains(General.getRoleType("7")) ||
                    tc.RoleName.Contains(General.getRoleType("8")) ||
                    tc.RoleName.Contains(General.getRoleType("9")) ||
                    tc.RoleName.Contains(General.getRoleType("10")) ||
                    tc.RoleName.Contains(General.getRoleType("11")) ||
                    tc.RoleName.Contains(General.getRoleType("12")) ||
                    tc.RoleName.Contains(General.getRoleType("14")) ||
                    tc.RoleName.Contains(General.getRoleType("15")) ||
                    tc.RoleName.Contains(General.getRoleType("16"))
                  )
                {
                    successResponse.data = FeedbackBusiness.GetFeedback(id, Certificate);
                    successResponse.response_code = 0;
                    successResponse.message = "Feedback Detail";
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
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }

        }

        [Authorize]
        [HttpGet("GetAllFeedBack")]
        public IActionResult GetAllFeedBack()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (
                    tc.RoleName.Contains(General.getRoleType("1")) ||
                    tc.RoleName.Contains(General.getRoleType("5")) ||
                    tc.RoleName.Contains(General.getRoleType("6")) ||
                    tc.RoleName.Contains(General.getRoleType("7")) ||
                    tc.RoleName.Contains(General.getRoleType("8")) ||
                    tc.RoleName.Contains(General.getRoleType("9")) ||
                    tc.RoleName.Contains(General.getRoleType("10")) ||
                    tc.RoleName.Contains(General.getRoleType("11")) ||
                    tc.RoleName.Contains(General.getRoleType("12")) ||
                    tc.RoleName.Contains(General.getRoleType("14")) ||
                    tc.RoleName.Contains(General.getRoleType("15")) ||
                    tc.RoleName.Contains(General.getRoleType("16"))
                  )
                {
                    //string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx

                    successResponse.data = FeedbackBusiness.GetAllFeedBack();
                    successResponse.response_code = 0;
                    successResponse.message = "Feedback Detail";
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
            catch (Exception ex)
            {
                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = "Failure";
                return StatusCode(500, unsuccessResponse);
            }
        }

        [Authorize]
        [HttpPost("GetAllFeedBackTest")]
        public IActionResult GetAllFeedBackTest(FeedBackFilterModel fdbackmodel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (
                    tc.RoleName.Contains(General.getRoleType("1")) ||
                    tc.RoleName.Contains(General.getRoleType("5")) ||
                    tc.RoleName.Contains(General.getRoleType("6")) ||
                    tc.RoleName.Contains(General.getRoleType("7")) ||
                    tc.RoleName.Contains(General.getRoleType("8")) ||
                    tc.RoleName.Contains(General.getRoleType("9")) ||
                    tc.RoleName.Contains(General.getRoleType("10")) ||
                    tc.RoleName.Contains(General.getRoleType("11")) ||
                    tc.RoleName.Contains(General.getRoleType("12")) ||
                    tc.RoleName.Contains(General.getRoleType("14")) ||
                    tc.RoleName.Contains(General.getRoleType("15")) ||
                    tc.RoleName.Contains(General.getRoleType("16"))
                  )
                {
                    //string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx

                    successResponse.data = FeedbackBusiness.GetAllFeedBacktest(fdbackmodel, tc.RoleName, long.Parse(tc.Id));
                    successResponse.response_code = 0;
                    successResponse.message = "Feedback Detail";
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