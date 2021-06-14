using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class QuizQuestion : EntityBase
    {
        [Required]
        public long QuizId { get; set; }
        [Required]
        public long QuestionId { get; set; }

        [ForeignKey("QuizId")]
        public Quiz Quiz { get; set; }
        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
    }
}