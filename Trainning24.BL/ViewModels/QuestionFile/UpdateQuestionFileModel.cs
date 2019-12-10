using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.QuestionFile
{
    public class UpdateQuestionFileModel
    {
        public long fileid { get; set; }
        public string Url { get; set; }
        public string duration { get; set; }
    }
    public class UpdateQuestionFileModel1
    {
        public long fileid { get; set; }
        public string Url { get; set; }
        public string name { get; set; }
        public string duration { get; set; }
    }
}
