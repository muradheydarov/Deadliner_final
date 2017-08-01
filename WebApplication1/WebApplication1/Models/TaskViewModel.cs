using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeadLiner.Models;

namespace DeadLiner.Models
{
    public class TaskViewModel
    {        
        public int TaskId { get; set; }
        public string Header { get; set; }
        public string Content { get; set; }
        public List<CheckBoxViewModel> Users { get; set; }
    }
}