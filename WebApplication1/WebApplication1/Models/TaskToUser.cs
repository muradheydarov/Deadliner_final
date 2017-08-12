using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeadLiner.Models
{
    public class TaskToUser
    {
        [Key]
        public int TaskToUserID { get; set; }
        public string Id { get; set; }
        public int UserIdInt { get; set; }
        public int TasksModelID { get; set; }

        [DataType(DataType.Url)]
        [Required(ErrorMessage = "Input can not be empty")]
        public string StudentReply { get; set; }
        public DateTime AnswerTime { get; set; }

        public virtual ApplicationUser User { get; set; }
        public virtual TasksModel TasksModel { get; set; }
    }
}