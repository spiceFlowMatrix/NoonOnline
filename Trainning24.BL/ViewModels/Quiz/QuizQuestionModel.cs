using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Quiz
{
    public class QuizQuestionModel
    {
        public long QuizId { get; set; }
        public List<long> lstQuestionId { get; set; }
    }
}
