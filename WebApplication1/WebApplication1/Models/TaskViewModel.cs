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
        public string Heading { get; set; }
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Status { get; set; }
        public HttpPostedFileBase Image { get; set; }

        public List<CheckBoxViewModel> Users { get; set; }
    }
}