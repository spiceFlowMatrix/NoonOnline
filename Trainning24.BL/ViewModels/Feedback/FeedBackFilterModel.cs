using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.Feedback
{
    public class FeedBackFilterModel
    {
        public string grade { get; set; }
        public string courseids { get; set; }
        public string enddate { get; set; }
        public int fltroptiongrade { get; set; }
        public int fltroptionenddate { get; set; }
        public bool isfilterneeded { get; set; }
    }
}
