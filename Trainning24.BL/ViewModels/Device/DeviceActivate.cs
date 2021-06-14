using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Device
{
    public class DeviceActivate
    {
        public long id { get; set; }
        public string macAddress { get; set; }
        public string ipAddress { get; set; }
        public string modelName { get; set; }
        public string modelNumber { get; set; }
        public DeviceOperatingSystem operatingSystem { get; set; }
        public List<DeviceTag> tags { get; set; }
    }
}
