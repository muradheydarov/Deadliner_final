using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeadLiner.Models
{
    public class TaskToUserViewModel
    {
        public int TaskToUserViewModelId { get; set; }
        public int TaskModelID { get; set; }
        public int UserIdInt { get; set; }
        public ApplicationUser User { get; set; }
        public TasksModel TasksModel { get; set; }
        [DataType(DataType.Url)]
        [Required(ErrorMessage = "Input can not be empty")]
        public string StudentReply { get; set; }
        public DateTime AnswerTime { get; set; }    
    }
}