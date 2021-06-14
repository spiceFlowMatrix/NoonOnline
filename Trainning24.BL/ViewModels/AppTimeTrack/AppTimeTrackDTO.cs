using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.AppTimeTrack
{
    public class AppTimeTrackDTO
    {
        public long userid { get; set; }
        public string latitude { get; set; }
        public string longitude { get; set; }
        public string serviceprovider { get; set; }
        public string school { get; set; }
        public string subjectstaken { get; set; }
        public string grade { get; set; }
        public string hardwareplatform { get; set; }
        public string operatingsystem { get; set; }
        public string version { get; set; }
        public string isp { get; set; }
        public string activitytime { get; set; }
        public string outtime { get; set; }
        public string networkspeed { get; set; }
    }

    public class AppTimeTrackCSVDTO
    {
        public long AppTimeId { get; set; }
    }

    public class AppTimeExcelDataDTO
    {
        public string AppTimeId { get; set; }
    }
}
