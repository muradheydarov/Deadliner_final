using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeadLiner.Models;

namespace DeadLiner.Models
{
    public class UserProfileDetailsView
    {
        public ApplicationUser User { get; set; }
        public List<LoadFileViewModel> UploadImg { get; set; }
    }
}