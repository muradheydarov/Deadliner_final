﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;

namespace DeadLiner.Models
{
    public class ApplicationUsersController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: ApplicationUsers
        public ActionResult Index()
        {
            if (User.IsInRole("Admin"))
            {
                return View("AdminIndex", db.Users.ToList());
            }
            return View("UsersIndex", db.Users.ToList());
        }

        // GET: ApplicationUsers/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);                        
            
            bool userExists = db.Users.Any(x => x.Id == id);
            List<LoadFileViewModel> ufls = new List<LoadFileViewModel>();
            if (userExists)
            {
                var userfiles = db.UserFileses.Where(x => x.UserId == id).ToList();
                foreach (var userFile in userfiles)
                {
                    string type = null;
                    int index = userFile.FileType.IndexOf('/');
                    if (index > 0) { type = userFile.FileType.Substring(0, index); }
                    ufls.Add(new LoadFileViewModel() { FileName = userFile.FileName, FileType = type, Id = userFile.Id });
                }
            }
            
            UserProfileDetailsView details = new UserProfileDetailsView {User = applicationUser,UploadImg = ufls};            

            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            if (User.IsInRole("Admin"))
            {
                return View("DetailsAdmin", details);
            }
            return View("DetailsUser", details);
        }

        // GET: ApplicationUsers/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ApplicationUsers/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,Surname,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,UserStatus,Gender")] ApplicationUser applicationUser)
        {
            if (ModelState.IsValid)
            {
                db.Users.Add(applicationUser);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(applicationUser);
        }

        // GET: ApplicationUsers/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Surname,Email,EmailConfirmed,PasswordHash,SecurityStamp,PhoneNumber,PhoneNumberConfirmed,TwoFactorEnabled,LockoutEndDateUtc,LockoutEnabled,AccessFailedCount,UserName,UserStatus,Gender")] ApplicationUser applicationUser)
        {            
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(applicationUser).State = EntityState.Modified;
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    HttpNotFound();
                }                                
                return RedirectToAction("Index");
            }
            return View(applicationUser);
        }

        // GET: ApplicationUsers/Delete/5
        public ActionResult Delete(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ApplicationUser applicationUser = db.Users.Find(id);
            if (applicationUser == null)
            {
                return HttpNotFound();
            }
            return View(applicationUser);
        }

        // POST: ApplicationUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(string id)
        {
            ApplicationUser applicationUser = db.Users.Find(id);
            db.Users.Remove(applicationUser);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
