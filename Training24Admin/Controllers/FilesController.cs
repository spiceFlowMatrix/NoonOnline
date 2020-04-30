using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using System.Web;
using AutoMapper;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly FilesBusiness FilesBusiness;
        private readonly LessonBusiness LessonBusiness;
        private IHostingEnvironment hostingEnvironment;
        private readonly IMapper _mapper;
        private IConfiguration Configuration { get; }

        public FilesController
        (
            IHostingEnvironment hostingEnvironment,
            FilesBusiness FilesBusiness,
            LessonBusiness LessonBusiness,
            IMapper mapper,
            IConfiguration Configuration
        )
        {
            this.hostingEnvironment = hostingEnvironment;
            this.FilesBusiness = FilesBusiness;
            this.LessonBusiness = LessonBusiness;
            this.Configuration = Configuration;
            _mapper = mapper;
        }

        [HttpPost]
        [RequestSizeLimit(1073741824)]
        public async Task<IActionResult> Post()
        {
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);

            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    string fileName = "";
                    IFormFile file = null;
                    if (Request.Form.Files.Count != 0)
                        file = Request.Form.Files[0];

                    var imageAcl = PredefinedObjectAcl.PublicRead;

                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var ext = fileName.Substring(fileName.LastIndexOf("."));
                    var extension = ext.ToLower();
                    Guid imageGuid = Guid.NewGuid();
                    fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                    if (Request.Form["fileTypeId"] == "1")
                    {
                        var imageObject = await storage.UploadObjectAsync(
                            bucket: "t24-primary-pdf-storage",
                            objectName: fileName,
                            contentType: file.ContentType,
                            source: file.OpenReadStream(),
                            options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        );
                        mediaLink = imageObject.MediaLink;
                    }

                    if (Request.Form["fileTypeId"] == "2")
                    {
                        var imageObject = await storage.UploadObjectAsync(
                            bucket: "t24-primary-video-storage",
                            objectName: fileName,
                            contentType: file.ContentType,
                            source: file.OpenReadStream(),
                            options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        );
                        mediaLink = imageObject.MediaLink;
                    }

                    if (Request.Form["fileTypeId"] == "3")
                    {
                        var imageObject = await storage.UploadObjectAsync(
                            bucket: "t24-primary-image-storage",
                            objectName: fileName,
                            contentType: file.ContentType,
                            source: file.OpenReadStream(),
                            options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        );
                        mediaLink = imageObject.MediaLink;
                    }

                    if (Request.Form["fileTypeId"] == "4")
                    {
                        var imageObject = await storage.UploadObjectAsync(
                            bucket: "t24-primary-image-storage",
                            objectName: fileName,
                            contentType: file.ContentType,
                            source: file.OpenReadStream(),
                            options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        );
                        mediaLink = imageObject.MediaLink;
                    }

                    if (Request.Form["fileTypeId"] == "6" || Request.Form["fileTypeId"] == "7" || Request.Form["fileTypeId"] == "8")
                    {
                        var imageObject = await storage.UploadObjectAsync(
                            bucket: "t24-primary-pdf-storage",
                            objectName: fileName,
                            contentType: file.ContentType,
                            source: file.OpenReadStream(),
                            options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        );
                        mediaLink = imageObject.MediaLink;
                    }

                    AddFilesModel FilesModel = new AddFilesModel();
                    if (!string.IsNullOrEmpty(Request.Form["description"].ToString()))
                        FilesModel.Description = Request.Form["description"];
                    FilesModel.Url = mediaLink;
                    FilesModel.Name = fileName;
                    FilesModel.FileName = fileName;
                    FilesModel.FileTypeId = long.Parse(Request.Form["fileTypeId"]);
                    FilesModel.FileSize = file.Length;
                    if (!string.IsNullOrEmpty(Request.Form["duration"].ToString()))
                        FilesModel.Duration = Request.Form["duration"].ToString();
                    if (!string.IsNullOrEmpty(Request.Form["totalpages"]))
                        FilesModel.TotalPages = int.Parse(Request.Form["totalpages"]);
                    Files newFiles = FilesBusiness.Create(FilesModel, int.Parse(tc.Id));
                    var filetype = FilesBusiness.FileType(newFiles);
                    ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                    responseFilesModel.Id = newFiles.Id;
                    responseFilesModel.name = newFiles.Name;
                    responseFilesModel.url = newFiles.Url;
                    responseFilesModel.filename = newFiles.FileName;
                    responseFilesModel.description = newFiles.Description;
                    responseFilesModel.filetypeid = newFiles.FileTypeId;
                    responseFilesModel.filesize = newFiles.FileSize;
                    responseFilesModel.filetypename = filetype.Filetype;
                    responseFilesModel.duration = newFiles.Duration;
                    responseFilesModel.totalpages = newFiles.TotalPages;

                    successResponse.data = responseFilesModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Files Created";
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
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }


        //[HttpPost("FileDetail")]
        //public IActionResult FileDetail()
        //{
        //    SuccessResponse successResponse = new SuccessResponse();
        //    UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

        //    try
        //    {
        //        string fileName = "";
        //        IFormFile file = null;
        //        //if (Request.Form.Files.Count != 0)
        //        //    file = Request.Form.Files[0];

        //        if (Request.Form.Files.Count != 0)
        //            file = Request.Form.Files[0];

        //        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //        var ext = fileName.Substring(fileName.LastIndexOf("."));
        //        var extension = ext.ToLower();


        //        successResponse.data = new { file.Name, file.FileName, file.Length , extension };
        //        successResponse.response_code = 0;
        //        successResponse.message = "Files Created";
        //        successResponse.status = "Success";
        //        return StatusCode(200, successResponse);
        //    }
        //    catch (Exception ex)
        //    {
        //        unsuccessResponse.response_code = 2;
        //        unsuccessResponse.message = ex.Message;
        //        unsuccessResponse.status = "Failure";
        //        return StatusCode(500, unsuccessResponse);
        //    }
        //}

        [HttpPut("{id}")]
        public IActionResult Put(int id, UpdateFilesModel updateFilesModel)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (ModelState.IsValid)
                {
                    if (
                        tc.RoleName.Contains(General.getRoleType("1")) ||
                        tc.RoleName.Contains(General.getRoleType("3"))
                        )
                    {
                        updateFilesModel.Id = id;
                        Files newFiles = FilesBusiness.Update(updateFilesModel, int.Parse(tc.Id));
                        ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                        var filetyped = FilesBusiness.FileType(newFiles);

                        responseFilesModel.Id = newFiles.Id;
                        responseFilesModel.name = newFiles.Name;
                        responseFilesModel.url = newFiles.Url;
                        responseFilesModel.filename = newFiles.FileName;
                        responseFilesModel.filetypeid = newFiles.FileTypeId;
                        responseFilesModel.description = newFiles.Description;
                        responseFilesModel.filesize = newFiles.FileSize;
                        responseFilesModel.filetypename = filetyped.Filetype;

                        successResponse.data = responseFilesModel;
                        successResponse.response_code = 0;
                        successResponse.message = "Files updated";
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
        public IActionResult Delete(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    Files newFiles = FilesBusiness.Delete(id, int.Parse(tc.Id));
                    ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                    var filetyped = FilesBusiness.FileType(newFiles);

                    responseFilesModel.Id = newFiles.Id;
                    responseFilesModel.name = newFiles.Name;
                    responseFilesModel.url = newFiles.Url;
                    responseFilesModel.filename = newFiles.FileName;
                    responseFilesModel.filetypeid = newFiles.FileTypeId;
                    responseFilesModel.description = newFiles.Description;
                    responseFilesModel.filetypename = filetyped.Filetype;
                    responseFilesModel.filesize = newFiles.FileSize;

                    successResponse.data = responseFilesModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Files Deleted";
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
        public IActionResult Get(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    Files newFiles = FilesBusiness.getFilesById(id);
                    ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                    var filetyped = FilesBusiness.FileType(newFiles);
                    responseFilesModel.Id = newFiles.Id;
                    responseFilesModel.name = newFiles.Name;
                    responseFilesModel.url = newFiles.Url;
                    responseFilesModel.filename = newFiles.FileName;
                    responseFilesModel.filetypeid = newFiles.FileTypeId;
                    responseFilesModel.description = newFiles.Description;
                    responseFilesModel.filetypename = filetyped.Filetype;
                    responseFilesModel.filesize = newFiles.FileSize;
                    responseFilesModel.duration = newFiles.Duration;
                    responseFilesModel.totalpages = newFiles.TotalPages;
                    successResponse.data = responseFilesModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Files Detail";
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
        public IActionResult Get(int pagenumber, int perpagerecord, int filetype, string search)
        {
            PaginationModel paginationModel = new PaginationModel();
            paginationModel.pagenumber = pagenumber;
            paginationModel.perpagerecord = perpagerecord;
            paginationModel.roleid = filetype;
            paginationModel.search = search;
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                List<ResponseFilesModel> FilesResponseList = new List<ResponseFilesModel>();
                List<Files> FilesList = FilesBusiness.FilesList(paginationModel, out int total);
                foreach (var newFiles in FilesList)
                {
                    ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                    var filetyped = FilesBusiness.FileType(newFiles);

                    responseFilesModel.Id = newFiles.Id;
                    responseFilesModel.name = newFiles.Name;
                    responseFilesModel.url = newFiles.Url;
                    responseFilesModel.filename = newFiles.FileName;
                    responseFilesModel.filetypeid = newFiles.FileTypeId;
                    responseFilesModel.description = newFiles.Description;
                    responseFilesModel.filetypename = filetyped.Filetype;
                    responseFilesModel.filesize = newFiles.FileSize;
                    responseFilesModel.duration = newFiles.Duration;
                    responseFilesModel.totalpages = newFiles.TotalPages;
                    FilesResponseList.Add(responseFilesModel);
                }
                successResponse.data = FilesResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Files Details";
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

        [HttpGet("ImportVideos")]
        public IActionResult ImportVideos()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                var credential = GoogleCredential.FromFile(jsonPath);
                var storage = StorageClient.Create(credential);

                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (
                tc.RoleName.Contains(General.getRoleType("1")) ||
                tc.RoleName.Contains(General.getRoleType("3"))
                )
                {
                    foreach (var obj in storage.ListObjects("t24-primary-video-storage", ""))
                    {
                        Files file = FilesBusiness.GetByUrl(obj.MediaLink);
                        if (file == null)
                        {
                            AddFilesModel FilesModel = new AddFilesModel();
                            FilesModel.Url = obj.MediaLink;
                            FilesModel.Name = obj.Name;
                            FilesModel.FileName = obj.Name;
                            FilesModel.FileTypeId = 2;
                            FilesModel.FileSize = long.Parse(obj.Size.ToString());
                            Files newFiles = FilesBusiness.Create(FilesModel, int.Parse("1"));
                        }
                    }

                    successResponse.data = null;
                    successResponse.response_code = 0;
                    successResponse.message = "Video Imported";
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

        [HttpGet("ImportImages")]
        public IActionResult ImportImages()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                var credential = GoogleCredential.FromFile(jsonPath);
                var storage = StorageClient.Create(credential);

                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (
                tc.RoleName.Contains(General.getRoleType("1")) ||
                tc.RoleName.Contains(General.getRoleType("3"))
                )
                {
                    foreach (var obj in storage.ListObjects("t24-primary-image-storage", ""))
                    {
                        Files file = FilesBusiness.GetByUrl(obj.MediaLink);
                        if (file == null)
                        {
                            AddFilesModel FilesModel = new AddFilesModel();
                            FilesModel.Url = obj.MediaLink;
                            FilesModel.Name = obj.Name;
                            FilesModel.FileName = obj.Name;
                            FilesModel.FileTypeId = 3;
                            FilesModel.FileSize = long.Parse(obj.Size.ToString());
                            Files newFiles = FilesBusiness.Create(FilesModel, int.Parse("1"));
                        }
                    }

                    successResponse.data = null;
                    successResponse.response_code = 0;
                    successResponse.message = "Image Imported";
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

        [HttpGet("ImportPdf")]
        public IActionResult ImportPdf()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                var credential = GoogleCredential.FromFile(jsonPath);
                var storage = StorageClient.Create(credential);

                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                if (
                tc.RoleName.Contains(General.getRoleType("1")) ||
                tc.RoleName.Contains(General.getRoleType("3"))
                )
                {
                    foreach (var obj in storage.ListObjects("t24-primary-pdf-storage", ""))
                    {
                        Files file = FilesBusiness.GetByUrl(obj.MediaLink);
                        if (file == null)
                        {
                            AddFilesModel FilesModel = new AddFilesModel();
                            FilesModel.Url = obj.MediaLink;
                            FilesModel.Name = obj.Name;
                            FilesModel.FileName = obj.Name;
                            FilesModel.FileTypeId = 1;
                            FilesModel.FileSize = long.Parse(obj.Size.ToString());
                            Files newFiles = FilesBusiness.Create(FilesModel, int.Parse("1"));
                        }
                    }

                    successResponse.data = null;
                    successResponse.response_code = 0;
                    successResponse.message = "PDF Imported";
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

        [HttpGet("GetSignedUrl")]
        public IActionResult GetSignedUrl(long fileid, long lessonid)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            SingnedUrlResponse singnedUrlResponse = new SingnedUrlResponse();
            try
            {
                Files newFiles = FilesBusiness.getFilesById(fileid);
                string bucketName = General.getBucketName(newFiles.FileTypeId);

                TimeSpan timeSpan = TimeSpan.FromMinutes(2);
                double exp = timeSpan.TotalMilliseconds;
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");

                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(Certificate);
                string url = urlSigner.Sign(
                                               bucketName,
                                               newFiles.FileName,
                                               timeSpan,
                                               HttpMethod.Get
                                          );

                singnedUrlResponse.url = url;
                singnedUrlResponse.exp = exp;

                successResponse.data = singnedUrlResponse;
                successResponse.response_code = 0;
                successResponse.message = "Singed url";
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

        [HttpGet("GetFileList")]
        public IActionResult GetFlieList(int pagenumber, int perpagerecord, int filetype, string search)
        {
            PaginationModel paginationModel = new PaginationModel();
            paginationModel.pagenumber = pagenumber;
            paginationModel.perpagerecord = perpagerecord;
            paginationModel.roleid = filetype;
            paginationModel.search = search;
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                //List<ResponseFilesModel> FilesResponseList = new List<ResponseFilesModel>();
                List<Files> FilesList = FilesBusiness.FilesList(paginationModel, out int total);
                //List<ResponseFilesModel1> FilesResponseList = _mapper.Map<List<Files>, List<ResponseFilesModel1>>(FilesList);


                List<ResponseFilesModel1> FilesResponseList = new List<ResponseFilesModel1>();
                foreach (var newFiles in FilesList)
                {
                    ResponseFilesModel1 responseFilesModel = new ResponseFilesModel1();
                    var filetyped = FilesBusiness.FileType(newFiles);

                    responseFilesModel.Id = newFiles.Id;
                    responseFilesModel.name = newFiles.Name;
                    //responseFilesModel.url = newFiles.Url;
                    //responseFilesModel.filename = newFiles.FileName;
                    responseFilesModel.filetypeid = newFiles.FileTypeId;
                    //responseFilesModel.description = newFiles.Description;
                    responseFilesModel.filetypename = filetyped.Filetype;
                    //responseFilesModel.filesize = newFiles.FileSize;
                    //responseFilesModel.duration = newFiles.Duration;
                    //responseFilesModel.totalpages = newFiles.TotalPages;
                    FilesResponseList.Add(responseFilesModel);
                }
                successResponse.data = FilesResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Files Details";
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

        [HttpGet("GetFileListNew")]
        public IActionResult GetFileListNew(int pagenumber, int perpagerecord, int filetype, string search)
        {
            PaginationModel paginationModel = new PaginationModel();
            paginationModel.pagenumber = pagenumber;
            paginationModel.perpagerecord = perpagerecord;
            paginationModel.roleid = filetype;
            paginationModel.search = search;
            PaginationResponse successResponse = new PaginationResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                List<Files> FilesList = FilesBusiness.FilesListNew(paginationModel, out int total);
                List<ResponseFilesModel1> FilesResponseList = new List<ResponseFilesModel1>();
                foreach (var newFiles in FilesList)
                {
                    ResponseFilesModel1 responseFilesModel = new ResponseFilesModel1();
                    var filetyped = FilesBusiness.FileType(newFiles);

                    responseFilesModel.Id = newFiles.Id;
                    responseFilesModel.name = newFiles.Name;
                    responseFilesModel.filetypeid = newFiles.FileTypeId;
                    responseFilesModel.filetypename = filetyped.Filetype;
                    FilesResponseList.Add(responseFilesModel);
                }
                successResponse.data = FilesResponseList;
                successResponse.totalcount = total;
                successResponse.response_code = 0;
                successResponse.message = "Files Details";
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

        [HttpPost("UploadMultiple")]
        [RequestSizeLimit(1073741824)]
        public async Task<IActionResult> UploadMultiple()
        {
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);

            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            List<ResponseFilesModel> lstresponseFilesModel = new List<ResponseFilesModel>();
            string mediaLink = "";
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    string fileName = "";
                    var allFiles = Request.Form.Files;
                    var fileTypeId = JsonConvert.DeserializeObject<string[]>(Request.Form["fileTypeId"].ToString());
                    var description = JsonConvert.DeserializeObject<string[]>(Request.Form["description"].ToString());
                    var duration = JsonConvert.DeserializeObject<string[]>(Request.Form["duration"].ToString());
                    var totalpages = JsonConvert.DeserializeObject<string[]>(Request.Form["totalpages"].ToString());
                    if (allFiles.Count != 0)
                    {
                        int i = 0;
                        foreach (var files in allFiles)
                        {
                            IFormFile file = null;
                            file = files;
                            var imageAcl = PredefinedObjectAcl.PublicRead;
                            fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                            var ext = fileName.Substring(fileName.LastIndexOf("."));
                            var extension = ext.ToLower();
                            Guid imageGuid = Guid.NewGuid();
                            fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;
                            if (fileTypeId[i] == "1")
                            {
                                var imageObject = await storage.UploadObjectAsync(
                                    bucket: "t24-primary-pdf-storage",
                                    objectName: fileName,
                                    contentType: file.ContentType,
                                    source: file.OpenReadStream(),
                                    options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                                );
                                mediaLink = imageObject.MediaLink;
                            }
                            if (fileTypeId[i] == "2")
                            {
                                var imageObject = await storage.UploadObjectAsync(
                                    bucket: "t24-primary-video-storage",
                                    objectName: fileName,
                                    contentType: file.ContentType,
                                    source: file.OpenReadStream(),
                                    options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                                );
                                mediaLink = imageObject.MediaLink;
                            }
                            if (fileTypeId[i] == "3")
                            {
                                var imageObject = await storage.UploadObjectAsync(
                                    bucket: "t24-primary-image-storage",
                                    objectName: fileName,
                                    contentType: file.ContentType,
                                    source: file.OpenReadStream(),
                                    options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                                );
                                mediaLink = imageObject.MediaLink;
                            }
                            if (fileTypeId[i] == "4")
                            {
                                var imageObject = await storage.UploadObjectAsync(
                                    bucket: "t24-primary-image-storage",
                                    objectName: fileName,
                                    contentType: file.ContentType,
                                    source: file.OpenReadStream(),
                                    options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                                );
                                mediaLink = imageObject.MediaLink;
                            }
                            if (fileTypeId[i] == "6" || fileTypeId[i] == "7" || fileTypeId[i] == "8")
                            {
                                var imageObject = await storage.UploadObjectAsync(
                                    bucket: "t24-primary-pdf-storage",
                                    objectName: fileName,
                                    contentType: file.ContentType,
                                    source: file.OpenReadStream(),
                                    options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                                );
                                mediaLink = imageObject.MediaLink;
                            }
                            AddFilesModel FilesModel = new AddFilesModel();
                            if (!string.IsNullOrEmpty(description[i].ToString()))
                                FilesModel.Description = description[i];
                            FilesModel.Url = mediaLink;
                            FilesModel.Name = fileName;
                            FilesModel.FileName = fileName;
                            FilesModel.FileTypeId = long.Parse(fileTypeId[i]);
                            FilesModel.FileSize = file.Length;
                            if (!string.IsNullOrEmpty(duration[i].ToString()))
                                FilesModel.Duration = duration[i].ToString();
                            if (!string.IsNullOrEmpty(totalpages[i]))
                                FilesModel.TotalPages = int.Parse(totalpages[i]);
                            Files newFiles = FilesBusiness.Create(FilesModel, int.Parse(tc.Id));
                            var filetype = FilesBusiness.FileType(newFiles);
                            ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                            responseFilesModel.Id = newFiles.Id;
                            responseFilesModel.name = newFiles.Name;
                            responseFilesModel.url = newFiles.Url;
                            responseFilesModel.filename = newFiles.FileName;
                            responseFilesModel.description = newFiles.Description;
                            responseFilesModel.filetypeid = newFiles.FileTypeId;
                            responseFilesModel.filesize = newFiles.FileSize;
                            responseFilesModel.filetypename = filetype.Filetype;
                            responseFilesModel.duration = newFiles.Duration;
                            responseFilesModel.totalpages = newFiles.TotalPages;
                            lstresponseFilesModel.Add(responseFilesModel);
                            i++;
                        }
                        successResponse.data = lstresponseFilesModel;
                        successResponse.response_code = 0;
                        successResponse.message = "Files Created";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        return StatusCode(406, ModelState);
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
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        #region Genrate sign url for upload object in bucket

        [HttpPost("UploadLessonVideo")]
        public async Task<IActionResult> UploadLessonVideo()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                                            .FromBucket(bucketName)
                                            .WithObjectName(fileName)
                                            .WithHttpMethod(HttpMethod.Put)
                                            .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                                            {
                                                                { "Content-Type", new[] { contentType } }
                                            });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = url,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("UploadLessonPdf")]
        public async Task<IActionResult> UploadLessonPdf()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = urlSigner.Sign(bucketName, fileName, timeSpan, HttpMethod.Put, null, null);

                //var urlDecode = HttpUtility.UrlDecode(url);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                                                            .FromBucket(bucketName)
                                                            .WithObjectName(fileName)
                                                            .WithHttpMethod(HttpMethod.Put)
                                                            .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                                                            {
                                                                { "Content-Type", new[] { contentType } }
                                                            });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
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

        [HttpPost("UploadLessonAssignment")]
        public async Task<IActionResult> UploadLessonAssignment()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                                            .FromBucket(bucketName)
                                            .WithObjectName(fileName)
                                            .WithHttpMethod(HttpMethod.Put)
                                            .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                                            {
                                                                { "Content-Type", new[] { contentType } }
                                            });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("UploadChapterAssignment")]
        public async Task<IActionResult> UploadChapterAssignment()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                                            .FromBucket(bucketName)
                                            .WithObjectName(fileName)
                                            .WithHttpMethod(HttpMethod.Put)
                                            .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                                            {
                                                                { "Content-Type", new[] { contentType } }
                                            });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("UploadQuizImage")]
        public async Task<IActionResult> UploadQuestionImage()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                            .FromBucket(bucketName)
                            .WithObjectName(fileName)
                            .WithHttpMethod(HttpMethod.Put)
                            .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                            {
                                                                { "Content-Type", new[] { contentType } }
                            });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("UploadQuizAnswerImage")]
        public async Task<IActionResult> UploadQuestionAnswerImage()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                            .FromBucket(bucketName)
                            .WithObjectName(fileName)
                            .WithHttpMethod(HttpMethod.Put)
                            .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                            {
                                      { "Content-Type", new[] { contentType } }
                            });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("UploadCourseCardImage")]
        public async Task<IActionResult> UploadCourseCardImage()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
            .FromBucket(bucketName)
            .WithObjectName(fileName)
            .WithHttpMethod(HttpMethod.Put)
            .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
            {
                                      { "Content-Type", new[] { contentType } }
            });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("LibraryBookPdf")]
        public async Task<IActionResult> LibraryBookPdf()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                        .FromBucket(bucketName)
                        .WithObjectName(fileName)
                        .WithHttpMethod(HttpMethod.Put)
                        .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                        {
                              { "Content-Type", new[] { contentType } }
                        });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("LibraryBookCardImage")]
        public async Task<IActionResult> LibraryBookCardImage()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                        .FromBucket(bucketName)
                        .WithObjectName(fileName)
                        .WithHttpMethod(HttpMethod.Put)
                        .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                        {
                                { "Content-Type", new[] { contentType } }
                        });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("FeedbackAttachmentFile")]
        public async Task<IActionResult> FeedbackAttachmentFile()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                        .FromBucket(bucketName)
                        .WithObjectName(fileName)
                        .WithHttpMethod(HttpMethod.Put)
                        .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                        {
                             { "Content-Type", new[] { contentType } }
                        });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("SalesDepositDocument")]
        public async Task<IActionResult> SalesDepositDocument()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                        .FromBucket(bucketName)
                        .WithObjectName(fileName)
                        .WithHttpMethod(HttpMethod.Put)
                        .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                        {
                                             { "Content-Type", new[] { contentType } }
                        });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("SalesPurchaseReceipt")]
        public async Task<IActionResult> SalesPurchaseReceipt()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                        .FromBucket(bucketName)
                        .WithObjectName(fileName)
                        .WithHttpMethod(HttpMethod.Put)
                        .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                        {
                                { "Content-Type", new[] { contentType } }
                        });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("SalesPurchaseIndividualDetailsDocument")]
        public async Task<IActionResult> SalesPurchaseIndividualDetailsDocument()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                            .FromBucket(bucketName)
                            .WithObjectName(fileName)
                            .WithHttpMethod(HttpMethod.Put)
                            .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                            {
                                   { "Content-Type", new[] { contentType } }
                            });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("SalesPurchaseSchoolDetailsDocument")]
        public async Task<IActionResult> SalesPurchaseSchoolDetailsDocument()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                        .FromBucket(bucketName)
                        .WithObjectName(fileName)
                        .WithHttpMethod(HttpMethod.Put)
                        .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                        {
                                { "Content-Type", new[] { contentType } }
                        });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("FileUploadImage")]
        public async Task<IActionResult> FileUploadImage()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                        .FromBucket(bucketName)
                        .WithObjectName(fileName)
                        .WithHttpMethod(HttpMethod.Put)
                        .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                        {
                                { "Content-Type", new[] { contentType } }
                        });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("FileUploadPdf")]
        public async Task<IActionResult> FileUploadPdf()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                        .FromBucket(bucketName)
                        .WithObjectName(fileName)
                        .WithHttpMethod(HttpMethod.Put)
                        .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                        {
                                { "Content-Type", new[] { contentType } }
                        });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        [HttpPost("FileUploadVideo")]
        public async Task<IActionResult> FileUploadVideo()
        {
            string credential = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                var fileName = Request.Form["fileName"].ToString();
                var ext = fileName.Substring(fileName.LastIndexOf("."));
                var extension = ext.ToLower();
                Guid imageGuid = Guid.NewGuid();
                fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                var contentType = Request.Form["contentType"].ToString();

                string bucketName = General.getBucketName(Request.Form["fileTypeId"].ToString());

                TimeSpan timeSpan = TimeSpan.FromHours(1);

                // Create a request template that will be used to create the signed URL.
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                //var contentheaders = new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "Content-Type", new[] { contentType } }
                //    };
                //UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                //string url = await urlSigner.SignAsync(bucketName, fileName, timeSpan, HttpMethod.Put, null);

                UrlSigner.RequestTemplate requestTemplate = UrlSigner.RequestTemplate
                        .FromBucket(bucketName)
                        .WithObjectName(fileName)
                        .WithHttpMethod(HttpMethod.Put)
                        .WithContentHeaders(new Dictionary<string, IEnumerable<string>>
                        {
                                { "Content-Type", new[] { contentType } }
                        });
                // Create options specifying for how long the signer URL will be valid.
                UrlSigner.Options options = UrlSigner.Options.FromDuration(TimeSpan.FromHours(1));
                // Create a signed URL which allows the requester to PUT data with the text/plain content-type.
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(credential);
                string url = await urlSigner.SignAsync(requestTemplate, options);

                var urlDecode = HttpUtility.UrlDecode(url);

                var signedurl = new
                {
                    signedurl = urlDecode,
                    filename = fileName
                };

                successResponse.data = signedurl;
                successResponse.response_code = 0;
                successResponse.message = "Url Created";
                successResponse.status = "Success";
                return StatusCode(200, successResponse);
            }
            catch (Exception ex)
            {

                unsuccessResponse.response_code = 2;
                unsuccessResponse.message = ex.Message;
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }

        #endregion

        #region Save file metadata in database
        [HttpPost("SaveFileMetaData")]
        public IActionResult SaveFileMetaData()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string mediaLink = "";
            try
            {
                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];

                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                if (ModelState.IsValid)
                {
                    AddFilesModel FilesModel = new AddFilesModel();

                    if (!string.IsNullOrEmpty(Request.Form["description"].ToString()))
                        FilesModel.Description = Request.Form["description"];

                    FilesModel.Url = mediaLink;
                    FilesModel.Name = Request.Form["filename"].ToString();
                    FilesModel.FileName = Request.Form["filename"].ToString();
                    FilesModel.FileTypeId = long.Parse(Request.Form["fileTypeId"]);

                    if (!string.IsNullOrEmpty(Request.Form["duration"].ToString()))
                        FilesModel.Duration = Request.Form["duration"].ToString();

                    if (!string.IsNullOrEmpty(Request.Form["totalpages"]))
                        FilesModel.TotalPages = int.Parse(Request.Form["totalpages"]);

                    Files newFiles = FilesBusiness.Create(FilesModel, int.Parse(tc.Id));

                    var filetype = FilesBusiness.FileType(newFiles);

                    ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                    responseFilesModel.Id = newFiles.Id;
                    responseFilesModel.name = newFiles.Name;
                    responseFilesModel.url = newFiles.Url;
                    responseFilesModel.filename = newFiles.FileName;
                    responseFilesModel.description = newFiles.Description;
                    responseFilesModel.filetypeid = newFiles.FileTypeId;
                    responseFilesModel.filesize = newFiles.FileSize;
                    responseFilesModel.filetypename = filetype.Filetype;
                    responseFilesModel.duration = newFiles.Duration;
                    responseFilesModel.totalpages = newFiles.TotalPages;

                    successResponse.data = responseFilesModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Files Created";
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
                unsuccessResponse.status = mediaLink;
                return StatusCode(500, unsuccessResponse);
            }
        }
        #endregion
    }
}