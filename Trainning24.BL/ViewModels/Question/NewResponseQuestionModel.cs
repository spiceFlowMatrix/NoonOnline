using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Question
{
    public class NewResponseQuestionModel
    {
        public long quizid { get; set; }
        public decimal passmark { get; set; }
        public List<QuestionModel> questions { get; set; }
    }

    public class NewResponseQuestionModel1
    {
        public long quizid { get; set; }
        public decimal passmark { get; set; }
        public QuizSummaryResponse QuizSummaryResponse { get; set; }
        public List<QuestionModel1> questions { get; set; }
    }

    public class QuizSummaryResponse
    {
        public long id { get; set; }
        //public long StudentId { get; set; }
        //public long QuizId { get; set; }
        public int QSummary { get; set; }
        public int Attempts { get; set; }
    }
}