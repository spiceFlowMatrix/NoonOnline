using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Question
{
    public class DeleteQuizDetailModel
    {
        public long quizid { get; set; }
        public long questionid { get; set; }
        public long answerid { get; set; }
        //public long questionanswerid { get; set; }
        public long fileid { get; set; }
        public int recordtodelete { get; set; }
    }
}
