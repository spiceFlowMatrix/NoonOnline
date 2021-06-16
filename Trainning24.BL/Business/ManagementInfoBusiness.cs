using System;
using System.Net;
using System.Net.Http;
using System.Web.Helpers;
using Trainning24.Abstract.Entity;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;
using Trainning24.BL.ViewModels.Users;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Globalization;
using Trainning24.BL.ViewModels.UserRole;

namespace Trainning24.BL.Business
{
    public class ManagementInfoBusiness
    {
        private readonly EFManagementInfoRepository _EFManagementInfoRepository;

        public ManagementInfoBusiness
        (
            EFManagementInfoRepository efManagementInfoRepository
        )
        {
            _EFManagementInfoRepository = efManagementInfoRepository;
        }
        public ManagementInfo GetAll()
        {
            var data = _EFManagementInfoRepository.GetSingle();
            return data;
        }

        public ManagementInfo CreateUpdate(ManagementInfo obj, string UserId)
        {
            var data = _EFManagementInfoRepository.GetSingle();
            if (data == null)
            {
                obj.CreatorUserId = int.Parse(UserId);
                obj.CreationTime = DateTime.Now.ToString();
                var mnIn = _EFManagementInfoRepository.Insert(obj);
                return obj;
            }
            else
            {
                data.LastModificationTime = DateTime.Now.ToString();
                data.LastModifierUserId = int.Parse(UserId);
                data.noon_background = obj.noon_background;
                data.sales_partner_dari = obj.sales_partner_dari;
                data.sales_partner_eng = obj.sales_partner_eng;
                data.school_receipt_notes = obj.school_receipt_notes;
                data.individual_receipt_notes = obj.individual_receipt_notes;
                var mnIn = _EFManagementInfoRepository.Update(data);
                return obj;
            }
        }
    }
}
