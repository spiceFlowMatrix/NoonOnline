using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1;
using Google.Cloud.Storage.V1;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels;
using Trainning24.BL.ViewModels.Assignment;
using Trainning24.BL.ViewModels.AssignmentFile;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.CourseGrade;
using Trainning24.BL.ViewModels.DiscussionTopic;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Grade;
using Trainning24.BL.ViewModels.Language;
using Trainning24.BL.ViewModels.Lesson;
using Trainning24.BL.ViewModels.LessonAssignment;
using Trainning24.BL.ViewModels.LessonAssignmentFile;
using Trainning24.BL.ViewModels.LessonFile;
using Trainning24.BL.ViewModels.StudentLessonProgress;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Domain.Helper;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class CourseBusiness
    {
        private readonly EFCourseRepository EFCourseRepository;
        private readonly EFChapterRepository EFChapterRepository;
        private readonly EFLessonRepository EFLessonRepository;
        private readonly EFGradeRepository EFGradeRepository;
        private readonly EFQuizRepository EFQuizRepository;
        private readonly EFCourseGradeRepository EFCourseGradeRepository;
        private readonly LessonBusiness LessonBusiness;
        private readonly AssignmentBusiness AssignmentBusiness;
        private readonly CourseGradeBusiness CourseGradeBusiness;
        private readonly GradeBusiness GradeBusiness;
        private readonly QuizBusiness QuizBusiness;
        private static Training24Context _training24Context;
        private readonly ChapterBusiness chapterBusiness;
        private readonly ProgressBusiness ProgressBusiness;
        private readonly EFStudentCourseRepository EFStudentCourseRepository;
        private readonly EFChapterQuizRepository EFChapterQuizRepository;
        private readonly UserNotificationBusiness UserNotificationBusiness;
        private readonly UsersBusiness usersBusiness;
        private readonly UserNotificationBusiness userNotificationBusiness;
        private readonly LogObjectBusiness _logObjectBusiness;
        private readonly EFAssignmentRepository _eFAssignmentRepository;
        private readonly LessonAssignmentBusiness _lessonAssignmentBusiness;
        public Language _language { get; }
        public CourseBusiness
        (
            EFCourseRepository EFCourseRepository,
            EFChapterRepository EFChapterRepository,
            EFLessonRepository EFLessonRepository,
            LessonBusiness LessonBusiness,
            EFQuizRepository EFQuizRepository,
            ProgressBusiness ProgressBusiness,
            ChapterBusiness chapterBusiness,
            QuizBusiness QuizBusiness,
            AssignmentBusiness AssignmentBusiness,
            CourseGradeBusiness CourseGradeBusiness,
            GradeBusiness GradeBusiness,
            Training24Context training24Context,
            EFCourseGradeRepository EFCourseGradeRepository,
            EFGradeRepository EFGradeRepository,
            EFStudentCourseRepository EFStudentCourseRepository,
            EFChapterQuizRepository EFChapterQuizRepository,
            UserNotificationBusiness UserNotificationBusiness,
            UsersBusiness usersBusiness,
            UserNotificationBusiness userNotificationBusiness,
            LogObjectBusiness logObjectBusiness,
            IOptions<Language> language,
            EFAssignmentRepository eFAssignmentRepository,
            LessonAssignmentBusiness lessonAssignmentBusiness
        )
        {
            _training24Context = training24Context;
            this.EFCourseRepository = EFCourseRepository;
            this.EFChapterRepository = EFChapterRepository;
            this.EFLessonRepository = EFLessonRepository;
            this.LessonBusiness = LessonBusiness;
            this.EFQuizRepository = EFQuizRepository;
            this.chapterBusiness = chapterBusiness;
            this.GradeBusiness = GradeBusiness;
            this.ProgressBusiness = ProgressBusiness;
            this.QuizBusiness = QuizBusiness;
            this.AssignmentBusiness = AssignmentBusiness;
            this.CourseGradeBusiness = CourseGradeBusiness;
            this.EFCourseGradeRepository = EFCourseGradeRepository;
            this.EFGradeRepository = EFGradeRepository;
            this.EFStudentCourseRepository = EFStudentCourseRepository;
            this.EFChapterQuizRepository = EFChapterQuizRepository;
            this.UserNotificationBusiness = UserNotificationBusiness;
            this.usersBusiness = usersBusiness;
            this.userNotificationBusiness = userNotificationBusiness;
            _logObjectBusiness = logObjectBusiness;
            _language = language.Value;
            _eFAssignmentRepository = eFAssignmentRepository;
            _lessonAssignmentBusiness = lessonAssignmentBusiness;
        }


        public void updateTrailCourse(long userid)
        {
            (from usercourse in _training24Context.UserCourse
             join courses in _training24Context.Course on usercourse.CourseId equals courses.Id
             where usercourse.UserId == userid &&
             (!string.IsNullOrEmpty(usercourse.EndDate) && Convert.ToDateTime(usercourse.EndDate, CultureInfo.InvariantCulture).Date < DateTime.Now.Date)
             //&& courses.istrial == true
             select usercourse).ToList().ForEach(x => x.IsExpire = true);

            _training24Context.SaveChanges();
        }

        public Course Create(AddCourseModel AddCourseModel, int id)
        {
            Course Course = new Course
            {
                Name = AddCourseModel.name,
                Code = AddCourseModel.code,
                Description = AddCourseModel.description,
                Image = AddCourseModel.image,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id,
                istrial = AddCourseModel.istrial,
                IsDeleted = false
            };

            EFCourseRepository.Insert(Course);
            AddCourseGradeModel addCourseGradeModel = new AddCourseGradeModel();
            addCourseGradeModel.courseid = Course.Id;
            addCourseGradeModel.gradeid = AddCourseModel.gradeid;

            CourseGradeBusiness.Create(addCourseGradeModel, id);
            _logObjectBusiness.AddLogsObject(1, Course.Id, id);
            return Course;
        }

        public Course Update(UpdateCourseModel UpdateCourseModel, int id)
        {
            Course Course = new Course
            {
                Id = UpdateCourseModel.id,
                Name = UpdateCourseModel.name,
                Code = UpdateCourseModel.code,
                Description = UpdateCourseModel.description,
                istrial = UpdateCourseModel.istrial,
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = id
            };

            if (!string.IsNullOrEmpty(UpdateCourseModel.image))
                Course.Image = UpdateCourseModel.image;

            EFCourseRepository.Update(Course);

            ////Send Notification to related student and Teacher
            //CreateNotificationViewModel createNotificationViewModel = new CreateNotificationViewModel();
            //createNotificationViewModel.type = 1;
            //createNotificationViewModel.courseid = Course.Id;
            //UserNotificationBusiness.CreateUserNotificationAsync(createNotificationViewModel);

            AddCourseGradeModel addCourseGradeModel = new AddCourseGradeModel();
            addCourseGradeModel.courseid = Course.Id;
            addCourseGradeModel.gradeid = UpdateCourseModel.gradeid;
            CourseGradeBusiness.Create(addCourseGradeModel, id);
            _logObjectBusiness.AddLogsObject(2, Course.Id, id);
            return Course;
        }

        public List<Course> CourseList(PaginationModel paginationModel, out int total)
        {
            List<Course> courseList = new List<Course>();
            courseList = EFCourseRepository.GetAll();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = courseList.Count();

                if (!string.IsNullOrEmpty(paginationModel.search))
                {
                    courseList = courseList.Where(b => b.Code != null && b.Code.ToLower().Contains(paginationModel.search.ToLower()) ||
                    b.Name != null && b.Name.ToLower().Contains(paginationModel.search.ToLower()) ||
                    b.Id.ToString().ToLower().Contains(paginationModel.search.ToLower())).ToList();

                    total = courseList.Count();

                    courseList = courseList.OrderByDescending(b => b.Id).
                                Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                                Take(paginationModel.perpagerecord).
                                ToList();

                    return courseList;
                }

                courseList = courseList.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();

                return courseList;
            }

            total = courseList.Count();
            return courseList;
        }



        public object GetCourseList(PaginationModel paginationModel, out int total)
        {
            var courseList = EFCourseRepository.ListQuery(c => c.IsDeleted != true).Select(s => new
            {
                s.Id,
                s.Name,
                s.Code
            }).ToList();
            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = courseList.Count();

                courseList = courseList.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();

                if (!string.IsNullOrEmpty(paginationModel.search))
                    courseList = courseList.OrderByDescending(b => b.Id).Where(b => b.Name.Any(k => b.Id.ToString().Contains(paginationModel.search)
                                                                    || b.Name.ToLower().Contains(paginationModel.search.ToLower()))
                                                                    || b.Code.ToLower().Contains(paginationModel.search.ToLower()))
                                                                    .ToList();

                return courseList;
            }

            if (!string.IsNullOrEmpty(paginationModel.search))
                courseList = courseList.OrderByDescending(b => b.Id).Where(b => b.Name.ToLower().Any(k => b.Name.Contains(paginationModel.search.ToLower()))
                                                                ).ToList();

            total = courseList.Count();
            return courseList;
        }

        public Course getCourseById(long id)
        {
            return EFCourseRepository.GetById(id);
        }

        public int Delete(int Id, int DeleterId)
        {
            Course Course = new Course();
            Course.Id = Id;
            Course.DeleterUserId = DeleterId;
            int i = EFCourseRepository.Delete(Course);
            _logObjectBusiness.AddLogsObject(3, Id, DeleterId);
            return i;
        }

        public CoursePreviewModel getCoursePreviewById(long id, string Certificate)
        {
            CoursePreviewModel coursePreview = new CoursePreviewModel();
            Course course = getCourseById(id);
            if (course == null)
                return coursePreview = null;
            coursePreview.Id = course.Id;
            coursePreview.Code = course.Code;
            coursePreview.Description = course.Description;
            if (!string.IsNullOrEmpty(course.Image))
            {
                if (course.Image.Contains("t24-primary-image-storage"))
                    coursePreview.Image = course.Image;
                else
                    coursePreview.Image = LessonBusiness.geturl(course.Image, Certificate);
            }
            coursePreview.Name = course.Name;
            List<Chapter> chapters = EFChapterRepository.ListQuery(b => b.CourseId == course.Id && b.IsDeleted != true).ToList();
            List<ChapterPreviewModel> chapterPreviewModels = new List<ChapterPreviewModel>();
            foreach (var chapter in chapters)
            {
                ChapterPreviewModel chapterPreview = new ChapterPreviewModel();
                chapterPreview.Id = chapter.Id;
                chapterPreview.Code = chapter.Code;
                chapterPreview.Name = chapter.Name;
                chapterPreview.itemorder = chapter.ItemOrder;
                List<ChapterQuiz> quizList = EFChapterQuizRepository.ListQuery(b => b.ChapterId == chapter.Id && b.IsDeleted != true).ToList();
                List<QuizPreviewModel> lstquizPreviewModel = new List<QuizPreviewModel>();
                if (quizList.Count != 0)
                {
                    foreach (var chquiz in quizList)
                    {
                        QuizPreviewModel quizPreviewModel = new QuizPreviewModel();
                        Quiz quiz = new Quiz();
                        quiz = QuizBusiness.QuizExistanceById(Convert.ToInt32(chquiz.QuizId));
                        if (quiz != null)
                        {
                            quizPreviewModel.id = quiz.Id;
                            quizPreviewModel.name = quiz.Name;
                            quizPreviewModel.code = quiz.Code;
                            quizPreviewModel.numquestions = quiz.NumQuestions;
                            quizPreviewModel.itemorder = chquiz.ItemOrder;
                            quizPreviewModel.type = 2;
                            lstquizPreviewModel.Add(quizPreviewModel);
                        }
                    }
                    lstquizPreviewModel = lstquizPreviewModel.OrderBy(b => b.itemorder).ToList();
                    //chapterPreview.quizs = lstquizPreviewModel;
                }
                else
                {
                    chapterPreview.quizs = null;
                }
                List<Assignment> assignments = _eFAssignmentRepository.ListQuery(b => b.ChapterId == chapter.Id && b.IsDeleted != true).ToList();
                List<AssignmentPreviewModel> assignmentPreviewModel = new List<AssignmentPreviewModel>();
                if (assignments.Count > 0)
                {
                    assignmentPreviewModel = (from x in assignments
                                              select new AssignmentPreviewModel
                                              {
                                                  id = x.Id,
                                                  name = x.Name,
                                                  code = x.Code,
                                                  description = x.Description,
                                                  itemorder = x.ItemOrder,
                                                  type = 3
                                              }).ToList();
                    assignmentPreviewModel = assignmentPreviewModel.OrderBy(b => b.itemorder).ToList();
                }
                else
                {
                    chapterPreview.assignments = null;
                }
                List<Lesson> lessons = EFLessonRepository.ListQuery(b => b.ChapterId == chapter.Id && b.IsDeleted != true).ToList();
                List<LessonPrivewModel> lessonPrivewModels = new List<LessonPrivewModel>();
                if (lessons.Count != 0)
                {
                    lessonPrivewModels = (from x in lessons
                                          select new LessonPrivewModel
                                          {
                                              id = x.Id,
                                              name = x.Name,
                                              code = x.Code,
                                              description = x.Description,
                                              itemorder = x.ItemOrder,
                                              type = 1
                                          }).ToList();
                    lessonPrivewModels = lessonPrivewModels.OrderBy(b => b.itemorder).ToList();
                    List<object> listobj = lessonPrivewModels.Cast<object>().ToList();
                    if (lstquizPreviewModel.Count != 0)
                        listobj.AddRange(lstquizPreviewModel);
                    if (assignmentPreviewModel.Count != 0)
                        listobj.AddRange(assignmentPreviewModel);
                    chapterPreview.lessons = listobj;
                }
                else
                {
                    List<object> listobj = lessonPrivewModels.Cast<object>().ToList();
                    if (lstquizPreviewModel.Count != 0)
                        listobj.AddRange(lstquizPreviewModel);
                    if (assignmentPreviewModel.Count != 0)
                        listobj.AddRange(assignmentPreviewModel);
                    chapterPreview.lessons = listobj;
                }
                chapterPreviewModels.Add(chapterPreview);
            }
            chapterPreviewModels = chapterPreviewModels.OrderBy(b => b.itemorder).ToList();
            coursePreview.chapters = chapterPreviewModels;
            return coursePreview;
        }

        public CoursePreviewModel getCoursePreviewById(long id, long studentid, string Certificate)
        {
            CoursePreviewModel coursePreview = new CoursePreviewModel();
            Course course = getCourseById(id);
            if (course == null)
                return coursePreview = null;
            coursePreview.Id = course.Id;
            coursePreview.Code = course.Code;
            coursePreview.Description = course.Description;
            if (!string.IsNullOrEmpty(course.Image))
            {
                if (course.Image.Contains("t24-primary-image-storage"))
                    coursePreview.Image = course.Image;
                else
                    coursePreview.Image = LessonBusiness.geturl(course.Image, Certificate);
            }
            coursePreview.Name = course.Name;
            List<Chapter> chapters = EFChapterRepository.ListQuery(b => b.CourseId == course.Id && b.IsDeleted != true).ToList();
            List<ChapterPreviewModel> chapterPreviewModels = new List<ChapterPreviewModel>();
            foreach (var chapter in chapters)
            {
                ChapterPreviewModel chapterPreview = new ChapterPreviewModel();
                chapterPreview.Id = chapter.Id;
                chapterPreview.Code = chapter.Code;
                chapterPreview.Name = chapter.Name;
                chapterPreview.itemorder = chapter.ItemOrder;

                List<ChapterQuiz> quizList = EFChapterQuizRepository.ListQuery(b => b.ChapterId == chapter.Id && b.IsDeleted != true).ToList();
                List<QuizPreviewModel> lstquizPreviewModel = new List<QuizPreviewModel>();
                if (quizList.Count != 0)
                {
                    foreach (var chquiz in quizList)
                    {
                        QuizPreviewModel quizPreviewModel = new QuizPreviewModel();
                        Quiz quiz = new Quiz();
                        quiz = QuizBusiness.QuizExistanceById(Convert.ToInt32(chquiz.QuizId));
                        if (quiz != null)
                        {
                            quizPreviewModel.id = quiz.Id;
                            quizPreviewModel.name = quiz.Name;
                            quizPreviewModel.code = quiz.Code;
                            quizPreviewModel.numquestions = quiz.NumQuestions;
                            quizPreviewModel.itemorder = chquiz.ItemOrder;
                            lstquizPreviewModel.Add(quizPreviewModel);
                        }
                    }
                    lstquizPreviewModel = lstquizPreviewModel.OrderBy(b => b.itemorder).ToList();
                }
                else
                {
                    chapterPreview.quizs = null;
                }
                List<Assignment> assignments = _eFAssignmentRepository.ListQuery(b => b.ChapterId == chapter.Id && b.IsDeleted != true).ToList();
                List<AssignmentPreviewModel> assignmentPreviewModel = new List<AssignmentPreviewModel>();
                if (assignments.Count > 0)
                {
                    assignmentPreviewModel = (from x in assignments
                                              select new AssignmentPreviewModel
                                              {
                                                  id = x.Id,
                                                  name = x.Name,
                                                  code = x.Code,
                                                  description = x.Description,
                                                  assignmentfiles = AssignmentBusiness.GetAssignmentFilesByAssignmentId(x.Id, Certificate)
                                              }).ToList();
                    assignmentPreviewModel = assignmentPreviewModel.OrderBy(b => b.itemorder).ToList();
                    chapterPreview.assignments = assignmentPreviewModel;
                }
                else
                {
                    chapterPreview.assignments = null;
                }
                List<Lesson> lessons = EFLessonRepository.ListQuery(b => b.ChapterId == chapter.Id && b.IsDeleted != true).ToList();
                List<LessonPrivewModel> lessonPrivewModels = new List<LessonPrivewModel>();
                if (lessons.Count != 0)
                {
                    try
                    {
                        lessonPrivewModels = (from x in lessons
                                              select new LessonPrivewModel
                                              {
                                                  id = x.Id,
                                                  name = x.Name,
                                                  code = x.Code,
                                                  description = x.Description,
                                                  itemorder = x.ItemOrder,
                                                  lessonfiles = LessonBusiness.GetLessionFilesByLessionId(x.Id, studentid, Certificate),
                                                  assignment = _lessonAssignmentBusiness.GetAssignmentByLesson(x.Id, Certificate)
                                              }).ToList();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                    lessonPrivewModels = lessonPrivewModels.OrderBy(b => b.itemorder).ToList();
                    List<object> listobj = lessonPrivewModels.Cast<object>().ToList();
                    if (lstquizPreviewModel.Count != 0)
                        listobj.AddRange(lstquizPreviewModel);
                    chapterPreview.lessons = listobj;
                }
                else
                {
                    List<object> listobj = lessonPrivewModels.Cast<object>().ToList();
                    if (lstquizPreviewModel.Count != 0)
                        listobj.AddRange(lstquizPreviewModel);
                    chapterPreview.lessons = listobj;
                }
                chapterPreviewModels.Add(chapterPreview);
            }
            chapterPreviewModels = chapterPreviewModels.OrderBy(b => b.itemorder).ToList();
            coursePreview.chapters = chapterPreviewModels;
            return coursePreview;
        }

        public CoursePreviewModel getCoursePreviewByIdTest(long id, string Certificate)
        {
            CoursePreviewModel coursePreview = new CoursePreviewModel();

            DBHelper dbHelper = new DBHelper(getconnectionstring());
            try
            {
                dbHelper.Open();
                DataTable course = dbHelper.ExcecuteQueryDT("select Id,Code,Name,Image,Description from Course where Id=" + id + " AND (IsDeleted!=true OR IsDeleted Is Null)");
                if (course.Rows.Count == 0)
                    return coursePreview = null;
                DataTable chaptor = dbHelper.ExcecuteQueryDT("select Id,Code,Name,Itemorder from Chapter where CourseId=" + id + " AND (IsDeleted!=true OR IsDeleted Is Null)");
                DataTable assignment = dbHelper.ExcecuteQueryDT("select Id,Code,Name,Description, Itemorder,ChapterId from Assignment where ChapterId in (select Id from Chapter where CourseId=" + id + " AND (IsDeleted!=true OR IsDeleted Is Null)) AND (IsDeleted!=true OR IsDeleted Is Null)");
                DataTable lession = dbHelper.ExcecuteQueryDT("select Id,Code,Name,Description,ChapterId,Itemorder from Lesson where ChapterId in (select Id from Chapter where CourseId=" + id + "  AND (IsDeleted!=true OR IsDeleted Is Null)) AND (IsDeleted!=true OR IsDeleted Is Null)");
                DataTable quizes = dbHelper.ExcecuteQueryDT("select c.ChapterId,q.Id AS chqId, q.NumQuestions, q.Itemorder as Itemorder ,q.Name AS chqname,q.Code as chqCode from chapterQuiz c join Quiz q on c.quizid=q.id where ChapterId in (select Id from Chapter where CourseId=" + id + " AND (IsDeleted!=true OR IsDeleted Is Null)) AND (q.IsDeleted!=true OR q.IsDeleted Is Null)AND (c.IsDeleted!=true OR c.IsDeleted Is Null)");
                dbHelper.Close();

                List<ChapterPreviewModel> chaptorPreviewList = new List<ChapterPreviewModel>();
                DataRow cour = course.Rows[0];
                coursePreview.Id = Convert.ToInt64(cour["Id"].ToString());
                coursePreview.Code = cour["Code"].ToString();
                coursePreview.Name = cour["Name"].ToString(); 
                coursePreview.Description = cour["Description"].ToString(); 
                string image = cour["Image"].ToString();
                if (!string.IsNullOrEmpty(image))
                {
                    if (image.Contains("t24-primary-image-storage"))
                        coursePreview.Image = image;
                    else
                        coursePreview.Image = LessonBusiness.geturl(image, Certificate);
                }
                if (chaptor.Rows.Count != 0)
                {
                    foreach (DataRow dr in chaptor.Rows)
                    {
                        ChapterPreviewModel chapterPreview = new ChapterPreviewModel();
                        chapterPreview.Id = Convert.ToInt64(dr["Id"].ToString());
                        chapterPreview.Code = dr["Code"].ToString();
                        chapterPreview.Name = dr["Name"].ToString();
                        chapterPreview.itemorder =Convert.ToInt32( dr["Itemorder"].ToString());

                        List<QuizPreviewModel> lstquizPreviewModel = new List<QuizPreviewModel>();
                        if (quizes.Rows.Count != 0)
                        {
                            foreach (DataRow chquiz in quizes.Rows)
                            {
                                if (Convert.ToInt64(chquiz["ChapterId"].ToString()) == Convert.ToInt64(dr["Id"].ToString()))
                                {
                                    QuizPreviewModel quizPreviewModel = new QuizPreviewModel();

                                    quizPreviewModel.id = Convert.ToInt64(chquiz["chqId"].ToString());
                                    quizPreviewModel.name = chquiz["chqname"].ToString();
                                    quizPreviewModel.code = chquiz["chqCode"].ToString();
                                    quizPreviewModel.itemorder = Convert.ToInt32( chquiz["Itemorder"].ToString());
                                    quizPreviewModel.type = 1;
                                    quizPreviewModel.numquestions = Convert.ToInt32(chquiz["NumQuestions"].ToString());
                                    lstquizPreviewModel.Add(quizPreviewModel);
                                }
                            }
                            chapterPreview.quizs = lstquizPreviewModel.Count > 0 ? lstquizPreviewModel.OrderBy(b=>b.itemorder).ToList() : null;
                        }
                        else
                        {
                            chapterPreview.quizs = null;
                        }
                        List<AssignmentPreviewModel> assignmentPreviewModel = new List<AssignmentPreviewModel>();
                        if (assignment.Rows.Count > 0)
                        {
                            foreach (DataRow assig in assignment.Rows)
                            {
                                if (Convert.ToInt64(assig["ChapterId"].ToString()) == Convert.ToInt64(dr["Id"].ToString()))
                                {
                                    AssignmentPreviewModel assignmentModel = new AssignmentPreviewModel();
                                    assignmentModel.id = Convert.ToInt64(assig["Id"].ToString());
                                    assignmentModel.name = assig["Name"].ToString();
                                    assignmentModel.code = assig["Code"].ToString();
                                    assignmentModel.description = assig["Description"].ToString();
                                    assignmentModel.itemorder= Convert.ToInt32(assig["Itemorder"].ToString());
                                    assignmentModel.type = 3;
                                    assignmentPreviewModel.Add(assignmentModel);
                                }
                            }
                            chapterPreview.assignments = assignmentPreviewModel.Count > 0 ? assignmentPreviewModel.OrderBy(b => b.itemorder).ToList() : null;
                        }
                        else
                        {
                            chapterPreview.assignments = null;
                        }
                        List<LessonPrivewModel> lessonPrivewModels = new List<LessonPrivewModel>();
                        if (lession.Rows.Count != 0)
                        {
                            foreach (DataRow less in lession.Rows)
                            {
                                if (Convert.ToInt64(less["ChapterId"].ToString()) == Convert.ToInt64(dr["Id"].ToString()))
                                {
                                    LessonPrivewModel lessonPrivewModel = new LessonPrivewModel();
                                    lessonPrivewModel.id = Convert.ToInt64(less["Id"].ToString());
                                    lessonPrivewModel.name = less["Name"].ToString();
                                    lessonPrivewModel.code = less["Code"].ToString();
                                    lessonPrivewModel.itemorder =Convert.ToInt32( less["Itemorder"].ToString());
                                    lessonPrivewModel.description = less["Description"].ToString();
                                    lessonPrivewModel.type = 1;
                                    lessonPrivewModels.Add(lessonPrivewModel);
                                }

                            }
                            List<object> listobj = lessonPrivewModels.OrderBy(b => b.itemorder).ToList().Cast<object>().ToList();
                            chapterPreview.lessons = lessonPrivewModels.Count > 0 ? listobj : null;
                        }
                        else
                        {
                            chapterPreview.lessons = null;
                        }
                        chaptorPreviewList.Add(chapterPreview);
                    }
                    coursePreview.chapters = chaptorPreviewList.OrderBy(b => b.itemorder).ToList();
                }
                else
                {
                    coursePreview.chapters = chaptorPreviewList;
                }
            }
            catch (Exception ex)
            {
                dbHelper.Close();
                throw ex;
            }
            finally
            {
                dbHelper.Dispose();
            }
            return coursePreview;
        }


        public CoursePreviewModel getCoursePreviewByIdTest(long id, long studentid, string Certificate)
        {
            CoursePreviewModel coursePreview = new CoursePreviewModel();
            DBHelper dbHelper = new DBHelper(getconnectionstring());
            try
            {
                dbHelper.Open();
                DataTable course = dbHelper.ExcecuteQueryDT("select Id,Code,Name,Image,Description from Course where Id=" + id + " AND (IsDeleted!=true OR IsDeleted Is Null)");
                if (course.Rows.Count == 0)
                    return coursePreview = null;
                DataTable chaptor = dbHelper.ExcecuteQueryDT("select Id,Code,Name,ItemOrder from Chapter where CourseId=" + id + " AND (IsDeleted!=true OR IsDeleted Is Null)");
                DataTable assignment = dbHelper.ExcecuteQueryDT("select Id,Code,Name,Description, ChapterId,ItemOrder from Assignment where ChapterId in (select Id from Chapter where CourseId=" + id + " AND (IsDeleted!=true OR IsDeleted Is Null)) AND (IsDeleted!=true OR IsDeleted Is Null)");
                DataTable lession = dbHelper.ExcecuteQueryDT("select Id,Code,Name,Description,ChapterId,ItemOrder from Lesson where ChapterId in (select Id from Chapter where CourseId=" + id + "  AND (IsDeleted!=true OR IsDeleted Is Null)) AND (IsDeleted!=true OR IsDeleted Is Null)");
                DataTable quizes = dbHelper.ExcecuteQueryDT("select c.ChapterId,q.Id AS chqId, q.NumQuestions, q.Name AS chqname, q.ItemOrder as ItemOrder,q.Code as chqCode from chapterQuiz c join Quiz q on c.quizid=q.id where ChapterId in (select Id from Chapter where CourseId=" + id + " AND (IsDeleted!=true OR IsDeleted Is Null)) AND (q.IsDeleted!=true OR q.IsDeleted Is Null)AND (c.IsDeleted!=true OR c.IsDeleted Is Null)");
                List<string> assignmentList = new List<string>();
                List<string> lessionList = new List<string>();
                List<string> lessionAssignmentList = new List<string>();
                DataTable assignmentFiles = new DataTable();
                DataTable lessionFiles = new DataTable();
                DataTable lessionAssignments = new DataTable();
                DataTable lessonAssignmentFiles = new DataTable();
                if (assignment.Rows.Count != 0)
                {
                    foreach (DataRow assig in assignment.Rows)
                    {
                        assignmentList.Add(assig["Id"].ToString());
                    }

                    if (assignmentList.Count > 0)
                        assignmentFiles = dbHelper.ExcecuteQueryDT("select af.Id as AssignmentFileId,af.AssignmentId, f.Id, f.Name, f.Url, f.FileName, f.FileTypeId, f.FileSize, f.Duration,f.Description, f.Totalpages, ft.Filetype from Files f join AssignmentFile af on f.Id= af.FileId  join FileTypes as ft on f.FileTypeId = ft.Id where af.AssignmentId in (" + string.Join(',', assignmentList.ToArray()
                            ) + ") AND (f.IsDeleted!=true OR f.IsDeleted Is Null) AND (af.IsDeleted!=true OR af.IsDeleted Is Null)");
                }
                if (lession.Rows.Count != 0)
                {
                    foreach (DataRow less in lession.Rows)
                    {
                        lessionList.Add(less["Id"].ToString());
                    }
                    if (lessionList.Count > 0)
                        lessionFiles = dbHelper.ExcecuteQueryDT("select af.Id as LessionFileId, af.LessionId, f.Id, f.Name, f.Url, f.FileName, f.FileTypeId, f.FileSize, f.Duration, f.Totalpages,f.Description,ft.Filetype from Files f join LessonFile af on f.Id= af.FileId  join FileTypes as ft on f.FileTypeId = ft.Id where af.LessionId in (" + string.Join(',', lessionList.ToArray()
                            ) + ") AND (f.IsDeleted!=true OR f.IsDeleted Is Null) AND (af.IsDeleted!=true OR af.IsDeleted Is Null)");
                }
                if (lession.Rows.Count != 0)
                {
                    if (lessionList.Count > 0)
                        lessionAssignments = dbHelper.ExcecuteQueryDT("Select Id,Code,Name,Description,LessonId from LessonAssignments as la where la.LessonId in (" + string.Join(',', lessionList.ToArray()
                            ) + ") AND (la.IsDeleted!=true OR la.IsDeleted Is Null)");
                    foreach (DataRow lesassig in lessionAssignments.Rows)
                    {
                        lessionAssignmentList.Add(lesassig["Id"].ToString());
                    }

                    if (lessionAssignmentList.Count > 0)
                        lessonAssignmentFiles = dbHelper.ExcecuteQueryDT("select af.Id as LessonAssignmentFileId,af.AssignmentId, f.Id, f.Name, f.Url, f.FileName, f.FileTypeId,  f.FileSize,f.Description, f.Duration, f.Totalpages,ft.Filetype from Files f join LessonAssignmentFiles  af on f.Id= af.FileId  join FileTypes as ft on f.FileTypeId = ft.Id where af.AssignmentId in (" + string.Join(',', lessionAssignmentList.ToArray()
                            ) + ") AND (f.IsDeleted!=true OR f.IsDeleted Is Null) AND (af.IsDeleted!=true OR af.IsDeleted Is Null)");

                    dbHelper.Close();
                }
                List<ChapterPreviewModel> chaptorPreviewList = new List<ChapterPreviewModel>();
                DataRow cour = course.Rows[0];
                coursePreview.Id = Convert.ToInt64(cour["Id"].ToString());
                coursePreview.Code = cour["Code"].ToString();
                coursePreview.Name = cour["Name"].ToString();
                coursePreview.Description = cour["Description"].ToString();
                string image = cour["Image"].ToString();
                if (!string.IsNullOrEmpty(image))
                {
                    if (image.Contains("t24-primary-image-storage"))
                        coursePreview.Image = image;
                    else
                        coursePreview.Image = LessonBusiness.geturl(image, Certificate);
                }
                if (chaptor.Rows.Count != 0)
                {
                    foreach (DataRow dr in chaptor.Rows)
                    {
                        ChapterPreviewModel chapterPreview = new ChapterPreviewModel();
                        chapterPreview.Id = Convert.ToInt64(dr["Id"].ToString());
                        chapterPreview.Code = dr["Code"].ToString();
                        chapterPreview.Name = dr["Name"].ToString();
                        chapterPreview.itemorder = Convert.ToInt32(dr["Itemorder"].ToString());

                        List<QuizPreviewModel> lstquizPreviewModel = new List<QuizPreviewModel>();
                        if (quizes.Rows.Count != 0)
                        {
                            foreach (DataRow chquiz in quizes.Rows)
                            {
                                if (Convert.ToInt64(chquiz["ChapterId"].ToString()) == Convert.ToInt64(dr["Id"].ToString()))
                                {
                                    QuizPreviewModel quizPreviewModel = new QuizPreviewModel();

                                    quizPreviewModel.id = Convert.ToInt64(chquiz["chqId"].ToString());
                                    quizPreviewModel.name = chquiz["chqname"].ToString();
                                    quizPreviewModel.code = chquiz["chqCode"].ToString();
                                    quizPreviewModel.itemorder =Convert.ToInt32( chquiz["Itemorder"].ToString());
                                    quizPreviewModel.type = 2;
                                    quizPreviewModel.numquestions = Convert.ToInt32(chquiz["NumQuestions"].ToString());
                                    lstquizPreviewModel.Add(quizPreviewModel);

                                }
                            }
                            chapterPreview.quizs = lstquizPreviewModel.Count > 0 ? lstquizPreviewModel.OrderBy(b=>b.itemorder).ToList() : null;
                        }
                        else
                        {
                            chapterPreview.quizs = null;
                        }
                        List<AssignmentPreviewModel> assignmentPreviewModel = new List<AssignmentPreviewModel>();
                        if (assignment.Rows.Count > 0)
                        {

                            foreach (DataRow assig in assignment.Rows)
                            {
                                if (Convert.ToInt64(assig["ChapterId"].ToString()) == Convert.ToInt64(dr["Id"].ToString()))
                                {
                                    AssignmentPreviewModel assignmentModel = new AssignmentPreviewModel();
                                    assignmentModel.id = Convert.ToInt64(assig["Id"].ToString());
                                    assignmentModel.name = assig["Name"].ToString();
                                    assignmentModel.code = assig["Code"].ToString();
                                    assignmentModel.description = assig["Description"].ToString();
                                    assignmentModel.itemorder = Convert.ToInt32( assig["Itemorder"].ToString());
                                    assignmentModel.type = 3;
                                    List<ResponseAssignmentFileModel> AssignmentFileList = new List<ResponseAssignmentFileModel>();
                                    foreach (DataRow af in assignmentFiles.Rows)
                                    {

                                        if (Convert.ToInt64(af["AssignmentId"].ToString()) == Convert.ToInt64(assig["Id"].ToString()))
                                        {
                                            ResponseAssignmentFileModel responseAssignmentFileModel = new ResponseAssignmentFileModel();
                                            ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                                            responseFilesModel.Id = Convert.ToInt64(af["Id"].ToString());
                                            responseFilesModel.name = af["Name"].ToString();
                                            if (!string.IsNullOrEmpty(af["Url"].ToString()))
                                                responseFilesModel.url = LessonBusiness.geturl(af["Url"].ToString(), Certificate);
                                            //responseFilesModel.url = newFiles.Url;
                                            responseFilesModel.filename = af["FileName"].ToString();
                                            responseFilesModel.filetypeid = Convert.ToInt32(af["FileTypeId"].ToString());
                                            responseFilesModel.description = af["Description"].ToString();
                                            responseFilesModel.filetypename = af["Filetype"].ToString();
                                            responseFilesModel.filesize = Convert.ToInt64(af["FileSize"].ToString());
                                            responseFilesModel.duration = af["Duration"].ToString();
                                            responseFilesModel.totalpages = Convert.ToInt32(af["Totalpages"].ToString());

                                            responseAssignmentFileModel.id = Convert.ToInt64(af["AssignmentFileId"].ToString());
                                            responseAssignmentFileModel.files = responseFilesModel;
                                            AssignmentFileList.Add(responseAssignmentFileModel);

                                        }
                                    }
                                    assignmentModel.assignmentfiles = AssignmentFileList;
                                    assignmentPreviewModel.Add(assignmentModel);
                                }
                            }

                            chapterPreview.assignments = assignmentPreviewModel.Count > 0 ? assignmentPreviewModel.OrderBy(b=>b.itemorder).ToList() : null;
                        }
                        else
                        {
                            chapterPreview.assignments = null;
                        }
                        List<LessonPrivewModel> lessonPrivewModels = new List<LessonPrivewModel>();
                        if (lession.Rows.Count != 0)
                        {
                            foreach (DataRow less in lession.Rows)
                            {
                                if (Convert.ToInt64(less["ChapterId"].ToString()) == Convert.ToInt64(dr["Id"].ToString()))
                                {
                                    LessonPrivewModel lessonPrivewModel = new LessonPrivewModel();
                                    lessonPrivewModel.id = Convert.ToInt64(less["Id"].ToString());
                                    lessonPrivewModel.name = less["Name"].ToString();
                                    lessonPrivewModel.code = less["Code"].ToString();
                                    lessonPrivewModel.description = less["Description"].ToString();
                                    lessonPrivewModel.itemorder =Convert.ToInt32( less["Itemorder"].ToString());
                                    lessonPrivewModel.type = 1;
                                    List<ResponseLessonFileModel> lessonFileList = new List<ResponseLessonFileModel>();
                                    foreach (DataRow lf in lessionFiles.Rows)
                                    {

                                        if (Convert.ToInt64(lf["LessionId"].ToString()) == Convert.ToInt64(less["Id"].ToString()))
                                        {
                                            ResponseLessonFileModel responseAssignmentFileModel = new ResponseLessonFileModel();
                                            ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                                            responseFilesModel.Id = Convert.ToInt64(lf["Id"].ToString());
                                            responseFilesModel.name = lf["Name"].ToString();
                                            if (!string.IsNullOrEmpty(lf["Url"].ToString()))
                                                responseFilesModel.url = LessonBusiness.geturl(lf["Url"].ToString(), Certificate);
                                            //responseFilesModel.url = newFiles.Url;
                                            responseFilesModel.filename = lf["FileName"].ToString();
                                            responseFilesModel.filetypeid = Convert.ToInt32(lf["FileTypeId"].ToString());
                                            responseFilesModel.description = lf["Description"].ToString();
                                            responseFilesModel.filetypename = lf["Filetype"].ToString();
                                            responseFilesModel.filesize = Convert.ToInt64(lf["FileSize"].ToString());
                                            responseFilesModel.duration = lf["Duration"].ToString();
                                            responseFilesModel.totalpages = Convert.ToInt32(lf["Totalpages"].ToString());

                                            responseAssignmentFileModel.id = Convert.ToInt64(lf["LessionFileId"].ToString());
                                            responseAssignmentFileModel.files = responseFilesModel;
                                            lessonFileList.Add(responseAssignmentFileModel);

                                        }
                                    }
                                    lessonPrivewModel.lessonfiles = lessonFileList;// LessonBusiness.GetLessionFilesByLessionId(x.Id, studentid, Certificate),
                                    ResponseLessionAssignmentDTO responseLessionAssignmentDTO = new ResponseLessionAssignmentDTO();
                                    foreach (DataRow la in lessionAssignments.Rows)
                                    {
                                        if (Convert.ToInt64(la["LessonId"].ToString()) == Convert.ToInt64(less["Id"].ToString()))
                                        {
                                            responseLessionAssignmentDTO.id = Convert.ToInt32(la["Id"].ToString());
                                            responseLessionAssignmentDTO.name = la["Name"].ToString();
                                            responseLessionAssignmentDTO.code = la["Code"].ToString();
                                            responseLessionAssignmentDTO.description = la["Description"].ToString();
                                            List<ResponseLessonAssignmentFileDTO> AssignmentFileList = new List<ResponseLessonAssignmentFileDTO>();
                                            foreach (DataRow af in lessonAssignmentFiles.Rows)
                                            {
                                                if (Convert.ToInt64(af["AssignmentId"].ToString()) == Convert.ToInt64(la["Id"].ToString()))
                                                {
                                                    ResponseLessonAssignmentFileDTO responseAssignmentFileModel = new ResponseLessonAssignmentFileDTO();
                                                    ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                                                    responseFilesModel.Id = Convert.ToInt64(af["Id"].ToString());
                                                    responseFilesModel.name = af["Name"].ToString();
                                                    string url = af["Url"].ToString();
                                                    if (!string.IsNullOrEmpty(url))
                                                        responseFilesModel.url = LessonBusiness.geturl(url, Certificate);
                                                    //responseFilesModel.url = newFiles.Url;
                                                    responseFilesModel.filename = af["FileName"].ToString();
                                                    responseFilesModel.filetypeid = Convert.ToInt32(af["FileTypeId"].ToString());
                                                    responseFilesModel.description = af["Description"].ToString();
                                                    responseFilesModel.filetypename = af["Filetype"].ToString();
                                                    responseFilesModel.filesize = Convert.ToInt64(af["FileSize"].ToString());
                                                    responseFilesModel.duration = af["Duration"].ToString();
                                                    responseFilesModel.totalpages = Convert.ToInt32(af["Totalpages"].ToString());

                                                    responseAssignmentFileModel.id = Convert.ToInt64(af["AssignmentId"].ToString());
                                                    responseAssignmentFileModel.files = responseFilesModel;
                                                    AssignmentFileList.Add(responseAssignmentFileModel);

                                                }
                                            }
                                            responseLessionAssignmentDTO.assignmentfiles = AssignmentFileList;
                                        }
                                    }
                                    lessonPrivewModel.assignment = responseLessionAssignmentDTO.id> 0? responseLessionAssignmentDTO:null;
                                    lessonPrivewModels.Add(lessonPrivewModel);
                                }

                            }
                            List<object> listobj = lessonPrivewModels.OrderBy(b=>b.itemorder).ToList().Cast<object>().ToList();
                            chapterPreview.lessons = lessonPrivewModels.Count > 0 ? listobj : null;
                        }
                        else
                        {
                            chapterPreview.lessons = null;
                        }
                        chaptorPreviewList.Add(chapterPreview);
                    }
                    coursePreview.chapters = chaptorPreviewList.OrderBy(b=>b.itemorder).ToList();
                }
                else
                {
                    coursePreview.chapters = chaptorPreviewList;
                }
            }
            catch (Exception ex)
            {
                dbHelper.Close();
                throw ex;
            }
            finally
            {
                dbHelper.Dispose();
            }
            return coursePreview;
        }

        public List<ResponseGradeModel> GetGradeByCourseId(long id)
        {
            List<CourseGrade> courseGrades = EFCourseGradeRepository.ListQuery(b => b.CourseId == id && b.IsDeleted != true).ToList();

            List<ResponseGradeModel> gradedt = new List<ResponseGradeModel>();

            foreach (var cgrd in courseGrades)
            {
                Grade newGrade = GradeBusiness.getGradeById(cgrd.Gradeid);

                if (newGrade != null)
                {
                    ResponseGradeModel responseGradeModel = new ResponseGradeModel
                    {
                        id = newGrade.Id,
                        name = newGrade.Name,
                        description = newGrade.Description
                    };
                    gradedt.Add(responseGradeModel);
                }
            }

            return gradedt;
        }

        public object getCoursePriviewGradeWise(long id, PaginationModel paginationModel, string Certificate, out int total)
        {
            List<CoursePriviewGradeWiseModel> coursePriviewGradeWiseModels = new List<CoursePriviewGradeWiseModel>();

            var list = EFStudentCourseRepository.ListQuery(s => s.UserId == id && s.IsDeleted != true && s.IsExpire != true).ToList();
            foreach (var usercourse in list)
            {
                List<CourseGrade> courseGrades = EFCourseGradeRepository.ListQuery(b => b.CourseId == usercourse.CourseId).ToList();
                Course course = EFCourseRepository.ListQuery(b => b.Id == usercourse.CourseId && b.IsDeleted != true).SingleOrDefault();

                if (course != null)
                {
                    foreach (var cgrade in courseGrades)
                    {
                        List<GradeWiseCoursePriviewModel> coursePriviewModelList = new List<GradeWiseCoursePriviewModel>();
                        Grade grade = EFGradeRepository.GetById(b => b.Id == cgrade.Gradeid);
                        GradeWiseCoursePriviewModel coursePriviewModel = new GradeWiseCoursePriviewModel();
                        coursePriviewModel.id = course.Id;
                        coursePriviewModel.code = course.Code;
                        coursePriviewModel.description = course.Description;
                        if (!string.IsNullOrEmpty(course.Image))
                        {
                            if (course.Image.Contains("t24-primary-image-storage"))
                                coursePriviewModel.image = course.Image;
                            else
                                coursePriviewModel.image = LessonBusiness.geturl(course.Image, Certificate);
                        }
                        coursePriviewModel.name = course.Name;
                        coursePriviewModel.startdate = usercourse.StartDate;
                        coursePriviewModel.enddate = usercourse.EndDate;
                        coursePriviewModelList.Add(coursePriviewModel);

                        CoursePriviewGradeWiseModel coursePriview = new CoursePriviewGradeWiseModel();
                        coursePriview.id = grade.Id;
                        coursePriview.name = grade.Name;
                        coursePriview.courses = coursePriviewModelList;

                        CoursePriviewGradeWiseModel cp = coursePriviewGradeWiseModels.Find(b => b.id == grade.Id);
                        if (cp == null)
                        {
                            coursePriviewGradeWiseModels.Add(coursePriview);
                        }
                        else
                        {
                            cp.courses.Add(coursePriviewModel);
                        }
                    }
                }
            }

            var coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModels;
            total = coursePriviewGradeWiseModelstest.Count();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0 && paginationModel.roleid != 0)
            {
                coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Where(b => b.id == paginationModel.roleid).ToList();
                total = coursePriviewGradeWiseModelstest.Count();
                coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                Take(paginationModel.perpagerecord).
                ToList();

                if (!string.IsNullOrEmpty(paginationModel.search))
                {

                    foreach (var take in coursePriviewGradeWiseModelstest.ToList())
                    {
                        bool t1 = take.id.ToString() == paginationModel.search;
                        bool t2 = take.name.ToLower() == paginationModel.search.ToLower();
                        if (t1 == true || t2 == true)
                            continue;
                        foreach (var coursetake in take.courses.ToList())
                        {
                            bool t3 = coursetake.name.ToLower() == paginationModel.search.ToLower();
                            bool t4 = coursetake.description.ToLower() == paginationModel.search.ToLower();
                            if (t3 == true || t4 == true)
                                continue;
                            else
                                take.courses.Remove(coursetake);
                        }
                        if (take.courses.Count == 0)
                        {
                            coursePriviewGradeWiseModelstest.Remove(take);
                        }
                    }
                }
            }

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                Take(paginationModel.perpagerecord).
                ToList();
                if (!string.IsNullOrEmpty(paginationModel.search))
                {
                    foreach (var take in coursePriviewGradeWiseModelstest.ToList())
                    {
                        bool t1 = take.id.ToString() == paginationModel.search;
                        bool t2 = take.name.ToLower() == paginationModel.search.ToLower();
                        if (t1 == true || t2 == true)
                            continue;
                        foreach (var coursetake in take.courses.ToList())
                        {
                            bool t3 = coursetake.name.ToLower() == paginationModel.search.ToLower();
                            bool t4 = coursetake.description.ToLower() == paginationModel.search.ToLower();
                            if (t3 == true || t4 == true)
                                continue;
                            else
                                take.courses.Remove(coursetake);
                        }
                        if (take.courses.Count == 0)
                        {
                            coursePriviewGradeWiseModelstest.Remove(take);
                        }
                    }
                }
            }


            if (!string.IsNullOrEmpty(paginationModel.search))
            {
                foreach (var take in coursePriviewGradeWiseModelstest.ToList())
                {
                    bool t1 = take.id.ToString() == paginationModel.search;
                    bool t2 = take.name.ToLower() == paginationModel.search.ToLower();
                    if (t1 == true || t2 == true)
                        continue;
                    foreach (var coursetake in take.courses.ToList())
                    {
                        bool t3 = coursetake.name.ToLower() == paginationModel.search.ToLower();
                        bool t4 = coursetake.description.ToLower() == paginationModel.search.ToLower();
                        if (t3 == true || t4 == true)
                            continue;
                        else
                            take.courses.Remove(coursetake);
                    }
                    if (take.courses.Count == 0)
                    {
                        coursePriviewGradeWiseModelstest.Remove(take);
                    }
                }
            }

            if (paginationModel.roleid != 0)
            {
                coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Where(b => b.id == paginationModel.roleid).
                                                   ToList();
                total = coursePriviewGradeWiseModelstest.Count();
            }

            return coursePriviewGradeWiseModelstest;
        }

        public object getCoursePriviewGradeWiseTest(long id, PaginationModel paginationModel, string Certificate, out int total)
        {
            List<CoursePriviewGradeWiseModel> coursePriviewGradeWiseModels = new List<CoursePriviewGradeWiseModel>();
            List<CoursePriviewGradeWiseModel> coursePriviewGradeWiseModelstest = new List<CoursePriviewGradeWiseModel>();
            DBHelper dbHelper = new DBHelper(getconnectionstring());
            try
            {
                dbHelper.Open();
                DataTable usercourselist = dbHelper.ExcecuteQueryDT("SELECT Id,UserId,CourseId,StartDate,EndDate FROM `UserCourse` as uc WHERE   uc.UserId = " + id + "  AND (uc.IsDeleted!=true OR uc.IsDeleted Is Null) AND (uc.IsExpire!=true OR uc.IsExpire Is Null)");
                List<string> CourseIdList = new List<string>();
                DataTable courselist = new DataTable();
                DataTable courseGradeslist = new DataTable();
                if (usercourselist.Rows.Count != 0)
                {
                    foreach (DataRow usercourse in usercourselist.Rows)
                    {
                        CourseIdList.Add(usercourse["CourseId"].ToString());
                    }
                    if (CourseIdList.Count > 0)
                    {
                        courseGradeslist = dbHelper.ExcecuteQueryDT("select cg.Id as cgId,cg.CourseId,cg.Gradeid,g.Id,g.Name,g.SchoolId,g.Description from CourseGrade as cg join Grade as g on cg.Gradeid = g.Id  where cg.CourseId in (" + string.Join(',', CourseIdList.ToArray()) + ") AND (cg.IsDeleted!=true OR cg.IsDeleted Is Null)");
                        courselist = dbHelper.ExcecuteQueryDT("select Id,Code,Name,Image,Description from Course where Id in (" + string.Join(',', CourseIdList.ToArray()
                             ) + ")AND (IsDeleted!=true OR IsDeleted Is Null)");
                    }
                }
                dbHelper.Close();
                if (usercourselist.Rows.Count == 0)
                {
                    total = coursePriviewGradeWiseModelstest.Count();
                    return coursePriviewGradeWiseModelstest = null;
                }
                foreach (DataRow usercourse in usercourselist.Rows)
                {
                    if (courselist.Rows.Count != 0)
                    {
                        foreach (DataRow course in courselist.Rows)
                        {
                            if (Convert.ToInt64(course["Id"].ToString()) == Convert.ToInt64(usercourse["CourseId"].ToString()))
                            {
                                foreach (DataRow cgrade in courseGradeslist.Rows)
                                {
                                    if (Convert.ToInt64(cgrade["CourseId"].ToString()) == Convert.ToInt64(usercourse["CourseId"].ToString()))
                                    {
                                        List<GradeWiseCoursePriviewModel> coursePriviewModelList = new List<GradeWiseCoursePriviewModel>();
                                        GradeWiseCoursePriviewModel coursePriviewModel = new GradeWiseCoursePriviewModel();
                                        coursePriviewModel.id = Convert.ToInt64(course["Id"].ToString());
                                        coursePriviewModel.code = course["Code"].ToString();
                                        coursePriviewModel.description = course["Description"].ToString();
                                        if (!string.IsNullOrEmpty(course["Image"].ToString()))
                                        {
                                            if (course["Image"].ToString().Contains("t24-primary-image-storage"))
                                                coursePriviewModel.image = course["Image"].ToString();
                                            else
                                                coursePriviewModel.image = LessonBusiness.geturl(course["Image"].ToString(), Certificate);
                                        }
                                        coursePriviewModel.name = course["Name"].ToString();
                                        coursePriviewModel.startdate = usercourse["StartDate"].ToString();
                                        coursePriviewModel.enddate = usercourse["EndDate"].ToString();
                                        coursePriviewModelList.Add(coursePriviewModel);
                                        CoursePriviewGradeWiseModel coursePriview = new CoursePriviewGradeWiseModel();
                                        coursePriview.id = Convert.ToInt64(cgrade["Id"].ToString());
                                        coursePriview.name = cgrade["Name"].ToString();
                                        coursePriview.courses = coursePriviewModelList;
                                        CoursePriviewGradeWiseModel cp = coursePriviewGradeWiseModels.Find(b => b.id == Convert.ToInt64(cgrade["Id"].ToString()));
                                        if (cp == null)
                                        {
                                            coursePriviewGradeWiseModels.Add(coursePriview);
                                        }
                                        else
                                        {
                                            cp.courses.Add(coursePriviewModel);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModels;
                total = coursePriviewGradeWiseModelstest.Count();
                if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0 && paginationModel.roleid != 0)
                {
                    coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Where(b => b.id == paginationModel.roleid).ToList();
                    total = coursePriviewGradeWiseModelstest.Count();
                    coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                    Take(paginationModel.perpagerecord).
                    ToList();
                    if (!string.IsNullOrEmpty(paginationModel.search))
                    {
                        foreach (var take in coursePriviewGradeWiseModelstest.ToList())
                        {
                            bool t1 = take.id.ToString() == paginationModel.search;
                            bool t2 = take.name.ToLower() == paginationModel.search.ToLower();
                            if (t1 == true || t2 == true)
                                continue;
                            foreach (var coursetake in take.courses.ToList())
                            {
                                bool t3 = coursetake.name.ToLower() == paginationModel.search.ToLower();
                                bool t4 = coursetake.description.ToLower() == paginationModel.search.ToLower();
                                if (t3 == true || t4 == true)
                                    continue;
                                else
                                    take.courses.Remove(coursetake);
                            }
                            if (take.courses.Count == 0)
                            {
                                coursePriviewGradeWiseModelstest.Remove(take);
                            }
                        }
                    }
                }
                if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
                {
                    coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                    Take(paginationModel.perpagerecord).
                    ToList();
                    if (!string.IsNullOrEmpty(paginationModel.search))
                    {
                        foreach (var take in coursePriviewGradeWiseModelstest.ToList())
                        {
                            bool t1 = take.id.ToString() == paginationModel.search;
                            bool t2 = take.name.ToLower() == paginationModel.search.ToLower();
                            if (t1 == true || t2 == true)
                                continue;
                            foreach (var coursetake in take.courses.ToList())
                            {
                                bool t3 = coursetake.name.ToLower() == paginationModel.search.ToLower();
                                bool t4 = coursetake.description.ToLower() == paginationModel.search.ToLower();
                                if (t3 == true || t4 == true)
                                    continue;
                                else
                                    take.courses.Remove(coursetake);
                            }
                            if (take.courses.Count == 0)
                            {
                                coursePriviewGradeWiseModelstest.Remove(take);
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(paginationModel.search))
                {
                    foreach (var take in coursePriviewGradeWiseModelstest.ToList())
                    {
                        bool t1 = take.id.ToString() == paginationModel.search;
                        bool t2 = take.name.ToLower() == paginationModel.search.ToLower();
                        if (t1 == true || t2 == true)
                            continue;
                        foreach (var coursetake in take.courses.ToList())
                        {
                            bool t3 = coursetake.name.ToLower() == paginationModel.search.ToLower();
                            bool t4 = coursetake.description.ToLower() == paginationModel.search.ToLower();
                            if (t3 == true || t4 == true)
                                continue;
                            else
                                take.courses.Remove(coursetake);
                        }
                        if (take.courses.Count == 0)
                        {
                            coursePriviewGradeWiseModelstest.Remove(take);
                        }
                    }
                }
                if (paginationModel.roleid != 0)
                {
                    coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Where(b => b.id == paginationModel.roleid).
                                                       ToList();
                    total = coursePriviewGradeWiseModelstest.Count();
                }
            }
            catch (Exception ex)
            {
                dbHelper.Close();
                throw ex;
            }
            finally
            {
                dbHelper.Dispose();
            }
            return coursePriviewGradeWiseModelstest;
        }

        public object GetAllDetails(long id, string search, string[] filter, string[] bygrade, string Certificate)
        {
            GetAllDetailsModel getAllDetailsModel = new GetAllDetailsModel();
            List<CourseDetailModel> courseList = new List<CourseDetailModel>();
            List<LessonDetailModel> responseLessonModel = new List<LessonDetailModel>();

            var allcourseList = EFStudentCourseRepository.ListQuery(s => s.UserId == id).ToList();

            foreach (var usercourse in allcourseList)
            {
                Course newCourse = EFCourseRepository.ListQuery(b => b.Id == usercourse.CourseId && b.IsDeleted != true).SingleOrDefault();
                List<ResponseGradeModel> gradeModels = GetGradeByCourseId(usercourse.CourseId);
                if (newCourse != null)
                {
                    string imageurl = "";
                    if (!string.IsNullOrEmpty(newCourse.Image))
                    {
                        if (newCourse.Image.Contains("t24-primary-image-storage"))
                            imageurl = newCourse.Image;
                        else
                            imageurl = LessonBusiness.geturl(newCourse.Image, Certificate);
                    }

                    CourseDetailModel responseCourseModel = new CourseDetailModel
                    {
                        Name = newCourse.Name,
                        Id = int.Parse(newCourse.Id.ToString()),
                        Image = imageurl,
                        StartDate = usercourse.StartDate,
                        EndDate = usercourse.EndDate,
                        GradeDetails = gradeModels,
                        Description = newCourse.Description
                    };
                    courseList.Add(responseCourseModel);

                    List<Lesson> newLesson = LessonBusiness.GetLessonByCourseId(int.Parse(usercourse.CourseId.ToString()));
                    foreach (var lesson in newLesson)
                    {
                        ResponseFilesModel1 lessonfile = LessonBusiness.GetLessionFilesByLessionId1(lesson.Id);

                        var quiz = EFChapterQuizRepository.ListQuery(b => b.ChapterId == lesson.ChapterId.Value && b.IsDeleted != true).ToList();

                        try
                        {
                            LessonDetailModel rlModel = new LessonDetailModel
                            {
                                id = int.Parse(lesson.Id.ToString()),
                                name = lesson.Name,
                                chapterid = lesson.ChapterId.Value,
                                courseid = usercourse.CourseId,
                                coursename = newCourse.Name,
                                lessonfileid = lessonfile.Id,
                                lessonfilename = lessonfile.name,
                                filetypeid = lessonfile.filetypeid,
                                filetypename = lessonfile.filetypename,
                                progressval = ProgressBusiness.GetStudentLessonProgressByLession(lesson.Id).lessonprogress,
                                lessonquizid = 0
                            };
                            responseLessonModel.Add(rlModel);

                            if (quiz.Count != 0)
                            {
                                rlModel = new LessonDetailModel
                                {
                                    id = int.Parse(lesson.Id.ToString()),
                                    name = lesson.Name,
                                    chapterid = lesson.ChapterId.Value,
                                    courseid = usercourse.CourseId,
                                    coursename = newCourse.Name,
                                    lessonfileid = 0,
                                    lessonfilename = "",
                                    filetypeid = 0,
                                    filetypename = "",
                                    progressval = ProgressBusiness.GetStudentLessonProgressByLession(lesson.Id).lessonprogress,
                                    lessonquizid = quiz[0].QuizId
                                };
                                responseLessonModel.Add(rlModel);
                            }
                        }
                        catch (Exception ex)
                        {

                            throw;
                        }

                    }
                }
            }

            List<Assignment> assignmentList = AssignmentBusiness.GetAssignmentStudentById(int.Parse(id.ToString()));
            List<AssignmentDetails> responseAssignmentModels = new List<AssignmentDetails>();

            foreach (var assignemnt in assignmentList)
            {
                AssignmentDetails responseAssignmentModel = new AssignmentDetails();
                responseAssignmentModel.id = int.Parse(assignemnt.Id.ToString());
                responseAssignmentModel.name = assignemnt.Name;

                Assignment assignment = AssignmentBusiness.GetAssignmentById(int.Parse(assignemnt.Id.ToString()));
                Chapter chapter = chapterBusiness.getChapterById(int.Parse(assignment.ChapterId.ToString()));

                Course newCourse = EFCourseRepository.ListQuery(b => b.Id == chapter.CourseId).SingleOrDefault();
                List<ResponseGradeModel> gradeModels = GetGradeByCourseId(chapter.CourseId);
                CourseDetailModel responseCourseModel = new CourseDetailModel
                {
                    Name = newCourse.Name,
                    Id = int.Parse(newCourse.Id.ToString()),
                    Image = newCourse.Image,
                    GradeDetails = gradeModels
                };

                responseAssignmentModel.coursewithgrade = responseCourseModel;
                responseAssignmentModels.Add(responseAssignmentModel);
            }

            if (search != null)
            {
                List<CourseDetailModel> tempCourseDetails = new List<CourseDetailModel>();
                List<CourseDetailModel> courseDetailModels1 = new List<CourseDetailModel>();
                List<CourseDetailModel> courseDetailModels2 = new List<CourseDetailModel>();
                tempCourseDetails.AddRange(courseList);
                courseList.Clear();
                try
                {
                    courseDetailModels1 = tempCourseDetails.Where(b => b.Name.ToLower().Contains(search.ToLower()) ||
                    !string.IsNullOrEmpty(b.Description) && b.Description.ToLower().Contains(search.ToLower())).ToList();
                    if (courseDetailModels1.Count != 0)
                    {
                        courseList.AddRange(courseDetailModels1);
                    }
                    responseAssignmentModels = responseAssignmentModels.Where(b => b.name.Any(k => b.name.ToLower().Contains(search.ToLower()))).ToList();
                    responseLessonModel = responseLessonModel.Where(b => b.name.Any(k => b.name.ToLower().Contains(search.ToLower()))).ToList();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

            if (filter != null)
            {
                List<CourseDetailModel> tempCourseList = new List<CourseDetailModel>();
                if (filter.Contains("1"))
                {
                    foreach (var nc in courseList)
                    {
                        if (!string.IsNullOrEmpty(nc.StartDate))
                        {
                            DateTime parsedDate = DateTime.ParseExact(nc.StartDate, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                            if (parsedDate >= DateTime.Now.Date.AddDays(-5))
                            {
                                tempCourseList.Add(nc);
                            }
                        }
                    }
                }
                if (filter.Contains("2"))
                {
                    foreach (var nc in courseList)
                    {
                        if (!string.IsNullOrEmpty(nc.StartDate))
                        {
                            DateTime parsedDateStart = DateTime.ParseExact(nc.StartDate, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                            DateTime parsedDateEnd = DateTime.ParseExact(nc.EndDate, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                            if (parsedDateStart >= DateTime.Now && parsedDateEnd <= DateTime.Now)
                            {
                                tempCourseList.Add(nc);
                            }
                        }
                    }
                }
                if (filter.Contains("3"))
                {
                    foreach (var nc in courseList)
                    {
                        if (!string.IsNullOrEmpty(nc.EndDate))
                        {
                            DateTime parsedDateEnd = DateTime.ParseExact(nc.EndDate, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                            if (parsedDateEnd <= DateTime.Now)
                            {
                                tempCourseList.Add(nc);
                            }
                        }
                    }
                }
                if (filter.Contains("4"))
                {

                }
                tempCourseList = tempCourseList.Distinct().ToList();
                courseList = tempCourseList;
                responseLessonModel = null;
                responseAssignmentModels = null;
            }
            if (bygrade != null)
            {
                List<CourseDetailModel> tempCourseList = new List<CourseDetailModel>();
                foreach (var nc in courseList)
                {
                    List<ResponseGradeModel> gradeModels = GetGradeByCourseId(nc.Id);
                    foreach (var gm in gradeModels)
                    {
                        for (int i = 0; i < bygrade.Length; i++)
                        {
                            if (gm.id == int.Parse(bygrade[i]))
                            {
                                tempCourseList.Add(nc);
                            }
                        }
                    }
                }
                tempCourseList = tempCourseList.Distinct().ToList();
                courseList = tempCourseList;
                responseLessonModel = null;
                responseAssignmentModels = null;
            }

            getAllDetailsModel.courses = courseList;
            getAllDetailsModel.lessons = responseLessonModel;
            getAllDetailsModel.assignment = responseAssignmentModels;

            return getAllDetailsModel;
        }


        public List<Course> getTrailCourses()
        {
            return EFCourseRepository.ListQuery(b => b.istrial == true).ToList();
        }

        public CourseDto GetCourseById(long Id, string certificate)
        {
            Course course = getCourseById(Id);
            CourseDto courseDto = new CourseDto();
            if (course != null)
            {
                courseDto.Id = course.Id;
                courseDto.Name = course.Name;
                courseDto.Code = course.Code;
                courseDto.Description = course.Description;
                if (!string.IsNullOrEmpty(course.Image))
                    courseDto.Image = LessonBusiness.geturl(course.Image, certificate);
            }
            return courseDto;
        }

        public int SendNotificationOnCourseUpdate(Course obj, string Certificate, long userId)
        {
            List<long> userList = usersBusiness.GetUserByCourseId(obj.Id);
            UserDetails user = usersBusiness.GetNotificationUserById(userId, Certificate);
            CourseDto course = GetCourseById(obj.Id, Certificate);
            CreateNotificationViewModel createNotificationViewModel = new CreateNotificationViewModel();
            if (_language.lan == "fa")
            {
                //createNotificationViewModel.Tag = "را به روز کرد" + " " + course.Name + " " + "کورس" + " " + user.fullname;
                createNotificationViewModel.Tag = user.fullname + " " + "کورس" + " " + course.Name + " " + "را به روز کرد";
            }
            else if (_language.lan == "ps")
            {
                //createNotificationViewModel.Tag = "نوی کړ" + " " + course.Name + " " + user.fullname;
                createNotificationViewModel.Tag = user.fullname + " " + course.Name + " " + "نوی کړ";
            }
            else
            {
                createNotificationViewModel.Tag = user.fullname + " " + "was updated course" + " " + course.Name;
            }
            createNotificationViewModel.IsRead = false;
            createNotificationViewModel.CourseId = obj.Id;
            createNotificationViewModel.CreatorUserId = userId;
            createNotificationViewModel.Type = 1;
            createNotificationViewModel.User = user;
            createNotificationViewModel.Course = course;
            var send = userNotificationBusiness.CreateUserNotification(userList, createNotificationViewModel);
            return 0;
        }

        public object getTrialCoursePriviewGradeWise(long id, PaginationModel paginationModel, string Certificate, out int total)
        {
            List<CoursePriviewGradeWiseModel> coursePriviewGradeWiseModels = new List<CoursePriviewGradeWiseModel>();

            var list = EFCourseRepository.ListQuery(b => b.istrial == true).ToList();
            foreach (var usercourse in list)
            {
                List<CourseGrade> courseGrades = EFCourseGradeRepository.ListQuery(b => b.CourseId == usercourse.Id).ToList();
                Course course = EFCourseRepository.ListQuery(b => b.Id == usercourse.Id && b.IsDeleted != true).SingleOrDefault();

                if (course != null)
                {
                    foreach (var cgrade in courseGrades)
                    {
                        List<GradeWiseCoursePriviewModel> coursePriviewModelList = new List<GradeWiseCoursePriviewModel>();
                        Grade grade = EFGradeRepository.GetById(b => b.Id == cgrade.Gradeid);
                        GradeWiseCoursePriviewModel coursePriviewModel = new GradeWiseCoursePriviewModel();
                        coursePriviewModel.id = course.Id;
                        coursePriviewModel.code = course.Code;
                        coursePriviewModel.description = course.Description;
                        if (!string.IsNullOrEmpty(course.Image))
                            coursePriviewModel.image = LessonBusiness.geturl(course.Image, Certificate);
                        coursePriviewModel.name = course.Name;
                        coursePriviewModelList.Add(coursePriviewModel);

                        CoursePriviewGradeWiseModel coursePriview = new CoursePriviewGradeWiseModel();
                        coursePriview.id = grade.Id;
                        coursePriview.name = grade.Name;
                        coursePriview.courses = coursePriviewModelList;

                        CoursePriviewGradeWiseModel cp = coursePriviewGradeWiseModels.Find(b => b.id == grade.Id);
                        if (cp == null)
                        {
                            coursePriviewGradeWiseModels.Add(coursePriview);
                        }
                        else
                        {
                            cp.courses.Add(coursePriviewModel);
                        }
                    }
                }
            }

            var coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModels;
            total = coursePriviewGradeWiseModelstest.Count();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0 && paginationModel.roleid != 0)
            {
                coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Where(b => b.id == paginationModel.roleid).ToList();
                total = coursePriviewGradeWiseModelstest.Count();
                coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                Take(paginationModel.perpagerecord).
                ToList();

                if (!string.IsNullOrEmpty(paginationModel.search))
                {

                    foreach (var take in coursePriviewGradeWiseModelstest.ToList())
                    {
                        bool t1 = take.id.ToString() == paginationModel.search;
                        bool t2 = take.name.ToLower() == paginationModel.search.ToLower();
                        if (t1 == true || t2 == true)
                            continue;
                        foreach (var coursetake in take.courses.ToList())
                        {
                            bool t3 = coursetake.name.ToLower() == paginationModel.search.ToLower();
                            bool t4 = coursetake.description.ToLower() == paginationModel.search.ToLower();
                            if (t3 == true || t4 == true)
                                continue;
                            else
                                take.courses.Remove(coursetake);
                        }
                        if (take.courses.Count == 0)
                        {
                            coursePriviewGradeWiseModelstest.Remove(take);
                        }
                    }
                }
            }

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                Take(paginationModel.perpagerecord).
                ToList();
                if (!string.IsNullOrEmpty(paginationModel.search))
                {
                    foreach (var take in coursePriviewGradeWiseModelstest.ToList())
                    {
                        bool t1 = take.id.ToString() == paginationModel.search;
                        bool t2 = take.name.ToLower() == paginationModel.search.ToLower();
                        if (t1 == true || t2 == true)
                            continue;
                        foreach (var coursetake in take.courses.ToList())
                        {
                            bool t3 = coursetake.name.ToLower() == paginationModel.search.ToLower();
                            bool t4 = coursetake.description.ToLower() == paginationModel.search.ToLower();
                            if (t3 == true || t4 == true)
                                continue;
                            else
                                take.courses.Remove(coursetake);
                        }
                        if (take.courses.Count == 0)
                        {
                            coursePriviewGradeWiseModelstest.Remove(take);
                        }
                    }
                }
            }


            if (!string.IsNullOrEmpty(paginationModel.search))
            {
                foreach (var take in coursePriviewGradeWiseModelstest.ToList())
                {
                    bool t1 = take.id.ToString() == paginationModel.search;
                    bool t2 = take.name.ToLower() == paginationModel.search.ToLower();
                    if (t1 == true || t2 == true)
                        continue;
                    foreach (var coursetake in take.courses.ToList())
                    {
                        bool t3 = coursetake.name.ToLower() == paginationModel.search.ToLower();
                        bool t4 = coursetake.description.ToLower() == paginationModel.search.ToLower();
                        if (t3 == true || t4 == true)
                            continue;
                        else
                            take.courses.Remove(coursetake);
                    }
                    if (take.courses.Count == 0)
                    {
                        coursePriviewGradeWiseModelstest.Remove(take);
                    }
                }
            }

            if (paginationModel.roleid != 0)
            {
                coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Where(b => b.id == paginationModel.roleid).
                                                   ToList();
                total = coursePriviewGradeWiseModelstest.Count();
            }

            return coursePriviewGradeWiseModelstest;
        }

        public object getTrialCoursePriviewGradeWiseTest(long id, PaginationModel paginationModel, string Certificate, out int total)
        {
            List<CoursePriviewGradeWiseModel> coursePriviewGradeWiseModels = new List<CoursePriviewGradeWiseModel>();
            List<CoursePriviewGradeWiseModel> coursePriviewGradeWiseModelstest = new List<CoursePriviewGradeWiseModel>();
            DBHelper dbHelper = new DBHelper(getconnectionstring());
            try
            {
                dbHelper.Open();
                DataTable usercourselist = dbHelper.ExcecuteQueryDT("SELECT Id,Name,Code,Description ,Image,PassMark FROM `Course` as uc WHERE  (uc.IsDeleted!=true OR uc.IsDeleted Is Null) AND (uc.istrial = true)");
                List<string> CourseIdList = new List<string>();
                DataTable courselist = new DataTable();
                DataTable courseGradeslist = new DataTable();
                if (usercourselist.Rows.Count != 0)
                {
                    foreach (DataRow usercourse in usercourselist.Rows)
                    {
                        CourseIdList.Add(usercourse["Id"].ToString());
                    }
                    if (CourseIdList.Count > 0)
                    {
                        courselist = dbHelper.ExcecuteQueryDT("select Id,Code,Name,Image,Description from Course where Id in (" + string.Join(',', CourseIdList.ToArray()
                             ) + ")AND (IsDeleted!=true OR IsDeleted Is Null)");
                        courseGradeslist = dbHelper.ExcecuteQueryDT("select cg.Id as cgId,cg.CourseId,cg.Gradeid,g.Id,g.Name,g.SchoolId,g.Description from CourseGrade as cg join Grade as g on cg.Gradeid = g.Id  where CourseId in (" + string.Join(',', CourseIdList.ToArray()) + ") AND (cg.IsDeleted!=true OR cg.IsDeleted Is Null)AND (g.IsDeleted!=true OR g.IsDeleted Is Null)");
                    }
                }
                dbHelper.Close();
                if (usercourselist.Rows.Count == 0)
                {
                    total = coursePriviewGradeWiseModelstest.Count();
                    return coursePriviewGradeWiseModelstest = null;
                }
                foreach (DataRow usercourse in usercourselist.Rows)
                {
                    if (courselist.Rows.Count != 0)
                    {
                        foreach (DataRow course in courselist.Rows)
                        {
                            if (Convert.ToInt64(course["Id"].ToString()) == Convert.ToInt64(usercourse["Id"].ToString()))
                            {
                                foreach (DataRow cgrade in courseGradeslist.Rows)
                                {
                                    if (Convert.ToInt64(cgrade["CourseId"].ToString()) == Convert.ToInt64(usercourse["Id"].ToString()))
                                    {
                                        List<GradeWiseCoursePriviewModel> coursePriviewModelList = new List<GradeWiseCoursePriviewModel>();
                                        GradeWiseCoursePriviewModel coursePriviewModel = new GradeWiseCoursePriviewModel();
                                        coursePriviewModel.id = Convert.ToInt64(course["Id"].ToString());
                                        coursePriviewModel.code = course["Code"].ToString();
                                        coursePriviewModel.description = course["Description"].ToString();
                                        if (!string.IsNullOrEmpty(course["Image"].ToString()))
                                        {
                                            if (course["Image"].ToString().Contains("t24-primary-image-storage"))
                                                coursePriviewModel.image = course["Image"].ToString();
                                            else
                                                coursePriviewModel.image = LessonBusiness.geturl(course["Image"].ToString(), Certificate);
                                        }
                                        coursePriviewModel.name = course["Name"].ToString();
                                        coursePriviewModelList.Add(coursePriviewModel);
                                        CoursePriviewGradeWiseModel coursePriview = new CoursePriviewGradeWiseModel();
                                        coursePriview.id = Convert.ToInt64(cgrade["Id"].ToString());
                                        coursePriview.name = cgrade["Name"].ToString();
                                        coursePriview.courses = coursePriviewModelList;
                                        CoursePriviewGradeWiseModel cp = coursePriviewGradeWiseModels.Find(b => b.id == Convert.ToInt64(cgrade["Id"].ToString()));
                                        if (cp == null)
                                        {
                                            coursePriviewGradeWiseModels.Add(coursePriview);
                                        }
                                        else
                                        {
                                            cp.courses.Add(coursePriviewModel);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModels;
                total = coursePriviewGradeWiseModelstest.Count();
                if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0 && paginationModel.roleid != 0)
                {
                    coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Where(b => b.id == paginationModel.roleid).ToList();
                    total = coursePriviewGradeWiseModelstest.Count();
                    coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                    Take(paginationModel.perpagerecord).
                    ToList();
                    if (!string.IsNullOrEmpty(paginationModel.search))
                    {
                        foreach (var take in coursePriviewGradeWiseModelstest.ToList())
                        {
                            bool t1 = take.id.ToString() == paginationModel.search;
                            bool t2 = take.name.ToLower() == paginationModel.search.ToLower();
                            if (t1 == true || t2 == true)
                                continue;
                            foreach (var coursetake in take.courses.ToList())
                            {
                                bool t3 = coursetake.name.ToLower() == paginationModel.search.ToLower();
                                bool t4 = coursetake.description.ToLower() == paginationModel.search.ToLower();
                                if (t3 == true || t4 == true)
                                    continue;
                                else
                                    take.courses.Remove(coursetake);
                            }
                            if (take.courses.Count == 0)
                            {
                                coursePriviewGradeWiseModelstest.Remove(take);
                            }
                        }
                    }
                }
                if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
                {
                    coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                    Take(paginationModel.perpagerecord).
                    ToList();
                    if (!string.IsNullOrEmpty(paginationModel.search))
                    {
                        foreach (var take in coursePriviewGradeWiseModelstest.ToList())
                        {
                            bool t1 = take.id.ToString() == paginationModel.search;
                            bool t2 = take.name.ToLower() == paginationModel.search.ToLower();
                            if (t1 == true || t2 == true)
                                continue;
                            foreach (var coursetake in take.courses.ToList())
                            {
                                bool t3 = coursetake.name.ToLower() == paginationModel.search.ToLower();
                                bool t4 = coursetake.description.ToLower() == paginationModel.search.ToLower();
                                if (t3 == true || t4 == true)
                                    continue;
                                else
                                    take.courses.Remove(coursetake);
                            }
                            if (take.courses.Count == 0)
                            {
                                coursePriviewGradeWiseModelstest.Remove(take);
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(paginationModel.search))
                {
                    foreach (var take in coursePriviewGradeWiseModelstest.ToList())
                    {
                        bool t1 = take.id.ToString() == paginationModel.search;
                        bool t2 = take.name.ToLower() == paginationModel.search.ToLower();
                        if (t1 == true || t2 == true)
                            continue;
                        foreach (var coursetake in take.courses.ToList())
                        {
                            bool t3 = coursetake.name.ToLower() == paginationModel.search.ToLower();
                            bool t4 = coursetake.description.ToLower() == paginationModel.search.ToLower();
                            if (t3 == true || t4 == true)
                                continue;
                            else
                                take.courses.Remove(coursetake);
                        }
                        if (take.courses.Count == 0)
                        {
                            coursePriviewGradeWiseModelstest.Remove(take);
                        }
                    }
                }
                if (paginationModel.roleid != 0)
                {
                    coursePriviewGradeWiseModelstest = coursePriviewGradeWiseModelstest.Where(b => b.id == paginationModel.roleid).
                                                       ToList();
                    total = coursePriviewGradeWiseModelstest.Count();
                }
            }
            catch (Exception ex)
            {
                dbHelper.Close();
                throw ex;
            }
            finally
            {
                dbHelper.Dispose();
            }
            return coursePriviewGradeWiseModelstest;
        }

        public string getconnectionstring()
        {
            return Environment.GetEnvironmentVariable("ASPNET_DB_CONNECTIONSTRING");
            //return "server=35.204.80.96;user id=root;password=B@llastW!ll5565;database=daristaging;SslMode=none;Convert Zero Datetime=true;CharSet=utf8;";
        }
    }
}
