using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Device
{
    public class ResponseUserDeviceModel
    {
        public int deviceLimit { get; set; }
        public int currentConsumption { get; set; }
        public long userId { get; set; }
        public string username { get; set; }
        public string email { get; set; }
    }

    public class ResponseDeviceModel
    {
        public long id { get; set; }
        public string approvedOn { get; set; }
        public string modelName { get; set; }
        public string modelNumber { get; set; }
        public bool? IsActive { get; set; }
        public string operatingSystem { get; set; }
    }
}
