using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.AppVersion;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business
{
    public class AppVersionBusiness
    {
        private readonly EFAppVersionRepository _eFDeviceRepository;

        public AppVersionBusiness(EFAppVersionRepository EFDeviceRepository)
        {
            _eFDeviceRepository = EFDeviceRepository;
        }
        public AppVersionResponseModel GetappVersion()
        {
            AppVersionResponseModel appVersionModel = new AppVersionResponseModel();

            var appVersion = _eFDeviceRepository.ListQuery(b => b.IsDeleted != true).FirstOrDefault();
            if (appVersion != null)
            {
                appVersionModel.IsForceUpdate = appVersion.IsForceUpdate;
                appVersionModel.Version = appVersion.Version;
            }
            return appVersionModel;
        }
        public AppVersion updateAppVersion(AppVersionResponseModel obj)
        {
            var appVersion = _eFDeviceRepository.ListQuery(b => b.IsDeleted != true).FirstOrDefault();
            if (appVersion != null)
            {
                appVersion.IsForceUpdate = obj.IsForceUpdate;
                appVersion.Version = obj.Version;
                _eFDeviceRepository.Update(appVersion);
            }
            else
                return appVersion;
            return appVersion;
        }
    }
}
