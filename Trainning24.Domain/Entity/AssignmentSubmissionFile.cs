using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class AssignmentSubmissionFile : EntityBase
    {
        public long SubmissionId { get; set; }
        public long FileId { get; set; }
    }
}
