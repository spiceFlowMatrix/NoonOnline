using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.DiscussionTopic;
using Trainning24.BL.ViewModels.Response;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class UserNotificationBusiness
    {
        private readonly EFUserNotificationRepository _EFUserNotificationRepository;
        private readonly UserSessionsBusiness UserSessionsBusiness;
        private readonly NotificationBusiness notificationBusiness;
        private readonly NotificationLogBusiness _notificationLogBusiness;
        public UserNotificationBusiness
        (
            EFUserNotificationRepository EFUserNotificationRepository,
            UserSessionsBusiness UserSessionsBusiness,
            NotificationBusiness notificationBusiness,
            NotificationLogBusiness notificationLogBusiness
        )
        {
            this._EFUserNotificationRepository = EFUserNotificationRepository;
            this.UserSessionsBusiness = UserSessionsBusiness;
            this.notificationBusiness = notificationBusiness;
            _notificationLogBusiness = notificationLogBusiness;
        }
        public NotificationResponse CreateUserNotification(List<long> userlist, CreateNotificationViewModel dto)
        {
            var response = new NotificationResponse();
            try
            {
                //type =1 for course
                if (dto.Type == 1)
                {
                    if (userlist.Count > 0)
                    {
                        foreach (var user in userlist)
                        {
                            UserNotifications userNotifications = new UserNotifications();
                            userNotifications.Type = dto.Type;
                            userNotifications.Tag = dto.Tag;
                            userNotifications.IsRead = false;
                            userNotifications.CourseId = dto.CourseId;
                            userNotifications.CreatorUserId = Convert.ToInt32(dto.CreatorUserId);
                            userNotifications.CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                            userNotifications.UserId = user;
                            userNotifications.IsDeleted = false;
                            _EFUserNotificationRepository.Insert(userNotifications);
                            _notificationLogBusiness.AddLogsObject(22, userNotifications.Id, dto.CreatorUserId, user, userNotifications.IsRead);
                        }
                        sendNotificationToUsers(userlist, dto);
                    }
                }
                //type = 2 for assignment
                else if (dto.Type == 2)
                {

                }
                //type = 3 for Lession
                else if (dto.Type == 3)
                {
                    if (userlist.Count > 0)
                    {
                        foreach (var user in userlist)
                        {
                            UserNotifications userNotifications = new UserNotifications();
                            userNotifications.Type = dto.Type;
                            userNotifications.Tag = dto.Tag;
                            userNotifications.IsRead = false;
                            userNotifications.CourseId = dto.CourseId;
                            userNotifications.CreatorUserId = Convert.ToInt32(dto.CreatorUserId);
                            userNotifications.CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                            userNotifications.UserId = user;
                            userNotifications.ChapterId = dto.ChapterId;
                            userNotifications.LessionId = dto.LessionId;
                            userNotifications.FileId = dto.FileId;
                            userNotifications.IsDeleted = false;
                            _EFUserNotificationRepository.Insert(userNotifications);
                            _notificationLogBusiness.AddLogsObject(22, userNotifications.Id, dto.CreatorUserId, user, userNotifications.IsRead);
                        }
                        sendNotificationToUsers(userlist, dto);
                    }
                }
                //type = 4 for Discussion
                else if (dto.Type == 4)
                {
                    if (userlist.Count > 0)
                    {
                        foreach (var user in userlist)
                        {
                            UserNotifications userNotifications = new UserNotifications();
                            userNotifications.Type = dto.Type;
                            userNotifications.Tag = dto.Tag;
                            userNotifications.IsRead = false;
                            userNotifications.DiscussionId = dto.DiscussionId;
                            userNotifications.CreatorUserId = Convert.ToInt32(dto.CreatorUserId);
                            userNotifications.CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                            userNotifications.UserId = user;
                            userNotifications.IsDeleted = false;
                            _EFUserNotificationRepository.Insert(userNotifications);
                            _notificationLogBusiness.AddLogsObject(22, userNotifications.Id, dto.CreatorUserId, user, userNotifications.IsRead);
                        }
                        sendNotificationToUsers(userlist, dto);
                    }
                }
                //type = 5 Discussion Files Add
                else if (dto.Type == 5)
                {

                }
                //type = 6 Comments
                else if (dto.Type == 6)
                {
                    if (userlist.Count > 0)
                    {
                        foreach (var user in userlist)
                        {
                            UserNotifications userNotifications = new UserNotifications();
                            userNotifications.Type = dto.Type;
                            userNotifications.Tag = dto.Tag;
                            userNotifications.IsRead = false;
                            userNotifications.DiscussionId = dto.DiscussionId;
                            userNotifications.CommentId = dto.CommentId;
                            userNotifications.CreatorUserId = Convert.ToInt32(dto.CreatorUserId);
                            userNotifications.CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                            userNotifications.UserId = user;
                            userNotifications.IsDeleted = false;
                            _EFUserNotificationRepository.Insert(userNotifications);
                            _notificationLogBusiness.AddLogsObject(22, userNotifications.Id, dto.CreatorUserId, user, userNotifications.IsRead);
                        }
                        sendNotificationToUsers(userlist, dto);
                    }
                }
                //type = 7 Comments file add
                else if (dto.Type == 7)
                {
                    if (userlist.Count > 0)
                    {
                        foreach (var user in userlist)
                        {
                            UserNotifications userNotifications = new UserNotifications();
                            userNotifications.Type = dto.Type;
                            userNotifications.Tag = dto.Tag;
                            userNotifications.IsRead = false;
                            userNotifications.DiscussionId = dto.DiscussionId;
                            userNotifications.CommentId = dto.CommentId;
                            userNotifications.FileId = dto.FileId;
                            userNotifications.CreatorUserId = Convert.ToInt32(dto.CreatorUserId);
                            userNotifications.CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                            userNotifications.UserId = user;
                            userNotifications.IsDeleted = false;
                            _EFUserNotificationRepository.Insert(userNotifications);
                            _notificationLogBusiness.AddLogsObject(22, userNotifications.Id, dto.CreatorUserId, user, userNotifications.IsRead);
                        }
                        sendNotificationToUsers(userlist, dto);
                    }
                }
                //type = 8 Chapter
                else if (dto.Type == 8)
                {
                    if (userlist.Count > 0)
                    {
                        foreach (var user in userlist)
                        {
                            UserNotifications userNotifications = new UserNotifications();
                            userNotifications.Type = dto.Type;
                            userNotifications.Tag = dto.Tag;
                            userNotifications.IsRead = false;
                            userNotifications.CourseId = dto.CourseId;
                            userNotifications.ChapterId = dto.ChapterId;
                            userNotifications.CreatorUserId = Convert.ToInt32(dto.CreatorUserId);
                            userNotifications.CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                            userNotifications.UserId = user;
                            userNotifications.IsDeleted = false;
                            _EFUserNotificationRepository.Insert(userNotifications);
                            _notificationLogBusiness.AddLogsObject(22, userNotifications.Id, dto.CreatorUserId, user, userNotifications.IsRead);
                        }
                        sendNotificationToUsers(userlist, dto);
                    }
                }
                //type = 9 for quiz
                else if (dto.Type == 9)
                {
                    if (userlist.Count > 0)
                    {
                        foreach (var user in userlist)
                        {
                            UserNotifications userNotifications = new UserNotifications();
                            userNotifications.Type = dto.Type;
                            userNotifications.Tag = dto.Tag;
                            userNotifications.IsRead = false;
                            userNotifications.CourseId = dto.CourseId;
                            userNotifications.ChapterId = dto.ChapterId;
                            userNotifications.QuizId = dto.QuizId;
                            userNotifications.CreatorUserId = Convert.ToInt32(dto.CreatorUserId);
                            userNotifications.CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                            userNotifications.UserId = user;
                            userNotifications.IsDeleted = false;
                            _EFUserNotificationRepository.Insert(userNotifications);
                            _notificationLogBusiness.AddLogsObject(22, userNotifications.Id, dto.CreatorUserId, user, userNotifications.IsRead);
                        }
                        sendNotificationToUsers(userlist, dto);
                    }
                }
                //type = 10 discussion topic public
                else if (dto.Type == 10)
                {

                }
                //type = 11 discussion comment public
                else if (dto.Type == 11)
                {
                    if (userlist.Count > 0)
                    {
                        foreach (var user in userlist)
                        {
                            UserNotifications userNotifications = new UserNotifications();
                            userNotifications.Type = dto.Type;
                            userNotifications.Tag = dto.Tag;
                            userNotifications.IsRead = false;
                            userNotifications.DiscussionId = dto.DiscussionId;
                            userNotifications.CommentId = dto.CommentId;
                            userNotifications.CreatorUserId = Convert.ToInt32(dto.CreatorUserId);
                            userNotifications.CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                            userNotifications.UserId = user;
                            userNotifications.IsDeleted = false;
                            _EFUserNotificationRepository.Insert(userNotifications);
                            _notificationLogBusiness.AddLogsObject(22, userNotifications.Id, dto.CreatorUserId, user, userNotifications.IsRead);
                        }
                        sendNotificationToUsers(userlist, dto);
                    }
                }
                //type = 12 discussion comment file public
                else if (dto.Type == 12)
                {
                    if (userlist.Count > 0)
                    {
                        foreach (var user in userlist)
                        {
                            UserNotifications userNotifications = new UserNotifications();
                            userNotifications.Type = dto.Type;
                            userNotifications.Tag = dto.Tag;
                            userNotifications.IsRead = false;
                            userNotifications.DiscussionId = dto.DiscussionId;
                            userNotifications.CommentId = dto.CommentId;
                            userNotifications.CreatorUserId = Convert.ToInt32(dto.CreatorUserId);
                            userNotifications.CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                            userNotifications.UserId = user;
                            userNotifications.IsDeleted = false;
                            _EFUserNotificationRepository.Insert(userNotifications);
                            _notificationLogBusiness.AddLogsObject(22, userNotifications.Id, dto.CreatorUserId, user, userNotifications.IsRead);
                        }
                        sendNotificationToUsers(userlist, dto);
                    }
                }
                response.success = true;
            }
            catch (Exception ex)
            {
                response.success = false;
                response.message = ex.Message;
            }
            return response;
        }

        public void sendNotificationToUsers(List<long> ids, CreateNotificationViewModel dto)
        {
            List<UserSessions> userSession = GetUserSessions(ids);
            var notificationContent = new NotificationContent { Alert = dto.Tag };
            notificationContent.AddCustom("n", Newtonsoft.Json.JsonConvert.SerializeObject(dto, Newtonsoft.Json.Formatting.None, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore }));
            notificationBusiness.SendNotification(userSession, notificationContent);
        }

        public List<UserSessions> GetUserSessions(List<long> obj)
        {
            return UserSessionsBusiness.GetUsersSession(obj);
        }

        public List<UserNotifications> GetNotificationList(PaginationModel paginationModel, int userid)
        {
            List<UserNotifications> userNotifications = _EFUserNotificationRepository.ListQuery(p => p.UserId == userid && p.IsDeleted != true && p.DeletionTime == null).ToList();
            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                userNotifications = userNotifications.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                return userNotifications;
            }
            return userNotifications;
        }

        public UserNotifications GetNotificationById(int id)
        {
            return _EFUserNotificationRepository.GetById(b => b.Id == id);
        }

        public UserNotifications UpdateReadStatus(UserNotifications obj)
        {
            obj.IsRead = true;
            _EFUserNotificationRepository.Update(obj);
            return obj;
        }
    }
}
