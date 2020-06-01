using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Device
{
    public class ResponseDeviceQuotaExtension
    {
        public long Id { get; set; }
        public string RequestedOn { get; set; }
        public long RequestedLimit { get; set; }
        public long CurrentQuotaLimit { get; set; }
        public string Status { get; set; }
        public long UserId { get; set; }
        public string username { get; set; }
        public string email { get; set; }
    }

    public class DeviceQuotaExtensionFilterModel
    {
        public int pagenumber { get; set; }
        public int perpagerecord { get; set; }
        public string search { get; set; }
        public string fromdate { get; set; }
        public string todate { get; set; }
       
    }
}
