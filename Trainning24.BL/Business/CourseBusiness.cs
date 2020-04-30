using Google.Apis.Auth.OAuth2;
using Google.Apis.Storage.v1;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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
            var coursePreView = (from c in _training24Context.Course
                                 where c.Id == id && c.IsDeleted != true
                                 let chapterPreviewModels = (from cpt in _training24Context.Chapter
                                                             where cpt.CourseId == c.Id && cpt.IsDeleted != true

                                                             let chapterQuizPrivewModels = (from cq in _training24Context.chapterQuiz
                                                                                            join q in _training24Context.Quiz on cq.QuizId equals q.Id
                                                                                            where cq.ChapterId == cpt.Id && cq.IsDeleted != true && q.IsDeleted != true
                                                                                            select new QuizPreviewModel()
                                                                                            {
                                                                                                id = q.Id,
                                                                                                name = q.Name,
                                                                                                code = q.Code,
                                                                                                numquestions = q.NumQuestions,
                                                                                                itemorder = q.ItemOrder,
                                                                                                type = 2
                                                                                            }).ToList()
                                                             let assignmentsPrivewModels = (from a in _training24Context.Assignment
                                                                                            where a.ChapterId == cpt.Id && a.IsDeleted != true
                                                                                            select new AssignmentPreviewModel
                                                                                            {
                                                                                                id = a.Id,
                                                                                                name = a.Name,
                                                                                                code = a.Code,
                                                                                                description = a.Description,
                                                                                                itemorder = a.ItemOrder,
                                                                                                type = 3,
                                                                                            }).ToList()
                                                             let lessonPrivewModels = (from x in _training24Context.Lesson
                                                                                       where x.ChapterId == cpt.Id && x.IsDeleted != true
                                                                                       select new LessonPrivewModel
                                                                                       {
                                                                                           id = x.Id,
                                                                                           name = x.Name,
                                                                                           code = x.Code,
                                                                                           description = x.Description,
                                                                                           itemorder = x.ItemOrder,
                                                                                           type = 1
                                                                                       }).ToList()
                                                             select new ChapterPreviewModel()
                                                             {
                                                                 Id = cpt.Id,
                                                                 Code = cpt.Code,
                                                                 Name = cpt.Name,
                                                                 itemorder = cpt.ItemOrder,
                                                                 quizs = chapterQuizPrivewModels,
                                                                 assignments = assignmentsPrivewModels,
                                                                 lessons = lessonPrivewModels.Cast<object>().ToList(),
                                                             }
                                                 ).ToList()
                                 select new CoursePreviewModel
                                 {
                                     Id = c.Id,
                                     Code = c.Code,
                                     Description = c.Description,
                                     Name = c.Name,
                                     Image = !string.IsNullOrEmpty(c.Image) ? c.Image.Contains("t24-primary-image-storage") ? c.Image : LessonBusiness.geturl(c.Image, Certificate) : "",
                                     chapters = chapterPreviewModels.OrderBy(b => b.itemorder).ToList()
                                 }).FirstOrDefault();
            return coursePreView;
        }


        public CoursePreviewModel getCoursePreviewByIdTest(long id, long studentid, string Certificate)
        {
            var coursePreView = (from c in _training24Context.Course
                                 where c.Id == id && c.IsDeleted != true
                                 let chapterPreviewModels = (from cpt in _training24Context.Chapter
                                                             where cpt.CourseId == c.Id && cpt.IsDeleted != true

                                                             let chapterQuizPrivewModels = (from cq in _training24Context.chapterQuiz
                                                                                            join q in _training24Context.Quiz on cq.QuizId equals q.Id
                                                                                            where cq.ChapterId == cpt.Id && cq.IsDeleted != true && q.IsDeleted != true
                                                                                            select new QuizPreviewModel()
                                                                                            {
                                                                                                id = q.Id,
                                                                                                name = q.Name,
                                                                                                code = q.Code,
                                                                                                numquestions = q.NumQuestions,
                                                                                                itemorder = q.ItemOrder,
                                                                                                type = 2
                                                                                            }).ToList()
                                                             let assignmentsPrivewModels = (from a in _training24Context.Assignment
                                                                                            where a.ChapterId == cpt.Id && a.IsDeleted != true
                                                                                            let chapterAssigmentFiles = (from laf in _training24Context.AssignmentFile
                                                                                                                         where laf.AssignmentId == a.Id && laf.IsDeleted != true
                                                                                                                         let assignmentFilesPreviewModel = (from f in _training24Context.Files
                                                                                                                                                            join ft in _training24Context.FileTypes on f.FileTypeId equals ft.Id
                                                                                                                                                            where f.Id == laf.FileId && f.IsDeleted != true && ft.IsDeleted != true
                                                                                                                                                            select new ResponseFilesModel()
                                                                                                                                                            {
                                                                                                                                                                Id = f.Id,
                                                                                                                                                                name = f.Name,
                                                                                                                                                                url = LessonBusiness.geturl(f.Url, Certificate),
                                                                                                                                                                filename = f.FileName,
                                                                                                                                                                filetypeid = f.FileTypeId,
                                                                                                                                                                description = f.Description,
                                                                                                                                                                filetypename = ft.Filetype,
                                                                                                                                                                filesize = f.FileSize,
                                                                                                                                                                duration = f.Duration,
                                                                                                                                                                totalpages = f.TotalPages,
                                                                                                                                                            }).FirstOrDefault()
                                                                                                                         select new ResponseAssignmentFileModel
                                                                                                                         {
                                                                                                                             id = laf.Id,
                                                                                                                             files = assignmentFilesPreviewModel
                                                                                                                         }).ToList()
                                                                                            select new AssignmentPreviewModel
                                                                                            {
                                                                                                id = a.Id,
                                                                                                name = a.Name,
                                                                                                code = a.Code,
                                                                                                description = a.Description,
                                                                                                assignmentfiles = chapterAssigmentFiles //AssignmentBusiness.GetAssignmentFilesByAssignmentId(a.Id, Certificate)
                                                                                            }).ToList()
                                                             let lessonPrivewModels = (from x in _training24Context.Lesson
                                                                                       where x.ChapterId == cpt.Id && x.IsDeleted != true
                                                                                       let lessonFilesPreviewModel = (from lf in _training24Context.LessonFile
                                                                                                                      where lf.LessionId == x.Id && lf.IsDeleted != true
                                                                                                                      let filesPreviewModel = (from f in _training24Context.Files
                                                                                                                                               join ft in _training24Context.FileTypes on f.FileTypeId equals ft.Id
                                                                                                                                               where lf.FileId == f.Id && f.IsDeleted != true && ft.IsDeleted != true
                                                                                                                                               select new ResponseFilesModel()
                                                                                                                                               {
                                                                                                                                                   Id = f.Id,
                                                                                                                                                   name = f.Name,
                                                                                                                                                   url = LessonBusiness.geturl(f.Url, Certificate),
                                                                                                                                                   filename = f.FileName,
                                                                                                                                                   filetypeid = f.FileTypeId,
                                                                                                                                                   description = f.Description,
                                                                                                                                                   filetypename = ft.Filetype,
                                                                                                                                                   filesize = f.FileSize,
                                                                                                                                                   duration = f.Duration,
                                                                                                                                                   totalpages = f.TotalPages,
                                                                                                                                               }).FirstOrDefault()
                                                                                                                      let studentLessonProgressModel = (from lp in _training24Context.StudentLessonProgress
                                                                                                                                                        where lp.LessonId == x.Id && lp.StudentId == studentid && lp.IsDeleted != true
                                                                                                                                                        select new ResponseStudentLessonProgressModel
                                                                                                                                                        {
                                                                                                                                                            id = lp.Id,
                                                                                                                                                            lessonid = lp.LessonId,
                                                                                                                                                            lessonstatus = lp.LessonStatus,
                                                                                                                                                            studentid = lp.StudentId,
                                                                                                                                                            lessonprogress = lp.LessonProgress
                                                                                                                                                        }).SingleOrDefault()
                                                                                                                      select new ResponseLessonFileModel
                                                                                                                      {
                                                                                                                          id = lf.Id,
                                                                                                                          files = filesPreviewModel,
                                                                                                                          progress = studentLessonProgressModel
                                                                                                                      }).ToList()
                                                                                       let responseLessionAssignmentDTO = (from la in _training24Context.LessonAssignments
                                                                                                                           where la.LessonId == x.Id && la.IsDeleted != true
                                                                                                                           let lessonAssigmentFiles = (from laf in _training24Context.LessonAssignmentFiles
                                                                                                                                                       where laf.AssignmentId == la.Id && laf.IsDeleted != true
                                                                                                                                                       let assignmentFilesPreviewModel = (from f in _training24Context.Files
                                                                                                                                                                                          join ft in _training24Context.FileTypes on f.FileTypeId equals ft.Id
                                                                                                                                                                                          where f.Id == laf.FileId && f.IsDeleted != true && ft.IsDeleted != true
                                                                                                                                                                                          select new ResponseFilesModel()
                                                                                                                                                                                          {
                                                                                                                                                                                              Id = f.Id,
                                                                                                                                                                                              name = f.Name,
                                                                                                                                                                                              url = LessonBusiness.geturl(f.Url, Certificate),
                                                                                                                                                                                              filename = f.FileName,
                                                                                                                                                                                              filetypeid = f.FileTypeId,
                                                                                                                                                                                              description = f.Description,
                                                                                                                                                                                              filetypename = ft.Filetype,
                                                                                                                                                                                              filesize = f.FileSize,
                                                                                                                                                                                              duration = f.Duration,
                                                                                                                                                                                              totalpages = f.TotalPages,
                                                                                                                                                                                          }).FirstOrDefault()
                                                                                                                                                       select new ResponseLessonAssignmentFileDTO
                                                                                                                                                       {
                                                                                                                                                           id = laf.Id,
                                                                                                                                                           files = assignmentFilesPreviewModel
                                                                                                                                                       }).ToList()
                                                                                                                           select new ResponseLessionAssignmentDTO
                                                                                                                           {
                                                                                                                               id = Convert.ToInt32(la.Id),
                                                                                                                               name = la.Name,
                                                                                                                               code = la.Code,
                                                                                                                               description = la.Description,
                                                                                                                               assignmentfiles = lessonAssigmentFiles
                                                                                                                           }).FirstOrDefault()

                                                                                       select new LessonPrivewModel
                                                                                       {
                                                                                           id = x.Id,
                                                                                           name = x.Name,
                                                                                           code = x.Code,
                                                                                           description = x.Description,
                                                                                           itemorder = x.ItemOrder,
                                                                                           lessonfiles = lessonFilesPreviewModel,
                                                                                           assignment = responseLessionAssignmentDTO
                                                                                       }).ToList()
                                                             select new ChapterPreviewModel()
                                                             {
                                                                 Id = cpt.Id,
                                                                 Code = cpt.Code,
                                                                 Name = cpt.Name,
                                                                 itemorder = cpt.ItemOrder,
                                                                 quizs = chapterQuizPrivewModels,
                                                                 assignments = assignmentsPrivewModels,
                                                                 lessons = lessonPrivewModels.Cast<object>().ToList(),
                                                             }
                                                 ).ToList()
                                 select new CoursePreviewModel
                                 {
                                     Id = c.Id,
                                     Code = c.Code,
                                     Description = c.Description,
                                     Name = c.Name,
                                     Image = !string.IsNullOrEmpty(c.Image) ? c.Image.Contains("t24-primary-image-storage") ? c.Image : LessonBusiness.geturl(c.Image, Certificate) : "",
                                     chapters = chapterPreviewModels.OrderBy(b => b.itemorder).ToList()
                                 }).FirstOrDefault();
            return coursePreView;
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
    }
}
