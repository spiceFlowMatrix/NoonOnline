using System;
using System.Collections.Generic;
using System.Text;

namespace Trainning24.Domain.Entity
{
    public class UserQuizResult : EntityBase
    {
        public long QuizId { get; set; }
        public long UserId { get; set; }
        public long TotalQuestion { get; set; }
        public long AnsweredQuestion { get; set; }
        public string PerformDate { get; set; }
        public long PassingScore { get; set; }
        public long Score { get; set; }
    }
}
