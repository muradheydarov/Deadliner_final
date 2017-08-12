﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DeadLiner.Models;
using Microsoft.AspNet.Identity;

namespace WebApplication1.Controllers
{
    public class TasksModelsController : Controller
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET
        public ActionResult MyTasks()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            var list = from a in db.TasksModels
                       join usr in db.TaskToUsers on a.TasksModelID equals usr.TaskToUserID
                       where usr.UserIdInt == user.ApplicationUserId
                       select a;

            return View(list);
        }

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
            var result = from a in db.Users
                         select new
                         {
                             a.ApplicationUserId,
                             a.UserName,
                             Checked = (from ab in db.TaskToUsers
                                        where (ab.UserIdInt == a.ApplicationUserId) & (ab.TasksModelID == id)
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

            var MyCheckBoxList = new List<CheckBoxViewModel>();

            foreach (var item in result)
            {
                MyCheckBoxList.Add(new CheckBoxViewModel
                {
                    Id = item.ApplicationUserId,
                    Name = item.UserName,
                    Checked = item.Checked
                });
            }

            MyViewModel.Users = MyCheckBoxList;
            return View(MyViewModel);
            // return Json(new { data = MyViewModel }, JsonRequestBehavior.AllowGet);
        }

        // GET: TasksModels/Create
        public ActionResult Create()
        {
            var result = from a in db.Users.Where(x => x.UserStatus == "Student")
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
                MyCheckBoxList.Add(new CheckBoxViewModel
                {
                    Id = item.ApplicationUserId,
                    Name = item.UserName,
                    Checked = item.Checked,
                });
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
                var newTask = new TasksModel()
                {
                    TasksModelID = tasksModel.TaskId,
                    Heading = tasksModel.Heading,
                    Content = tasksModel.Content,
                    StartDate = tasksModel.StartDate,
                    CreatedBy = User.Identity.GetUserName(),
                    CreatedOn = DateTime.Now,
                    EndDate = tasksModel.EndDate
                };

                db.TasksModels.Add(newTask);

                db.SaveChanges();

                foreach (var item in tasksModel.Users)
                {
                    if (item.Checked)
                    {
                        db.TaskToUsers.Add(new TaskToUser() { UserIdInt = item.Id, TasksModelID = newTask.TasksModelID });
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
            TasksModel tasksModel = db.TasksModels.Find(id);
            if (tasksModel == null)
            {
                return HttpNotFound();
            }

            var result = from a in db.Users.Where(x => x.UserStatus == "Student")
                         select new
                         {
                             a.ApplicationUserId,
                             a.UserName,
                             Checked = (from ab in db.TaskToUsers
                                        where (ab.UserIdInt == a.ApplicationUserId) & (ab.TasksModelID == id)
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
                MyCheckBoxList.Add(new CheckBoxViewModel
                {
                    Id = item.ApplicationUserId,
                    Name = item.UserName,
                    Checked = item.Checked
                });
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
                var MyTask = db.TasksModels.Find(tasksModel.TaskId);
                MyTask.Heading = tasksModel.Heading;
                MyTask.Content = tasksModel.Content;
                MyTask.StartDate = tasksModel.StartDate;
                MyTask.EndDate = tasksModel.EndDate;
                MyTask.CreatedBy = tasksModel.CreatedBy;
                MyTask.CreatedOn = tasksModel.CreatedOn;
                MyTask.Status = tasksModel.Status;

                foreach (var item in db.TaskToUsers)
                {
                    if (item.TasksModelID == tasksModel.TaskId)
                    {
                        db.Entry(item).State = EntityState.Deleted;
                    }
                }

                foreach (var item in tasksModel.Users)
                {
                    if (item.Checked)
                    {
                        db.TaskToUsers.Add(
                            new TaskToUser() { UserIdInt = item.Id, TasksModelID = tasksModel.TaskId });
                    }
                }
                //return RedirectToAction("Index");
                db.SaveChanges();
                status = true;
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
            bool status = false;
            using (db)
            {
                TasksModel tasksModel = db.TasksModels.Find(id);
                if (tasksModel != null)
                {
                    db.TasksModels.Remove(tasksModel);
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
            var tor = db.TasksModels.Select(s => new
            {
                s.Heading,
                s.TasksModelID,
                s.EndDate,
                s.StartDate,
                s.Content,
                s.CreatedBy,
                s.CreatedOn,
                Status = s.EndDate > now && s.StartDate < now ? "Open" : "Closed"
            }).ToList();
            return Json(new { data = tor }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult GetDataMyTasks()
        {
            var user = db.Users.Find(User.Identity.GetUserId());
            DateTime now = DateTime.Now;

            var list = from t in db.TasksModels
                       join usr in db.TaskToUsers on t.TasksModelID equals usr.TasksModelID
                       where usr.UserIdInt == user.ApplicationUserId
                       select new
                       {
                           t.Heading,
                           t.TasksModelID,
                           t.EndDate,
                           t.StartDate,
                           t.Content,
                           t.CreatedBy,
                           t.CreatedOn,
                           Status = t.EndDate > now && t.StartDate < now ? "Open" : "Closed"
                       };

            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
        }

        [ChildActionOnly]
        public ActionResult SendDataForNotify()
        {
            var userid = db.Users.Find(User.Identity.GetUserId());
            DateTime now = DateTime.Now;

            List<TaskViewModel> list = (from t in db.TasksModels
                                        join user in db.TaskToUsers on t.TasksModelID equals user.TasksModelID
                                        where user.UserIdInt == userid.ApplicationUserId
                                        select new TaskViewModel()
                                        {
                                            EndDate = t.EndDate,
                                            StartDate = t.StartDate,
                                            Status = t.EndDate > now && t.StartDate < now ? "Open" : "Closed"
                                        }).Where(x => x.Status == "Open").ToList();

            return PartialView("~/Views/Shared/_LoginPartial.cshtml", list.Count);
        }

        public ActionResult ReplyToTask(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var userid = db.Users.Find(User.Identity.GetUserId()).ApplicationUserId;
            var taskToUse = db.TaskToUsers.Where(x => x.UserIdInt == userid && x.TasksModelID == id);
            dynamic taskToUseId = "";
            foreach (var item in taskToUse)
            {
                taskToUseId = item.TaskToUserID;
            }

            TaskToUser tasksToUser = db.TaskToUsers.Find(taskToUseId);
            if (tasksToUser == null)
            {
                return HttpNotFound();
            }

            var taskToUserViewModel = new TaskToUserViewModel();
            taskToUserViewModel.TaskToUserViewModelId = tasksToUser.TaskToUserID;
            taskToUserViewModel.StudentReply = tasksToUser.StudentReply;
            taskToUserViewModel.AnswerTime = tasksToUser.AnswerTime;
            taskToUserViewModel.TasksModel = tasksToUser.TasksModel;
            taskToUserViewModel.User = tasksToUser.User;

            return View(taskToUserViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReplyToTask(TaskToUserViewModel taskToUser)
        {
            bool status = false;

            if (ModelState.IsValid)
            {
                var MyTaskToUser = db.TaskToUsers.Find(taskToUser.TaskToUserViewModelId);
                MyTaskToUser.StudentReply = taskToUser.StudentReply;
                MyTaskToUser.AnswerTime = DateTime.Now;
                MyTaskToUser.UserIdInt = taskToUser.UserIdInt;
                MyTaskToUser.TasksModelID = taskToUser.TaskModelID;
                MyTaskToUser.User = taskToUser.User;
                MyTaskToUser.TasksModel = taskToUser.TasksModel;

                //return RedirectToAction("Index");
                db.SaveChanges();
                status = true;
            }
            //return View(tor);            
            return new JsonResult { Data = new { status = status } };
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
