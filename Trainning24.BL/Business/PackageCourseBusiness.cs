using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class PackageCourseBusiness
    {
        private readonly EFPackageCourseRepository _EFPackageCourseRepository;
        private readonly LogObjectBusiness _logObjectBusiness;

        public PackageCourseBusiness(
            EFPackageCourseRepository EFPackageCourseRepository,
            LogObjectBusiness logObjectBusiness
        )
        {
            _EFPackageCourseRepository = EFPackageCourseRepository;
            _logObjectBusiness = logObjectBusiness;
        }

        public PackageCourse GetById(long Id)
        {
            return _EFPackageCourseRepository.GetById(b => b.Id == Id && b.DeletionTime == null);
        }

        public bool CheckDuplicateEntry(long packageid, long courseid)
        {
            var data = _EFPackageCourseRepository.GetById(b => b.PackageId == packageid && b.CourseId == courseid && b.DeletionTime == null);
            if (data != null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public List<PackageCourse> GetPackageCourseList(int id, PaginationModel paginationModel)
        {
            var data = _EFPackageCourseRepository.ListQuery(p => p.PackageId == id && p.DeletionTime == null).ToList();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
            {
                data = data.OrderByDescending(b => b.Id).
                            Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                            Take(paginationModel.perpagerecord).
                            ToList();
                return data;
            }
            return data;
        }

        public PackageCourse AddPackageCourse(PackageCourse obj)
        {
            _EFPackageCourseRepository.Insert(obj);
            _logObjectBusiness.AddLogsObject(43, obj.Id, obj.CreatorUserId ?? 0);
            return obj;
        }

        public PackageCourse UpdatePackageCourse(PackageCourse obj)
        {
            var package = GetById(obj.Id);
            if (package != null)
            {
                package.PackageId = obj.PackageId;
                package.CourseId = obj.CourseId;
                package.LastModificationTime = obj.LastModificationTime;
                package.LastModifierUserId = obj.LastModifierUserId;
                _EFPackageCourseRepository.Update(package);
                _logObjectBusiness.AddLogsObject(44, package.Id, package.LastModifierUserId ?? 0);
            }
            return package;
        }

        public int DeletePackageCourse(PackageCourse obj)
        {
            var data = GetById(obj.Id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeletionTime = obj.DeletionTime;
                data.DeleterUserId = obj.DeleterUserId;
                _EFPackageCourseRepository.Update(data);
                _logObjectBusiness.AddLogsObject(45, data.Id, data.DeleterUserId ?? 0);
                return 1;
            }
            else
            {
                return 0;
            }
        }

        public List<PackageCourse> GetPackageCourseListByPackageId(long Id)
        {
            return _EFPackageCourseRepository.ListQuery(p => p.PackageId == Id && p.DeletionTime == null).ToList();
        }
    }
}
