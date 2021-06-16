using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Lesson
{
    public class UpdateLessonModel
    {
        public int id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public string description { get; set; }
        //public long? fileid { get; set; }
        //public long? quizid { get; set; }
        public long chapterid { get; set; }
        public int itemorder { get; set; }
    }
}
