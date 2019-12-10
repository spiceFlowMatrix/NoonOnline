using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class ChapterQuiz : EntityBase
    {
        public long QuizId { get; set; }
        public long ChapterId { get; set; }
        public int ItemOrder { get; set; }
    }
}
