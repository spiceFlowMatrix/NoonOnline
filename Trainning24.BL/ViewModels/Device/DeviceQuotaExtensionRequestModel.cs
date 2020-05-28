using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Device
{
    public class DeviceQuotaExtensionRequestModel
    {
        public long UserId { get; set; }
        public string RequestedOn { get; set; }
        public long RequestedLimit { get; set; }
        public string Status { get; set; }
        public string ApprovedOn { get; set; }
        public string RejectedOn { get; set; }
    }
}
