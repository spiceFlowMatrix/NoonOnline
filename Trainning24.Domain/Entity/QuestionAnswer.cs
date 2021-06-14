using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trainning24.Domain.Entity
{
    public class QuestionAnswer : EntityBase
    {
        [Required]
        [StringLength(255)]
        public string Answer { get; set; }
        [StringLength(255)]
        public string ExtraText { get; set; }
        public bool IsCorrect { get; set; }
        public long QuestionId { get; set; }
        //public long FileId { get; set; }

        [ForeignKey("QuestionId")]
        public Question Question { get; set; }
    }
}
