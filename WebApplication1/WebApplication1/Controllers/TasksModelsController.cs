using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeadLiner.Models;

namespace DeadLiner.Controllers
{
    public class TasksModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: TasksModels
        public ActionResult Index()
        {
            return View(db.TasksModels.ToList());
        }

        // GET: TasksModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasksModel tasksModel = db.TasksModels.Find(id);
            if (tasksModel == null)
            {
                return HttpNotFound();
            }
            return View(tasksModel);
        }

        // GET: TasksModels/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: TasksModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Heading,Content,StartDate,DeadlineTask,EndDate,CreatedBy,CreatedOn,Status")] TasksModel tasksModel)
        {
            if (ModelState.IsValid)
            {
                db.TasksModels.Add(tasksModel);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(tasksModel);
        }

        // GET: TasksModels/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasksModel tasksModel = db.TasksModels.Find(id);
            if (tasksModel == null)
            {
                return HttpNotFound();
            }
            return View(tasksModel);
        }

        // POST: TasksModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Heading,Content,StartDate,DeadlineTask,EndDate,CreatedBy,CreatedOn,Status")] TasksModel tasksModel)
        {
            if (ModelState.IsValid)
            {
                db.Entry(tasksModel).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(tasksModel);
        }

        // GET: TasksModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasksModel tasksModel = db.TasksModels.Find(id);
            if (tasksModel == null)
            {
                return HttpNotFound();
            }
            return View(tasksModel);
        }

        // POST: TasksModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            TasksModel tasksModel = db.TasksModels.Find(id);
            db.TasksModels.Remove(tasksModel);
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
