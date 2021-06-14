using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.BL.ViewModels.TaskFeedBack
{
    public class CreateFeedBackDTO
    {
        public string title { get; set; }
        public string description { get; set; }
        public long categoryid { get; set; }
        public long? gradeid { get; set; }
        public long? courseid { get; set; }
        public long? chapterid { get; set; }
        public long? lessonid { get; set; }
        public string time { get; set; }
        public string device { get; set; }
        public string version { get; set; }
        public string appversion { get; set; }
        public string operatingsystem { get; set; }
        public List<long> filesids { get; set; }
    }
}
