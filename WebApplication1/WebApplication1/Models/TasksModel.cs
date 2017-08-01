using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeadLiner.Models
{
    public class TasksModel
    {
        public int Id { get; set; }
        public string Heading { get; set; }
        public string Content { get; set; }

        public virtual ICollection<TaskToUser> UserToTasks { get; set; }
    }
}