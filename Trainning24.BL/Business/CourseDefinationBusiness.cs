using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class CourseDefinationBusiness
    {

        private readonly EFCourseDefinationRepository EFCourseDefinationRepository;

        public CourseDefinationBusiness(
            EFCourseDefinationRepository EFCourseDefinationRepository
        )
        {
            this.EFCourseDefinationRepository = EFCourseDefinationRepository;
        }


        public CourseDefination Create(CourseDefination CourseDefination, int id)
        {
            CourseDefination courseDefination = EFCourseDefinationRepository.ListQuery(b => b.Subject == CourseDefination.Subject).SingleOrDefault();

            if (courseDefination != null)
            {
                return null;
            }

            CourseDefination.CreationTime = DateTime.Now.ToString();
            CourseDefination.CreatorUserId = id;
            CourseDefination.IsDeleted = false;
            EFCourseDefinationRepository.Update(CourseDefination);
            return EFCourseDefinationRepository.GetById(b => b.Id == CourseDefination.Id);
        }


        public CourseDefination Update(CourseDefination CourseDefination, int id)
        {

            CourseDefination courseDefination = EFCourseDefinationRepository.ListQuery(b => b.Subject == CourseDefination.Subject && b.Id != CourseDefination.Id).SingleOrDefault();

            if (courseDefination != null)
            {
                return null;
            }


            CourseDefination.BasePrice = CourseDefination.BasePrice;
            CourseDefination.CourseId = CourseDefination.CourseId;
            CourseDefination.GradeId = CourseDefination.GradeId;
            CourseDefination.Subject = CourseDefination.Subject;
            CourseDefination.LastModificationTime = DateTime.Now.ToString();
            CourseDefination.LastModifierUserId = id;
            EFCourseDefinationRepository.Update(CourseDefination);
            return EFCourseDefinationRepository.GetById(b => b.Id == CourseDefination.Id);
        }

        public List<CourseDefination> CourseDefinationList(PaginationModel paginationModel, out int total)
        {
            List<CourseDefination> CourseDefinationList = new List<CourseDefination>();
            CourseDefinationList = EFCourseDefinationRepository.GetAll().OrderByDescending(b => b.Id).ToList(); ;

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = CourseDefinationList.Count();

                CourseDefinationList = CourseDefinationList.OrderByDescending(b => b.Id).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();

                if (!string.IsNullOrEmpty(paginationModel.search))
                    CourseDefinationList = CourseDefinationList.OrderByDescending(b => b.Id).Where(
                                                       b => b.Id.ToString().Any(k => b.Id.ToString().Contains(paginationModel.search))
                                                    || b.Subject.ToString().ToLower().Any(k => b.Subject.ToString().Contains(paginationModel.search))
                                                    || b.BasePrice.ToString().ToLower().Any(k => b.BasePrice.ToString().Contains(paginationModel.search))
                                                    ).ToList();

                return CourseDefinationList;
            }

            total = CourseDefinationList.Count();
            return CourseDefinationList;
        }

        public CourseDefination getCourseDefinationById(long id)
        {
            return EFCourseDefinationRepository.GetById(b => b.Id == id && b.IsDeleted != true);
        }

        public CourseDefination getCourseDefinationForAgentSummary(long id)
        {
            return EFCourseDefinationRepository.GetById(b => b.Id == id);
        }

        public CourseDefination Delete(int Id, int DeleterId)
        {
            CourseDefination CourseDefination = new CourseDefination();
            CourseDefination.Id = Id;
            CourseDefination.DeleterUserId = DeleterId;
            EFCourseDefinationRepository.Delete(CourseDefination);
            CourseDefination deletedCourseDefination = EFCourseDefinationRepository.GetById(b => b.Id == Id);
            return deletedCourseDefination;
        }


        public List<CourseDefination> GetSubjectList(string search)
        {
            List<CourseDefination> CourseDefinationList = new List<CourseDefination>();
            CourseDefinationList = EFCourseDefinationRepository.GetAll().ToList();
            if (!string.IsNullOrEmpty(search))
            {
                CourseDefinationList = CourseDefinationList.Where(b=> b.Subject.ToLower().Contains(search)).ToList();
                return CourseDefinationList;
            }
            return CourseDefinationList;
        }

    }
}
