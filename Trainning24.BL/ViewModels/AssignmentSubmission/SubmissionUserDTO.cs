using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.AssignmentSubmission
{
    public class SubmissionUserDTO
    {
        public long id { get; set; }
        public string name { get; set; }
        public int submissions { get; set; }
        public int status { get; set; }
    }
}
