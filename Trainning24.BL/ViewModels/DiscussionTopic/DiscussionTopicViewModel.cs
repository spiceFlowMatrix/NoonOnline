using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Files;
using Trainning24.BL.ViewModels.Users;

namespace Trainning24.BL.ViewModels.DiscussionTopic
{
    public class DiscussionTopicViewModel
    {
        public long id { get; set; }
        public long courseid { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int createduserid { get; set; }
        public bool isprivate { get; set; }
        public long comments { get; set; }
        public string[] filesid { get; set; }
        public List<DiscussionFilesModel> files { get; set; }
        public bool ispublic { get; set; }
        public string createddate { get; set; }
        public object user { get; set; }
        public bool iseditable { get; set; }
        public int likecount { get; set; }
        public int dislikecount { get; set; }
        public bool liked { get; set; }
        public bool disliked { get; set; }
    }

    public class DiscussionTopicDto
    {
        public long id { get; set; }
        public long courseid { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public bool isprivate { get; set; }
        public bool iseditable { get; set; }
        public int likecount { get; set; }
        public int dislikecount { get; set; }
        public bool liked { get; set; }
        public bool disliked { get; set; }
    }

    public class DiscussionTopicDTO
    {
        public long id { get; set; }
        public long courseid { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public int createduserid { get; set; }
        public bool isprivate { get; set; }
        public bool ispublic { get; set; }
        public string createddate { get; set; }
        public BasicUserDTO user { get; set; }
    }
}
