using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class FeedBackTask : EntityBase
    {
        public long FeedbackId { get; set; }        
        public string Description { get; set; }
        public string FileLink { get; set; }
        public long Type { get; set; }
        //public int Status { get; set; }
    }

    public class FeedBackTaskStatus : EntityBase
    {
        public long FeedbackId { get; set; }
        public long Status { get; set; }
    }

    public class FeedBackTaskStatusOption : EntityBase
    {        
        public string Name { get; set; }
    }
}
