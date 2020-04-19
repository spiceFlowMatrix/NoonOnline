using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Trainning24.Domain.Entity;
using Trainning24.Repository.EF;

namespace Trainning24.BL.Business.Device
{
    public class DeviceBusiness
    {
        private readonly EFDeviceRepository _eFDeviceRepository;

        public DeviceBusiness(EFDeviceRepository eFDeviceRepository)
        {
            _eFDeviceRepository = eFDeviceRepository;
        }

        public List<Devices> GetAll(int UserId)
        {
            return _eFDeviceRepository.ListQuery(b => b.UserId == UserId).ToList();
        }

        public Devices Create(Devices obj, int id)
        {
            obj.CreationTime = DateTime.Now.ToString();
            obj.CreatorUserId = id;
            obj.IsDeleted = false;
            _eFDeviceRepository.Insert(obj);
            return obj;
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

        public int Delete(int UserId)
        {
            Devices devices = _eFDeviceRepository.GetById(b => b.UserId == UserId);
            if (devices != null)
            {
                return _eFDeviceRepository.Delete(devices);
            }
            return 0;
        }
    }
}
