using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.FeedbackStaff
{
    public class AddFeedBackStaffModel
    {
        public long id { get; set; }
        public long feedbackid { get; set; }
        public long userid { get; set; }
        public long type { get; set; }
        public bool ismanager { get; set; }
    }
}
