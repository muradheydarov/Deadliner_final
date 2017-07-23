using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeadLiner.Models
{
    public class Tasks
    {
        public int id { get; set; }
        public string Heading { get; set; }
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedOn { get; set; }
        public string Status { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}