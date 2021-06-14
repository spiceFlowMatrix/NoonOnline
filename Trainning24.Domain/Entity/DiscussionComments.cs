using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class DiscussionComments : EntityBase
    {
        public string Comment { get; set; }
        public long TopicId { get; set; }
    }
}
