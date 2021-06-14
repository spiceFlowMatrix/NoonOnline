using System;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using Trainning24.Abstract.Entity;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;
using Trainning24.BL.ViewModels.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Trainning24.BL.ViewModels.UserRole;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.DiscussionTopic;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.Language;
using Microsoft.Extensions.Options;

namespace Trainning24.BL.Business
{
    public class DiscussionCommentsBusiness
    {
        private readonly EFDiscussionCommentsRepository _EFDiscussionCommentsRepository;
        private readonly UsersBusiness UsersBusiness;
        private readonly UserNotificationBusiness userNotificationBusiness;
        private readonly DiscussionTopicBusiness discussionTopicBusiness;
        private readonly DiscussionCommentFilesBusiness discussionCommentFilesBusiness;
        private readonly LogObjectBusiness _logObjectBusiness;
        private readonly EFDiscussionTopicRepository _eFDiscussionTopicRepository;
        public Language _language { get; }
        public DiscussionCommentsBusiness
        (
            EFDiscussionCommentsRepository _efDiscussionCommentsRepository,
            UsersBusiness UsersBusiness,
            UserNotificationBusiness userNotificationBusiness,
            DiscussionTopicBusiness discussionTopicBusiness,
            DiscussionCommentFilesBusiness discussionCommentFilesBusiness,
            LogObjectBusiness logObjectBusiness,
            EFDiscussionTopicRepository eFDiscussionTopicRepository,
            IOptions<Language> language
        )
        {
            _EFDiscussionCommentsRepository = _efDiscussionCommentsRepository;
            this.UsersBusiness = UsersBusiness;
            this.userNotificationBusiness = userNotificationBusiness;
            this.discussionTopicBusiness = discussionTopicBusiness;
            this.discussionCommentFilesBusiness = discussionCommentFilesBusiness;
            _logObjectBusiness = logObjectBusiness;
            _eFDiscussionTopicRepository = eFDiscussionTopicRepository;
            _language = language.Value;
        }

        public DiscussionComments AddComments(DiscussionComments obj)
        {
            obj.CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            obj.IsDeleted = false;
            _EFDiscussionCommentsRepository.Insert(obj);
            _logObjectBusiness.AddLogsObject(28, obj.Id, obj.CreatorUserId ?? 0);
            return obj;
        }

        public DiscussionComments UpdateComments(DiscussionComments obj)
        {
            DiscussionComments getComment = _EFDiscussionCommentsRepository.GetById(b => b.Id == obj.Id && b.IsDeleted != true);
            if (getComment != null)
            {
                getComment.IsDeleted = false;
                getComment.LastModificationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                getComment.Comment = obj.Comment;
                getComment.LastModifierUserId = obj.LastModifierUserId;
                _EFDiscussionCommentsRepository.Update(getComment);
                _logObjectBusiness.AddLogsObject(29, getComment.Id, getComment.LastModifierUserId ?? 0);
                return getComment;
            }
            else
            {
                return null;
            }
        }
        public List<DiscussionComments> GetCommentsListByUserId(long userid)
        {
            return _EFDiscussionCommentsRepository.ListQuery(b => b.CreatorUserId == userid && b.IsDeleted != true).ToList();
        }

        public DiscussionComments GetCommentById(long commentId)
        {
            return _EFDiscussionCommentsRepository.GetById(b => b.Id == commentId && b.IsDeleted != true);
        }

        public DiscussionComments ValidateUser(long userId)
        {
            return _EFDiscussionCommentsRepository.GetById(b => b.CreatorUserId == userId);
        }

        public int DeleteComment(int commentId, int userId)
        {
            DiscussionComments getComment = _EFDiscussionCommentsRepository.GetById(b => b.Id == commentId && b.IsDeleted != true);
            if (getComment != null)
            {
                getComment.IsDeleted = true;
                getComment.DeletionTime = DateTime.Now.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                getComment.DeleterUserId = userId;
                _EFDiscussionCommentsRepository.Update(getComment);
                _logObjectBusiness.AddLogsObject(30, getComment.Id, getComment.DeleterUserId ?? 0);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public List<DiscussionComments> GetCommentList(PaginationModel paginationModel, int topicId)
        {
            List<DiscussionComments> commentList = new List<DiscussionComments>();
            commentList = _EFDiscussionCommentsRepository.ListQuery(b => b.TopicId == topicId && b.IsDeleted != true).ToList();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                commentList = commentList.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                return commentList;
            }
            return commentList;
        }

        public int GetCommentCounts(long topicId)
        {
            return _EFDiscussionCommentsRepository.ListQuery(b => b.TopicId == topicId && b.IsDeleted != true).Count();
        }

        public int SendNotificationOnComment(DiscussionCommentViewModel obj, string Certificate, long userId)
        {
            List<long> userList = GetCommentUserByTopicId(obj.topicid, userId);
            UserDetails user = UsersBusiness.GetNotificationUserById(userId, Certificate);
            DiscussionTopicDto topic = discussionTopicBusiness.GetNotificationTopicById(obj.topicid);
            DiscussionCommentDto comments = GetCommentsById(obj.Id);
            CreateNotificationViewModel createNotificationViewModel = new CreateNotificationViewModel();
            string name = "";
            if (!string.IsNullOrEmpty(user.fullname))
            {
                name = user.fullname;
            }
            else
            {
                name = user.username;
            }
            if (_language.lan == "fa")
            {
                //createNotificationViewModel.Tag = "نظر داد" + " " + topic.title + " " + "روی بحث" + " " + name;
                createNotificationViewModel.Tag = name + " " + "روی بحث" + " " + topic.title + " " + "نظر داد";
            }
            else if (_language.lan == "ps")
            {
                //createNotificationViewModel.Tag = "باندې نظر ورکړ" + " " + topic.title + " " + "پر" + " " + name;
                createNotificationViewModel.Tag = name + " " + "پر" + " " + topic.title + " " + "باندې نظر ورکړ";
            }
            else
            {
                createNotificationViewModel.Tag = name + " " + "commented on discussion" + " " + topic.title;
            }
            createNotificationViewModel.IsRead = false;
            createNotificationViewModel.DiscussionId = topic.id;
            createNotificationViewModel.CommentId = obj.Id;
            createNotificationViewModel.CreatorUserId = userId;
            createNotificationViewModel.Type = 6;
            createNotificationViewModel.User = user;
            createNotificationViewModel.comments = comments;
            createNotificationViewModel.discussion = topic;
            var send = userNotificationBusiness.CreateUserNotification(userList, createNotificationViewModel);
            return 0;
        }

        public int SendNotificationOnFileupload(DiscussionCommentViewModel obj, string Certificate, long userId)
        {
            List<long> userList = GetCommentUserByTopicId(obj.topicid, userId);
            UserDetails user = UsersBusiness.GetNotificationUserById(userId, Certificate);
            DiscussionTopicDto topic = discussionTopicBusiness.GetNotificationTopicById(obj.topicid);
            DiscussionCommentDto comments = GetCommentsById(obj.Id);
            CreateNotificationViewModel createNotificationViewModel = new CreateNotificationViewModel();
            string name = "";
            if (!string.IsNullOrEmpty(user.fullname))
            {
                name = user.fullname;
            }
            else
            {
                name = user.username;
            }
            if (_language.lan == "fa")
            {
                //createNotificationViewModel.Tag = "فایل اضافه کرد" + " " + topic.title + " " + "در بحث" + " " + name;
                createNotificationViewModel.Tag = name + " " + "در بحث" + " " + topic.title + " " + "فایل اضافه کرد";
            }
            else if (_language.lan == "ps")
            {
                //createNotificationViewModel.Tag = "بحث کې فایل اضافه کړ" + " " + topic.title + " " + "په" + " " + name;
                createNotificationViewModel.Tag = name + " " + "په" + " " + topic.title + " " + "بحث کې فایل اضافه کړ";
            }
            else
            {
                createNotificationViewModel.Tag = name + " " + "added file on discussion" + " " + topic.title;
            }
            createNotificationViewModel.IsRead = false;
            createNotificationViewModel.DiscussionId = topic.id;
            createNotificationViewModel.CommentId = obj.Id;
            createNotificationViewModel.FileId = Convert.ToInt32(obj.filesid.FirstOrDefault());
            createNotificationViewModel.CreatorUserId = userId;
            createNotificationViewModel.Type = 7;
            createNotificationViewModel.User = user;
            createNotificationViewModel.comments = comments;
            createNotificationViewModel.files = obj.files;
            createNotificationViewModel.discussion = topic;
            var send = userNotificationBusiness.CreateUserNotification(userList, createNotificationViewModel);
            return 0;
        }


        public List<long> GetCommentUserByTopicId(long Id, long userid)
        {
            var obj = _EFDiscussionCommentsRepository.ListQuery(b => b.TopicId == Id && b.DeleterUserId == null && b.CreatorUserId != userid)
                      .Select(b => b.CreatorUserId).Distinct().ToList();

            List<long> longs = obj.ConvertAll(i => (long)i);

            var getTopicOwner = _eFDiscussionTopicRepository.ListQuery(b => b.Id == Id && b.DeletionTime == null && b.CreatorUserId != userid)
                                .Select(b => b.CreatorUserId).FirstOrDefault();
            if (getTopicOwner != null)
            {
                long topicOwner = (long)getTopicOwner;
                longs.Add(topicOwner);
                longs = longs.Distinct().ToList();
            }
            return longs;
        }

        public DiscussionCommentDto GetCommentsById(long Id)
        {
            DiscussionComments discussionComments = _EFDiscussionCommentsRepository.GetById(b => b.Id == Id && b.IsDeleted != true);
            DiscussionCommentDto discussionCommentDto = new DiscussionCommentDto();
            if (discussionComments != null)
            {
                discussionCommentDto.Id = discussionComments.Id;
                discussionCommentDto.comment = discussionComments.Comment;
                discussionCommentDto.topicid = discussionComments.TopicId;
                discussionCommentDto.createtime = discussionComments.CreationTime;
            }
            return discussionCommentDto;
        }


        public int SendNotificationOnPublicComment(DiscussionCommentViewModel obj, string Certificate, long userId)
        {
            List<long> userList = GetCommentUserByTopicId(obj.topicid, userId);
            UserDetails user = UsersBusiness.GetNotificationUserById(userId, Certificate);
            DiscussionTopicDto topic = discussionTopicBusiness.GetNotificationTopicById(obj.topicid);
            DiscussionCommentDto comments = GetCommentsById(obj.Id);
            CreateNotificationViewModel createNotificationViewModel = new CreateNotificationViewModel();
            string name = "";
            if (!string.IsNullOrEmpty(user.fullname))
            {
                name = user.fullname;
            }
            else
            {
                name = user.username;
            }
            if (_language.lan == "fa")
            {
                //createNotificationViewModel.Tag = "نظر داد" + " " + topic.title + " " + "روی بحث" + " " + name;
                createNotificationViewModel.Tag = name + " " + "روی بحث" + " " + topic.title + " " + "نظر داد";
            }
            else if (_language.lan == "ps")
            {
                //createNotificationViewModel.Tag = "باندې نظر ورکړ" + " " + topic.title + " " + "پر" + " " + name;
                createNotificationViewModel.Tag = name + " " + "پر" + " " + topic.title + " " + "باندې نظر ورکړ";
            }
            else
            {
                createNotificationViewModel.Tag = name + " " + "commented on discussion" + " " + topic.title;
            }
            createNotificationViewModel.IsRead = false;
            createNotificationViewModel.DiscussionId = topic.id;
            createNotificationViewModel.CommentId = obj.Id;
            createNotificationViewModel.CreatorUserId = userId;
            createNotificationViewModel.Type = 11;
            createNotificationViewModel.User = user;
            createNotificationViewModel.comments = comments;
            createNotificationViewModel.discussion = topic;
            var send = userNotificationBusiness.CreateUserNotification(userList, createNotificationViewModel);
            return 0;
        }

        public int SendNotificationOnFileuploadPublic(DiscussionCommentViewModel obj, string Certificate, long userId)
        {
            List<long> userList = GetCommentUserByTopicId(obj.topicid, userId);
            UserDetails user = UsersBusiness.GetNotificationUserById(userId, Certificate);
            DiscussionTopicDto topic = discussionTopicBusiness.GetNotificationTopicById(obj.topicid);
            DiscussionCommentDto comments = GetCommentsById(obj.Id);
            CreateNotificationViewModel createNotificationViewModel = new CreateNotificationViewModel();
            string name = "";
            if (!string.IsNullOrEmpty(user.fullname))
            {
                name = user.fullname;
            }
            else
            {
                name = user.username;
            }
            if (_language.lan == "fa")
            {
                //createNotificationViewModel.Tag = "فایل اضافه کرد" + " " + topic.title + " " + "در بحث" + " " + name;
                createNotificationViewModel.Tag = name + " " + "در بحث" + " " + topic.title + " " + "فایل اضافه کرد";
            }
            else if (_language.lan == "ps")
            {
                //createNotificationViewModel.Tag = "بحث کې فایل اضافه کړ" + " " + topic.title + " " + "په" + " " + name;
                createNotificationViewModel.Tag = name + " " + "په" + " " + topic.title + " " + "بحث کې فایل اضافه کړ";
            }
            else
            {
                createNotificationViewModel.Tag = name + " " + "added file on discussion" + " " + topic.title;
            }
            createNotificationViewModel.IsRead = false;
            createNotificationViewModel.DiscussionId = topic.id;
            createNotificationViewModel.CommentId = obj.Id;
            createNotificationViewModel.FileId = Convert.ToInt32(obj.filesid.FirstOrDefault());
            createNotificationViewModel.CreatorUserId = userId;
            createNotificationViewModel.Type = 12;
            createNotificationViewModel.User = user;
            createNotificationViewModel.comments = comments;
            createNotificationViewModel.files = obj.files;
            createNotificationViewModel.discussion = topic;
            var send = userNotificationBusiness.CreateUserNotification(userList, createNotificationViewModel);
            return 0;
        }
    }
}
