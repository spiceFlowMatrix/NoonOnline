using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.CourseGrade;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class CourseGradeBusiness
    {
        private readonly EFCourseGradeRepository EFCourseGradeRepository;
        private readonly EFCourseRepository EFCourseRepository;

        public CourseGradeBusiness(
            EFCourseGradeRepository EFCourseGradeRepository,
            EFCourseRepository EFCourseRepository
        )
        {
            this.EFCourseGradeRepository = EFCourseGradeRepository;
            this.EFCourseRepository = EFCourseRepository;
        }

        public CourseGrade Createtmp(AddCourseGradeModel AddCourseGradeModel, int id)
        {
            CourseGrade courseGradeExits = EFCourseGradeRepository.ListQuery(b => b.CourseId == AddCourseGradeModel.courseid && b.Gradeid == AddCourseGradeModel.gradeid).SingleOrDefault();

            if(courseGradeExits != null){
                return null;
            }

            CourseGrade CourseGrade = new CourseGrade
            {
                CourseId = AddCourseGradeModel.courseid,
                Gradeid = AddCourseGradeModel.gradeid,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };

            EFCourseGradeRepository.Insert(CourseGrade);

            return CourseGrade;
        }


        public CourseGrade Create(AddCourseGradeModel AddCourseGradeModel, int id)
        {
            CourseGrade courseGradeExits = EFCourseGradeRepository.ListQuery(b => b.CourseId == AddCourseGradeModel.courseid).SingleOrDefault();
            CourseGrade courseGrade = new CourseGrade();
            if (courseGradeExits == null)
            {
                courseGrade.CourseId = AddCourseGradeModel.courseid;
                courseGrade.Gradeid = AddCourseGradeModel.gradeid;
                courseGrade.IsDeleted = false;
                courseGrade.CreationTime = DateTime.Now.ToString();
                courseGrade.CreatorUserId = id;
                EFCourseGradeRepository.Insert(courseGrade);
                return EFCourseGradeRepository.GetById(b => b.Id == courseGrade.Id);
            }
            else
            {
                if(courseGradeExits.Gradeid != AddCourseGradeModel.gradeid)
                {
                    courseGradeExits.Id = courseGradeExits.Id;
                    courseGradeExits.CourseId = AddCourseGradeModel.courseid;
                    courseGradeExits.Gradeid = AddCourseGradeModel.gradeid;
                    courseGradeExits.IsDeleted = false;
                    courseGradeExits.LastModificationTime = DateTime.Now.ToString();
                    courseGradeExits.LastModifierUserId = id;
                    EFCourseGradeRepository.Update(courseGradeExits);
                }
                return EFCourseGradeRepository.GetById(b => b.Id == courseGradeExits.Id);
            }           
        }

        public CourseGrade Update(UpdateCourseGradeModel UpdateCourseGradeModel, int id)
        {
            CourseGrade CourseGrade = EFCourseGradeRepository.GetById(b => b.Id == UpdateCourseGradeModel.id);
            CourseGrade.CourseId = UpdateCourseGradeModel.courseid;
            CourseGrade.Gradeid = UpdateCourseGradeModel.gradeid;
            CourseGrade.LastModificationTime = DateTime.Now.ToString();
            CourseGrade.LastModifierUserId = id;

            if (EFCourseGradeRepository.Update(CourseGrade) == 1)
            {
                return EFCourseGradeRepository.GetById(b => b.Id == CourseGrade.Id);
            }
            else
            {
                return null;
            }
        }

        public List<CourseGrade> CourseGradeList(BundleCoursePaginationModel paginationModel, out int total)
        {
            List<CourseGrade> CourseGradeList = new List<CourseGrade>();
            CourseGradeList = EFCourseGradeRepository.ListQuery(b=>b.Gradeid == paginationModel.bundleid && b.IsDeleted != true).ToList();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = CourseGradeList.Count();

                CourseGradeList = CourseGradeList.OrderByDescending(b => b.CreationTime).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();
                return CourseGradeList;
            }

            total = CourseGradeList.Count();
            return CourseGradeList;
        }

        public CourseGrade getCourseGradeById(long id)
        {
            return EFCourseGradeRepository.GetById(b => b.Id == id);
        }

        public CourseGrade Delete(int Id, int DeleterId)
        {
            CourseGrade CourseGrade = new CourseGrade();
            CourseGrade.Id = Id;
            CourseGrade.DeleterUserId = DeleterId;
            int i = EFCourseGradeRepository.Delete(CourseGrade);
            CourseGrade deletedCourseGrade = EFCourseGradeRepository.GetById(b => b.Id == Id);
            return deletedCourseGrade;
        }

        public CourseGrade GetGradeByCourseId(long Id)
        {
            return EFCourseGradeRepository.GetById(b => b.CourseId == Id);
        }
    }
}
