using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.BL.ViewModels.Quiz
{
    public class AddQuizModel
    {
        public AddQuizModel()
        {
            NumQuestions = 4;
            PassMark = 50;
        }

        [Required]
        [StringLength(255, MinimumLength = 3)]
        public string Name { get; set; }

        [StringLength(50, MinimumLength = 3)]
        public string Code { get; set; }

        public int NumQuestions { get; set; }

        public override string ToString()
        {
            return Code + ":" + Name;
        }

        [Required]
        public Decimal PassMark { get; set; }

        public int TimeOut { get; set; }

        public long chapterid { get; set; }
        public int itemorder {get; set;}
    }

    public class QuizDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
    }
}
