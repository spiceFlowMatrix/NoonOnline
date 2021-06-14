using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class Question : EntityBase
    {
        public long QuestionTypeId { get; set; }

        [Required]
        [StringLength(255)]
		public string QuestionText { get; set; }

        [StringLength(255)]
        public string Explanation { get; set; }
        public bool IsMultiAnswer { get; set; } = false;

        [ForeignKey("QuestionTypeId")]
        public QuestionType QuestionType { get; set; }
    }
}
