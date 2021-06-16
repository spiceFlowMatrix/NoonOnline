using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class AddtionalServicesBusiness
    {
        private readonly EFAddtionalServicesRepository _EFAddtionalServicesRepository;
        private readonly LogObjectBusiness _logObjectBusiness;
        public AddtionalServicesBusiness(
            EFAddtionalServicesRepository EFAddtionalServicesRepository,
            LogObjectBusiness logObjectBusiness
        )
        {
            _EFAddtionalServicesRepository = EFAddtionalServicesRepository;
            _logObjectBusiness = logObjectBusiness;
        }

        public AddtionalServices GetById(long Id)
        {
            return _EFAddtionalServicesRepository.GetById(b => b.Id == Id && b.DeletionTime == null);
        }

        public List<AddtionalServices> GetServicesList(PaginationModel paginationModel)
        {
            var data = _EFAddtionalServicesRepository.ListQuery(p => p.DeletionTime == null).ToList();

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

        public AddtionalServices AddService(AddtionalServices obj)
        {
            _EFAddtionalServicesRepository.Insert(obj);
            _logObjectBusiness.AddLogsObject(37, obj.Id, obj.CreatorUserId ?? 0);
            return obj;
        }

        public AddtionalServices UpdateService(AddtionalServices obj)
        {
            var service = GetById(obj.Id);
            if (service != null)
            {
                service.Name = obj.Name;
                service.Price = obj.Price;
                service.LastModificationTime = obj.LastModificationTime;
                service.LastModifierUserId = obj.LastModifierUserId;
                _EFAddtionalServicesRepository.Update(service);
                _logObjectBusiness.AddLogsObject(38, service.Id, service.LastModifierUserId ?? 0);
            }
            return service;
        }

        public int DeleteService(AddtionalServices obj)
        {
            var data = GetById(obj.Id);
            if (data != null)
            {
                data.IsDeleted = true;
                data.DeletionTime = obj.DeletionTime;
                data.DeleterUserId = obj.DeleterUserId;
                _EFAddtionalServicesRepository.Update(data);
                _logObjectBusiness.AddLogsObject(39, data.Id, data.DeleterUserId ?? 0);
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }
}
