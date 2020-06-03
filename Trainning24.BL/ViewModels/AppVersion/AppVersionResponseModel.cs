using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.AppVersion
{
    public class AppVersionResponseModel
    {
        public string Version { get; set; }
        public string VersionCode { get; set; }
        public bool IsForceUpdate { get; set; }
    }
}
