using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Chapter
{
    public class UpdateChapterModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public long courseid { get; set; }
        public long? quizid { get; set; }
        public int itemorder { get; set; }
    }
}
