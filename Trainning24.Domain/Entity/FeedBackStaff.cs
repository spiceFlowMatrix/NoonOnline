using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class FeedBackStaff : EntityBase
    {
        public long FeedBackId { get; set; }
        public long UserId { get; set; }
        public long Type { get; set; }
        public bool IsManager { get; set; }
    }
}
