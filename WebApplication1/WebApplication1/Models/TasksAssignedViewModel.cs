using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeadLiner.Models
{
    public class TasksAssignedViewModel
    {
        public int? TaskModelId { get; set; }
        public int? UserId { get; set; }
        public string TaskModelName { get; set; }
        public string UserName { get; set; }
    }
}