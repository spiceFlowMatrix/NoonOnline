using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Training24Admin.Model;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using Trainning24.BL.Business;
using Trainning24.Domain.Entity;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Trainning24.BL.ViewModels.Lesson;
using CsvHelper;

namespace Training24Admin.Controllers
{

    [Route("api/v1/[controller]")]
    [ApiController]
    [Authorize]
    public class UploadController : ControllerBase
    {

        private IHostingEnvironment _hostingEnvironment;
        private readonly PasswordBusiness _PasswordBusiness;
        private readonly ProfileBusiness _ProfileBusiness;
        private readonly UsersBusiness UsersBusiness;
        private readonly LessonBusiness LessonBusiness;
        private IHostingEnvironment hostingEnvironment;

        public UploadController(
            IHostingEnvironment hostingEnvironment,
            UsersBusiness UsersBusiness,
            PasswordBusiness PasswordBusiness,
            ProfileBusiness ProfileBusiness,
            LessonBusiness LessonBusiness
            )
        {
            this.hostingEnvironment = hostingEnvironment;
            this.UsersBusiness = UsersBusiness;
            _PasswordBusiness = PasswordBusiness;
            _hostingEnvironment = hostingEnvironment;
            _ProfileBusiness = ProfileBusiness;
            this.LessonBusiness = LessonBusiness;
        }

        [HttpPut]
        [Route("UploadProfilePicture")]
        public async Task<IActionResult> UploadFile()
        {
            string jsonPath = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            var credential = GoogleCredential.FromFile(jsonPath);
            var _storageClient = StorageClient.Create(credential);

            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx

                //string Authorization = Request.Headers["Authorization"];
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);

                User user = UsersBusiness.AuthenticationByAuth(tc.sub);
                if (user == null)
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "User not found.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(404, unsuccessResponse);
                }
                else
                {
                    string fileName = "";

                    IFormFile file = null;
                    if (Request.Form.Files.Count != 0)
                        file = Request.Form.Files[0];
                    string mediaLink = "";

                    if (file.Length > 0)
                    {
                        fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                        IList<string> AllowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                        var ext = fileName.Substring(fileName.LastIndexOf("."));
                        var extension = ext.ToLower();
                        if (AllowedFileExtensions.Contains(extension))
                        {
                            Guid imageGuid = Guid.NewGuid();
                            fileName = fileName.Split(".")[0] + "_" + imageGuid.ToString() + extension;

                            var imageAcl = PredefinedObjectAcl.PublicRead;
                            var imageObject = await _storageClient.UploadObjectAsync(
                                bucket: "edg-primary-profile-image-storage",
                                objectName: fileName,
                                contentType: file.ContentType,
                                source: file.OpenReadStream(),
                                options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                            );
                            mediaLink = imageObject.MediaLink;

                            _ProfileBusiness.isProfilePhototUpload(user.Email, mediaLink);
                        }

                        //successResponse.data = mediaLink;
                        if (!string.IsNullOrEmpty(mediaLink))
                            successResponse.data = LessonBusiness.geturl(mediaLink, Certificate);
                        successResponse.response_code = 0;
                        successResponse.message = "Photo uploaded";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 1;
                        unsuccessResponse.message = "Empty";
                        unsuccessResponse.status = "Unsuccess";
                        return StatusCode(404, unsuccessResponse);
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
        }

        [HttpPost]
        [Route("ImportLessonCSV")]
        [AllowAnonymous]
        public async Task<IActionResult> ImportLessonCSV()
        {
            IFormFile file = null;
            if (Request.Form.Files.Count != 0)
                file = Request.Form.Files[0];
            else
                return StatusCode(400);
            List<LessonFileExcelDTO> LessonFileExcelDTO = new List<LessonFileExcelDTO>();
            List<LessonDeleteExcelDTO> LessonDeleteExcelDTO = new List<LessonDeleteExcelDTO>();
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var csvReader = new CsvReader(reader);
                        var requests = csvReader.GetRecords<LessonExcelDataDTO>().ToList();
                        foreach (var data in requests)
                        {
                            if (!string.IsNullOrEmpty(data.FileId) && !string.IsNullOrEmpty(data.Lesson_Id))
                            {
                                LessonFileExcelDTO lessonFileExcel = new LessonFileExcelDTO();
                                lessonFileExcel.LessonId = long.Parse(data.Lesson_Id);
                                lessonFileExcel.FileId = long.Parse(data.FileId);
                                LessonFileExcelDTO.Add(lessonFileExcel);
                            }
                            if (!string.IsNullOrEmpty(data.LessonIdsThatNeedsToBeDeleted))
                            {
                                LessonDeleteExcelDTO lessonDeleteExcel = new LessonDeleteExcelDTO();
                                lessonDeleteExcel.LessonId = long.Parse(data.LessonIdsThatNeedsToBeDeleted);
                                LessonDeleteExcelDTO.Add(lessonDeleteExcel);
                            }
                        }
                    }
                }
                await LessonBusiness.SaveUpdateLessonFiles(LessonFileExcelDTO);
                await LessonBusiness.DeleteLesson(LessonDeleteExcelDTO);
            }
            catch (Exception ex)
            {

            }
            return StatusCode(200);
        }
    }
}
