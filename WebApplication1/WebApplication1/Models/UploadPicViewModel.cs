using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using DeadLiner.Models;

namespace DeadLiner.Models
{
    public class UserFiles
    {
        public string Id { get; set; }
        public string UserId { get; set; }
        public byte[] UserFile { get; set; }
        public string FileType { get; set; }
        public string FileName { get; set; }
        public ApplicationUser User { get; set; }
    }

    public class UploadViewModel
    {
        [Display(Name = "File")]
        public List<HttpPostedFileBase> Files { get; set; }
    }

    public class UploadDataViewModel
    {        
        public string UserName { get; set; }

        [Display(Name = "File")]        
        public List<HttpPostedFileBase> Files { get; set; }
    }

    public class LoadFileViewModel
    {
        public string FileName { get; set; }
        public string FileType { get; set; }
        public string Id { get; set; }
    }
}
