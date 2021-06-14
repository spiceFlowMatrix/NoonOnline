using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using AutoMapper;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.Course;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZipUploadingController : ControllerBase
    {
        private readonly CourseBusiness CourseBusiness;
        private readonly IMapper _mapper;
        private IHostingEnvironment hostingEnvironment;


        public ZipUploadingController(
            IHostingEnvironment hostingEnvironment,
            CourseBusiness CourseBusiness,
            IMapper mapper
            )
        {
            this.hostingEnvironment = hostingEnvironment;
            this.CourseBusiness = CourseBusiness;
            _mapper = mapper;
        }
        
        [HttpPost]
        public async Task<IActionResult> PostAsync()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            var credential = GoogleCredential.FromFile(jsonPath);
            var _storageClient = StorageClient.Create(credential);

            //string Authorization = Request.Headers["Authorization"];
            //TokenClaims tc = General.GetClaims(Authorization);

            try
            {
                if (ModelState.IsValid)
                {
                    //string startPath = @"d:\example\start";
                    //string zipPath = @"d:\example\-Biology Grade 10.zip";
                    //string extractPath = @"d:\example\extract";
                    //ZipFile.CreateFromDirectory(startPath, zipPath);
                    //ZipFile.ExtractToDirectory(zipPath, extractPath);
                    string fileName = "";
                    IFormFile file = null;

                    if (Request.Form.Files.Count != 0)
                        file = Request.Form.Files[0];

                    fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                    var ext = file.FileName.Substring(file.FileName.LastIndexOf('.'));

                    var extension = ext.ToString().ToLower();
                    Guid imageGuid = Guid.NewGuid();
                    var filename = hostingEnvironment.WebRootPath + "\\Zip" + "\\" + fileName;    
                    
                    using (FileStream fs = System.IO.File.Create(filename))
                    {
                        file.CopyTo(fs);
                        fs.Flush();
                    }

                    var extractPath = hostingEnvironment.WebRootPath + "\\Extract\\" ;
                    ZipFile.ExtractToDirectory(filename, extractPath + fileName);
                    Item items = new Item();
                    using (StreamReader r = new StreamReader(extractPath + "manifest.json"))
                    {
                        string json = r.ReadToEnd();
                        items = JsonConvert.DeserializeObject<Item>(json);
                    }

                    var files = Directory.EnumerateFiles("d:\\example\\extract\\Course", "*.*", SearchOption.AllDirectories)
                                        .Where(s => s.EndsWith(".jpg"));

                    List<string> exts = new List<string> { ".jpg", ".jpeg", ".png", ".gif", ".tif" };
                    FileInfo[] filesList = new DirectoryInfo("d:\\example\\extract\\Course").EnumerateFiles("*.*", SearchOption.AllDirectories)
                    .Where(path => exts.Contains(Path.GetExtension(path.Name)))
                    .Select(x => new FileInfo(x.FullName)).ToArray();                
                    FileInfo test = filesList[0];                                        

                    string mediaLink = "";
                    var imageAcl = PredefinedObjectAcl.PublicRead;
                    var imageObject = await _storageClient.UploadObjectAsync(
                        bucket: "t24-primary-image-storage",
                        objectName: test.Name,
                        contentType: "image/jpg",
                        source: test.OpenRead(),
                        options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                    );
                    mediaLink = imageObject.MediaLink;
                 
                    AddCourseModel CourseModel = new AddCourseModel
                    {
                        image = mediaLink,
                        name = items.courseTitle,
                        code = items.courseCode,
                        description = ""
                    };

                    Course newCourse = CourseBusiness.Create(CourseModel, 1);

                    successResponse.data = _mapper.Map<Course, ResponseCourseModel>(newCourse); ;
                    successResponse.response_code = 0;
                    successResponse.message = "Zip Uploaded";
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



    }


    public class Item
    {
        public string courseTitle { get; set; }
        public string courseCardFileName { get; set; }
        public string courseCode { get; set; }
        public List<chapters> chapters { get; set; }
    }

    public class chapters
    {
        public string chapterName { get; set; }
        public int chapterOrder { get; set; }
        public List<items> items { get; set; }
    }


    public class items
    {
        public string itemName { get; set; }
        public int itemOrder { get; set; }
        public string itemType { get; set; }
        public int contentId { get; set; }
        public string contentFileName { get; set; }
        public int noOfQuizQuestionsToUse { get; set; }
        public int quizPassingPercentage { get; set; }
    }
}