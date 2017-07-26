using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeadLiner.Models
{
    public class TaskAssigned
    {
        public int Id { get; set; }
        public int TaskModelId { get; set; }
        public int UserId { get; set; }
        public virtual TasksModel TasksModel { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}