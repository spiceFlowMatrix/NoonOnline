using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.Device;
using Trainning24.BL.ViewModels.Users;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;
using Trainning24.Repository.EF.Device;

namespace Trainning24.BL.Business.Device
{
    public class DeviceQuotasBusiness
    {
        private readonly EFDeviceQuotaRepository _eFDeviceQuotaRepository;
        private readonly EFDeviceQuotaExtensionRequestRepository _eFDeviceQuotaExtensionRequestRepository;
        private readonly EFUsersRepository _EFUsersRepository;

        public DeviceQuotasBusiness(EFDeviceQuotaRepository eFDeviceQuotaRepository,
             EFDeviceQuotaExtensionRequestRepository eFDeviceQuotaExtensionRequestRepository,
              EFUsersRepository eFUsersRepository)
        {

            _eFDeviceQuotaRepository = eFDeviceQuotaRepository;
            _eFDeviceQuotaExtensionRequestRepository = eFDeviceQuotaExtensionRequestRepository;
            _EFUsersRepository = eFUsersRepository;
        }

        /// <summary>
        /// Register device quota for user and activate new device. 
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public int ExtensionRequest(int userId, int requestedLimit)
        {
            DeviceQuotaExtensionRequest deviceQuotaExtensionRequest = new DeviceQuotaExtensionRequest()
            {
                RequestedLimit = requestedLimit,
                Status = "requested",
                UserId = userId,
                RequestedOn = DateTime.Now.ToString(),
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = userId,
                IsDeleted = false,
            };
            return _eFDeviceQuotaExtensionRequestRepository.Insert(deviceQuotaExtensionRequest);
        }

        /// <summary>
        /// Get ExtensionRequest details by Id.
        /// </summary>
        /// <param name="extensionRequestId"></param>
        /// <returns></returns>
        public DeviceQuotaExtensionRequest GetbyId(int extensionRequestId)
        {
            return _eFDeviceQuotaExtensionRequestRepository.GetById(b => b.Id == extensionRequestId);
        }

        /// <summary>
        /// Get ExtensionRequest details by userId which is not aprooved.
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public DeviceQuotaExtensionRequest GetbyUserId(int UserId)
        {
            return _eFDeviceQuotaExtensionRequestRepository.GetById(b => b.UserId == UserId && b.ApprovedOn == null);
        }

        /// <summary>
        /// Get add Device QuotaExtension request List with filters text, date and pagination.
        /// </summary>
        /// <param name="paginationModel"></param>
        /// <param name="total"></param>
        /// <returns></returns>
        public List<ResponseDeviceQuotaExtension> QuotaExtensionList(DeviceQuotaExtensionFilterModel paginationModel, out int total)
        {
            List<ResponseDeviceQuotaExtension> deviceQuotaExtensionslist = new List<ResponseDeviceQuotaExtension>();
            var usersList = _EFUsersRepository.GetAll();
            var deviceQuotaList = _eFDeviceQuotaRepository.GetAll();
            var DQExtensionRequestList = _eFDeviceQuotaExtensionRequestRepository.ListQuery(b => b.ApprovedOn == null);

            deviceQuotaExtensionslist = (from dqr in DQExtensionRequestList
                                         join dq in deviceQuotaList on dqr.UserId equals dq.UserId
                                         join x in usersList on dqr.UserId equals x.Id
                                         select new ResponseDeviceQuotaExtension
                                         {
                                             Id = dqr.Id,
                                             RequestedLimit = dqr.RequestedLimit,
                                             RequestedOn = dqr.RequestedOn,
                                             Status = dqr.Status,
                                             CurrentQuotaLimit = dq.DeviceLimit,
                                             UserId = x.Id,
                                             email = x.Email,
                                             username = x.Username,
                                         }).ToList();
            total = deviceQuotaExtensionslist != null ? deviceQuotaExtensionslist.Count() : 0;
            if (!string.IsNullOrEmpty(paginationModel.fromdate) && !string.IsNullOrEmpty(paginationModel.todate))
                deviceQuotaExtensionslist = deviceQuotaExtensionslist.FindAll(
                    b => DateTime.ParseExact(b.RequestedOn, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture) >= DateTime.ParseExact(paginationModel.fromdate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture)
                    && DateTime.ParseExact(b.RequestedOn, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture) <= DateTime.ParseExact(paginationModel.todate, "yyyy-MM-dd'T'HH:mm:ss.fff'Z'", CultureInfo.InvariantCulture));

            if (!string.IsNullOrEmpty(paginationModel.search))
                deviceQuotaExtensionslist = deviceQuotaExtensionslist.Where(b => b.Id.ToString().Any(k => b.Id.ToString().Contains(paginationModel.search)
                                                            || b.UserId.ToString().Contains(paginationModel.search)
                                                            || b.RequestedLimit.ToString().Contains(paginationModel.search)
                                                            || b.CurrentQuotaLimit.ToString().Contains(paginationModel.search)
                                                            || (b.email != null && b.email.ToLower().Contains(paginationModel.search.ToLower()))
                                                            || (b.username != null && b.username.ToLower().Contains(paginationModel.search.ToLower()))
                                                            || b.Status.ToLower().Contains(paginationModel.search.ToLower())
                                                           )).ToList();

            if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
                deviceQuotaExtensionslist = deviceQuotaExtensionslist.Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                Take(paginationModel.perpagerecord).
                ToList();

            return deviceQuotaExtensionslist;
        }

        /// <summary>
        /// ExtensionRequest will approve  or reject.
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="IsAccepted"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public int ExtensionRequestChangeStatus(DeviceQuotaExtensionRequest obj, bool IsAccepted, int userId)
        {
            if (IsAccepted)
            {
                obj.Status = "accepted";
                obj.LastModifierUserId = userId;
                obj.ApprovedOn = DateTime.Now.ToString();
                obj.LastModificationTime = DateTime.Now.ToString();
                _eFDeviceQuotaExtensionRequestRepository.Update(obj);
            
                var DeviceQuota = _eFDeviceQuotaRepository.GetById(i => i.UserId == userId);
                DeviceQuota.DeviceLimit = Convert.ToInt32(obj.RequestedLimit);
                _eFDeviceQuotaRepository.Update(DeviceQuota);
                return _eFDeviceQuotaRepository.Update(DeviceQuota);
            }
            else
            {
                obj.Status = "rejected";
                obj.LastModifierUserId = userId;
                obj.RejectedOn = DateTime.Now.ToString();
                obj.LastModificationTime = DateTime.Now.ToString();
                return _eFDeviceQuotaExtensionRequestRepository.Update(obj);
            }
           
        }

    }
}
