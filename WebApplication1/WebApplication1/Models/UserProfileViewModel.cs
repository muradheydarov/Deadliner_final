using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DeadLiner.Models;

namespace DeadLiner.Models
{
    public class UserProfileViewModel
    {
        public IndexViewModel IndexViewModels { get; set; }
        public ChangePasswordViewModel ChangePasswordViewModels { get; set; }
        public ApplicationUser Users { get; set; }
        public List<LoadFileViewModel> ImgFile { get; set; }
        public UploadDataViewModel UploadData { get; set; }
    }
}