using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.AssignmentSubmission
{
    public class AssignmentSubmissionDTO
    {
        public long assignmentid { get; set; }
        public long userid { get; set; }
        public bool issubmission { get; set; }
        public List<int> files { get; set; }
    }

    public class AssignmentSubmissionCommentDTO
    {
        public long assignmentid { get; set; }
        public string comment { get; set; }
        public long userid { get; set; }
        public long teacherid { get; set; }
    }

    public class ApprovedAssignmentSubmissionDTO
    {
        public long assignmentid { get; set; }
        public long userid { get; set; }
        public long teacherid { get; set; }
        public bool isapproved { get; set; }
        public long score { get; set; }
        public string remark { get; set; }
    }
}
