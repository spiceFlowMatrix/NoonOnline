using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Device;

namespace Trainning24.BL.ViewModels
{
    public class DeviceQuotas
    {
        public long id { get; set; }
        public string requestedOn { get; set; }
        public long requestedLimit { get; set; }
        public string status { get; set; }
        public string approvedOn { get; set; }
        public string rejectedOn { get; set; }
        public DeviceUser user { get; set; }
    }
}
