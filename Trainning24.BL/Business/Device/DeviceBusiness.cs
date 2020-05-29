using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using Trainning24.BL.ViewModels.Device;
using Trainning24.Domain.Entity;
using Trainning24.Domain.Helper;
using Trainning24.Repository.EF;
using Trainning24.Repository.EF.Device;

namespace Trainning24.BL.Business.Device
{
    public class DeviceBusiness
    {
        private readonly EFDeviceRepository _eFDeviceRepository;
        private readonly EFDeviceOSRepository _eFDeviceOSRepository;
        private readonly EFDeviceTagsRepository _eFDeviceTagsRepository;
        private readonly EFDeviceQuotaRepository _eFDeviceQuotaRepository;
        private readonly DeviceQuotasBusiness DeviceQuotasBusiness;


        public DeviceBusiness(EFDeviceRepository eFDeviceRepository, EFDeviceOSRepository eFDeviceOSRepository, EFDeviceTagsRepository eFDeviceTagsRepository, EFDeviceQuotaRepository eFDeviceQuotaRepository, DeviceQuotasBusiness deviceQuotasBusiness)
        {
            _eFDeviceRepository = eFDeviceRepository;
            _eFDeviceOSRepository = eFDeviceOSRepository;
            _eFDeviceTagsRepository = eFDeviceTagsRepository;
            _eFDeviceQuotaRepository = eFDeviceQuotaRepository;
            this.DeviceQuotasBusiness = deviceQuotasBusiness;
        }

        public List<Devices> GetAll(int UserId)
        {
            return _eFDeviceRepository.ListQuery(b => b.UserId == UserId).ToList();
        }
        /// <summary>
        /// list of device which is active
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public List<Devices> GetAllActiveDeviceByUserId(int UserId)
        {
            return _eFDeviceRepository.ListQuery(b => b.UserId == UserId && b.IsDeleted != true).ToList();
        }

        /// <summary>
        /// Register device quota for user and activate new device. 
        /// </summary>
        /// <param name="objData"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public Devices Create(DeviceActivate objData, int id)
        {
            Devices deviceData = new Devices();

            deviceData.MacAddress = objData.macAddress;
            deviceData.IpAddress = objData.ipAddress;
            deviceData.ModelName = objData.modelName;
            deviceData.ModelNumber = objData.modelNumber;
            deviceData.IpAddress = objData.ipAddress;
            deviceData.UserId = id;
            deviceData.DeviceToken = Guid.NewGuid().ToString();
            deviceData.CreationTime = DateTime.Now.ToString();
            deviceData.CreatorUserId = id;
            deviceData.IsDeleted = false;
            _eFDeviceRepository.Insert(deviceData);

            DeviceOS deviceOS = new DeviceOS()
            {
                DeviceId = deviceData.Id,
                Name = objData.operatingSystem.name,
                Version = objData.operatingSystem.version,
                CreationTime = DateTime.Now.ToString(),
                CreatorUserId = id,
                IsDeleted = false
            };
            _eFDeviceOSRepository.Insert(deviceOS);

            if (objData.tags.Count > 0)
                foreach (var tag in objData.tags)
                {
                    DeviceTags deviceTags = new DeviceTags()
                    {
                        DeviceId = deviceData.Id,
                        Name = tag.name,
                        CreationTime = DateTime.Now.ToString(),
                        CreatorUserId = id,
                        IsDeleted = false
                    };
                    _eFDeviceTagsRepository.Insert(deviceTags);
                }
            return deviceData;
        }

        public Devices Update(Devices obj, int id)
        {
            Devices device = _eFDeviceRepository.GetById(b => b.Id == obj.Id);
            if (device != null)
            {
                device.MacAddress = obj.MacAddress;
                device.IpAddress = obj.IpAddress;
                device.UserId = obj.UserId;
                device.LastModificationTime = DateTime.Now.ToString();
                device.LastModifierUserId = id;
                _eFDeviceRepository.Update(device);
                return device;
            }
            return null;
        }

        /// <summary>
        ///  Active/Deactivate device 
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <param name="deviceId">Id of device</param>
        /// <returns></returns> 
        public int activeDeactiveDevice(int UserId, int deviceId)
        {
            Devices devices = _eFDeviceRepository.GetById(b => b.Id == deviceId && b.UserId == UserId);
            if (devices != null)
            {
                devices.IsDeleted = devices.IsDeleted != true;
                devices.DeleterUserId = UserId;
                devices.DeletionTime = devices.IsDeleted != true ? DateTime.Now.ToString() : null;
                _eFDeviceRepository.Update(devices);
                if (devices.IsDeleted != true)
                    return 1;
                else return 2;
            }
            return 0;
        }

        /// <summary>
        ///  return device quota limit for user and if user quota is not exist then default quota limit will set 1.
        /// </summary>
        /// <param name="userId">Id of user</param>
        /// <returns>intiger</returns>  
        public int CheckDeviceQuota(int UserId)
        {
            int Quotaconsumption = GetAllActiveDeviceByUserId(UserId).Count();
            var DeviceQuata = _eFDeviceQuotaRepository.GetById(b => b.UserId == UserId);
            if (DeviceQuata != null)
            {
                if (DeviceQuata.DeviceLimit > Quotaconsumption)
                    return DeviceQuata.DeviceLimit;
                else
                    return 0;
            }
            else
            {
                DeviceQuotas deviceQuotas = new DeviceQuotas()
                {
                    DeviceLimit = 1,
                    CreationTime = DateTime.Now.ToString(),
                    CreatorUserId = UserId,
                    UserId = UserId,
                    IsDeleted = false
                };
                _eFDeviceQuotaRepository.Insert(deviceQuotas);
                return deviceQuotas.DeviceLimit;
            }
        }

        /// <summary>
        /// get device by user device mac address.
        /// </summary>
        /// <param name="macAdd">device mac Address</param>
        /// <returns>Devices details</returns>  
        public Devices GetDevicesByMacAdd(string macAdd)
        {
            return _eFDeviceRepository.GetById(b => b.MacAddress == macAdd);
        }

        /// <summary>
        /// All device with device status
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResponceDeviceModel GetAllDeviceByUserId(int userId)
        {
            ResponceDeviceModel responceDeviceModel = new ResponceDeviceModel();

            var devices = _eFDeviceRepository.ListQuery(b => b.UserId == userId).ToList();
            if (devices != null)
            {
                var deiviceIds = devices.Select(s => s.Id).ToList();
                var deviceOS = _eFDeviceOSRepository.ListQuery(b => deiviceIds.Contains(b.DeviceId)).ToList();

                var devicesModel = (from x in devices
                                    join n in deviceOS on x.Id equals n.DeviceId
                                    select new DeviceModel
                                    {
                                        id = x.Id,
                                        deviceToken = x.DeviceToken,
                                        ipAddress = x.IpAddress,
                                        macAddress = x.MacAddress,
                                        modelName = x.ModelName,
                                        modelNumber = x.ModelNumber,
                                        IsActive = !x.IsDeleted,
                                        operatingSystem = new DeviceOperatingSystem() { DeviceId = x.Id, name = n.Name, version = n.Version },
                                        tags = GetDeviceTag(x.Id)
                                    }).ToList();
                responceDeviceModel.devicesModel = devicesModel;
                var extensionRequest = DeviceQuotasBusiness.GetbyUserId(userId);
                responceDeviceModel.IsPendingRequest = extensionRequest != null ? true : false; ;
                var devicequotalimit = _eFDeviceQuotaRepository.GetById(b => b.UserId == userId);
                responceDeviceModel.deviceLimit = devicequotalimit != null ? devicequotalimit.DeviceLimit : 0;
                responceDeviceModel.currentConsumption = devicesModel.Where(b => b.IsActive == true).Count();
            }
            return responceDeviceModel;
        }

        /// <summary>
        /// Get Device tags by Device Id.
        /// </summary>
        /// <param name="deviceId"></param>
        /// <returns></returns>
        public List<DeviceTag> GetDeviceTag(long deviceId)
        {
            var deviceTagsData = _eFDeviceTagsRepository.ListQuery(b => b.DeviceId == deviceId).ToList();
            List<DeviceTag> deviceTags = new List<DeviceTag>();
            foreach (var item in deviceTagsData)
            {
                DeviceTag deviceTag = new DeviceTag();
                deviceTag.id = item.Id;
                deviceTag.name = item.Name;
                deviceTags.Add(deviceTag);
            }
            return deviceTags;
        }

        /// <summary>
        /// All device with device status
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<ResponseUserDeviceModel> GetAllDeviceUserWise()
        {
            List<ResponseUserDeviceModel> responceUserDeviceModel = new List<ResponseUserDeviceModel>();

            DBHelper dbHelper = new DBHelper(getconnectionstring());
            try
            {
                dbHelper.Open();
                DataTable userDevicelist = dbHelper.ExcecuteQueryDT("SELECT u.Id,u.fullname,u.email,u.username,dq.DeviceLimit ,(SELECT count(*) from devices as d where  d.UserId = u.Id AND (d.IsDeleted!=true OR d.IsDeleted Is Null) ) as currentConsumption FROM `devicequotas` as dq Left JOIN `users` as u ON dq.UserId = u.Id  WHERE (dq.IsDeleted != true OR dq.IsDeleted Is Null) && (u.IsDeleted != true OR u.IsDeleted Is Null)");
                dbHelper.Close();
                if (userDevicelist.Rows.Count != 0)
                {
                    foreach (DataRow userDevice in userDevicelist.Rows)
                    {
                        ResponseUserDeviceModel responseUserDeviceModel = new ResponseUserDeviceModel();
                        responseUserDeviceModel.deviceLimit = Convert.ToInt32(userDevice["DeviceLimit"].ToString());
                        responseUserDeviceModel.currentConsumption = Convert.ToInt32(userDevice["currentConsumption "].ToString());
                        responseUserDeviceModel.userId = Convert.ToInt64(userDevice["Id"].ToString());
                        responseUserDeviceModel.username = userDevice["username"].ToString();
                        responseUserDeviceModel.email = userDevice["email"].ToString();
                        responceUserDeviceModel.Add(responseUserDeviceModel);
                    }
                }
            }
            catch (Exception ex)
            {
                dbHelper.Close();
                throw ex;
            }
            finally
            {
                dbHelper.Dispose();
            }


            return responceUserDeviceModel;
        }
        private string getconnectionstring()
        {
            return Environment.GetEnvironmentVariable("ASPNET_DB_CONNECTIONSTRING");
        }
    }

}
