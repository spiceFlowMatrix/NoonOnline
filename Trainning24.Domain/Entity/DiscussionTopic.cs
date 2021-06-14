using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class DiscussionTopic : EntityBase
    {
        public long CourseId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPrivate { get; set; }
        public bool IsPublic { get; set; }
    }
}
