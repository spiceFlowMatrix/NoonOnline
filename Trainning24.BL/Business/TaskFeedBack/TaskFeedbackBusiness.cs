using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Chapter;
using Trainning24.BL.ViewModels.Contact;
using Trainning24.BL.ViewModels.Course;
using Trainning24.BL.ViewModels.Feedback;
using Trainning24.BL.ViewModels.FeedBackCategory;
using Trainning24.BL.ViewModels.FeedBackTask;
using Trainning24.BL.ViewModels.FeedbackTime;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Grade;
using Trainning24.BL.ViewModels.Lesson;
using Trainning24.BL.ViewModels.QuestionFile;
using Trainning24.BL.ViewModels.TaskFeedBack;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class TaskFeedbackBusiness
    {
        private readonly LessonBusiness LessonBusiness;
        private readonly FilesBusiness FilesBusiness;
        private readonly UsersBusiness UsersBusiness;
        private readonly EFTaskFeedBack _eFTaskFeedBack;
        private readonly EFTaskFileFeedback _eFTaskFileFeedback;
        private readonly EFTaskActivityFeedBack _eFTaskActivityFeedback;
        private readonly EFTaskFeedBackCategory _eFTaskFeedBackCategory;
        private readonly EFGradeRepository _eFGradeRepository;
        private readonly EFCourseRepository _eFCourseRepository;
        private readonly EFChapterRepository _eFChapterRepository;

        public TaskFeedbackBusiness(
            LessonBusiness LessonBusiness,
            UsersBusiness UsersBusiness,
            FilesBusiness FilesBusiness,
            EFTaskFeedBack eFTaskFeedBack,
            EFTaskFileFeedback eFTaskFileFeedback,
            EFTaskActivityFeedBack eFTaskActivityFeedback,
            EFTaskFeedBackCategory eFTaskFeedBackCategory,
            EFGradeRepository eFGradeRepository,
            EFCourseRepository eFCourseRepository,
            EFChapterRepository eFChapterRepository
            )
        {
            this.FilesBusiness = FilesBusiness;
            this.UsersBusiness = UsersBusiness;
            this.LessonBusiness = LessonBusiness;
            _eFTaskFeedBack = eFTaskFeedBack;
            _eFTaskFileFeedback = eFTaskFileFeedback;
            _eFTaskActivityFeedback = _eFTaskActivityFeedback;
            _eFTaskFeedBackCategory = eFTaskFeedBackCategory;
            _eFGradeRepository = eFGradeRepository;
            _eFCourseRepository = eFCourseRepository;
            _eFChapterRepository = eFChapterRepository;
        }

        public TaskFeedBack AddFeedback(TaskFeedBack feedback)
        {
            _eFTaskFeedBack.Insert(feedback);
            return feedback;
        }

        public TaskFileFeedBack AddFeedbackFiles(TaskFileFeedBack taskFileFeedBack)
        {
            _eFTaskFileFeedback.Insert(taskFileFeedBack);
            return taskFileFeedBack;
        }

        public List<TaskFeedBackListDTO> GetInQueueTaskList(FeedbackPaginationModel dto, string certificate, out int total)
        {
            List<TaskFeedBackListDTO> taskFeedBackListDTOs = new List<TaskFeedBackListDTO>();
            var taskFeedBack = _eFTaskFeedBack.ListQuery(b => b.Status == 1 && b.IsDeleted != true).ToList();
            total = taskFeedBack.Count();
            if (taskFeedBack.Count > 0)
            {
                if (dto.categoryid != 0)
                {
                    taskFeedBack = taskFeedBack.Where(b => b.CategoryId == dto.categoryid).ToList();
                }
                if (!string.IsNullOrEmpty(dto.begin) && !string.IsNullOrEmpty(dto.end))
                {
                    taskFeedBack = taskFeedBack.Where(b => Convert.ToDateTime(b.CreationTime).Date >= Convert.ToDateTime(dto.begin).Date && Convert.ToDateTime(b.CreationTime).Date <= Convert.ToDateTime(dto.end).Date).ToList();
                }
                if (dto.userid != 0)
                {
                    taskFeedBack = taskFeedBack.Where(b => b.CreatorUserId == dto.userid).ToList();
                }
                total = taskFeedBack.Count();
                taskFeedBack = taskFeedBack.OrderByDescending(b => b.Id).
                            Skip(dto.perpagerecord * (dto.pagenumber - 1)).
                            Take(dto.perpagerecord).
                            ToList();

                if (taskFeedBack.Count > 0)
                {
                    foreach (var feedback in taskFeedBack)
                    {
                        TaskFeedBackListDTO taskFeedBackData = new TaskFeedBackListDTO();
                        taskFeedBackData.id = feedback.Id;
                        taskFeedBackData.title = feedback.Title;
                        taskFeedBackData.description = feedback.Description;
                        taskFeedBackData.category = GetFeedBackCategory(feedback.CategoryId);
                        taskFeedBackData.startdate = feedback.StartDate;
                        taskFeedBackData.complateddate = feedback.ComplatedDate;
                        taskFeedBackData.status = feedback.Status;
                        taskFeedBackData.user = UsersBusiness.GetUsersDetails(feedback.CreatorUserId ?? 0, certificate);
                        taskFeedBackData.submitteddate = feedback.CreationTime;
                        taskFeedBackData.archiveddate = feedback.ArchivedDate;
                        taskFeedBackListDTOs.Add(taskFeedBackData);
                    }
                }
            }
            return taskFeedBackListDTOs;
        }
        public List<TaskFeedBackListDTO> GetInProcessTaskList(FeedbackPaginationModel dto, string certificate, out int total)
        {
            List<TaskFeedBackListDTO> taskFeedBackListDTOs = new List<TaskFeedBackListDTO>();
            var taskFeedBack = _eFTaskFeedBack.ListQuery(b => b.Status == 2 && b.IsDeleted != true).ToList();
            total = taskFeedBack.Count();
            if (taskFeedBack.Count > 0)
            {
                if (dto.categoryid != 0)
                {
                    taskFeedBack = taskFeedBack.Where(b => b.CategoryId == dto.categoryid).ToList();
                }
                if (!string.IsNullOrEmpty(dto.begin) && !string.IsNullOrEmpty(dto.end))
                {
                    taskFeedBack = taskFeedBack.Where(b => Convert.ToDateTime(b.CreationTime).Date >= Convert.ToDateTime(dto.begin).Date && Convert.ToDateTime(b.CreationTime).Date <= Convert.ToDateTime(dto.end).Date).ToList();
                }
                if (dto.userid != 0)
                {
                    taskFeedBack = taskFeedBack.Where(b => b.CreatorUserId == dto.userid).ToList();
                }
                if (!string.IsNullOrEmpty(dto.startdate))
                {
                    taskFeedBack = taskFeedBack.Where(b => Convert.ToDateTime(b.StartDate).Date == Convert.ToDateTime(dto.startdate).Date).ToList();
                }
                total = taskFeedBack.Count();
                taskFeedBack = taskFeedBack.OrderByDescending(b => b.Id).
                            Skip(dto.perpagerecord * (dto.pagenumber - 1)).
                            Take(dto.perpagerecord).
                            ToList();

                if (taskFeedBack.Count > 0)
                {
                    foreach (var feedback in taskFeedBack)
                    {
                        TaskFeedBackListDTO taskFeedBackData = new TaskFeedBackListDTO();
                        taskFeedBackData.id = feedback.Id;
                        taskFeedBackData.title = feedback.Title;
                        taskFeedBackData.description = feedback.Description;
                        taskFeedBackData.category = GetFeedBackCategory(feedback.CategoryId);
                        taskFeedBackData.startdate = feedback.StartDate;
                        taskFeedBackData.complateddate = feedback.ComplatedDate;
                        taskFeedBackData.status = feedback.Status;
                        taskFeedBackData.user = UsersBusiness.GetUsersDetails(feedback.CreatorUserId ?? 0, certificate);
                        taskFeedBackData.submitteddate = feedback.CreationTime;
                        taskFeedBackData.archiveddate = feedback.ArchivedDate;
                        taskFeedBackListDTOs.Add(taskFeedBackData);
                    }
                }
            }
            return taskFeedBackListDTOs;
        }
        public List<TaskFeedBackListDTO> GetComplatedTaskList(FeedbackPaginationModel dto, string certificate, out int total)
        {
            List<TaskFeedBackListDTO> taskFeedBackListDTOs = new List<TaskFeedBackListDTO>();
            var taskFeedBack = _eFTaskFeedBack.ListQuery(b => b.Status == 3 && b.IsDeleted != true).ToList();
            total = taskFeedBack.Count();
            if (taskFeedBack.Count > 0)
            {
                if (dto.categoryid != 0)
                {
                    taskFeedBack = taskFeedBack.Where(b => b.CategoryId == dto.categoryid).ToList();
                }
                if (!string.IsNullOrEmpty(dto.begin) && !string.IsNullOrEmpty(dto.end))
                {
                    taskFeedBack = taskFeedBack.Where(b => Convert.ToDateTime(b.CreationTime).Date >= Convert.ToDateTime(dto.begin).Date && Convert.ToDateTime(b.CreationTime).Date <= Convert.ToDateTime(dto.end).Date).ToList();
                }
                if (dto.userid != 0)
                {
                    taskFeedBack = taskFeedBack.Where(b => b.CreatorUserId == dto.userid).ToList();
                }
                if (!string.IsNullOrEmpty(dto.startdate))
                {
                    taskFeedBack = taskFeedBack.Where(b => Convert.ToDateTime(b.StartDate).Date == Convert.ToDateTime(dto.startdate).Date).ToList();
                }
                total = taskFeedBack.Count();
                taskFeedBack = taskFeedBack.OrderByDescending(b => b.Id).
                            Skip(dto.perpagerecord * (dto.pagenumber - 1)).
                            Take(dto.perpagerecord).
                            ToList();

                if (taskFeedBack.Count > 0)
                {
                    foreach (var feedback in taskFeedBack)
                    {
                        TaskFeedBackListDTO taskFeedBackData = new TaskFeedBackListDTO();
                        taskFeedBackData.id = feedback.Id;
                        taskFeedBackData.title = feedback.Title;
                        taskFeedBackData.description = feedback.Description;
                        taskFeedBackData.category = GetFeedBackCategory(feedback.CategoryId);
                        taskFeedBackData.startdate = feedback.StartDate;
                        taskFeedBackData.complateddate = feedback.ComplatedDate;
                        taskFeedBackData.status = feedback.Status;
                        taskFeedBackData.user = UsersBusiness.GetUsersDetails(feedback.CreatorUserId ?? 0, certificate);
                        taskFeedBackData.submitteddate = feedback.CreationTime;
                        taskFeedBackData.archiveddate = feedback.ArchivedDate;
                        taskFeedBackListDTOs.Add(taskFeedBackData);
                    }
                }
            }
            return taskFeedBackListDTOs;
        }
        public List<TaskFeedBackListDTO> GetArchivedTaskList(FeedbackPaginationModel dto, string certificate, out int total)
        {
            List<TaskFeedBackListDTO> taskFeedBackListDTOs = new List<TaskFeedBackListDTO>();
            var taskFeedBack = _eFTaskFeedBack.ListQuery(b => b.Status == 4 && b.IsDeleted != true).ToList();
            total = taskFeedBack.Count();
            if (taskFeedBack.Count > 0)
            {
                if (dto.categoryid != 0)
                {
                    taskFeedBack = taskFeedBack.Where(b => b.CategoryId == dto.categoryid).ToList();
                }
                if (!string.IsNullOrEmpty(dto.begin) && !string.IsNullOrEmpty(dto.end))
                {
                    taskFeedBack = taskFeedBack.Where(b => Convert.ToDateTime(b.CreationTime).Date >= Convert.ToDateTime(dto.begin).Date && Convert.ToDateTime(b.CreationTime).Date <= Convert.ToDateTime(dto.end).Date).ToList();
                }
                if (dto.userid != 0)
                {
                    taskFeedBack = taskFeedBack.Where(b => b.CreatorUserId == dto.userid).ToList();
                }
                if (!string.IsNullOrEmpty(dto.archiveddate))
                {
                    taskFeedBack = taskFeedBack.Where(b => Convert.ToDateTime(b.ArchivedDate).Date == Convert.ToDateTime(dto.archiveddate).Date).ToList();
                }
                total = taskFeedBack.Count();
                taskFeedBack = taskFeedBack.OrderByDescending(b => b.Id).
                            Skip(dto.perpagerecord * (dto.pagenumber - 1)).
                            Take(dto.perpagerecord).
                            ToList();

                if (taskFeedBack.Count > 0)
                {
                    foreach (var feedback in taskFeedBack)
                    {
                        TaskFeedBackListDTO taskFeedBackData = new TaskFeedBackListDTO();
                        taskFeedBackData.id = feedback.Id;
                        taskFeedBackData.title = feedback.Title;
                        taskFeedBackData.description = feedback.Description;
                        taskFeedBackData.category = GetFeedBackCategory(feedback.CategoryId);
                        taskFeedBackData.startdate = feedback.StartDate;
                        taskFeedBackData.complateddate = feedback.ComplatedDate;
                        taskFeedBackData.status = feedback.Status;
                        taskFeedBackData.user = UsersBusiness.GetUsersDetails(feedback.CreatorUserId ?? 0, certificate);
                        taskFeedBackData.submitteddate = feedback.CreationTime;
                        taskFeedBackData.archiveddate = feedback.ArchivedDate;
                        taskFeedBackListDTOs.Add(taskFeedBackData);
                    }
                }
            }
            return taskFeedBackListDTOs;
        }

        public List<TaskFeedBackListDTO> GetInQueueTaskListApp(PaginationModel paginationModel, string certificate, out int total, int userid)
        {
            List<TaskFeedBackListDTO> taskFeedBackListDTOs = new List<TaskFeedBackListDTO>();
            var taskFeedBack = _eFTaskFeedBack.ListQuery(b => b.CreatorUserId == userid && b.Status == 1 && b.IsDeleted != true).ToList();
            total = taskFeedBack.Count();
            if (taskFeedBack.Count > 0)
            {
                taskFeedBack = taskFeedBack.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();

                if (taskFeedBack.Count > 0)
                {
                    foreach (var feedback in taskFeedBack)
                    {
                        TaskFeedBackListDTO taskFeedBackData = new TaskFeedBackListDTO();
                        taskFeedBackData.id = feedback.Id;
                        taskFeedBackData.title = feedback.Title;
                        taskFeedBackData.description = feedback.Description;
                        taskFeedBackData.category = GetFeedBackCategory(feedback.CategoryId);
                        taskFeedBackData.startdate = feedback.StartDate;
                        taskFeedBackData.complateddate = feedback.ComplatedDate;
                        taskFeedBackData.status = feedback.Status;
                        taskFeedBackData.user = UsersBusiness.GetUsersDetails(feedback.CreatorUserId ?? 0, certificate);
                        taskFeedBackData.submitteddate = feedback.CreationTime;
                        taskFeedBackData.archiveddate = feedback.ArchivedDate;
                        taskFeedBackListDTOs.Add(taskFeedBackData);
                    }
                }
            }
            return taskFeedBackListDTOs;
        }
        public List<TaskFeedBackListDTO> GetInProcessTaskListApp(PaginationModel paginationModel, string certificate, out int total, int userid)
        {
            List<TaskFeedBackListDTO> taskFeedBackListDTOs = new List<TaskFeedBackListDTO>();
            var taskFeedBack = _eFTaskFeedBack.ListQuery(b => b.CreatorUserId == userid && b.Status == 2 && b.IsDeleted != true).ToList();
            total = taskFeedBack.Count();
            if (taskFeedBack.Count > 0)
            {
                taskFeedBack = taskFeedBack.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();

                if (taskFeedBack.Count > 0)
                {
                    foreach (var feedback in taskFeedBack)
                    {
                        TaskFeedBackListDTO taskFeedBackData = new TaskFeedBackListDTO();
                        taskFeedBackData.id = feedback.Id;
                        taskFeedBackData.title = feedback.Title;
                        taskFeedBackData.description = feedback.Description;
                        taskFeedBackData.category = GetFeedBackCategory(feedback.CategoryId);
                        taskFeedBackData.startdate = feedback.StartDate;
                        taskFeedBackData.complateddate = feedback.ComplatedDate;
                        taskFeedBackData.status = feedback.Status;
                        taskFeedBackData.user = UsersBusiness.GetUsersDetails(feedback.CreatorUserId ?? 0, certificate);
                        taskFeedBackData.submitteddate = feedback.CreationTime;
                        taskFeedBackData.archiveddate = feedback.ArchivedDate;
                        taskFeedBackListDTOs.Add(taskFeedBackData);
                    }
                }
            }
            return taskFeedBackListDTOs;
        }
        public List<TaskFeedBackListDTO> GetComplatedTaskListApp(PaginationModel paginationModel, string certificate, out int total, int userid)
        {
            List<TaskFeedBackListDTO> taskFeedBackListDTOs = new List<TaskFeedBackListDTO>();
            var taskFeedBack = _eFTaskFeedBack.ListQuery(b => b.CreatorUserId == userid && b.Status == 3 && b.IsDeleted != true).ToList();
            total = taskFeedBack.Count();
            if (taskFeedBack.Count > 0)
            {
                taskFeedBack = taskFeedBack.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();

                if (taskFeedBack.Count > 0)
                {
                    foreach (var feedback in taskFeedBack)
                    {
                        TaskFeedBackListDTO taskFeedBackData = new TaskFeedBackListDTO();
                        taskFeedBackData.id = feedback.Id;
                        taskFeedBackData.title = feedback.Title;
                        taskFeedBackData.description = feedback.Description;
                        taskFeedBackData.category = GetFeedBackCategory(feedback.CategoryId);
                        taskFeedBackData.startdate = feedback.StartDate;
                        taskFeedBackData.complateddate = feedback.ComplatedDate;
                        taskFeedBackData.status = feedback.Status;
                        taskFeedBackData.user = UsersBusiness.GetUsersDetails(feedback.CreatorUserId ?? 0, certificate);
                        taskFeedBackData.submitteddate = feedback.CreationTime;
                        taskFeedBackData.archiveddate = feedback.ArchivedDate;
                        taskFeedBackListDTOs.Add(taskFeedBackData);
                    }
                }
            }
            return taskFeedBackListDTOs;
        }

        public TaskFeedBackCategoryDTO GetFeedBackCategory(long id)
        {
            TaskFeedBackCategoryDTO taskFeedBackCategoryDTO = new TaskFeedBackCategoryDTO();
            var category = _eFTaskFeedBackCategory.GetById(b => b.Id == id);
            if (category != null)
            {
                taskFeedBackCategoryDTO.id = category.Id;
                taskFeedBackCategoryDTO.name = category.Name;
            }
            return taskFeedBackCategoryDTO;
        }

        public TaskFeedBack MoveInProgress(long id, int userid)
        {
            var taskFeedBack = _eFTaskFeedBack.GetById(b => b.Id == id && b.IsDeleted != true);
            if (taskFeedBack != null)
            {
                taskFeedBack.Status = 2;
                taskFeedBack.StartDate = DateTime.UtcNow.ToString();
                taskFeedBack.LastModificationTime = DateTime.UtcNow.ToString();
                taskFeedBack.LastModifierUserId = userid;
                _eFTaskFeedBack.Update(taskFeedBack);
            }
            return taskFeedBack;
        }

        public TaskFeedBack MoveInCompleted(long id, int userid)
        {
            var taskFeedBack = _eFTaskFeedBack.GetById(b => b.Id == id && b.IsDeleted != true);
            if (taskFeedBack != null)
            {
                taskFeedBack.Status = 3;
                taskFeedBack.ComplatedDate = DateTime.UtcNow.ToString();
                taskFeedBack.LastModificationTime = DateTime.UtcNow.ToString();
                taskFeedBack.LastModifierUserId = userid;
                _eFTaskFeedBack.Update(taskFeedBack);
            }
            return taskFeedBack;
        }

        public TaskFeedBack MoveInArchived(long id, int userid)
        {
            var taskFeedBack = _eFTaskFeedBack.GetById(b => b.Id == id && b.IsDeleted != true);
            if (taskFeedBack != null)
            {
                taskFeedBack.Status = 4;
                taskFeedBack.ArchivedDate = DateTime.UtcNow.ToString();
                taskFeedBack.LastModificationTime = DateTime.UtcNow.ToString();
                taskFeedBack.LastModifierUserId = userid;
                _eFTaskFeedBack.Update(taskFeedBack);
            }
            return taskFeedBack;
        }

        public TaskFeedBackDetailsDTO GetTaskFeedBackDetailsById(string certificate, long id)
        {
            TaskFeedBackDetailsDTO taskFeedBackDetailsDTO = new TaskFeedBackDetailsDTO();
            var taskFeedBack = _eFTaskFeedBack.GetById(b => b.Id == id && b.IsDeleted != true);
            if (taskFeedBack != null)
            {

                taskFeedBackDetailsDTO.id = taskFeedBack.Id;
                taskFeedBackDetailsDTO.title = taskFeedBack.Title;
                taskFeedBackDetailsDTO.description = taskFeedBack.Description;
                taskFeedBackDetailsDTO.category = GetFeedBackCategory(taskFeedBack.CategoryId);
                taskFeedBackDetailsDTO.startdate = taskFeedBack.StartDate;
                taskFeedBackDetailsDTO.complateddate = taskFeedBack.ComplatedDate;
                taskFeedBackDetailsDTO.time = taskFeedBack.Time;
                taskFeedBackDetailsDTO.status = taskFeedBack.Status;
                taskFeedBackDetailsDTO.device = taskFeedBack.Device;
                taskFeedBackDetailsDTO.version = taskFeedBack.Version;
                taskFeedBackDetailsDTO.appversion = taskFeedBack.AppVersion;
                taskFeedBackDetailsDTO.operatingsystem = taskFeedBack.OperatingSystem;
                taskFeedBackDetailsDTO.user = UsersBusiness.GetUsersDetails(taskFeedBack.CreatorUserId ?? 0, certificate);
                taskFeedBackDetailsDTO.submitteddate = taskFeedBack.CreationTime;
                taskFeedBackDetailsDTO.archiveddate = taskFeedBack.ArchivedDate;
                taskFeedBackDetailsDTO.assign = taskFeedBack.Assign;
                taskFeedBackDetailsDTO.files = GetFeedBackFilesById(taskFeedBack.Id, certificate);
                if (taskFeedBack.GradeId != null && taskFeedBack.GradeId != 0)
                {
                    taskFeedBackDetailsDTO.grade = GetGradeDetails(taskFeedBack.GradeId ?? 0);
                }
                if (taskFeedBack.CourseId != null && taskFeedBack.CourseId != 0)
                {
                    taskFeedBackDetailsDTO.course = GetCourseDetails(taskFeedBack.CourseId ?? 0);
                }
                if (taskFeedBack.ChapterId != null && taskFeedBack.ChapterId != 0)
                {
                    taskFeedBackDetailsDTO.chapter = GetChapterDetails(taskFeedBack.ChapterId ?? 0);
                }
                if (taskFeedBack.LessonId != null && taskFeedBack.LessonId != 0)
                {
                    taskFeedBackDetailsDTO.lesson = GetLessonDetails(taskFeedBack.LessonId ?? 0);
                }
            }
            return taskFeedBackDetailsDTO;
        }
        public List<FeedBackFilesDTO> GetFeedBackFilesById(long Id, string Certificate)
        {
            List<TaskFileFeedBack> feedBackFiles = _eFTaskFileFeedback.GetFeedBackFile(Id);
            List<FeedBackFilesDTO> feedBackFilesDTOs = new List<FeedBackFilesDTO>();
            List<ResponseFilesModel> responseFilesModelList = new List<ResponseFilesModel>();
            if (feedBackFiles.Count > 0)
            {
                foreach (var file in feedBackFiles)
                {
                    Files newFiles = FilesBusiness.getFilesById(file.FileId);
                    if (newFiles != null)
                    {
                        FeedBackFilesDTO feedBackFile = new FeedBackFilesDTO();
                        ResponseFilesModel responseFilesModel = new ResponseFilesModel();
                        var filetyped = FilesBusiness.FileType(newFiles);
                        responseFilesModel.Id = newFiles.Id;
                        responseFilesModel.name = newFiles.Name;
                        if (!string.IsNullOrEmpty(newFiles.Url))
                            responseFilesModel.url = LessonBusiness.geturl(newFiles.Url, Certificate);
                        //responseFilesModel.url = newFiles.Url;
                        responseFilesModel.filename = newFiles.FileName;
                        responseFilesModel.filetypeid = newFiles.FileTypeId;
                        responseFilesModel.description = newFiles.Description;
                        responseFilesModel.filetypename = filetyped.Filetype;
                        responseFilesModel.filesize = newFiles.FileSize;
                        feedBackFile.id = file.Id;
                        feedBackFile.files = responseFilesModel;
                        feedBackFilesDTOs.Add(feedBackFile);
                    }
                }
            }
            return feedBackFilesDTOs;
        }

        public GradeDetailsDTO GetGradeDetails(long id)
        {
            GradeDetailsDTO gradeDetailsDTO = new GradeDetailsDTO();
            var grade = _eFGradeRepository.GetById(b => b.Id == id && b.IsDeleted != true);
            if (grade != null)
            {
                gradeDetailsDTO.id = grade.Id;
                gradeDetailsDTO.name = grade.Name;
            }
            return gradeDetailsDTO;
        }
        public CourseDetailsDTO GetCourseDetails(long id)
        {
            CourseDetailsDTO courseDetailsDTO = new CourseDetailsDTO();
            var course = _eFCourseRepository.GetById(b => b.Id == id && b.IsDeleted != true);
            if (course != null)
            {
                courseDetailsDTO.id = course.Id;
                courseDetailsDTO.name = course.Name;
            }
            return courseDetailsDTO;
        }
        public ChapterDetailsDTO GetChapterDetails(long id)
        {
            ChapterDetailsDTO chapterDetailsDTO = new ChapterDetailsDTO();
            var chapter = _eFChapterRepository.GetById(b => b.Id == id && b.IsDeleted != true);
            if (chapter != null)
            {
                chapterDetailsDTO.id = chapter.Id;
                chapterDetailsDTO.name = chapter.Name;
            }
            return chapterDetailsDTO;
        }
        public LessonDetailsDTO GetLessonDetails(long id)
        {
            LessonDetailsDTO lessonDetailsDTO = new LessonDetailsDTO();
            var lesson = LessonBusiness.getLessonById((int)id);
            if (lesson != null)
            {
                lessonDetailsDTO.id = lesson.Id;
                lessonDetailsDTO.name = lesson.Name;
            }
            return lessonDetailsDTO;
        }

        public List<TaskFeedBackCategoryDTO> GetFeedBackCategoryList()
        {
            List<TaskFeedBackCategoryDTO> taskFeedBackCategoryDTOs = new List<TaskFeedBackCategoryDTO>();
            var category = _eFTaskFeedBackCategory.GetAll();
            if (category.Count > 0)
            {
                foreach (var cat in category)
                {
                    TaskFeedBackCategoryDTO taskFeedBackCategoryDTO = new TaskFeedBackCategoryDTO();
                    taskFeedBackCategoryDTO.id = cat.Id;
                    taskFeedBackCategoryDTO.name = cat.Name;
                    taskFeedBackCategoryDTOs.Add(taskFeedBackCategoryDTO);
                }
            }
            return taskFeedBackCategoryDTOs;
        }

        public List<int> GetFeedbackUsersIds()
        {
            return _eFTaskFeedBack.GetAll().Select(b => b.CreatorUserId ?? 0).Distinct().ToList();
        }

        public TaskFeedBack AssignUserTask(long TaskId, long UserId, long ModifiedBy)
        {
            var task = _eFTaskFeedBack.GetById(b => b.Id == TaskId && b.IsDeleted != true);
            if (task != null)
            {
                task.Assign = UserId;
                task.LastModifierUserId = (int)ModifiedBy;
                task.LastModificationTime = DateTime.UtcNow.ToString();
                _eFTaskFeedBack.Update(task);
            }
            return task;
        }
    }
}
