using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeadLiner.Models;
using Microsoft.AspNet.Identity;

namespace DeadLiner.Controllers
{
    public class TaskAssignedsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TaskAssigneds
        public ActionResult Index()
        {
            var list = db.TaskAssigneds.Include(t => t.TasksModel).Include(u => u.User).Select(l => new TasksAssignedViewModel
            {
                TaskModelId = l.TasksModel.Id,
                UserId = l.User.ApplicationUserId,
                TaskModelName = l.TasksModel.Heading,
                UserName = l.User.UserName
            });

            return View(list.ToList());
        }

        // GET: TaskAssigneds/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskAssigned taskAssigned = db.TaskAssigneds.Find(id);
            if (taskAssigned == null)
            {
                return HttpNotFound();
            }
            return View(taskAssigned);
        }

        // GET: TaskAssigneds/Create
        public ActionResult Create()
        {            
            ViewBag.TaskModelId = new SelectList(db.TasksModels,"Id", "Heading");
            ViewBag.UserId = new SelectList(db.Users, "ApplicationUserId", "UserName");
            return View();
        }

        // POST: TaskAssigneds/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,TaskModelId,UserId")] TaskAssigned taskAssigned)
        {
            if (ModelState.IsValid)
            {
                db.TaskAssigneds.Add(taskAssigned);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.TaskModelId = new SelectList(db.TasksModels, "Id", "Heading", taskAssigned.TaskModelId);
            ViewBag.UserId = new SelectList(db.Users, "ApplicationUserId", "UserName", taskAssigned.UserId);

            return View(taskAssigned);
        }

        // GET: TaskAssigneds/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskAssigned taskAssigned = db.TaskAssigneds.Find(id);
            if (taskAssigned == null)
            {
                return HttpNotFound();
            }
            return View(taskAssigned);
        }

        // POST: TaskAssigneds/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,TaskModelId,UserId")] TaskAssigned taskAssigned)
        {
            if (ModelState.IsValid)
            {
                db.Entry(taskAssigned).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(taskAssigned);
        }

        // GET: TaskAssigneds/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaskAssigned taskAssigned = db.TaskAssigneds.Find(id);
            if (taskAssigned == null)
            {
                return HttpNotFound();
            }
            return View(taskAssigned);
        }

        // POST: TaskAssigneds/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TaskAssigned taskAssigned = db.TaskAssigneds.Find(id);
            db.TaskAssigneds.Remove(taskAssigned);
            db.SaveChanges();
            return RedirectToAction("Index");
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
