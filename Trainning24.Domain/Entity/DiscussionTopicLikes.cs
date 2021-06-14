using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class DiscussionTopicLikes : EntityBase
    {
        public long TopicId { get; set; }
        public long UserId { get; set; }
        public bool Like { get; set; }
        public bool DisLike { get; set; }
    }
}
