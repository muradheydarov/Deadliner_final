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
using Microsoft.AspNet.Identity.EntityFramework;

namespace DeadLiner.Controllers
{
    public class TasksModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET
        public ActionResult MyTasks()
        {
            var userid = db.Users.Find(User.Identity.GetUserId());

            var result = db.TaskToUsers.Include(t => t.TaskModel).Include(u => u.ApplicationUser)
                .Where(w => w.UserIdInt == userid.ApplicationUserId).ToList();
            
            return View(result);
        }

        // GET: TasksModels
        public ActionResult Index()
        {
            return View(db.TaskModels.ToList());
        }

        // GET: TasksModels/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            TasksModel tasksModel = db.TaskModels.Find(id);
            if (tasksModel == null)
            {
                return HttpNotFound();
            }
            var result = from a in db.Users
                         select new
                         {
                             a.ApplicationUserId,
                             a.UserName,
                             Checked = (from ab in db.TaskToUsers
                                        where (ab.UserIdInt == id) & (ab.TaskId == a.ApplicationUserId)
                                        select ab).Any()
                         };

            var MyViewModel = new TaskViewModel();
            MyViewModel.TaskId = id.Value;
            MyViewModel.Heading = tasksModel.Heading;
            MyViewModel.Content = tasksModel.Content;

            var MyCheckBoxList = new List<CheckBoxViewModel>();

            foreach (var item in result)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.ApplicationUserId, Name = item.UserName, Checked = item.Checked });
            }

            MyViewModel.Users = MyCheckBoxList;
            return View(MyViewModel);
            // return Json(new { data = MyViewModel }, JsonRequestBehavior.AllowGet);
        }

        // GET: TasksModels/Create
        public ActionResult Create()
        {
            var result = from a in db.Users
                         select new
                         {
                             a.ApplicationUserId,
                             a.UserName,
                             Checked = (from ab in db.TaskToUsers
                                        select ab).Any()
                         };

            var MyViewModel = new TaskViewModel();

            var MyCheckBoxList = new List<CheckBoxViewModel>();

            foreach (var item in result)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.ApplicationUserId, Name = item.UserName, Checked = item.Checked });
            }

            MyViewModel.Users = MyCheckBoxList;
            return View(MyViewModel);
        }

        // POST: TasksModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(TaskViewModel tasksModel)
        {
            if (ModelState.IsValid)
            {
                var newTask = new TasksModel() { Id = tasksModel.TaskId, Heading = tasksModel.Heading, Content = tasksModel.Content, StartDate = tasksModel.StartDate, CreatedBy = User.Identity.GetUserName(), CreatedOn = DateTime.Now, EndDate = tasksModel.EndDate };

                db.TaskModels.Add(newTask);

                db.SaveChanges();

                foreach (var item in tasksModel.Users)
                {
                    if (item.Checked)
                    {
                        db.TaskToUsers.Add(new TaskToUser() { UserIdInt = newTask.Id, TaskId = item.Id });
                    }
                }
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
            TasksModel tasksModel = db.TaskModels.Find(id);
            if (tasksModel == null)
            {
                return HttpNotFound();
            }

            var result = from a in db.Users
                         select new
                         {
                             a.ApplicationUserId,
                             a.UserName,
                             Checked = (from ab in db.TaskToUsers
                                        where (ab.UserIdInt == id) & (ab.TaskId == a.ApplicationUserId)
                                        select ab).Any()
                         };

            var MyViewModel = new TaskViewModel();
            MyViewModel.TaskId = id.Value;
            MyViewModel.Heading = tasksModel.Heading;
            MyViewModel.Content = tasksModel.Content;
            MyViewModel.CreatedBy = tasksModel.CreatedBy;
            MyViewModel.CreatedOn = tasksModel.CreatedOn;
            MyViewModel.StartDate = tasksModel.StartDate;
            MyViewModel.EndDate = tasksModel.EndDate;
            MyViewModel.Status = tasksModel.Status;

            var MyCheckBoxList = new List<CheckBoxViewModel>();

            foreach (var item in result)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.ApplicationUserId, Name = item.UserName, Checked = item.Checked });
            }

            MyViewModel.Users = MyCheckBoxList;
            return View(MyViewModel);
        }

        // POST: TasksModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(TaskViewModel tasksModel)
        {
            bool status = false;

            if (ModelState.IsValid)
            {
                using (db)
                {
                    var MyTask = db.TaskModels.Find(tasksModel.TaskId);
                    MyTask.Heading = tasksModel.Heading;
                    MyTask.Content = tasksModel.Content;
                    MyTask.StartDate = tasksModel.StartDate;
                    MyTask.EndDate = tasksModel.EndDate;
                    MyTask.CreatedBy = tasksModel.CreatedBy;
                    MyTask.CreatedOn = tasksModel.CreatedOn;
                    MyTask.Status = tasksModel.Status;

                    db.SaveChanges();

                    foreach (var item in db.TaskToUsers)
                    {
                        if (item.TaskId == tasksModel.TaskId)
                        {
                            db.Entry(item).State = EntityState.Deleted;
                            db.SaveChanges();
                        }
                    }

                    foreach (var item in tasksModel.Users)
                    {
                        if (item.Checked)
                        {
                            db.TaskToUsers.Add(new TaskToUser() { UserIdInt = MyTask.Id, TaskId = item.Id });
                        }
                    }
                    //return RedirectToAction("Index");
                    db.SaveChanges();
                    status = true;
                }
            }
            //return View(tasksModel);            
            return new JsonResult { Data = new { status = status } };
        }

        // GET: TasksModels/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TasksModel tasksModel = db.TaskModels.Find(id);
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
            bool status = false;
            using (db)
            {
                TasksModel tasksModel = db.TaskModels.Find(id);
                if (tasksModel != null)
                {
                    db.TaskModels.Remove(tasksModel);
                    db.SaveChanges();
                    status = true;
                }
            }
            //return RedirectToAction("Index");            
            return new JsonResult { Data = new { status = status } };
        }

        public ActionResult GetData()
        {
            DateTime now = DateTime.Now;
            var tor = db.TaskModels.Select(s => new
            {
                s.Heading,
                s.Id,
                s.EndDate,
                s.StartDate,
                s.Content,
                s.CreatedBy,
                s.CreatedOn,
                Status = s.EndDate > now && s.StartDate<now ? "Open" : "Closed"
            }).ToList();
            return Json(new { data = tor }, JsonRequestBehavior.AllowGet);
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
