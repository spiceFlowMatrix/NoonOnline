using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.Language;
using Trainning24.BL.ViewModels.Lesson;
using Trainning24.BL.ViewModels.Quiz;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class NotificationThreadsBusiness
    {
        private readonly UsersBusiness _usersBusiness;
        private readonly CourseBusiness _courseBusiness;
        private readonly UserNotificationBusiness _userNotificationBusiness;
        private readonly ChapterBusiness _chapterBusiness;
        private readonly LessonBusiness _lessonBusiness;
        private readonly ChapterQuizBusiness _chapterQuizBusiness;
        private readonly QuizBusiness _quizBusiness;
        private readonly EFLessonRepository _eFLessonRepository;
        public Language _language { get; }
        public NotificationThreadsBusiness(
        UsersBusiness usersBusiness,
        CourseBusiness courseBusiness,
        ChapterBusiness chapterBusiness,
        UserNotificationBusiness userNotificationBusiness,
        LessonBusiness lessonBusiness,
        EFLessonRepository eFLessonRepository,
        ChapterQuizBusiness chapterQuizBusiness,
        QuizBusiness quizBusiness,
        IOptions<Language> language
        )
        {
            _usersBusiness = usersBusiness;
            _courseBusiness = courseBusiness;
            _userNotificationBusiness = userNotificationBusiness;
            _chapterBusiness = chapterBusiness;
            _lessonBusiness = lessonBusiness;
            _eFLessonRepository = eFLessonRepository;
            _chapterQuizBusiness = chapterQuizBusiness;
            _quizBusiness = quizBusiness;
            _language = language.Value;
        }
        public int SendNotificationOnLessionAdd(Lesson obj, string Certificate, long userId)
        {
            UserDetails user = _usersBusiness.GetNotificationUserById(userId, Certificate);
            ChapterDTO chapter = _chapterBusiness.GetChapterById(Convert.ToInt32(obj.ChapterId));
            CourseDto course = _courseBusiness.GetCourseById(chapter.courseid, Certificate);
            List<long> userList = _usersBusiness.GetUserByCourseId(course.Id);
            LessonFile lessonFile = _eFLessonRepository.GetSingleLessionFile(obj.Id);
            LessonDTO lesson = _lessonBusiness.GetLessionById(obj);
            CreateNotificationViewModel createNotificationViewModel = new CreateNotificationViewModel();
            if (_language.lan == "fa")
            {
                //createNotificationViewModel.Tag = "اضافه شد" + " " + course.Name + " " + "به " + " " + obj.Name;
                createNotificationViewModel.Tag = obj.Name + " " + "به " + " " + course.Name + " " + "اضافه شد";
            }
            else if (_language.lan == "ps")
            {
                //createNotificationViewModel.Tag = "کې اضافه شو" + " " + course.Name + " " + "په" + " " + obj.Name;
                createNotificationViewModel.Tag = obj.Name + " " + "په" + " " + course.Name + " " + "کې اضافه شو";
            }
            else
            {
                createNotificationViewModel.Tag = obj.Name + " " + "was added to" + " " + course.Name;
            }
            createNotificationViewModel.IsRead = false;
            createNotificationViewModel.CourseId = course.Id;
            createNotificationViewModel.ChapterId = chapter.Id;
            createNotificationViewModel.LessionId = obj.Id;
            createNotificationViewModel.CreatorUserId = userId;
            createNotificationViewModel.Type = 3;
            createNotificationViewModel.User = user;
            createNotificationViewModel.Course = course;
            createNotificationViewModel.Chapter = chapter;
            createNotificationViewModel.lesson = lesson;
            if (lessonFile != null)
            {
                createNotificationViewModel.FileId = lessonFile.FileId;
            }
            var send = _userNotificationBusiness.CreateUserNotification(userList, createNotificationViewModel);
            return 0;
        }

        public int SendNotificationOnLessionUpdate(Lesson obj, string Certificate, long userId)
        {
            UserDetails user = _usersBusiness.GetNotificationUserById(userId, Certificate);
            ChapterDTO chapter = _chapterBusiness.GetChapterById(Convert.ToInt32(obj.ChapterId));
            CourseDto course = _courseBusiness.GetCourseById(chapter.courseid, Certificate);
            List<long> userList = _usersBusiness.GetUserByCourseId(course.Id);
            LessonFile lessonFile = _eFLessonRepository.GetSingleLessionFile(obj.Id);
            LessonDTO lesson = _lessonBusiness.GetLessionById(obj);
            CreateNotificationViewModel createNotificationViewModel = new CreateNotificationViewModel();
            if (_language.lan == "fa")
            {
                //createNotificationViewModel.Tag = "به‌روز شد" + " " + course.Name + " " + "در" + " " + obj.Name;
                createNotificationViewModel.Tag = obj.Name + " " + "در" + " " + course.Name + " " + "به‌روز شد";
            }
            else if (_language.lan == "ps")
            {
                //createNotificationViewModel.Tag = "کې نوی شو" + " " + course.Name + " " + "په" + " " + obj.Name;
                createNotificationViewModel.Tag = obj.Name + " " + "په" + " " + course.Name + " " + "کې نوی شو";
            }
            else
            {
                createNotificationViewModel.Tag = obj.Name + " " + "was updated in" + " " + course.Name;
            }
            createNotificationViewModel.IsRead = false;
            createNotificationViewModel.CourseId = course.Id;
            createNotificationViewModel.ChapterId = chapter.Id;
            createNotificationViewModel.LessionId = obj.Id;
            createNotificationViewModel.CreatorUserId = userId;
            createNotificationViewModel.Type = 3;
            createNotificationViewModel.User = user;
            createNotificationViewModel.Course = course;
            createNotificationViewModel.Chapter = chapter;
            createNotificationViewModel.lesson = lesson;
            if (lessonFile != null)
            {
                createNotificationViewModel.FileId = lessonFile.FileId;
            }
            var send = _userNotificationBusiness.CreateUserNotification(userList, createNotificationViewModel);
            return 0;
        }

        public int SendNotificationOnQuizAdd(Quiz obj, string Certificate, long userId)
        {
            UserDetails user = _usersBusiness.GetNotificationUserById(userId, Certificate);
            ChapterQuiz chapterQuiz = _chapterQuizBusiness.GetChaperQuizByQuizId(obj.Id);
            ChapterDTO chapter = _chapterBusiness.GetChapterById(Convert.ToInt32(chapterQuiz.ChapterId));
            CourseDto course = _courseBusiness.GetCourseById(chapter.courseid, Certificate);
            List<long> userList = _usersBusiness.GetUserByCourseId(course.Id);
            QuizDTO quizDTO = _quizBusiness.GetQuizDataById(obj);
            CreateNotificationViewModel createNotificationViewModel = new CreateNotificationViewModel();
            if (_language.lan == "fa")
            {
                //createNotificationViewModel.Tag = "اضافه شد" + " " + course.Name + " " + "به" + " " + obj.Name;
                createNotificationViewModel.Tag = obj.Name + " " + "به" + " " + course.Name + " " + "اضافه شد";
            }
            else if (_language.lan == "ps")
            {
                //createNotificationViewModel.Tag = "کې اضافه شو" + " " + course.Name + " " + "په" + " " + obj.Name;
                createNotificationViewModel.Tag = obj.Name + " " + "په" + " " + course.Name + " " + "کې اضافه شو";
            }
            else
            {
                createNotificationViewModel.Tag = obj.Name + " " + "was added to" + " " + course.Name;
            }
            createNotificationViewModel.IsRead = false;
            createNotificationViewModel.CourseId = course.Id;
            createNotificationViewModel.ChapterId = chapter.Id;
            createNotificationViewModel.QuizId = obj.Id;
            createNotificationViewModel.CreatorUserId = userId;
            createNotificationViewModel.Type = 9;
            createNotificationViewModel.User = user;
            createNotificationViewModel.Course = course;
            createNotificationViewModel.Chapter = chapter;
            createNotificationViewModel.quiz = quizDTO;
            var send = _userNotificationBusiness.CreateUserNotification(userList, createNotificationViewModel);
            return 0;
        }

        public int SendNotificationOnQuizUpdate(Quiz obj, string Certificate, long userId)
        {
            UserDetails user = _usersBusiness.GetNotificationUserById(userId, Certificate);
            ChapterQuiz chapterQuiz = _chapterQuizBusiness.GetChaperQuizByQuizId(obj.Id);
            ChapterDTO chapter = _chapterBusiness.GetChapterById(Convert.ToInt32(chapterQuiz.ChapterId));
            CourseDto course = _courseBusiness.GetCourseById(chapter.courseid, Certificate);
            List<long> userList = _usersBusiness.GetUserByCourseId(course.Id);
            QuizDTO quizDTO = _quizBusiness.GetQuizDataById(obj);
            CreateNotificationViewModel createNotificationViewModel = new CreateNotificationViewModel();
            if (_language.lan == "fa")
            {
                //createNotificationViewModel.Tag = "به‌روز شد" + " " + course.Name + " " + "در" + " " + obj.Name;
                createNotificationViewModel.Tag = obj.Name + " " + "در" + " " + course.Name + " " + "به‌روز شد";
            }
            else if (_language.lan == "ps")
            {
                //createNotificationViewModel.Tag = "کې نوی شو" + " " + course.Name + " " + "په" + " " + obj.Name;
                createNotificationViewModel.Tag = obj.Name + " " + "په" + " " + course.Name + " " + "کې نوی شو";
            }
            else
            {
                createNotificationViewModel.Tag = obj.Name + " " + "was updated to" + " " + course.Name;
            }
            createNotificationViewModel.IsRead = false;
            createNotificationViewModel.CourseId = course.Id;
            createNotificationViewModel.ChapterId = chapter.Id;
            createNotificationViewModel.QuizId = obj.Id;
            createNotificationViewModel.CreatorUserId = userId;
            createNotificationViewModel.Type = 9;
            createNotificationViewModel.User = user;
            createNotificationViewModel.Course = course;
            createNotificationViewModel.Chapter = chapter;
            createNotificationViewModel.quiz = quizDTO;
            var send = _userNotificationBusiness.CreateUserNotification(userList, createNotificationViewModel);
            return 0;
        }

        public int SendNotificationOnChapterAdd(Chapter obj, string Certificate, long userId)
        {
            UserDetails user = _usersBusiness.GetNotificationUserById(userId, Certificate);
            ChapterDTO chapter = _chapterBusiness.GetChapterById(Convert.ToInt32(obj.Id));
            CourseDto course = _courseBusiness.GetCourseById(obj.CourseId, Certificate);
            List<long> userList = _usersBusiness.GetUserByCourseId(course.Id);
            CreateNotificationViewModel createNotificationViewModel = new CreateNotificationViewModel();
            if (_language.lan == "fa")
            {
                //createNotificationViewModel.Tag = "اضافه شد" + " " + course.Name + " " + "به" + " " + chapter.Name;
                createNotificationViewModel.Tag = chapter.Name + " " + "به" + " " + course.Name + " " + "اضافه شد";
            }
            else if (_language.lan == "ps")
            {
                //createNotificationViewModel.Tag = "کې اضافه شو" + " " + course.Name + " " + "په" + " " + chapter.Name;
                createNotificationViewModel.Tag = chapter.Name + " " + "په" + " " + course.Name + " " + "کې اضافه شو";
            }
            else
            {
                createNotificationViewModel.Tag = chapter.Name + " " + "was added to" + " " + course.Name;
            }
            createNotificationViewModel.IsRead = false;
            createNotificationViewModel.CourseId = obj.Id;
            createNotificationViewModel.ChapterId = obj.Id;
            createNotificationViewModel.CreatorUserId = userId;
            createNotificationViewModel.Type = 8;
            createNotificationViewModel.User = user;
            createNotificationViewModel.Course = course;
            createNotificationViewModel.Chapter = chapter;
            var send = _userNotificationBusiness.CreateUserNotification(userList, createNotificationViewModel);
            return 0;
        }

        public int SendNotificationOnChapterUpdate(string oldname, Chapter obj, string Certificate, long userId)
        {
            UserDetails user = _usersBusiness.GetNotificationUserById(userId, Certificate);
            ChapterDTO chapter = _chapterBusiness.GetChapterById(Convert.ToInt32(obj.Id));
            CourseDto course = _courseBusiness.GetCourseById(obj.CourseId, Certificate);
            List<long> userList = _usersBusiness.GetUserByCourseId(course.Id);
            CreateNotificationViewModel createNotificationViewModel = new CreateNotificationViewModel();
            if (_language.lan == "fa")
            {
                //createNotificationViewModel.Tag = "تغییر داده شد" + " " + course.Name + " " + "در" + " " + oldname;
                createNotificationViewModel.Tag = oldname + " " + "در" + " " + course.Name + " " + "تغییر داده شد";
            }
            else if (_language.lan == "ps")
            {
                //createNotificationViewModel.Tag = "کې بدلون وموند" + " " + course.Name + " " + "په" + " " + oldname;
                createNotificationViewModel.Tag = oldname + " " + "په" + " " + course.Name + " " + "کې بدلون وموند";
            }
            else
            {
                createNotificationViewModel.Tag = oldname + " " + "was changed in" + " " + course.Name;
            }
            createNotificationViewModel.IsRead = false;
            createNotificationViewModel.CourseId = obj.Id;
            createNotificationViewModel.ChapterId = obj.Id;
            createNotificationViewModel.CreatorUserId = userId;
            createNotificationViewModel.Type = 8;
            createNotificationViewModel.User = user;
            createNotificationViewModel.Course = course;
            createNotificationViewModel.Chapter = chapter;
            var send = _userNotificationBusiness.CreateUserNotification(userList, createNotificationViewModel);
            return 0;
        }
    }
}
