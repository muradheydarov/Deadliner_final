using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeadLiner.Models;

namespace WebApplication1.Controllers
{
    public class TasksModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

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
            MyViewModel.Header = tasksModel.Heading;
            MyViewModel.Content = tasksModel.Content;

            var MyCheckBoxList = new List<CheckBoxViewModel>();

            foreach (var item in result)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel { Id = item.ApplicationUserId, Name = item.UserName, Checked = item.Checked });
            }

            MyViewModel.Users = MyCheckBoxList;


            return View(MyViewModel);
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
                var newTask = new TasksModel() { Id = tasksModel.TaskId, Heading = tasksModel.Header, Content = tasksModel.Content };

                db.TaskModels.Add(newTask);

                db.SaveChanges(); // user id is null

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
            MyViewModel.Header = tasksModel.Heading;
            MyViewModel.Content = tasksModel.Content;

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
            if (ModelState.IsValid)
            {
                var MyTask = db.TaskModels.Find(tasksModel.TaskId);
                MyTask.Heading = tasksModel.Header;
                MyTask.Content = tasksModel.Content;

                foreach (var item in db.TaskToUsers)
                {
                    if (item.UserIdInt == tasksModel.TaskId)
                    {
                        db.Entry(item).State = EntityState.Deleted;
                    }
                }

                foreach (var item in tasksModel.Users)
                {
                    if (item.Checked)
                    {
                        db.TaskToUsers.Add(new TaskToUser() { UserIdInt = item.Id, TaskId = tasksModel.TaskId });
                    }
                }
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
            TasksModel tasksModel = db.TaskModels.Find(id);
            db.TaskModels.Remove(tasksModel);
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
