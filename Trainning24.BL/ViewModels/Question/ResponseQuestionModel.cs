using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trainning24.BL.ViewModels.LessonFile;

namespace Trainning24.BL.ViewModels.Question
{
    public class ResponseQuestionModel
    {
        public long Id { get; set; }
        public long QuestionTypeId { get; set; }
        public string QuestionText { get; set; }
        public string Explanation { get; set; }
        public bool IsMultiAnswer { get; set; } = false;
        public string QuesionType { get; set; }

        public List<ResponseLessonFileModel> files { get; set; }


    }
}
