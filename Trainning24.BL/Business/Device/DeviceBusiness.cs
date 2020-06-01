using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
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
            deviceData.CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
            deviceData.CreatorUserId = id;
            deviceData.IsDeleted = false;
            _eFDeviceRepository.Insert(deviceData);

            DeviceOS deviceOS = new DeviceOS()
            {
                DeviceId = deviceData.Id,
                Name = objData.operatingSystem.name,
                Version = objData.operatingSystem.version,
                CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
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
                        CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
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
                device.LastModificationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture);
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
                devices.DeletionTime = devices.IsDeleted != true ? DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture) : null;
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
                    CreationTime = DateTime.UtcNow.ToString("MM/dd/yyyy hh:mm:ss tt", CultureInfo.InvariantCulture),
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
        public Devices GetDevicesByMacAdd(string macAdd, int userId)
        {

            return _eFDeviceRepository.GetById(b => b.MacAddress == macAdd && b.UserId == userId);
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
        /// All device with device status
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<ResponseDeviceModel> GetUserDeviceByUserId(int userId)
        {
            List<ResponseDeviceModel> responceDeviceModel = new List<ResponseDeviceModel>();

            var devices = _eFDeviceRepository.ListQuery(b => b.UserId == userId).ToList();
            if (devices != null)
            {
                var deiviceIds = devices.Select(s => s.Id).ToList();
                var deviceOS = _eFDeviceOSRepository.ListQuery(b => deiviceIds.Contains(b.DeviceId)).ToList();

               responceDeviceModel = (from x in devices
                                    join n in deviceOS on x.Id equals n.DeviceId
                                    select new ResponseDeviceModel
                                    {
                                        id = x.Id,
                                        approvedOn = x.CreationTime,
                                        modelName = x.ModelName,
                                        modelNumber = x.ModelNumber,
                                        IsActive = !x.IsDeleted,
                                        operatingSystem = n.Name
                                    }).ToList();

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
        public ResponseUserDeviceModel GetAllDeviceUserWise(DeviceQuotaExtensionFilterModel paginationModel, out int total)
        {
            ResponseUserDeviceModel responceUserDeviceModel = new ResponseUserDeviceModel();
            List<UserDeviceModel> userDeviceModels = new List<UserDeviceModel>();
            DBHelper dbHelper = new DBHelper(getconnectionstring());
            try
            {
                dbHelper.Open();
                string query = null; string usreidfilter = null; string pagenation = null; string search = null;


                //if (paginationModel.userId != 0)
                //    usreidfilter = " && u.Id = " + paginationModel.userId;
                //if (pagenation != null) query += pagenation;

                if (paginationModel.search != null)
                    search = " AND ( LOWER(u.username) Like LOWER('%" + paginationModel.search + "%') OR LOWER(u.email) Like 'LOWER(%" + paginationModel.search + "%') OR LOWER(u.email) Like LOWER('%" + paginationModel.search + "%'))";
                if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
                    pagenation = " LIMIT " + paginationModel.perpagerecord * (paginationModel.pagenumber - 1) + ", " + paginationModel.perpagerecord;

                query = "SELECT u.Id,u.fullname,u.email,u.username,dq.DeviceLimit ,(SELECT count(*) from devices as d where  d.UserId = u.Id AND (d.IsDeleted!=true OR d.IsDeleted Is Null) ) as currentConsumption FROM `devicequotas` as dq Left JOIN `users` as u ON dq.UserId = u.Id  WHERE (dq.IsDeleted != true OR dq.IsDeleted Is Null) AND (u.IsDeleted != true OR u.IsDeleted Is Null)";
                if (usreidfilter != null) query += usreidfilter; if (search != null) query += search; 
                DataTable userDevicelist = dbHelper.ExcecuteQueryDT(query);
                query = "SELECT u.Id,u.fullname,u.email,u.username,dq.DeviceLimit ,(SELECT count(*) from devices as d where  d.UserId = u.Id AND (d.IsDeleted!=true OR d.IsDeleted Is Null) ) as currentConsumption FROM `devicequotas` as dq Left JOIN `users` as u ON dq.UserId = u.Id  WHERE (dq.IsDeleted != true OR dq.IsDeleted Is Null) AND (u.IsDeleted != true OR u.IsDeleted Is Null)";
                DataTable userEmaillist = dbHelper.ExcecuteQueryDT(query);
                dbHelper.Close();
                if (userDevicelist.Rows.Count != 0)
                {
                    foreach (DataRow userDevice in userDevicelist.Rows)
                    {
                        UserDeviceModel UserDeviceModel = new UserDeviceModel();
                        UserDeviceModel.deviceLimit = Convert.ToInt32(userDevice["DeviceLimit"].ToString());
                        UserDeviceModel.currentConsumption = Convert.ToInt32(userDevice["currentConsumption"].ToString());
                        UserDeviceModel.userId = Convert.ToInt64(userDevice["Id"].ToString());
                        UserDeviceModel.username = userDevice["username"].ToString();
                        UserDeviceModel.email = userDevice["email"].ToString();
                        userDeviceModels.Add(UserDeviceModel);
                    }
                    List<UserEmail> userEmaillst = new List<UserEmail>();
                    foreach (DataRow userDevice in userEmaillist.Rows)
                    {
                        UserEmail userEmail = new UserEmail { Id = Convert.ToInt64(userDevice["Id"].ToString()), email = userDevice["email"].ToString() };
                       userEmaillst.Add(userEmail);

                    }
                    responceUserDeviceModel.userEmails = userEmaillst;
                    total = userDeviceModels.Count();
                    if (paginationModel.pagenumber != 0 && paginationModel.perpagerecord != 0)
                        userDeviceModels = userDeviceModels.Skip(paginationModel.perpagerecord * (paginationModel.pagenumber - 1)).
                        Take(paginationModel.perpagerecord).
                        ToList();
                    responceUserDeviceModel.userDeviceModels = userDeviceModels;
                    return responceUserDeviceModel;
                }
                else
                {
                    total = 0;
                    return null;

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


        }
        private string getconnectionstring()
        {
            return Environment.GetEnvironmentVariable("ASPNET_DB_CONNECTIONSTRING");
        }


    }

}
