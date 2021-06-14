using System;
using System.Collections.Generic;
using System.Text;
using Trainning24.BL.ViewModels.Users;

namespace Trainning24.BL.ViewModels.FeedBackActivity
{
    public class ResponseFeedBackActivityModel
    {
        public long id { get; set; }
        public long FeedbackId { get; set; }
        public long FeedbackTaskId { get; set; }
        public FeedBackUser userid { get; set; }
        public long Type { get; set; }
    }
}
