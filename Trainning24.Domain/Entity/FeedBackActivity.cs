using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class FeedBackActivity : EntityBase
    {
        public long FeedbackId { get; set; }
        public long FeedbackTaskId { get; set; }
        public long UserId { get; set; }
        public long Type { get; set; }
    }
}
