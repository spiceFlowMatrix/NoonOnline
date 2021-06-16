using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class PackageBusiness
    {
        private readonly EFPackageRepository _EFPackageRepository;
        private readonly LogObjectBusiness _logObjectBusiness;

        public PackageBusiness(
            EFPackageRepository EFPackageRepository,
            LogObjectBusiness logObjectBusiness
        )
        {
            _EFPackageRepository = EFPackageRepository;
            _logObjectBusiness = logObjectBusiness;
        }

        public Package GetById(long Id)
        {
            return _EFPackageRepository.GetById(b => b.Id == Id && b.DeletionTime == null);
        }

        public List<Package> GetPackageList(PaginationModel paginationModel, out int total)
        {
            var data = _EFPackageRepository.ListQuery(p => p.DeletionTime == null).ToList();
            total = data.Count();
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

        public Package AddPackage(Package obj)
        {
            _EFPackageRepository.Insert(obj);
            _logObjectBusiness.AddLogsObject(40, obj.Id, obj.CreatorUserId ?? 0);
            return obj;
        }

        public Package UpdatePackage(Package obj)
        {
            var package = GetById(obj.Id);
            if (package != null)
            {
                package.Name = obj.Name;
                package.Price = obj.Price;
                package.LastModificationTime = obj.LastModificationTime;
                package.LastModifierUserId = obj.LastModifierUserId;
                _EFPackageRepository.Update(package);
                _logObjectBusiness.AddLogsObject(41, package.Id, package.LastModifierUserId ?? 0);
            }
            return package;
        }

        public int DeletePackage(Package obj)
        {
            var data = GetById(obj.Id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeletionTime = obj.DeletionTime;
                data.DeleterUserId = obj.DeleterUserId;
                _EFPackageRepository.Update(data);
                _logObjectBusiness.AddLogsObject(42, data.Id, data.DeleterUserId ?? 0);
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
