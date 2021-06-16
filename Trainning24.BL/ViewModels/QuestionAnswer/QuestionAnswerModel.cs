using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.QuestionFile;

namespace Trainning24.BL.ViewModels.QuestionAnswer
{
    public class QuestionAnswerModel
    {
        public long id { get; set; }
        public string answer { get; set; }
        public string extratext { get; set; }
        public bool iscorrect { get; set; }
        public long questionid { get; set; }
        public List<UpdateQuestionFileModel> images { get; set; }
    }


    public class QuestionAnswerModel1
    {
        public long id { get; set; }
        public string answer { get; set; }
        public string extratext { get; set; }
        public bool iscorrect { get; set; }
        public long questionid { get; set; }
        public List<UpdateQuestionFileModel> images { get; set; }
    }
}
