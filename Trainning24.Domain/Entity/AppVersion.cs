using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class AppVersion : EntityBase
    {
        public string Version { get; set; }
        public bool IsForceUpdate { get; set; }
    }
}
