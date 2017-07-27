using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace DeadLiner.Models
{
    public class TasksModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Heading is required")]
        public string Heading { get; set; }

        [Required(ErrorMessage = "Content is required")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Start Date is required")]
        [DisplayName(displayName: "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "Deadline is required")]
        [DisplayName(displayName: "Deadline")]
        public int DeadlineTask { get; set; }

        [DisplayName(displayName: "End Date")]
        public DateTime EndDate { get; set; }

        [DisplayName(displayName: "Created By")]
        public string CreatedBy { get; set; }

        [DisplayName(displayName: "Created On")]        
        public DateTime CreatedOn { get; set; }

        public string Status { get; set; }
    }
}