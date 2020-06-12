using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.AppTimeTrack;
using Trainning24.BL.ViewModels.ProgressSync;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProgessSyncController : ControllerBase
    {
        private readonly ProgessSyncBusiness _ProgessSyncBusiness;
        private readonly LessonBusiness LessonBusiness;
        private readonly AppTimeTrackBusiness _appTimeTrackBusiness;
        private readonly IMapper _mapper;
        public ProgessSyncController
        (
            ProgessSyncBusiness ProgessSyncBusiness,
            LessonBusiness LessonBusiness,
            AppTimeTrackBusiness appTimeTrackBusiness,
            IMapper mapper
        )
        {
            this.LessonBusiness = LessonBusiness;
            _ProgessSyncBusiness = ProgessSyncBusiness;
            _appTimeTrackBusiness = appTimeTrackBusiness;
            _mapper = mapper;
        }

        [Authorize]
        [HttpPost]
        [Route("ProgessSyncAdd")]
        public async Task<IActionResult> ProgessSyncAdd([FromBody]SyncData data)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                if (data != null)
                {

                    if (data.progressdata.Count > 0)
                    {
                        var result = await _ProgessSyncBusiness.ProgessSyncRecord(data.progressdata);

                    }
                    if (data.timerdata.Count > 0)
                    {
                        var addTimer = _ProgessSyncBusiness.AddQuizTimerRecordInBulk(data.timerdata);
                    }
                    #region Comment old code
                    //foreach (var sync in data.progressdata)
                    //{
                    //    ProgessSync progess = _ProgessSyncBusiness.CheckRecordExists(sync.GradeId, sync.LessonId, sync.QuizId, sync.UserId);
                    //    if (progess != null)
                    //    {
                    //        ProgessSync updateRecord = _ProgessSyncBusiness.UpdateRecord(sync);
                    //    }
                    //    else
                    //    {
                    //        ProgessSync addRecord = _ProgessSyncBusiness.AddRecord(sync);
                    //    }
                    //}
                    //foreach (var timer in data.timerdata)
                    //{
                    //QuizTimerSync timerd = _ProgessSyncBusiness.CheckTimerRecordExists(timer.userId, timer.quizId);
                    //if (timerd != null)
                    //{
                    //    QuizTimerSync updateTimer = _ProgessSyncBusiness.UpdateQuizTimerRecord(timer);
                    //}
                    //else
                    //{
                    //    QuizTimerSync addTimer = _ProgessSyncBusiness.AddQuizTimerRecord(timer);
                    //}
                    //} 
                    #endregion

                    successResponse.response_code = 0;
                    successResponse.message = "Synced";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Data is null. Please fill data and try again.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(422, unsuccessResponse);
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
        [HttpGet]
        [Route("GetSyncRecords")]
        public IActionResult GetSyncRecords()
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();

            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                List<ProgessSync> lstSyncRecords = _ProgessSyncBusiness.GetRecordById(Convert.ToInt32(tc.Id));
                List<QuizTimerSync> quizTimerSyncs = _ProgessSyncBusiness.GetTimerRecordById(Convert.ToInt32(tc.Id));
                SyncDataResponse syncData = new SyncDataResponse();
                syncData.progressdata = _mapper.Map<List<ProgessSync>, List<ProgressSyncModel>>(lstSyncRecords);
                syncData.timerdata = _mapper.Map<List<QuizTimerSync>, List<TimerSyncModel>>(quizTimerSyncs);
                successResponse.data = syncData;
                successResponse.response_code = 0;
                successResponse.message = "Synced";
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

        // [Authorize]
        [HttpPost]
        [Route("AppTimeTrack")]
        public async Task<IActionResult> AppTimeTrack([FromBody]List<AppTimeTrackDTO> data)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                if (data.Count > 0)
                {
                    List<AppTimeTrack> InsertProgress = data.Select(sync => new AppTimeTrack()
                    {
                        UserId = sync.userid,
                        Latitude = sync.latitude,
                        Longitude = sync.longitude,
                        ServiceProvider = sync.serviceprovider,
                        School = sync.school,
                        SubjectsTaken = sync.subjectstaken,
                        Grade = sync.grade,
                        HardwarePlatform = sync.hardwareplatform,
                        OperatingSystem = sync.operatingsystem,
                        Version = sync.version,
                        Isp = sync.isp,
                        ActivityTime = sync.activitytime,
                        Outtime = sync.outtime,
                        NetworkSpeed = sync.networkspeed,
                        IsDeleted = false,
                        CreationTime = DateTime.Now.ToString(),
                        CreatorUserId = Convert.ToInt32(sync.userid)
                    }).ToList();
                    await _appTimeTrackBusiness.AddRecordBulk(InsertProgress);
                    #region Comment Odl Code
                    //foreach (var sync in data)
                    //{
                    //    AppTimeTrack appTimeTrack = new AppTimeTrack
                    //    {
                    //        UserId = sync.userid,
                    //        Latitude = sync.latitude,
                    //        Longitude = sync.longitude,
                    //        ServiceProvider = sync.serviceprovider,
                    //        School = sync.school,
                    //        SubjectsTaken = sync.subjectstaken,
                    //        Grade = sync.grade,
                    //        HardwarePlatform = sync.hardwareplatform,
                    //        OperatingSystem = sync.operatingsystem,
                    //        Version = sync.version,
                    //        Isp = sync.isp,
                    //        ActivityTime = sync.activitytime,
                    //        Outtime = sync.outtime,
                    //        NetworkSpeed = sync.networkspeed,
                    //        IsDeleted = false,
                    //        CreationTime = DateTime.Now.ToString(),
                    //        CreatorUserId = Convert.ToInt32(sync.userid)
                    //    };
                    //    _appTimeTrackBusiness.AddRecord(appTimeTrack);
                    //} 
                    #endregion
                    successResponse.response_code = 0;
                    successResponse.message = "Synced";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 1;
                    unsuccessResponse.message = "Data is null. Please fill data and try again.";
                    unsuccessResponse.status = "Unsuccess";
                    return StatusCode(422, unsuccessResponse);
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
        [Route("ImportAppTimeCSV")]
        public async Task<IActionResult> ImportAppTimeCSV()
        {
            IFormFile file = null;
            if (Request.Form.Files.Count != 0)
                file = Request.Form.Files[0];
            else
                return StatusCode(400);
            List<AppTimeTrackCSVDTO> AppTimeTrackCSVDTO = new List<AppTimeTrackCSVDTO>();
            try
            {
                using (var stream = file.OpenReadStream())
                {
                    using (var reader = new StreamReader(stream))
                    {
                        var csvReader = new CsvReader(reader);
                        var requests = csvReader.GetRecords<AppTimeExcelDataDTO>().ToList();
                        foreach (var data in requests)
                        {
                            if (!string.IsNullOrEmpty(data.AppTimeId))
                            {
                                AppTimeTrackCSVDTO appTimeTrackCSVDTO = new AppTimeTrackCSVDTO();
                                appTimeTrackCSVDTO.AppTimeId = long.Parse(data.AppTimeId);
                                AppTimeTrackCSVDTO.Add(appTimeTrackCSVDTO);
                            }
                        }
                    }
                }
                await _appTimeTrackBusiness.SaveUpdateRecords(AppTimeTrackCSVDTO);
            }
            catch (Exception ex)
            {

            }
            return StatusCode(200);
        }
    }
}