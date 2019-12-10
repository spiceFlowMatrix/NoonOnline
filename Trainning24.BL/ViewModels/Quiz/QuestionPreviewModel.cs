using System.Collections.Generic;

namespace Trainning24.BL.ViewModels.Quiz
{
    public class QuestionPreviewModel
    {
        public long id { get; set; }
        public long questiontypeid { get; set; }
        public string questiontext { get; set; }
        public string explanation { get; set; }
        public bool ismultianswer { get; set; }
        public string quesiontype { get; set; }
        public List<AnswerPreviewModel> answers { get; set; }
    }
}
