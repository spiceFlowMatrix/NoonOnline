using System.Collections.Generic;

namespace Trainning24.BL.ViewModels.Quiz
{
    public class QuizPreviewModel
    {
        public long id { get; set; }
        public string name { get; set; }
        public string code { get; set; }
        public int numquestions { get; set; }
        public decimal passmark { get; set; }
        public int timeout { get; set; }
        public List<QuestionPreviewModel> questions { get; set; }
    }
}
