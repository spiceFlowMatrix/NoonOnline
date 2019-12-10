using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.DiscussionTopic
{
    public class DiscussionTopicLikeDTO
    {
        public long topicid { get; set; }
        public bool like { get; set; }
        public bool dislike { get; set; }
    }
}
