using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class Devices : EntityBase
    {
        public string DeviceToken { get; set; }
        public string ModelName { get; set; }
        public string ModelNumber { get; set; }
        public string MacAddress { get; set; }
        public string IpAddress { get; set; }
        public long UserId { get; set; }
    }
}
