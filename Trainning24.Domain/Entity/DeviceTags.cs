using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class DeviceTags : EntityBase
    {
        public long DeviceId { get; set; }
        public string Name { get; set; }
    }
}
