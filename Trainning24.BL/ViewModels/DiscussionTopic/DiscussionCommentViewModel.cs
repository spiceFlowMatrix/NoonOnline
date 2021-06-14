using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.DiscussionTopic
{
    public class DiscussionCommentViewModel
    {
        public long Id { get; set; }
        public string comment { get; set; }
        public string[] filesid { get; set; }
        public long topicid { get; set; }
        public object user { get; set; }
        public string createtime { get; set; }
        public List<DiscussionCommentFileViewModel> files { get; set; }
        public bool ispublic { get; set; }
        public int likecount { get; set; }
        public int dislikecount { get; set; }
        public bool liked { get; set; }
        public bool disliked { get; set; }
    }

    public class DiscussionCommentDto
    {
        public long Id { get; set; }
        public string comment { get; set; }
        public long topicid { get; set; }
        public string createtime { get; set; }
    }
}
