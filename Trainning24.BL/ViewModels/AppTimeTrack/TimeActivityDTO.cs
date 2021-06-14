using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Subscriptions;

namespace Trainning24.BL.ViewModels.AppTimeTrack
{
    public class TimeActivityDTO
    {
        public DateTime activitytime { get; set; }
        public DateTime outtime { get; set; }
        public long id { get; set; }
    }

    public class AllTimeActivityDTO
    {
        public List<TimeChartDTO> mo { get; set; }
        public List<TimeChartDayDTO> da { get; set; }
    }

    public class TimeChartDTO
    {
        public long x { get; set; }
        public double y { get; set; }
    }

    public class TimeChartDayDTO
    {
        public DateTime x { get; set; }
        public double y { get; set; }
    }

    public class PieChartDTO
    {
        public double score { get; set; }
        public int totaltries { get; set; }
        public double overallscore { get; set; }
    }

    public class CurrentGPA
    {
        public string grade { get; set; }
    }

    public class UserLocationActivityDTO
    {
        public UserDetailsDTO user { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
    }

    public class UserLocationDTO
    {
        public DateTime datetime { get; set; }
        public long userid { get; set; }
        public string lat { get; set; }
        public string lng { get; set; }
        public long id { get; set; }
    }

    public class ParentPostDTO
    {
        public List<int> id { get; set; }
        public int month { get; set; }
        public int courseid { get; set; }
        public int content { get; set; }
    }
}
