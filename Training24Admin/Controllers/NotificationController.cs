using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Training24Admin.Model;
using Trainning24.BL.Business;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.DiscussionTopic;
using Trainning24.BL.ViewModels.Lesson;
using Trainning24.BL.ViewModels.Quiz;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;

namespace Training24Admin.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly LessonBusiness _lessionBusiness;
        private readonly UserNotificationBusiness _userNotificationBusiness;
        private readonly UsersBusiness _usersBusiness;
        private readonly CourseBusiness _courseBusiness;
        private readonly DiscussionTopicBusiness _discussionTopicBusiness;
        private readonly DiscussionCommentsBusiness _discussionCommentsBusiness;
        private readonly ChapterBusiness _chapterBusiness;
        private readonly QuizBusiness _quizBusiness;
        private IHostingEnvironment _hostingEnvironment;
        private readonly DiscussionTopicLikeBusiness _discussionTopicLikeBusiness;
        public NotificationController(
            LessonBusiness lessonBusiness,
            UserNotificationBusiness userNotificationBusiness,
            UsersBusiness usersBusiness,
            CourseBusiness courseBusiness,
            DiscussionTopicBusiness discussionTopicBusiness,
            DiscussionCommentsBusiness discussionCommentsBusiness,
            ChapterBusiness chapterBusiness,
            QuizBusiness quizBusiness,
            IHostingEnvironment hostingEnvironment,
            DiscussionTopicLikeBusiness discussionTopicLikeBusiness
            )
        {
            _lessionBusiness = lessonBusiness;
            _userNotificationBusiness = userNotificationBusiness;
            _usersBusiness = usersBusiness;
            _courseBusiness = courseBusiness;
            _discussionTopicBusiness = discussionTopicBusiness;
            _discussionCommentsBusiness = discussionCommentsBusiness;
            _chapterBusiness = chapterBusiness;
            _quizBusiness = quizBusiness;
            _hostingEnvironment = hostingEnvironment;
            _discussionTopicLikeBusiness = discussionTopicLikeBusiness;
        }
        [Authorize]
        [HttpGet("GetAllNotifications")]
        public IActionResult GetAllTopics(int pagenumber, int perpagerecord)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string jsonPath = Path.GetFileName(_hostingEnvironment.WebRootPath + "/training24-28e994f9833c.json");
            List<CreateNotificationViewModel> notificationlst = new List<CreateNotificationViewModel>();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessionBusiness.getUserId(tc.sub);
            PaginationModel paginationModel = new PaginationModel
            {
                pagenumber = pagenumber,
                perpagerecord = perpagerecord
            };
            try
            {
                List<UserNotifications> userNotifications = _userNotificationBusiness.GetNotificationList(paginationModel, int.Parse(tc.Id));
                if (userNotifications.Count > 0)
                {
                    foreach (var noti in userNotifications)
                    {
                        CreateNotificationViewModel objnotificationModel = new CreateNotificationViewModel();
                        objnotificationModel.Id = noti.Id;
                        objnotificationModel.Tag = noti.Tag;
                        objnotificationModel.Type = noti.Type;
                        objnotificationModel.UserId = noti.UserId;
                        objnotificationModel.IsRead = noti.IsRead;
                        objnotificationModel.CourseId = noti.CourseId;
                        objnotificationModel.AssignmentId = noti.AssignmentId;
                        objnotificationModel.LessionId = noti.LessionId;
                        objnotificationModel.DiscussionId = noti.DiscussionId;
                        objnotificationModel.CommentId = noti.CommentId;
                        objnotificationModel.ChapterId = noti.ChapterId;
                        objnotificationModel.QuizId = noti.QuizId;
                        objnotificationModel.FileId = noti.FileId;
                        objnotificationModel.DateCreated = noti.CreationTime;
                        objnotificationModel.CreatorUserId = noti.CreatorUserId ?? 0;
                        if (noti.CreatorUserId != null)
                        {
                            UserDetails userDetails = _usersBusiness.GetNotificationUserById(noti.CreatorUserId ?? 0, jsonPath);
                            objnotificationModel.User = userDetails;
                        }
                        if (noti.CourseId != null)
                        {
                            CourseDto course = _courseBusiness.GetCourseById(noti.CourseId ?? 0, jsonPath);
                            objnotificationModel.Course = course;
                        }
                        if (noti.LessionId != null)
                        {
                            LessonDTO lesson = _lessionBusiness.GetLessionById(Convert.ToInt32(noti.LessionId ?? 0));
                            objnotificationModel.lesson = lesson;
                        }
                        if (noti.DiscussionId != null)
                        {
                            DiscussionTopicDto discussion = _discussionTopicBusiness.GetNotificationTopicById(Convert.ToInt32(noti.DiscussionId ?? 0));
                            discussion.iseditable = _discussionTopicBusiness.CheckTopicOwner(discussion.id, long.Parse(tc.Id));
                            discussion.likecount = _discussionTopicLikeBusiness.TotalLikeCount(discussion.id);
                            discussion.dislikecount = _discussionTopicLikeBusiness.TotalDisLikeCount(discussion.id);
                            discussion.liked = _discussionTopicLikeBusiness.LikedByUser(discussion.id, long.Parse(tc.Id));
                            discussion.disliked = _discussionTopicLikeBusiness.DisLikedByUser(discussion.id, long.Parse(tc.Id));
                            objnotificationModel.discussion = discussion;
                        }
                        if (noti.CommentId != null)
                        {
                            DiscussionCommentDto comment = _discussionCommentsBusiness.GetCommentsById(Convert.ToInt32(noti.CommentId ?? 0));
                            objnotificationModel.comments = comment;
                        }
                        if (noti.ChapterId != null)
                        {
                            ChapterDTO chapter = _chapterBusiness.GetChapterById(Convert.ToInt32(noti.ChapterId ?? 0));
                            objnotificationModel.Chapter = chapter;
                        }
                        if (noti.QuizId != null)
                        {
                            QuizDTO quiz = _quizBusiness.GetQuizDataById(Convert.ToInt32(noti.QuizId ?? 0));
                            objnotificationModel.quiz = quiz;
                        }
                        notificationlst.Add(objnotificationModel);
                    }
                    successResponse.data = notificationlst;
                    successResponse.response_code = 0;
                    successResponse.message = "Record get sucess";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
                }
                else
                {
                    successResponse.response_code = 0;
                    successResponse.message = "No notification found";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
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
        [HttpPut("NotificationRead/{id}")]
        public IActionResult NotificationRead(int id)
        {
            SuccessResponse successResponse = new SuccessResponse();
            UnsuccessResponse unsuccessResponse = new UnsuccessResponse();
            string Authorization = Request.Headers["id_token"];
            TokenClaims tc = General.GetClaims(Authorization);
            tc.Id = _lessionBusiness.getUserId(tc.sub);
            try
            {
                UserNotifications userNotifications = _userNotificationBusiness.GetNotificationById(id);
                if (userNotifications != null)
                {
                    if (userNotifications.IsRead)
                    {
                        successResponse.response_code = 0;
                        successResponse.message = "Notification already read.";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                    else
                    {
                        UserNotifications updateReadStatus = _userNotificationBusiness.UpdateReadStatus(userNotifications);
                        successResponse.response_code = 0;
                        successResponse.message = "Notification read.";
                        successResponse.status = "Success";
                        return StatusCode(200, successResponse);
                    }
                }
                else
                {
                    successResponse.response_code = 0;
                    successResponse.message = "No notification found";
                    successResponse.status = "Success";
                    return StatusCode(200, successResponse);
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