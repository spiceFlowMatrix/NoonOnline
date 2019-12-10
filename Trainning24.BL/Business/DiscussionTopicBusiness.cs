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
using Trainning24.BL.ViewModels.DiscussionTopic;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.Language;
using Microsoft.Extensions.Options;

namespace Trainning24.BL.Business
{
    public class DiscussionTopicBusiness
    {
        private readonly EFDiscussionTopicRepository _EFDiscussionTopicRepository;
        private readonly UsersBusiness UsersBusiness;
        private readonly UserSessionsBusiness UserSessionsBusiness;
        private readonly UserNotificationBusiness userNotificationBusiness;
        private readonly CourseBusiness courseBusiness;
        private readonly LogObjectBusiness _logObjectBusiness;
        private readonly StudentCourseBusiness _studentCourseBusiness;
        public Language _language { get; }
        public DiscussionTopicBusiness
        (
            EFDiscussionTopicRepository _efDiscussionTopicRepository,
            UsersBusiness UsersBusiness,
            UserSessionsBusiness UserSessionsBusiness,
            UserNotificationBusiness userNotificationBusiness,
            CourseBusiness courseBusiness,
            LogObjectBusiness logObjectBusiness,
            IOptions<Language> language,
            StudentCourseBusiness studentCourseBusiness
        )
        {
            _EFDiscussionTopicRepository = _efDiscussionTopicRepository;
            this.UsersBusiness = UsersBusiness;
            this.UserSessionsBusiness = UserSessionsBusiness;
            this.userNotificationBusiness = userNotificationBusiness;
            this.courseBusiness = courseBusiness;
            _logObjectBusiness = logObjectBusiness;
            _language = language.Value;
            _studentCourseBusiness = studentCourseBusiness;
        }

        public DiscussionTopic AddTopic(DiscussionTopic obj)
        {
            obj.CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            obj.IsDeleted = false;
            _EFDiscussionTopicRepository.Insert(obj);
            _logObjectBusiness.AddLogsObject(25, obj.Id, obj.CreatorUserId ?? 0);
            return obj;

        }

        public DiscussionTopic UpdateTopic(DiscussionTopic obj)
        {
            DiscussionTopic getTopic = _EFDiscussionTopicRepository.GetById(b => b.Id == obj.Id && b.IsDeleted != true);
            if (getTopic != null)
            {
                getTopic.IsDeleted = false;
                getTopic.LastModificationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                getTopic.Title = obj.Title;
                getTopic.Description = obj.Description;
                getTopic.LastModifierUserId = obj.LastModifierUserId;
                getTopic.IsPrivate = obj.IsPrivate;
                getTopic.IsPublic = obj.IsPublic;
                _EFDiscussionTopicRepository.Update(getTopic);
                _logObjectBusiness.AddLogsObject(26, getTopic.Id, getTopic.LastModifierUserId ?? 0);
                return getTopic;
            }
            else
            {
                return null;
            }
        }
        public List<DiscussionTopic> GetTopicsListByUserId(long userid)
        {
            return _EFDiscussionTopicRepository.ListQuery(b => b.CreatorUserId == userid && b.IsDeleted != true).ToList();
        }

        public DiscussionTopic GetTopicById(long topicId)
        {
            return _EFDiscussionTopicRepository.GetById(b => b.Id == topicId && b.IsDeleted != true);
        }

        public int DeleteTopic(int topicId, int userId)
        {
            DiscussionTopic getTopic = _EFDiscussionTopicRepository.GetById(b => b.Id == topicId && b.IsDeleted != true);
            if (getTopic != null)
            {
                getTopic.IsDeleted = true;
                getTopic.DeletionTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
                getTopic.DeleterUserId = userId;
                _EFDiscussionTopicRepository.Update(getTopic);
                _logObjectBusiness.AddLogsObject(27, getTopic.Id, getTopic.DeleterUserId ?? 0);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public List<DiscussionTopic> GetTopicListForStudents(PaginationModel paginationModel, int userid, int courseid)
        {
            List<DiscussionTopic> getPrivateRecords = new List<DiscussionTopic>();
            List<DiscussionTopic> getPublicList = new List<DiscussionTopic>();
            getPrivateRecords = _EFDiscussionTopicRepository.ListQuery(p => p.CreatorUserId == userid && p.CourseId == courseid && p.IsPrivate == true).ToList();
            getPublicList = _EFDiscussionTopicRepository.ListQuery(b => b.IsDeleted != true && b.CourseId == courseid && b.IsPrivate == false).ToList();

            if (getPrivateRecords.Count > 0)
            {
                getPublicList.AddRange(getPrivateRecords);
            }

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                getPublicList = getPublicList.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                return getPublicList;
            }
            return getPublicList;
        }

        public List<DiscussionTopic> GetPublicTopicListStudent(PaginationModel paginationModel, int userid, bool ispublic)
        {
            List<DiscussionTopic> getPublicList = new List<DiscussionTopic>();
            getPublicList = _EFDiscussionTopicRepository.ListQuery(b => b.IsDeleted != true && b.IsPublic == ispublic && b.IsPrivate == false).ToList();
            List<long> getCourseByUserId = _studentCourseBusiness.GetCourseIdByUser(userid);
            getCourseByUserId = getCourseByUserId.Distinct().ToList();
            if (getCourseByUserId.Count > 0)
            {
                foreach (var courseid in getCourseByUserId)
                {
                    var _sprivateRecords = _EFDiscussionTopicRepository.ListQuery(p => p.CreatorUserId == userid && p.CourseId == courseid && p.IsPrivate == true).ToList();
                    var _spublicRecords = _EFDiscussionTopicRepository.ListQuery(b => b.IsDeleted != true && b.CourseId == courseid && b.IsPrivate == false).ToList();
                    getPublicList.AddRange(_sprivateRecords);
                    getPublicList.AddRange(_spublicRecords);
                }
            }

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                getPublicList = getPublicList.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                return getPublicList;
            }
            return getPublicList;
        }

        public List<DiscussionTopic> GetPublicTopicListTeacher(PaginationModel paginationModel, int userid, bool ispublic)
        {
            List<DiscussionTopic> getPublicList = new List<DiscussionTopic>();
            getPublicList = _EFDiscussionTopicRepository.ListQuery(b => b.IsDeleted != true && b.IsPublic == ispublic && b.IsPrivate == false).ToList();
            List<long> getCourseByUserId = _studentCourseBusiness.GetCourseIdByUser(userid);
            getCourseByUserId = getCourseByUserId.Distinct().ToList();
            if (getCourseByUserId.Count > 0)
            {
                foreach (var courseid in getCourseByUserId)
                {
                    var techerRecords = _EFDiscussionTopicRepository.ListQuery(b => b.IsDeleted != true && b.CourseId == courseid).ToList();
                    getPublicList.AddRange(techerRecords);
                }
            }

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                getPublicList = getPublicList.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                return getPublicList;
            }
            return getPublicList;
        }

        public List<DiscussionTopic> GetTopicListForTeachers(PaginationModel paginationModel, int userid, int courseid)
        {
            List<DiscussionTopic> getPublicList = new List<DiscussionTopic>();
            getPublicList = _EFDiscussionTopicRepository.ListQuery(b => b.IsDeleted != true && b.CourseId == courseid).ToList();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                getPublicList = getPublicList.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                return getPublicList;
            }
            return getPublicList;
        }

        public List<long> GetTeacherByCourse(long courseId)
        {
            return UsersBusiness.GetTeachersByCourseId(courseId);
        }
        public List<long> GetStudentsByCourseId(long courseId)
        {
            return UsersBusiness.GetStudentsByCourseId(courseId);
        }

        //public List<UserSessions> GetUserSessions(List<long> obj)
        //{
        //    return UserSessionsBusiness.GetUsersSession(obj);
        //}

        public int SendNotificationToTeachers(DiscussionTopicViewModel obj, string Certificate)
        {
            List<long> userList = GetTeacherByCourse(obj.courseid);
            CourseDto course = courseBusiness.GetCourseById(obj.courseid, Certificate);
            UserDetails user = UsersBusiness.GetNotificationUserById(obj.createduserid, Certificate);
            DiscussionTopicDto discussion = GetNotificationTopicById(obj.id);
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
                //createNotificationViewModel.Tag = "اضافه کرد" + " " + course.Name + " " + "بحث را در" + " " + discussion.title + " " + "تحت عنوان" + " " + name;
                createNotificationViewModel.Tag = name + " " + "تحت عنوان" + " " + discussion.title + " " + "بحث را در" + " " + course.Name + " " + "اضافه کرد";
            }
            else if (_language.lan == "ps")
            {
                //createNotificationViewModel.Tag = "کې اضافه کړ" + " " + course.Name + " " + "تر عنوان لاندې بحث په" + " " + discussion.title + " " + "د" + " " + name;
                createNotificationViewModel.Tag = name + " " + "د" + " " + discussion.title + " " + "تر عنوان لاندې بحث په" + " " + course.Name + " " + "کې اضافه کړ";
            }
            else
            {
                createNotificationViewModel.Tag = name + " " + "was added discussion" + " " + discussion.title + "in" + " " + course.Name;
            }
            createNotificationViewModel.IsRead = false;
            createNotificationViewModel.DiscussionId = obj.id;
            createNotificationViewModel.CreatorUserId = obj.createduserid;
            createNotificationViewModel.Type = 4;
            //createNotificationViewModel.Course = course;
            createNotificationViewModel.User = user;
            createNotificationViewModel.discussion = discussion;
            var send = userNotificationBusiness.CreateUserNotification(userList, createNotificationViewModel);
            return 0;
        }

        public int SendNotificationToStudents(DiscussionTopicViewModel obj, string Certificate)
        {
            List<long> userList = GetStudentsByCourseId(obj.courseid);
            CourseDto course = courseBusiness.GetCourseById(obj.courseid, Certificate);
            UserDetails user = UsersBusiness.GetNotificationUserById(obj.createduserid, Certificate);
            DiscussionTopicDto discussion = GetNotificationTopicById(obj.id);
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
                //createNotificationViewModel.Tag = "اضافه کرد" + " " + course.Name + " " + "بحث را در" + " " + discussion.title + " " + "تحت عنوان" + " " + name;
                createNotificationViewModel.Tag = name + " " + "تحت عنوان" + " " + discussion.title + " " + "بحث را در" + " " + course.Name + " " + "اضافه کرد";
            }
            else if (_language.lan == "ps")
            {
                //createNotificationViewModel.Tag = "کې اضافه کړ" + " " + course.Name + " " + "تر عنوان لاندې بحث په" + " " + discussion.title + " " + "د" + " " + name;
                createNotificationViewModel.Tag = name + " " + "د" + " " + discussion.title + " " + "تر عنوان لاندې بحث په" + " " + course.Name + " " + "کې اضافه کړ";
            }
            else
            {
                createNotificationViewModel.Tag = name + " " + "was added discussion" + " " + discussion.title + "in" + " " + course.Name;
            }
            createNotificationViewModel.IsRead = false;
            createNotificationViewModel.DiscussionId = obj.id;
            createNotificationViewModel.CreatorUserId = obj.createduserid;
            createNotificationViewModel.Type = 4;
            //createNotificationViewModel.Course = course;
            createNotificationViewModel.User = user;
            createNotificationViewModel.discussion = discussion;
            var send = userNotificationBusiness.CreateUserNotification(userList, createNotificationViewModel);
            return 0;
        }

        public DiscussionTopicDto GetNotificationTopicById(long topicId)
        {
            DiscussionTopic discussionTopic = _EFDiscussionTopicRepository.GetById(b => b.Id == topicId && b.IsDeleted != true);
            DiscussionTopicDto discussionTopicDto = new DiscussionTopicDto();
            if (discussionTopic != null)
            {
                discussionTopicDto.id = discussionTopic.Id;
                discussionTopicDto.courseid = discussionTopic.CourseId;
                discussionTopicDto.title = discussionTopic.Title;
                discussionTopicDto.description = discussionTopic.Description;
                discussionTopicDto.isprivate = discussionTopic.IsPrivate;
            }
            return discussionTopicDto;
        }

        public bool CheckTopicOwner(long topicId, long userId)
        {
            bool value = false;
            var getTopic = _EFDiscussionTopicRepository.GetById(b => b.Id == topicId);
            if (getTopic != null)
            {
                if (getTopic.CreatorUserId == userId)
                {
                    value = true;
                }
            }
            return value;
        }

        public List<DiscussionTopicDTO> SearchTopicListForStudents(PaginationModel paginationModel, int userid, int courseid, bool IsPrivate, string certificate)
        {
            List<DiscussionTopic> getPublicList = new List<DiscussionTopic>();
            List<DiscussionTopicDTO> DiscussionTopicViewModel = new List<DiscussionTopicDTO>();
            if (IsPrivate)
            {
                getPublicList = _EFDiscussionTopicRepository.ListQuery(p => p.CreatorUserId == userid && p.CourseId == courseid && p.IsPrivate == true).ToList();
            }
            else
            {
                getPublicList = _EFDiscussionTopicRepository.ListQuery(b => b.IsDeleted != true && b.CourseId == courseid).ToList();
            }

            if (getPublicList.Count > 0)
            {
                DiscussionTopicViewModel = (from x in getPublicList
                                            select new DiscussionTopicDTO
                                            {
                                                id = x.Id,
                                                courseid = x.CourseId,
                                                title = x.Title,
                                                description = x.Description,
                                                isprivate = x.IsPrivate,
                                                ispublic = x.IsPublic,
                                                createduserid = x.CreatorUserId ?? 0,
                                                createddate = x.CreationTime,
                                                user = UsersBusiness.GetUsersDetails(x.CreatorUserId ?? 0, certificate)
                                            }).ToList();
            }

            if (!string.IsNullOrEmpty(paginationModel.search) && DiscussionTopicViewModel.Count > 0)
                DiscussionTopicViewModel = DiscussionTopicViewModel.OrderByDescending(b => b.id).Where(
                                   b => b.id.ToString().Contains(paginationModel.search)
                                || !string.IsNullOrEmpty(b.title) && b.title.ToLower().Contains(paginationModel.search.ToLower())
                                || !string.IsNullOrEmpty(b.description) && b.description.ToLower().Contains(paginationModel.search.ToLower())
                                || !string.IsNullOrEmpty(b.user.fullname) && b.user.fullname.ToLower().Contains(paginationModel.search.ToLower())
                                || !string.IsNullOrEmpty(b.user.username) && b.user.username.ToLower().Contains(paginationModel.search.ToLower())
                                ).ToList();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                DiscussionTopicViewModel = DiscussionTopicViewModel.OrderByDescending(b => b.id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                return DiscussionTopicViewModel;
            }
            return DiscussionTopicViewModel;
        }

        public List<DiscussionTopicDTO> SearchTopicListForTeachers(PaginationModel paginationModel, int userid, int courseid, bool IsPrivate, string certificate)
        {
            List<DiscussionTopic> getPublicList = new List<DiscussionTopic>();
            List<DiscussionTopicDTO> DiscussionTopicViewModel = new List<DiscussionTopicDTO>();
            if (IsPrivate)
            {
                getPublicList = _EFDiscussionTopicRepository.ListQuery(p => p.CourseId == courseid && p.IsPrivate == true).ToList();
            }
            else
            {
                getPublicList = _EFDiscussionTopicRepository.ListQuery(b => b.IsDeleted != true && b.CourseId == courseid).ToList();
            }

            if (getPublicList.Count > 0)
            {
                DiscussionTopicViewModel = (from x in getPublicList
                                            select new DiscussionTopicDTO
                                            {
                                                id = x.Id,
                                                courseid = x.CourseId,
                                                title = x.Title,
                                                description = x.Description,
                                                isprivate = x.IsPrivate,
                                                ispublic = x.IsPublic,
                                                createduserid = x.CreatorUserId ?? 0,
                                                createddate = x.CreationTime,
                                                user = UsersBusiness.GetUsersDetails(x.CreatorUserId ?? 0, certificate)
                                            }).ToList();
            }

            if (!string.IsNullOrEmpty(paginationModel.search) && DiscussionTopicViewModel.Count > 0)
                DiscussionTopicViewModel = DiscussionTopicViewModel.OrderByDescending(b => b.id).Where(
                                   b => b.id.ToString().Contains(paginationModel.search)
                                || !string.IsNullOrEmpty(b.title) && b.title.ToLower().Contains(paginationModel.search.ToLower())
                                || !string.IsNullOrEmpty(b.description) && b.description.ToLower().Contains(paginationModel.search.ToLower())
                                || !string.IsNullOrEmpty(b.user.fullname) && b.user.fullname.ToLower().Contains(paginationModel.search.ToLower())
                                || !string.IsNullOrEmpty(b.user.username) && b.user.username.ToLower().Contains(paginationModel.search.ToLower())
                                ).ToList();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                DiscussionTopicViewModel = DiscussionTopicViewModel.OrderByDescending(b => b.id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                return DiscussionTopicViewModel;
            }
            return DiscussionTopicViewModel;
        }
    }
}
