using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace DeadLiner.Models
{
    public class TaskAssigned
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("TasksModel")]
        public int? TaskModel_Id { get; set; }
        public string UserId { get; set; }
        public virtual TasksModel TasksModel { get; set; }
        public virtual ApplicationUser User { get; set; }
    }
}