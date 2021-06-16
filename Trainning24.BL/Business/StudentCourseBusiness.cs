using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.BundleCourse;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class StudentCourseBusiness
    {
        private readonly EFStudentCourseRepository EFStudentCourseRepository;

        public StudentCourseBusiness(
            EFStudentCourseRepository EFStudentCourseRepository
        )
        {
            this.EFStudentCourseRepository = EFStudentCourseRepository;
        }

        public UserCourse Create(AddStudentCourseModel AddStudentCourseModel, int id)
        {
            UserCourse StudentCourse = new UserCourse
            {
                CourseId = AddStudentCourseModel.courseid,
                UserId = AddStudentCourseModel.userid,
                StartDate = AddStudentCourseModel.startdate.ToString(),
                EndDate = AddStudentCourseModel.enddate.ToString(),
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id,
                IsDeleted = false
            };

            List<UserCourse> StudentCourses = EFStudentCourseRepository.ListQuery(b => b.CourseId == StudentCourse.CourseId && b.UserId == StudentCourse.UserId && b.IsDeleted != true).ToList();

            if (StudentCourses.Count == 0)
            {
                EFStudentCourseRepository.Insert(StudentCourse);
            }
            else
            {
                StudentCourse = null;
            }

            return StudentCourse;
        }

        public UserCourse Update(UpdateStudentCourseModel UpdateStudentCourseModel, int id)
        {
            UserCourse userCourse = getStudentCourseById(UpdateStudentCourseModel.id);

            userCourse.CourseId = UpdateStudentCourseModel.courseid;
            userCourse.UserId = UpdateStudentCourseModel.userid;
            userCourse.StartDate = UpdateStudentCourseModel.startdate.ToString();
            userCourse.EndDate = UpdateStudentCourseModel.enddate.ToString();
            userCourse.LastModificationTime = DateTime.Now.ToString();
            userCourse.LastModifierUserId = id;
            userCourse.IsExpire = false;

            //UserCourse StudentCourse = new UserCourse
            //{
            //    Id = UpdateStudentCourseModel.id,
            //    CourseId = UpdateStudentCourseModel.courseid,
            //    UserId = UpdateStudentCourseModel.userid,
            //    StartDate = UpdateStudentCourseModel.startdate.ToString(),
            //    EndDate = UpdateStudentCourseModel.enddate.ToString(),
            //    IsDeleted = false,
            //    LastModificationTime = DateTime.Now.ToString(),
            //    LastModifierUserId = id
            //};

            EFStudentCourseRepository.Update(userCourse);

            return userCourse;
        }

        public List<UserCourse> GetCoursesByStudentId(CourseStudentPaginationModel paginationModel, out int total)
        {
            List<UserCourse> usercourselist = new List<UserCourse>();
            usercourselist = EFStudentCourseRepository.ListQuery(b => b.UserId == paginationModel.studentid
            && b.IsDeleted != true).ToList();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = usercourselist.Count();

                usercourselist = usercourselist.OrderByDescending(b => b.CreationTime).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();
                return usercourselist;
            }

            total = usercourselist.Count();

            return usercourselist;
        }

        public List<UserCourse> GetStudentsByCourseId(StudentCoursePaginationModel paginationModel, out int total)
        {
            List<UserCourse> usercourselist = new List<UserCourse>();
            usercourselist = EFStudentCourseRepository.ListQuery(b => b.CourseId == paginationModel.courseid
            && b.IsDeleted != true).ToList();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = usercourselist.Count();

                usercourselist = usercourselist.OrderByDescending(b => b.CreationTime).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();
                return usercourselist;
            }

            total = usercourselist.Count();

            return usercourselist;
        }

        public UserCourse getStudentCourseById(long id)
        {
            return EFStudentCourseRepository.GetById(b => b.Id == id);
        }

        public UserCourse Delete(long Id, int DeleterId)
        {
            UserCourse StudentCourse = new UserCourse();
            StudentCourse.Id = Id;
            StudentCourse.DeleterUserId = DeleterId;
            int i = EFStudentCourseRepository.Delete(Id, StudentCourse);
            UserCourse deletedStudentCourse = EFStudentCourseRepository.GetById(int.Parse(Id.ToString()));
            return deletedStudentCourse;
        }

        public List<UserCourse> StudentCourseList(PaginationModel paginationModel, out int total)
        {
            List<UserCourse> usercourselist = new List<UserCourse>();
            usercourselist = EFStudentCourseRepository.GetAll();
            usercourselist = usercourselist.Where(b => Convert.ToDateTime(b.EndDate, CultureInfo.InvariantCulture).Date >= DateTime.Now.Date).ToList();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = usercourselist.Count();

                usercourselist = usercourselist.OrderByDescending(b => b.CreationTime).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();
                return usercourselist;
            }

            total = usercourselist.Count();
            return usercourselist;
        }

        //public long GetCourseCount(int userid, int roleid)
        //{
        //    var count = EFStudentCourseRepository.ListQuery(b => b.UserId == userid && b.IsDeleted != true).Count();
        //    return count;
        //}

        public List<UserCourse> GetUserListByCourseId(long courseid)
        {
            return EFStudentCourseRepository.ListQuery(b => b.CourseId == courseid && b.IsDeleted != true).ToList();
        }
        public List<long> GetCourseIdByUser(long userid)
        {
            return EFStudentCourseRepository.ListQuery(b => b.UserId == userid && b.IsDeleted != true).Select(b => b.CourseId).ToList();
        }

        public List<long> GetAllCourseByUserId(List<int> userid)
        {
            return EFStudentCourseRepository.ListQuery(b => userid.Contains((int)b.UserId) && b.IsDeleted != true && b.IsExpire != true).Select(b => b.CourseId).ToList();
        }

        public List<UserCourse> GetAllCourseByUserId(long userid)
        {
            return EFStudentCourseRepository.ListQuery(b => b.UserId == userid && b.IsDeleted != true && b.IsExpire != true).ToList();
        }
    }
}
