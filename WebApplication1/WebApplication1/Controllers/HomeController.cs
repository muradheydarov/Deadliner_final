using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Hosting;
using System.Web.Mvc;
using DeadLiner.Models;
using Microsoft.AspNet.Identity;
using WebApplication1.Models;

namespace DeadLiner.Controllers
{
    [Authorize]
    [RequireHttps]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController()
        {
            db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            return View();           
        }                

        public ActionResult Media(string id)
        {
            var userFile = db.UserFileses.FirstOrDefault(x => x.Id == id);

            long fSize = userFile.UserFile.Length;
            long startbyte = 0;
            long endbyte = fSize - 1;
            int statusCode = 200;
            if (Request.Headers["Range"] != null)
            {
                //Get the actual byte range from the range header string, and set the starting byte.
                string[] range = Request.Headers["Range"].Split(new char[] { '=', '-' });
                startbyte = Convert.ToInt64(range[1]);
                if (range.Length > 2 && range[2] != "") endbyte = Convert.ToInt64(range[2]);
                //If the start byte is not equal to zero, that means the user is requesting partial content.
                if (startbyte != 0 || endbyte != fSize - 1 || range.Length > 2 && range[2] == "")
                { statusCode = 206; }//Set the status code of the response to 206 (Partial Content) and add a content range header.                                    
            }
            long desSize = endbyte - startbyte + 1;
            //Headers
            Response.StatusCode = statusCode;
            Response.AddHeader("Content-Accept", userFile.FileType);
            Response.AddHeader("Content-Length", desSize.ToString());
            Response.AddHeader("Content-Range", string.Format("bytes {0}-{1}/{2}", startbyte, endbyte, fSize));

            var fs = new MemoryStream(userFile.UserFile, (int)startbyte, (int)desSize);
            return new FileStreamResult(fs, userFile.FileType);
        }

        [HttpPost]
        public ActionResult UploadToDb(UserProfileViewModel model)
        {
            var userId = User.Identity.GetUserId();
            bool userExists = db.Users.Any(x => x.Id == userId);
            if (!userExists)
            {
                HttpNotFound();
            }
            if (model.UploadData.Files != null && model.UploadData.Files.Count > 0 && model.UploadData.Files[0] != null)
            {
                foreach (var file in model.UploadData.Files)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    bool fileExists = db.UserFileses.Any(x => x.FileName == fileName);
                    if (!fileExists && fileName.ToLower().EndsWith(".png") || fileName.ToLower().EndsWith(".jpg") || fileName.ToLower().EndsWith(".gif"))
                    {
                        var fileType = file.ContentType;
                        var fileContent = new byte[file.InputStream.Length];                        
                        file.InputStream.Read(fileContent, 0, fileContent.Length);
                        var userFileDb = db.UserFileses.FirstOrDefault(x => x.UserId == userId);
                        if (userFileDb!=null)
                        {
                            userFileDb.FileName = fileName;
                            userFileDb.FileType = fileType;
                            userFileDb.UserFile = fileContent;
                            db.Entry(userFileDb).State = EntityState.Modified;
                        }
                        else
                        {
                            UserFiles uf = new UserFiles
                            {
                                Id = Guid.NewGuid().ToString(),
                                FileName = fileName,
                                FileType = fileType,
                                UserFile = fileContent,
                                UserId = userId
                            };
                            db.UserFileses.Add(uf);
                        }                                                                                     
                        db.SaveChanges();                        
                    }
                    else
                    {
                        ModelState.AddModelError("", "File exists or file format is not correct");
                    }
                }
            }
            return RedirectToAction("CustomChangePassword","Manage");
        }
        
    }

}