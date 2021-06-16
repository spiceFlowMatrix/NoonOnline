using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class Quiz : EntityBase, IQuiz
    {
        public Quiz()
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

        public int ItemOrder { get; set; }
    }
}