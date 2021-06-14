using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Users
{
    public class UserStatistics
    {
        public long unreadnotifications { get; set; }
        public long avragequizscore { get; set; }
        public long avrageassignmentscore { get; set; }
        public long complatecourse { get; set; }
        public long totalavailablescore { get; set; }
        public long terminatedcourse { get; set; }
        public long totalcourse { get; set; }
    }
}
