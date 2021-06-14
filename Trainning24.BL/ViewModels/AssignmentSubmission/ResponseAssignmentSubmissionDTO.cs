using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Users;

namespace Trainning24.BL.ViewModels.AssignmentSubmission
{
    public class ResponseAssignmentSubmissionDTO
    {
        public long id { get; set; }
        public long assignmentid { get; set; }
        public long userid { get; set; }
        public long score { get; set; }
        public string remark { get; set; }
        public bool issubmission { get; set; }
        public string comment { get; set; }
        public bool isapproved { get; set; }
        public string datecreated { get; set; }
        public List<SubmissionFileDTO> submissionfiles { get; set; }
    }

    public class AssignmentSubmissionDetailsDTO
    {
        public bool isapproved { get; set; }
        public List<AssignmentSubmissionDetailDTO> details { get; set; }
    }

    public class AssignmentSubmissionDetailDTO
    {
        public long id { get; set; }
        public long assignmentid { get; set; }
        public long userid { get; set; }
        public long score { get; set; }
        public string remark { get; set; }
        public bool issubmission { get; set; }
        public string comment { get; set; }
        public bool isapproved { get; set; }
        public string datecreated { get; set; }
        public List<SubmissionFileDTO> submissionfiles { get; set; }
        public BasicUserDTO user { get; set; }
    }
}
