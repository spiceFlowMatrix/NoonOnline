using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.StudentChapterProgress;
using Trainning24.BL.ViewModels.StudentCourseProgress;
using Trainning24.BL.ViewModels.StudentLessonProgress;
using Trainning24.BL.ViewModels.StudentProgress;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class ProgressBusiness
    {
        public readonly EFStudentCourseProgressRepository EFStudentCourseProgressRepository;
        public readonly EFStudentChapterProgressRepository EFStudentChapterProgressRepository;
        public readonly EFStudentLessonProgressRepository EFStudentLessonProgressRepository;
        public readonly EFStudentProgressRepository EFStudentProgressRepository;

        public ProgressBusiness
        (
            EFStudentCourseProgressRepository EFStudentCourseProgressRepository,
            EFStudentChapterProgressRepository EFStudentChapterProgressRepository,
            EFStudentLessonProgressRepository EFStudentLessonProgressRepository,
            EFStudentProgressRepository EFStudentProgressRepository
        )
        {
            this.EFStudentCourseProgressRepository = EFStudentCourseProgressRepository;
            this.EFStudentChapterProgressRepository = EFStudentChapterProgressRepository;
            this.EFStudentLessonProgressRepository = EFStudentLessonProgressRepository;
            this.EFStudentProgressRepository = EFStudentProgressRepository;
        }

        public ResponseStudentCourseProgressModel Create(AddStudentCourseProgressModel AddStudentCourseProgressModel, int id)
        {
            StudentCourseProgress StudentCourseProgress = new StudentCourseProgress
            {
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };
            EFStudentCourseProgressRepository.Insert(StudentCourseProgress);
            ResponseStudentCourseProgressModel responseStudentCourseProgressModel = new ResponseStudentCourseProgressModel
            {
                id = StudentCourseProgress.Id,
                courseid = StudentCourseProgress.CourseId,
                courseprogress = StudentCourseProgress.CourseProgress,
                coursestatus = StudentCourseProgress.CourseStatus,
                studentid = StudentCourseProgress.StudentId
            };
            return responseStudentCourseProgressModel;
        }        

        public ResponseStudentChapterProgressModel Create(AddStudentChapterProgressModel AddStudentChapterProgressModel, int id)
        {
            StudentChapterProgress StudentChapterProgress = new StudentChapterProgress
            {                
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };
            EFStudentChapterProgressRepository.Insert(StudentChapterProgress);
            ResponseStudentChapterProgressModel responseStudentChapterProgressModel = new ResponseStudentChapterProgressModel
            {
                id = StudentChapterProgress.Id,
                chapterid = StudentChapterProgress.ChapterId,
                chapterstatus = StudentChapterProgress.ChapterStatus,
                studentid = StudentChapterProgress.StudentId
            };
            return responseStudentChapterProgressModel;
        }

        public ResponseStudentProgressModel Create(AddStudentProgressModel AddStudentProgressModel, int id)
        {
            StudentProgress StudentProgress = new StudentProgress
            {             
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };
            EFStudentProgressRepository.Insert(StudentProgress);
            ResponseStudentProgressModel responseStudentProgressModel = new ResponseStudentProgressModel {
                id = StudentProgress.Id,
                chapterid = StudentProgress.ChapterId,
                chapterstatus = StudentProgress.ChapterStatus,
                studentid = StudentProgress.StudentId,
                lessonstatus = StudentProgress.LessonStatus,
                courseid = StudentProgress.CourseId,
                courseprogress = StudentProgress.CourseProgress,
                coursestatus = StudentProgress.CourseStatus,
                lessonid = StudentProgress.LessonId
            };
            return responseStudentProgressModel;
        }

        public ResponseStudentLessonProgressModel Create(AddStudentLessonProgressModel AddStudentLessonProgressModel, int id)
        {
            StudentLessonProgress StudentLessonProgress = new StudentLessonProgress
            {                
                LessonId = AddStudentLessonProgressModel.lessonid,
                StudentId = AddStudentLessonProgressModel.studentid,
                LessonProgress = AddStudentLessonProgressModel.lessonprogress,
                LessonStatus = AddStudentLessonProgressModel.lessonstatus,  
                Duration = AddStudentLessonProgressModel.duration,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };
            EFStudentLessonProgressRepository.Insert(StudentLessonProgress);
            ResponseStudentLessonProgressModel responseStudentLessonProgressModel = new ResponseStudentLessonProgressModel {
                id = StudentLessonProgress.Id,
                lessonid = StudentLessonProgress.LessonId,
                lessonstatus = StudentLessonProgress.LessonId,
                studentid = StudentLessonProgress.StudentId,
                lessonprogress = StudentLessonProgress.LessonProgress,   
                duration = StudentLessonProgress.Duration
            };
            return responseStudentLessonProgressModel;
        }

        public ResponseStudentLessonProgressModel Update(AddStudentLessonProgressModel AddStudentLessonProgressModel, int id)
        {
            StudentLessonProgress StudentLessonProgress = new StudentLessonProgress
            {
                Id = AddStudentLessonProgressModel.id,
                LessonId = AddStudentLessonProgressModel.lessonid,
                StudentId = AddStudentLessonProgressModel.studentid,
                LessonProgress = AddStudentLessonProgressModel.lessonprogress,
                LessonStatus = AddStudentLessonProgressModel.lessonstatus,     
                Duration = AddStudentLessonProgressModel.duration,
                IsDeleted = false,
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = id
            };
            EFStudentLessonProgressRepository.Update(StudentLessonProgress);
            ResponseStudentLessonProgressModel responseStudentLessonProgressModel = new ResponseStudentLessonProgressModel {
                id = StudentLessonProgress.Id,
                lessonid = StudentLessonProgress.LessonId,
                lessonstatus = StudentLessonProgress.LessonId,
                studentid = StudentLessonProgress.StudentId,
                duration = StudentLessonProgress.Duration,
                lessonprogress = StudentLessonProgress.LessonProgress
            };
            return responseStudentLessonProgressModel;
        }


        public ResponseStudentLessonProgressModel GetStudentLessonProgressByLession(long id,long studentid)
        {
            StudentLessonProgress studentLessonProgress = EFStudentLessonProgressRepository.ListQuery(b => b.LessonId == id && b.StudentId == studentid).SingleOrDefault();
            ResponseStudentLessonProgressModel responseStudentLessonProgressModel = new ResponseStudentLessonProgressModel();
            if (studentLessonProgress != null)
            {
                responseStudentLessonProgressModel = new ResponseStudentLessonProgressModel
                {
                    id = studentLessonProgress.Id,
                    lessonid = studentLessonProgress.LessonId,
                    lessonstatus = studentLessonProgress.LessonStatus,
                    studentid = studentLessonProgress.StudentId,
                    lessonprogress = studentLessonProgress.LessonProgress
                };
            }
            return responseStudentLessonProgressModel;
        }


        public ResponseStudentLessonProgressModel GetStudentLessonProgressByLession(long id)
        {
            StudentLessonProgress studentLessonProgress = EFStudentLessonProgressRepository.ListQuery(b => b.LessonId == id).SingleOrDefault();
            ResponseStudentLessonProgressModel responseStudentLessonProgressModel = new ResponseStudentLessonProgressModel();
            if (studentLessonProgress != null)
            {
                responseStudentLessonProgressModel = new ResponseStudentLessonProgressModel
                {
                    id = studentLessonProgress.Id,
                    lessonid = studentLessonProgress.LessonId,
                    lessonstatus = studentLessonProgress.LessonStatus,
                    studentid = studentLessonProgress.StudentId,
                    lessonprogress = studentLessonProgress.LessonProgress
                };
            }
            return responseStudentLessonProgressModel;
        }
    }
}
