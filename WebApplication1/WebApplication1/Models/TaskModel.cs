using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DeadLiner.Models;

namespace DeadLiner.Models
{
    public class TaskModel
    {
        public int TaskId { get; set; }

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

        [Required(ErrorMessage = "End Date is required")]
        [DisplayName(displayName: "End Date")]        
        public DateTime EndDate { get; set; }

        [Required(ErrorMessage = "Created By is required")]
        [DisplayName(displayName: "Created By")]
        public string CreatedBy { get; set; }

        [DisplayName(displayName: "Created On")]
        [Required(ErrorMessage = "Created on required")]        
        public DateTime CreatedOn { get; set; }

        public string Status { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}