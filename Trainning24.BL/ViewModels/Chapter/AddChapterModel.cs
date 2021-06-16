using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Chapter
{
    public class AddChapterModel
    {
        public string name { get; set; }
        public string code { get; set; }
        public long courseid { get; set; }
        public long? quizid { get; set; }
    }

    public class ChapterDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public long courseid { get; set; }
    }
}
