using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Bundle;
using Trainning24.BL.ViewModels.BundleCourse;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class BundleBusiness
    {
        private readonly EFBundleRepository EFBundleRepository;
        private readonly EFBundleCourseCourseRepository EFBundleCourseCourseRepository;

        public BundleBusiness
        (
            EFBundleRepository EFBundleRepository,
            EFBundleCourseCourseRepository EFBundleCourseCourseRepository
        )
        {
            this.EFBundleRepository = EFBundleRepository;
            this.EFBundleCourseCourseRepository = EFBundleCourseCourseRepository;
        }

        public Bundle Create(AddBundleModel AddBundleModel,int id)
        {
            Bundle bundle = new Bundle
            {
                Name = AddBundleModel.Name,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };

            EFBundleRepository.Insert(bundle);

            return bundle;
        }

        public Bundle Update(UpdateBundleModel UpdateBundleModel, int id)
        {
            Bundle bundle = new Bundle
            {
                Id = UpdateBundleModel.Id,
                Name = UpdateBundleModel.Name,                                                                
                LastModificationTime = DateTime.Now.ToString(),
                LastModifierUserId = id
            };

            EFBundleRepository.Update(bundle);

            return bundle;
        }

        public List<Bundle> BundleList(PaginationModel paginationModel, out int total)
        {
            List<Bundle> bundleList = new List<Bundle>();
            bundleList = EFBundleRepository.GetAll();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = bundleList.Count();

                if (!string.IsNullOrEmpty(paginationModel.search)) {
                    bundleList = bundleList.Where(b => b.Name.Any(k => b.Id.ToString().Contains(paginationModel.search)
                                                || b.Name.ToLower().Contains(paginationModel.search.ToLower())
                                                 )).ToList();

                    total = bundleList.Count();

                    return bundleList;
                }


                bundleList = bundleList.OrderByDescending(b => b.CreationTime).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();


                return bundleList;
            }

            total = bundleList.Count();
            return EFBundleRepository.GetAll();
        }

        public Bundle getBundleById(int id)
        {
            return EFBundleRepository.GetById(id);
        }


        public Bundle Delete(int Id,int DeleterId) {
            Bundle bundle = new Bundle();
            bundle.Id = Id;
            bundle.DeleterUserId = DeleterId;
            int i = EFBundleRepository.Delete(bundle);
            return bundle;
        }



        public BundleCourse CreateBundleCourse(AddBundleCourseModel AddBundleCourseModel, int id)
        {
            BundleCourse BundleCourse = new BundleCourse
            {
                BundleId = AddBundleCourseModel.bundleid,
                CourseId = AddBundleCourseModel.courseid,
                IsDeleted = false,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id
            };

            List<BundleCourse> BundleCourses = EFBundleCourseCourseRepository.
                                                ListQuery1(b => b.BundleId == AddBundleCourseModel.bundleid 
                                                && b.CourseId == AddBundleCourseModel.courseid && b.IsDeleted == false);

            if (BundleCourses.Count == 0)
            {
                EFBundleCourseCourseRepository.Insert(BundleCourse);
            }
            else {
                BundleCourse = null;
            }
          

            return BundleCourse;
        }
        
        public BundleCourse DeleteBundleCourse(int Id,int DeleterId) {
            BundleCourse bundle = new BundleCourse();
            bundle.Id = Id;
            bundle.DeleterUserId = DeleterId;
            int i = EFBundleCourseCourseRepository.Delete(bundle);
            return bundle;
        }

        public List<BundleCourse> BundleCourseList(PaginationModel paginationModel, out int total)
        {
            List<BundleCourse> bundleList = new List<BundleCourse>();
            bundleList = EFBundleCourseCourseRepository.GetAll();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                total = bundleList.Count();

                bundleList = bundleList.OrderByDescending(b => b.CreationTime).
                        Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();
                return bundleList;
            }

            total = bundleList.Count();
            return EFBundleCourseCourseRepository.GetAll();
        }

        public BundleCourse getBundleCourseById(int id)
        {
            return EFBundleCourseCourseRepository.GetById(id);
        }

        public List<BundleCourse> getCourseByBundleId(BundleCoursePaginationModel BundleCoursePaginationModel , out int total)
        {
            List<BundleCourse> bundleList = new List<BundleCourse>();
            bundleList = EFBundleCourseCourseRepository.ListQuery1(b => b.BundleId == BundleCoursePaginationModel.bundleid && b.IsDeleted != true);

            if (BundleCoursePaginationModel.pagenumber != 0 && BundleCoursePaginationModel.perpagerecord != 0)
            {
                total = bundleList.Count();

                bundleList = bundleList.OrderByDescending(b => b.CreationTime).
                        Skip(BundleCoursePaginationModel.perpagerecord * (BundleCoursePaginationModel.pagenumber - 1)).
                        Take(BundleCoursePaginationModel.perpagerecord).
                        ToList();
                return bundleList;
            }

            total = bundleList.Count();

            return bundleList;
        }
    }
}
