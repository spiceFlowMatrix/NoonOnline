using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.QuestionFile;

namespace Trainning24.BL.ViewModels.FeedbackTime
{
    public class ResponseFeedbackTime
    {
        public long Id { get; set; }
        public long FeedbackId { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
        public List<UpdateQuestionFileModel> feedbackTimeFiles { get; set; }
    }

    public class ResponseFdTime
    {
        public long Id { get; set; }
        public string Time { get; set; }
        public string Description { get; set; }
        public List<UpdateQuestionFileModel1> feedbackTimeFiles { get; set; }
    }
}
