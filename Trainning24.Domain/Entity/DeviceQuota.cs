using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class DeviceQuotas : EntityBase
    {
        public long UserId { get; set; }
        public int  DeviceLimit { get; set; }
    }
}
