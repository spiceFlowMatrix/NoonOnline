using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.PubSub.V1;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Logs;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly LogsBusiness LogsBusiness;
        private readonly FilesBusiness FilesBusiness;
        private IHostingEnvironment hostingEnvironment;

        public LogsController
        (
            IHostingEnvironment hostingEnvironment,
            LogsBusiness LogsBusiness,
            FilesBusiness FilesBusiness
        )
        {
            this.hostingEnvironment = hostingEnvironment;
            this.LogsBusiness = LogsBusiness;
            this.FilesBusiness = FilesBusiness;
        }


        [HttpPost]
        public IActionResult Post(NotifiactionPayload obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();


            try
            {
                string base64EncodedExternalAccount = obj.message.data;
                byte[] byteArray = Convert.FromBase64String(base64EncodedExternalAccount);

                string bucket = obj.message.attributes.bucketId;

                string jsonBack = Encoding.UTF8.GetString(byteArray);
                DecodedData decodedData = JsonConvert.DeserializeObject<DecodedData>(jsonBack);

                AddFilesModel FilesModel = new AddFilesModel();
                FilesModel.Url = decodedData.mediaLink;
                FilesModel.Name = decodedData.name;
                FilesModel.FileName = decodedData.name;

                switch (bucket)
                {
                    case "t24-primary-video-storage":
                        FilesModel.FileTypeId = 2;
                        break;
                    case "t24-primary-image-storage":
                        FilesModel.FileTypeId = 3;
                        break;
                    case "t24-primary-pdf-storage":
                        FilesModel.FileTypeId = 1;
                        break;
                    case "edg-primary-course-image-storage":
                        FilesModel.FileTypeId = 1;
                        break;
                    case "edg-primary-profile-image-storage":
                        FilesModel.FileTypeId = 1;
                        break;
                }

                FilesModel.FileSize = long.Parse(decodedData.size);
                Files newFiles = FilesBusiness.Create(FilesModel, 0);

                successResponse.data = null;
                successResponse.response_code = 0;
                successResponse.message = "Log inserted";
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

        [HttpGet]
        public IActionResult Get()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {
                List<Logs> logs = LogsBusiness.LogsList();
                List<ResponseLogsModel> responseLogsModels = new List<ResponseLogsModel>();

                foreach (var log in logs)
                {
                    ResponseLogsModel rlm = new ResponseLogsModel();
                    rlm.id = log.Id;
                    rlm.data = log.Data;
                    responseLogsModels.Add(rlm);
                }

                successResponse.data = responseLogsModels;
                successResponse.response_code = 0;
                successResponse.message = "All logs";
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


        [HttpGet("CallPullService")]
        public IActionResult CallPullService()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            try
            {

                string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
                var credential = GoogleCredential.FromFile(jsonPath);
                var storage = StorageClient.Create(credential);

                successResponse.data = LogsBusiness.PullMessagesAsync("training24-197210", "testpullservice", true);
                successResponse.response_code = 0;
                successResponse.message = "All logs";
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