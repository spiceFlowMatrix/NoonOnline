using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Users;

namespace Trainning24.BL.ViewModels.FeedBackTask
{
    public class ResponseFeedBackTaskModel
    {
        public long id { get; set; }
        public long feedbackid { get; set; }        
        public string description { get; set; }
        public string filelink { get; set; }
        public long type { get; set; }
        public string creationtime { get; set; }
        public string Status { get; set; }
    }
}
