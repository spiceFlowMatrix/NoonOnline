using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class LessonAssignmentSubmission : EntityBase
    {
        public long AssignmentId { get; set; }
        public long UserId { get; set; }
        public long TeacherId { get; set; }
        public long Score { get; set; }
        public string Remark { get; set; }
        public bool IsSubmission { get; set; }
        public string Comment { get; set; }
        public bool IsApproved { get; set; }
    }
}
