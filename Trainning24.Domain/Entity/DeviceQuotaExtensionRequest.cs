﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class DeviceQuotaExtensionRequest : EntityBase
    {
        public long UserId { get; set; }
        public string RequestedOn { get; set; }
        public long RequestedLimit { get; set; }
        public string Status { get; set; }
        public string ApprovedOn { get; set; }
        public string RejectedOn { get; set; }
    }
}
