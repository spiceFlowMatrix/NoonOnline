using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Device
{
    public class ResponceDeviceModel
    {
       
        public List<DeviceModel> devicesModel { get; set; }
        public int deviceLimit { get; set; }
        public int currentConsumption { get; set; }
    }
}
