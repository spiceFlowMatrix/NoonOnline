using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.Assignment;

namespace Trainning24.BL.ViewModels.Course
{
    public class ChapterPreviewModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Code { get; set; }
        public int itemorder { get; set; }
        public List<object> lessons { get; set; }

        public List<QuizPreviewModel> quizs { get; set; }
        public List<AssignmentPreviewModel> assignments { get; set; }
    }
}
