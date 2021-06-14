using System;
using System.ComponentModel.DataAnnotations;
using Trainning24.Abstract.Entity;

namespace Trainning24.Domain.Entity
{
    public class QuestionType : EntityBase, IQuestionType
    {

        [Required]
        [StringLength(6)]
        public string Code { get; set; }

    }
}
