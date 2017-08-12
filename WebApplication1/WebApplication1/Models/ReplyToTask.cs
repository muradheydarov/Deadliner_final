using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeadLiner.Models
{
    public class ReplyToTask
    {
        [Key]
        public int ReplyToTaskId { get; set; }

        [DataType(DataType.Url)]
        [Required(ErrorMessage = "Answer is required")]
        [Display(Name = "User Answer")]
        public string UserAnswer { get; set; }

        [DataType(DataType.DateTime)]
        [Display(Name = "Answer Date")]
        public DateTime? AnswerTime { get; set; }

        public int TaskToUserID { get; set; }

        public virtual TaskToUser TaskToUser { get; set; }
    }
}