using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeadLiner.Models
{
    public class TaskViewModel
    {
        public string UserId { get; set; }
        public int? UserIdInt { get; set; }
        public int? TaskId { get; set; }
        public string UserName { get; set; }
        public int TaskAssaignedId { get; set; }
        public string TaskType { get; set; }
    }
}