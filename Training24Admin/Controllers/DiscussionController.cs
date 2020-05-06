using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels.DiscussionTopic;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using System.Net.Http.Headers;
using Trainning24.BL.ViewModels.Files;
using Microsoft.Extensions.Options;
using System.Net.Http;
using System.Globalization;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class DiscussionController : ControllerBase
    {
        private readonly DiscussionTopicBusiness DiscussionTopicBusiness;
        private readonly DiscussionCommentsBusiness discussionCommentsBusiness;
        private readonly LessonBusiness LessonBusiness;
        private IHostingEnvironment hostingEnvironment;
        private readonly UsersBusiness usersBusiness;
        private readonly DiscussionFilesBusiness discussionFilesBusiness;
        private readonly IOptions<FilesSettings> _filesSettings;
        private readonly DiscussionCommentFilesBusiness discussionCommentFilesBusiness;
        private readonly DiscussionTopicLikeBusiness _discussionTopicLikeBusiness;
        private readonly DiscussionCommentLikeBusiness _discussionCommentLikeBusiness;
        public DiscussionController(DiscussionTopicBusiness DiscussionTopicBusiness,
            LessonBusiness LessonBusiness,
            DiscussionCommentsBusiness DiscussionCommentsBusiness,
            IHostingEnvironment hostingEnvironment,
            UsersBusiness usersBusiness,
            DiscussionFilesBusiness DiscussionFilesBusiness,
            IOptions<FilesSettings> filesSettings,
            DiscussionCommentFilesBusiness discussionCommentFilesBusiness,
            DiscussionTopicLikeBusiness discussionTopicLikeBusiness,
            DiscussionCommentLikeBusiness discussionCommentLikeBusiness
            )
        {
            this.DiscussionTopicBusiness = DiscussionTopicBusiness;
            this.LessonBusiness = LessonBusiness;
            this.discussionCommentsBusiness = DiscussionCommentsBusiness;
            this.hostingEnvironment = hostingEnvironment;
            this.usersBusiness = usersBusiness;
            this.discussionFilesBusiness = DiscussionFilesBusiness;
            this._filesSettings = filesSettings;
            this.discussionCommentFilesBusiness = discussionCommentFilesBusiness;
            _discussionTopicLikeBusiness = discussionTopicLikeBusiness;
            _discussionCommentLikeBusiness = discussionCommentLikeBusiness;
        }
        #region Add Topic
        [Authorize]
        [HttpPost("AddDiscussionTopic")]
        public IActionResult AddTopic(DiscussionTopicViewModel obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            List<DiscussionFilesModel> lstdiscussionFilesModels = new List<DiscussionFilesModel>();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (!string.IsNullOrEmpty(obj.title))
                {
                    obj.createduserid = int.Parse(tc.Id);
                    DiscussionTopic discussionTopic = new DiscussionTopic
                    {
                        CourseId = obj.courseid,
                        Title = obj.title,
                        CreatorUserId = obj.createduserid,
                        Description = obj.description,
                        IsPrivate = obj.isprivate,
                        IsPublic = obj.ispublic
                    };
                    DiscussionTopic newTopic = DiscussionTopicBusiness.AddTopic(discussionTopic);
                    if (obj.filesid != null && obj.filesid.Length > 0)
                    {
                        List<string> files_ids = new List<string>(obj.filesid);
                        foreach (var file in files_ids)
                        {
                            discussionFilesBusiness.UpdateTopicId(Convert.ToInt32(file), Convert.ToInt32(newTopic.Id));
                        }
                    }
                    obj.id = newTopic.Id;
                    obj.courseid = newTopic.CourseId;
                    obj.createduserid = newTopic.CreatorUserId ?? 0;
                    obj.description = newTopic.Description;
                    obj.isprivate = newTopic.IsPrivate;
                    obj.ispublic = newTopic.IsPublic;
                    var filesList = discussionFilesBusiness.GetFilesByTopicId(Convert.ToInt32(obj.id));
                    if (filesList.Count > 0)
                    {
                        foreach (var file in filesList)
                        {
                            DiscussionFilesModel objFiles = new DiscussionFilesModel();
                            objFiles.Id = file.Id;
                            objFiles.TopicId = file.TopicId;
                            objFiles.Name = file.Name;
                            objFiles.FileName = file.FileName;
                            objFiles.Url = file.Url;
                            objFiles.FileSize = file.FileSize;
                            objFiles.FileTypeId = file.FileTypeId;
                            objFiles.TotalPages = file.TotalPages;
                            objFiles.Duration = file.Duration;
                            objFiles.SignedUrl = GetSignedUrl(file.Id);
                            lstdiscussionFilesModels.Add(objFiles);
                        }
                    }
                    obj.files = lstdiscussionFilesModels;
                    string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                    if (!obj.ispublic)
                    {
                        if (tc.RoleName.Contains(LessonBusiness.getRoleType("3")))
                        {
                            DiscussionTopicBusiness.SendNotificationToStudents(obj, Certificate);
                        }
                        if (tc.RoleName.Contains(LessonBusiness.getRoleType("4")))
                        {
                            DiscussionTopicBusiness.SendNotificationToTeachers(obj, Certificate);
                        }
                    }
                    successResponse.data = obj;
                    successResponse.response_code = 0;
                    successResponse.message = "Topic added";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 2;
                    unsuccessResponse.message = "Title is required";
                    unsuccessResponse.status = "Failure";
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

        [Authorize]
        [HttpPost("UpdateDiscussionTopic")]
        public IActionResult UpdateTopic(DiscussionTopicViewModel obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            List<DiscussionFilesModel> lstdiscussionFilesModels = new List<DiscussionFilesModel>();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (!string.IsNullOrEmpty(obj.title))
                {
                    DiscussionTopic discussionTopic = new DiscussionTopic
                    {
                        Id = obj.id,
                        CourseId = obj.courseid,
                        Title = obj.title,
                        LastModifierUserId = int.Parse(tc.Id),
                        Description = obj.description,
                        IsPrivate = obj.isprivate,
                        IsPublic = obj.ispublic
                    };
                    DiscussionTopic updateTopic = DiscussionTopicBusiness.UpdateTopic(discussionTopic);
                    if (obj.filesid != null && obj.filesid.Length > 0)
                    {
                        List<string> files_ids = new List<string>(obj.filesid);
                        foreach (var file in files_ids)
                        {
                            discussionFilesBusiness.UpdateTopicId(Convert.ToInt32(file), Convert.ToInt32(updateTopic.Id));
                        }
                    }
                    if (updateTopic != null)
                    {
                        obj.id = updateTopic.Id;
                        obj.courseid = updateTopic.CourseId;
                        obj.createduserid = updateTopic.CreatorUserId ?? 0;
                        obj.description = updateTopic.Description;
                        obj.isprivate = updateTopic.IsPrivate;
                        obj.ispublic = updateTopic.IsPublic;
                        var filesList = discussionFilesBusiness.GetFilesByTopicId(Convert.ToInt32(obj.id));
                        if (filesList.Count > 0)
                        {
                            foreach (var file in filesList)
                            {
                                DiscussionFilesModel objFiles = new DiscussionFilesModel();
                                objFiles.Id = file.Id;
                                objFiles.TopicId = file.TopicId;
                                objFiles.Name = file.Name;
                                objFiles.FileName = file.FileName;
                                objFiles.Url = file.Url;
                                objFiles.FileSize = file.FileSize;
                                objFiles.FileTypeId = file.FileTypeId;
                                objFiles.TotalPages = file.TotalPages;
                                objFiles.Duration = file.Duration;
                                objFiles.SignedUrl = GetSignedUrl(file.Id);
                                lstdiscussionFilesModels.Add(objFiles);
                            }
                        }
                        obj.files = lstdiscussionFilesModels;
                        successResponse.data = obj;
                        successResponse.response_code = 0;
                        successResponse.message = "Topic Updated";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "Topic not found";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
                    }
                }
                else
                {
                    unsuccessResponse.response_code = 2;
                    unsuccessResponse.message = "Title is required";
                    unsuccessResponse.status = "Failure";
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


        [Authorize]
        [HttpGet("GetAllTopics")]
        public IActionResult GetAllTopics(int pagenumber, int perpagerecord, int courseid, bool ispublic)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            List<DiscussionTopicViewModel> DiscussionTopicViewModel = new List<DiscussionTopicViewModel>();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            PaginationModel paginationModel = new PaginationModel
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord
            };
            try
            {
                List<DiscussionTopic> data = new List<DiscussionTopic>();
                if (ispublic != true)
                {
                    if (tc.RoleName.Contains(LessonBusiness.getRoleType("4")))
                    {
                        data = DiscussionTopicBusiness.GetTopicListForStudents(paginationModel, int.Parse(tc.Id), courseid);
                    }
                    if (tc.RoleName.Contains(LessonBusiness.getRoleType("3")))
                    {
                        data = DiscussionTopicBusiness.GetTopicListForTeachers(paginationModel, int.Parse(tc.Id), courseid);
                    }
                }
                else
                {
                    if (tc.RoleName.Contains(LessonBusiness.getRoleType("4")))
                    {
                        data = DiscussionTopicBusiness.GetPublicTopicListStudent(paginationModel, int.Parse(tc.Id), ispublic);
                    }
                    if (tc.RoleName.Contains(LessonBusiness.getRoleType("3")))
                    {
                        data = DiscussionTopicBusiness.GetPublicTopicListTeacher(paginationModel, int.Parse(tc.Id), ispublic);
                    }
                }
                if (data != null)
                {
                    foreach (var topic in data)
                    {
                        DiscussionTopicViewModel temp = new DiscussionTopicViewModel();
                        temp.id = topic.Id;
                        temp.courseid = topic.CourseId;
                        temp.title = topic.Title;
                        temp.description = topic.Description;
                        temp.isprivate = topic.IsPrivate;
                        temp.ispublic = topic.IsPublic;
                        temp.comments = discussionCommentsBusiness.GetCommentCounts(topic.Id);
                        temp.createduserid = topic.CreatorUserId ?? 0;
                        temp.createddate = topic.CreationTime;
                        temp.user = GetUserById(topic.CreatorUserId ?? 0);
                        temp.iseditable = DiscussionTopicBusiness.CheckTopicOwner(topic.Id, long.Parse(tc.Id));
                        temp.likecount = _discussionTopicLikeBusiness.TotalLikeCount(topic.Id);
                        temp.dislikecount = _discussionTopicLikeBusiness.TotalDisLikeCount(topic.Id);
                        temp.liked = _discussionTopicLikeBusiness.LikedByUser(topic.Id, long.Parse(tc.Id));
                        temp.disliked = _discussionTopicLikeBusiness.DisLikedByUser(topic.Id, long.Parse(tc.Id));
                        DiscussionTopicViewModel.Add(temp);
                    }
                    successResponse.data = DiscussionTopicViewModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Record get sucess";
                    successResponse.status = "Success";
                }
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

        [Authorize]
        [HttpGet("GetAllTopicsById")]
        public IActionResult GetAllTopicsById(int topicId)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            DiscussionTopicViewModel DiscussionTopicViewModel = new DiscussionTopicViewModel();
            List<DiscussionFilesModel> lstdiscussionFilesModels = new List<DiscussionFilesModel>();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                var data = DiscussionTopicBusiness.GetTopicById(topicId);
                if (data != null)
                {
                    DiscussionTopicViewModel.id = data.Id;
                    DiscussionTopicViewModel.courseid = data.CourseId;
                    DiscussionTopicViewModel.title = data.Title;
                    DiscussionTopicViewModel.description = data.Description;
                    DiscussionTopicViewModel.isprivate = data.IsPrivate;
                    DiscussionTopicViewModel.ispublic = data.IsPublic;
                    DiscussionTopicViewModel.comments = 0;
                    DiscussionTopicViewModel.createduserid = data.CreatorUserId ?? 0;
                    DiscussionTopicViewModel.createddate = data.CreationTime;
                    DiscussionTopicViewModel.user = GetUserById(data.CreatorUserId ?? 0);
                    DiscussionTopicViewModel.iseditable = DiscussionTopicBusiness.CheckTopicOwner(data.Id, long.Parse(tc.Id));
                    DiscussionTopicViewModel.likecount = _discussionTopicLikeBusiness.TotalLikeCount(data.Id);
                    DiscussionTopicViewModel.dislikecount = _discussionTopicLikeBusiness.TotalDisLikeCount(data.Id);
                    DiscussionTopicViewModel.liked = _discussionTopicLikeBusiness.LikedByUser(data.Id, long.Parse(tc.Id));
                    DiscussionTopicViewModel.disliked = _discussionTopicLikeBusiness.DisLikedByUser(data.Id, long.Parse(tc.Id));
                    var GetFiles = discussionFilesBusiness.GetFilesByTopicId(Convert.ToInt32(DiscussionTopicViewModel.id));
                    if (GetFiles.Count > 0)
                    {
                        foreach (var file in GetFiles)
                        {
                            DiscussionFilesModel discussionFilesModel = new DiscussionFilesModel();
                            discussionFilesModel.Id = file.Id;
                            discussionFilesModel.TopicId = file.TopicId;
                            discussionFilesModel.Name = file.Name;
                            discussionFilesModel.FileName = file.FileName;
                            discussionFilesModel.Description = file.Description;
                            discussionFilesModel.Url = file.Url;
                            discussionFilesModel.FileSize = file.FileSize;
                            discussionFilesModel.FileTypeId = file.FileTypeId;
                            discussionFilesModel.TotalPages = file.TotalPages;
                            discussionFilesModel.Duration = file.Duration;
                            discussionFilesModel.SignedUrl = GetSignedUrl(file.Id);
                            lstdiscussionFilesModels.Add(discussionFilesModel);
                        }
                        DiscussionTopicViewModel.files = lstdiscussionFilesModels;
                    }
                    successResponse.data = DiscussionTopicViewModel;
                    successResponse.response_code = 0;
                    successResponse.message = "Record get sucess";
                    successResponse.status = "Success";
                }
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

        [Authorize]
        [HttpDelete("DeleteTopic")]
        public IActionResult DeleteTopic(int topicId)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                var data = DiscussionTopicBusiness.DeleteTopic(topicId, int.Parse(tc.Id));
                successResponse.response_code = 0;
                successResponse.message = "Record Deleted";
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
        #endregion

        #region Add Comment
        [Authorize]
        [HttpPost("AddComment")]
        public IActionResult AddComment(DiscussionCommentViewModel obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            List<DiscussionCommentFileViewModel> commentfileslist = new List<DiscussionCommentFileViewModel>();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                //&& obj.files.Length > 0
                if (!string.IsNullOrEmpty(obj.comment) || obj.filesid.Length > 0)
                {
                    DiscussionComments newComment = new DiscussionComments();
                    newComment.Comment = obj.comment;
                    newComment.TopicId = obj.topicid;
                    newComment.CreatorUserId = int.Parse(tc.Id);
                    newComment = discussionCommentsBusiness.AddComments(newComment);
                    if (obj.filesid != null && obj.filesid.Length > 0)
                    {
                        List<string> files_ids = new List<string>(obj.filesid);
                        foreach (var file in files_ids)
                        {
                            discussionCommentFilesBusiness.UpdateCommentId(Convert.ToInt32(file), Convert.ToInt32(newComment.Id));
                        }
                    }
                    obj.Id = newComment.Id;
                    obj.createtime = newComment.CreationTime;
                    obj.user = GetUserById(int.Parse(tc.Id));
                    var filesList = discussionCommentFilesBusiness.GetFilesByCommentId(Convert.ToInt32(newComment.Id));
                    if (filesList.Count > 0)
                    {
                        foreach (var file in filesList)
                        {
                            DiscussionCommentFileViewModel objFiles = new DiscussionCommentFileViewModel();
                            objFiles.Id = file.Id;
                            objFiles.CommentId = file.CommentId;
                            objFiles.Name = file.Name;
                            objFiles.FileName = file.FileName;
                            objFiles.Url = file.Url;
                            objFiles.FileSize = file.FileSize;
                            objFiles.FileTypeId = file.FileTypeId;
                            objFiles.TotalPages = file.TotalPages;
                            objFiles.Duration = file.Duration;
                            objFiles.SignedUrl = GetUrlForComment(file.Id);
                            commentfileslist.Add(objFiles);
                        }
                    }
                    obj.files = commentfileslist;
                    string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx
                    if (!obj.ispublic)
                    {
                        if (!string.IsNullOrEmpty(obj.comment) && obj.filesid.Length <= 0)
                        {
                            discussionCommentsBusiness.SendNotificationOnComment(obj, Certificate, Convert.ToInt32(tc.Id));
                        }

                        if (string.IsNullOrEmpty(obj.comment) && obj.filesid.Length > 0)
                        {
                            discussionCommentsBusiness.SendNotificationOnFileupload(obj, Certificate, Convert.ToInt32(tc.Id));
                        }
                    }
                    else
                    {
                        if (!string.IsNullOrEmpty(obj.comment) && obj.filesid.Length <= 0)
                        {
                            discussionCommentsBusiness.SendNotificationOnPublicComment(obj, Certificate, Convert.ToInt32(tc.Id));
                        }

                        if (string.IsNullOrEmpty(obj.comment) && obj.filesid.Length > 0)
                        {
                            discussionCommentsBusiness.SendNotificationOnFileuploadPublic(obj, Certificate, Convert.ToInt32(tc.Id));
                        }
                    }
                    successResponse.data = obj;
                    successResponse.response_code = 0;
                    successResponse.message = "Comment added";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    unsuccessResponse.response_code = 2;
                    unsuccessResponse.message = "commets or file is required";
                    unsuccessResponse.status = "Failure";
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

        [Authorize]
        [HttpPost("UpdateComment")]
        public IActionResult UpdateComment(DiscussionCommentViewModel obj)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            List<DiscussionCommentFileViewModel> commentfileslist = new List<DiscussionCommentFileViewModel>();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                if (!string.IsNullOrEmpty(obj.comment))
                {
                    var commentBy = discussionCommentsBusiness.ValidateUser(int.Parse(tc.Id));
                    if (commentBy != null)
                    {
                        DiscussionComments updateComment = new DiscussionComments();
                        updateComment.LastModifierUserId = int.Parse(tc.Id);
                        updateComment.Comment = obj.comment;
                        updateComment.Id = obj.Id;
                        updateComment = discussionCommentsBusiness.UpdateComments(updateComment);
                        if (obj.filesid != null && obj.filesid.Length > 0)
                        {
                            List<string> files_ids = new List<string>(obj.filesid);
                            foreach (var file in files_ids)
                            {
                                discussionCommentFilesBusiness.UpdateCommentId(Convert.ToInt32(file), Convert.ToInt32(updateComment.Id));
                            }
                        }
                        if (updateComment != null)
                        {
                            obj.user = GetUserById(int.Parse(tc.Id));
                            var filesList = discussionCommentFilesBusiness.GetFilesByCommentId(Convert.ToInt32(updateComment.Id));
                            if (filesList.Count > 0)
                            {
                                foreach (var file in filesList)
                                {
                                    DiscussionCommentFileViewModel objFiles = new DiscussionCommentFileViewModel();
                                    objFiles.Id = file.Id;
                                    objFiles.CommentId = file.CommentId;
                                    objFiles.Name = file.Name;
                                    objFiles.FileName = file.FileName;
                                    objFiles.Url = file.Url;
                                    objFiles.FileSize = file.FileSize;
                                    objFiles.FileTypeId = file.FileTypeId;
                                    objFiles.TotalPages = file.TotalPages;
                                    objFiles.Duration = file.Duration;
                                    objFiles.SignedUrl = GetUrlForComment(file.Id);
                                    commentfileslist.Add(objFiles);
                                }
                                obj.files = commentfileslist;
                            }
                            successResponse.data = obj;
                            successResponse.response_code = 0;
                            successResponse.message = "Topic Updated";
                            successResponse.status = "Success";
                            return StatusCode(200, successResponse);
                        }
                        else
                        {
                            unsuccessResponse.response_code = 2;
                            unsuccessResponse.message = "Topic not found";
                            unsuccessResponse.status = "Failure";
                            return StatusCode(406, unsuccessResponse);
                        }
                    }
                    else
                    {
                        unsuccessResponse.response_code = 2;
                        unsuccessResponse.message = "You are not able to update this comment.";
                        unsuccessResponse.status = "Failure";
                        return StatusCode(406, unsuccessResponse);
                    }
                }
                else
                {
                    unsuccessResponse.response_code = 2;
                    unsuccessResponse.message = "Title is required";
                    unsuccessResponse.status = "Failure";
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

        [Authorize]
        [HttpGet("GetComments")]
        public IActionResult GetComments(int pagenumber, int perpagerecord, int topicId)
        {
            PaginationModel paginationModel = new PaginationModel
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord
            };

            //get claims after decoding id_token 
            string Authorization = Request.Headers["id_token"];

            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            List<DiscussionCommentViewModel> lstCommentobj = new List<DiscussionCommentViewModel>();
            try
            {
                List<DiscussionComments> commentList = discussionCommentsBusiness.GetCommentList(paginationModel, topicId);
                foreach (var data in commentList)
                {
                    DiscussionCommentViewModel objcomment = new DiscussionCommentViewModel();
                    objcomment.Id = data.Id;
                    objcomment.topicid = data.TopicId;
                    objcomment.comment = data.Comment;
                    objcomment.createtime = data.CreationTime;
                    objcomment.user = GetUserById(data.CreatorUserId ?? 0);
                    objcomment.dislikecount = _discussionCommentLikeBusiness.TotalDisLikeCount(data.Id);
                    objcomment.likecount = _discussionCommentLikeBusiness.TotalLikeCount(data.Id);
                    objcomment.liked = _discussionCommentLikeBusiness.LikedByUser(data.Id, long.Parse(tc.Id));
                    objcomment.disliked = _discussionCommentLikeBusiness.DisLikedByUser(data.Id, long.Parse(tc.Id)); ;
                    List<DiscussionCommentFileViewModel> commentfileslist = new List<DiscussionCommentFileViewModel>();
                    var filesList = discussionCommentFilesBusiness.GetFilesByCommentId(Convert.ToInt32(objcomment.Id));
                    if (filesList.Count > 0)
                    {
                        foreach (var file in filesList)
                        {
                            DiscussionCommentFileViewModel objFiles = new DiscussionCommentFileViewModel();
                            objFiles.Id = file.Id;
                            objFiles.CommentId = file.CommentId;
                            objFiles.Name = file.Name;
                            objFiles.FileName = file.FileName;
                            objFiles.Url = file.Url;
                            objFiles.FileSize = file.FileSize;
                            objFiles.FileTypeId = file.FileTypeId;
                            objFiles.TotalPages = file.TotalPages;
                            objFiles.Duration = file.Duration;
                            objFiles.SignedUrl = GetUrlForComment(file.Id);
                            commentfileslist.Add(objFiles);
                        }
                    }
                    objcomment.files = commentfileslist;
                    lstCommentobj.Add(objcomment);
                }
                successResponse.data = lstCommentobj;
                successResponse.response_code = 0;
                successResponse.message = "Comments Details";
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

        [Authorize]
        [HttpDelete("DeleteComment")]
        public IActionResult DeleteComment(int Id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            try
            {
                var data = discussionCommentsBusiness.DeleteComment(Id, int.Parse(tc.Id));
                successResponse.response_code = 0;
                successResponse.message = "Record Deleted";
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
        #endregion

        #region users details 
        [HttpGet("{id}")]
        public DetailUserDTO GetUserById(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            CreateUserViewModel createUserViewModel = new CreateUserViewModel();

            //string Authorization = Request.Headers["Authorization"];
            string Authorization = Request.Headers["id_token"];


            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            createUserViewModel.Id = id;
            string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json"); //xxx

            User user = usersBusiness.UserExistanceById(createUserViewModel);
            if (user == null)
            {
                return null;
            }
            else
            {
                DetailUserDTO userDto = new DetailUserDTO
                {
                    Id = user.Id,
                    Username = user.Username,
                    FullName = user.FullName,
                    Email = user.Email,
                    is_skippable = user.is_skippable,
                    Bio = user.Bio,
                    timeout = user.timeout,
                    intervals = user.intervals,
                    reminder = user.reminder,
                    phonenumber = user.phonenumber,
                    istimeouton = user.istimeouton
                };
                if (!string.IsNullOrEmpty(user.ProfilePicUrl))
                    userDto.profilepicurl = LessonBusiness.geturl(user.ProfilePicUrl, Certificate);
                //List<Role> roles = usersBusiness.Role(user);

                //if (roles != null)
                //{
                //    List<long> roleids = new List<long>();
                //    List<string> rolenames = new List<string>();
                //    foreach (var role in roles)
                //    {
                //        roleids.Add(role.Id);
                //        rolenames.Add(role.Name);
                //    }
                //    userDto.Roles = roleids;
                //    userDto.RoleName = rolenames;
                //}
                return userDto;
            }
        }
        #endregion


        #region Discussion topics files
        [Authorize]
        [HttpPost("AddTopicFile")]
        public async Task<IActionResult> Post()
        {
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + _filesSettings.Value.CredentialFile);
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);

            DiscussionFileResponse successResponse = new DiscussionFileResponse();
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

                    if (Request.Form["fileTypeId"] != "")
                    {
                        var imageObject = await storage.UploadObjectAsync(
                            bucket: _filesSettings.Value.DiscussionBucket,
                            objectName: fileName,
                            contentType: file.ContentType,
                            source: file.OpenReadStream(),
                            options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        );
                        mediaLink = imageObject.MediaLink;
                    }
                    DiscussionFilesModel FilesModel = new DiscussionFilesModel();
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
                    DiscussionFiles newFiles = discussionFilesBusiness.Create(FilesModel, int.Parse(tc.Id));
                    var filetype = discussionFilesBusiness.FileType(newFiles);
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

                    successResponse.file_id = responseFilesModel.Id;
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

        [Authorize]
        [HttpPut("UpdateTopicFile/{id}")]
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
                    updateFilesModel.Id = id;
                    DiscussionFiles newFiles = discussionFilesBusiness.Update(updateFilesModel, int.Parse(tc.Id));
                    ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                    var filetyped = discussionFilesBusiness.FileType(newFiles);

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
        [HttpDelete("DeleteFile/{id}")]
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
                    DiscussionFiles newFiles = discussionFilesBusiness.Delete(id, int.Parse(tc.Id));
                    ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                    var filetyped = discussionFilesBusiness.FileType(newFiles);

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
        #endregion

        #region Get Signed Url
        [HttpGet]
        public string GetSignedUrl(long fileid)
        {
            try
            {
                SingnedUrlResponse singnedUrlResponse = new SingnedUrlResponse();
                DiscussionFiles newFiles = discussionFilesBusiness.getFilesById(fileid);
                string bucketName = _filesSettings.Value.DiscussionBucket;
                TimeSpan timeSpan = TimeSpan.FromHours(1);
                double exp = timeSpan.TotalMilliseconds;

                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + _filesSettings.Value.CredentialFile);
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(Certificate);
                string url = urlSigner.Sign(
                                               bucketName,
                                               newFiles.FileName,
                                               timeSpan,
                                               HttpMethod.Get
                                          );
                singnedUrlResponse.url = url;
                //singnedUrlResponse.exp = exp;
                return singnedUrlResponse.url;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Discussion Comment Files
        [Authorize]
        [HttpPost("AddCommentFile")]
        public async Task<IActionResult> AddCommentFile()
        {
            string jsonPath = Path.GetFileName(hostingEnvironment.WebRootPath + _filesSettings.Value.CredentialFile);
            var credential = GoogleCredential.FromFile(jsonPath);
            var storage = StorageClient.Create(credential);

            DiscussionFileResponse successResponse = new DiscussionFileResponse();
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

                    if (Request.Form["fileTypeId"] != "")
                    {
                        var imageObject = await storage.UploadObjectAsync(
                            bucket: _filesSettings.Value.DiscussionBucket,
                            objectName: fileName,
                            contentType: file.ContentType,
                            source: file.OpenReadStream(),
                            options: new UploadObjectOptions { PredefinedAcl = imageAcl }
                        );
                        mediaLink = imageObject.MediaLink;
                    }
                    DiscussionCommentFiles FilesModel = new DiscussionCommentFiles();
                    FilesModel.Url = mediaLink;
                    FilesModel.Name = fileName;
                    FilesModel.FileName = fileName;
                    FilesModel.FileTypeId = long.Parse(Request.Form["fileTypeId"]);
                    FilesModel.FileSize = file.Length;
                    if (!string.IsNullOrEmpty(Request.Form["duration"].ToString()))
                        FilesModel.Duration = Request.Form["duration"].ToString();
                    if (!string.IsNullOrEmpty(Request.Form["totalpages"]))
                        FilesModel.TotalPages = int.Parse(Request.Form["totalpages"]);
                    DiscussionCommentFiles newFiles = discussionCommentFilesBusiness.Create(FilesModel, int.Parse(tc.Id));
                    var filetype = discussionCommentFilesBusiness.FileType(newFiles);
                    ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                    responseFilesModel.Id = newFiles.Id;
                    responseFilesModel.name = newFiles.Name;
                    responseFilesModel.url = newFiles.Url;
                    responseFilesModel.filename = newFiles.FileName;
                    responseFilesModel.filetypeid = newFiles.FileTypeId;
                    responseFilesModel.filesize = newFiles.FileSize;
                    responseFilesModel.filetypename = filetype.Filetype;
                    responseFilesModel.duration = newFiles.Duration;
                    responseFilesModel.totalpages = newFiles.TotalPages;

                    successResponse.file_id = responseFilesModel.Id;
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

        [HttpGet("GetUrlForComment")]
        public string GetUrlForComment(long fileid)
        {
            try
            {
                SingnedUrlResponse singnedUrlResponse = new SingnedUrlResponse();
                DiscussionCommentFiles newFiles = discussionCommentFilesBusiness.getFilesById(fileid);
                string bucketName = _filesSettings.Value.DiscussionBucket;
                TimeSpan timeSpan = TimeSpan.FromHours(1);
                double exp = timeSpan.TotalMilliseconds;

                string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + _filesSettings.Value.CredentialFile);
                UrlSigner urlSigner = UrlSigner.FromServiceAccountPath(Certificate);
                string url = urlSigner.Sign(
                                               bucketName,
                                               newFiles.FileName,
                                               timeSpan,
                                               HttpMethod.Get
                                          );
                singnedUrlResponse.url = url;
                //singnedUrlResponse.exp = exp;
                return singnedUrlResponse.url;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        #region Like Dislike comment/topic
        [HttpPost("DiscussionTopicLike")]
        public IActionResult DiscussionTopicLike(DiscussionTopicLikeDTO dto)
        {
            DiscussionFileResponse successResponse = new DiscussionFileResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                var likeExist = _discussionTopicLikeBusiness.Exists(dto.topicid, long.Parse(tc.Id));
                if (likeExist != null)
                {
                    if (dto.like)
                    {
                        likeExist.Like = true;
                        likeExist.DisLike = false;
                    }
                    else if (dto.dislike)
                    {
                        likeExist.Like = false;
                        likeExist.DisLike = true;
                    }
                    else
                    {
                        likeExist.Like = false;
                        likeExist.DisLike = false;
                    }
                    likeExist.LastModificationTime = DateTime.UtcNow.ToString();
                    likeExist.LastModifierUserId = int.Parse(tc.Id);
                    _discussionTopicLikeBusiness.UpdateLike(likeExist);
                }
                else
                {
                    DiscussionTopicLikes discussionTopicLikes = new DiscussionTopicLikes();
                    if (dto.like)
                    {
                        discussionTopicLikes.Like = true;
                        discussionTopicLikes.DisLike = false;
                    }
                    else if (dto.dislike)
                    {
                        discussionTopicLikes.Like = false;
                        discussionTopicLikes.DisLike = true;
                    }
                    discussionTopicLikes.TopicId = dto.topicid;
                    discussionTopicLikes.UserId = long.Parse(tc.Id);
                    discussionTopicLikes.CreationTime = DateTime.UtcNow.ToString();
                    discussionTopicLikes.CreatorUserId = int.Parse(tc.Id);
                    _discussionTopicLikeBusiness.AddLike(discussionTopicLikes);
                }
                successResponse.response_code = 0;
                successResponse.message = "Success";
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

        [HttpPost("DiscussionCommentLike")]
        public IActionResult DiscussionCommentLike(DiscussionCommentLikeDTO dto)
        {
            DiscussionFileResponse successResponse = new DiscussionFileResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            try
            {
                string Authorization = Request.Headers["id_token"];
                TokenClaims tc = General.GetClaims(Authorization);
                tc.Id = LessonBusiness.getUserId(tc.sub);
                var likeExist = _discussionCommentLikeBusiness.Exists(dto.commentid, long.Parse(tc.Id));
                if (likeExist != null)
                {
                    if (dto.like)
                    {
                        likeExist.Like = true;
                        likeExist.DisLike = false;
                    }
                    else if (dto.dislike)
                    {
                        likeExist.Like = false;
                        likeExist.DisLike = true;
                    }
                    else
                    {
                        likeExist.Like = false;
                        likeExist.DisLike = false;
                    }
                    _discussionCommentLikeBusiness.UpdateLike(likeExist);
                }
                else
                {
                    DiscussionCommentLikes discussionCommentLikes = new DiscussionCommentLikes();
                    if (dto.like)
                    {
                        discussionCommentLikes.Like = true;
                        discussionCommentLikes.DisLike = false;
                    }
                    else if (dto.dislike)
                    {
                        discussionCommentLikes.Like = false;
                        discussionCommentLikes.DisLike = true;
                    }
                    discussionCommentLikes.CommentId = dto.commentid;
                    discussionCommentLikes.UserId = long.Parse(tc.Id);
                    discussionCommentLikes.CreationTime = DateTime.UtcNow.ToString();
                    discussionCommentLikes.CreatorUserId = int.Parse(tc.Id);
                    _discussionCommentLikeBusiness.AddLike(discussionCommentLikes);
                }
                successResponse.response_code = 0;
                successResponse.message = "Success";
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
        #endregion

        #region SearchTopic
        [Authorize]
        [HttpGet("SearchDiscussionTopic")]
        public IActionResult SearchDiscussionTopic(string keyword, int courseid, bool isprivate)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Certificate = Path.GetFileName(hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = LessonBusiness.getUserId(tc.sub);
            PaginationModel paginationModel = new PaginationModel
            {
                search = keyword
            };
            try
            {
                List<DiscussionTopicDTO> data = new List<DiscussionTopicDTO>();
                if (tc.RoleName.Contains(LessonBusiness.getRoleType("4")))
                {
                    data = DiscussionTopicBusiness.SearchTopicListForStudents(paginationModel, int.Parse(tc.Id), courseid, isprivate, Certificate);
                }
                if (tc.RoleName.Contains(LessonBusiness.getRoleType("3")))
                {
                    data = DiscussionTopicBusiness.SearchTopicListForTeachers(paginationModel, int.Parse(tc.Id), courseid, isprivate, Certificate);
                }
                successResponse.data = data;
                successResponse.response_code = 0;
                successResponse.message = "Record get sucess";
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
        #endregion
    }
}