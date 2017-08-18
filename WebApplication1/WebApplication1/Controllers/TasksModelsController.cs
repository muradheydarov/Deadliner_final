using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
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
                             UserName = a.Name + " " + a.Surname,
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
                    EndDate = tasksModel.EndDate,
                    Status = tasksModel.Status
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
                             UserName = a.Name +" "+a.Surname,
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
                db.SaveChanges();
                status = true;
            }
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

            List<TaskViewModel> list = db.TasksModels
                .Join(db.TaskToUsers, t => t.TasksModelID, user => user.TasksModelID, (t, user) => new {t, user})
                .Where(@t1 => @t1.user.UserIdInt == userid.ApplicationUserId)
                .Select(@t1 => new TaskViewModel()
                {
                    EndDate = @t1.t.EndDate,
                    StartDate = @t1.t.StartDate,
                    Status = @t1.t.EndDate > now && @t1.t.StartDate < now ? "Open" : "Closed"
                }).Where(x => x.Status == "Open").ToList();

            var userID = User.Identity.GetUserId();
            bool userExists = db.Users.Any(x => x.Id == userID);
            List<LoadFileViewModel> ufls = new List<LoadFileViewModel>();
            if (userExists)
            {                
                var userfiles = db.UserFileses.Where(x => x.UserId == userID).ToList();
                foreach (var userFile in userfiles)
                {
                    string type = null;
                    int index = userFile.FileType.IndexOf('/');
                    if (index > 0) { type = userFile.FileType.Substring(0, index); }
                    ufls.Add(new LoadFileViewModel() { FileName = userFile.FileName, FileType = type, Id = userFile.Id });
                }
            }
            var LoginPartialView = new _LoginPartialView();
            LoginPartialView.TaskCount = list.Count;
            LoginPartialView.UploadImg = ufls;
            if (User.IsInRole("Teacher"))
            {
                return PartialView("~/Views/Shared/_LoginPartialForTeacher.cshtml", LoginPartialView);
            }
            return PartialView("~/Views/Shared/_LoginPartial.cshtml", LoginPartialView);
        }

        //GET
        public ActionResult ReplyToTask(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userid = db.Users.Find(User.Identity.GetUserId()).ApplicationUserId;

            var taskToUseId = db.TaskToUsers.FirstOrDefault(x => x.UserIdInt == userid && x.TasksModelID == id).TaskToUserID;

            if (db.ReplyToTasks.FirstOrDefault(x => x.TaskToUserID == taskToUseId) != null)
            {
                var replyToTaskDefault = db.ReplyToTasks.FirstOrDefault(x => x.TaskToUserID == taskToUseId);
                return View(replyToTaskDefault);
            }

            var replyToTask = new ReplyToTask();
            replyToTask.TaskToUserID = taskToUseId;
            return View(replyToTask);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReplyToTask(ReplyToTask taskToUser)
        {
            bool status = false;

            if (ModelState.IsValid)
            {
                if (taskToUser.ReplyToTaskId > 0)
                {
                    taskToUser.AnswerTime = DateTime.Now;
                    db.Entry(taskToUser).State = EntityState.Modified;
                }
                else
                {
                    taskToUser.AnswerTime = DateTime.Now;
                    db.ReplyToTasks.Add(taskToUser);
                }

                db.SaveChanges();
                status = true;
            }
            return new JsonResult { Data = new { status = status } };
        }

        //GET
        public ActionResult TeacherShowAnswer()
        {
            List<TaskAnswerView> taskAnswer = new List<TaskAnswerView>();
            return View(taskAnswer);
        }

        //GET DATA
        public ActionResult GetDataTeacherShowAnswer()
        {
            var now = DateTime.Now;
            var data = db.TasksModels.Select(s => new
            {
                s.Heading,
                s.StartDate,
                s.EndDate,
                s.CreatedBy,
                s.CreatedOn,
                Status = s.EndDate > now && s.StartDate < now ? "Open" : "Closed",

                ttu = s.TaskToUsers.Select(t => new
                {
                    reply = t.ReplyToTasks.Select(r => new
                    {
                        answer = r.UserAnswer,
                        answerTime = r.AnswerTime,
                    }),
                    user = db.Users.Where(f => f.ApplicationUserId == t.UserIdInt).Select(u => new
                    {
                        fullName = u.Name + " " + u.Surname
                    })
                })
            }).ToList();

            return Json(new { data = data }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult IndividualUser(ReplyToTask taskToUser)
        {
            var now = DateTime.Now;
            var user = db.Users.Find(User.Identity.GetUserId());
            var list = db.TasksModels
                .Join(db.TaskToUsers, t => t.TasksModelID, usr => usr.TasksModelID, (t, usr) => new { t, usr })
                .Where(t1 => t1.usr.UserIdInt == user.ApplicationUserId)
                .Select(t1 => new
                {
                    t1.t.Heading,
                    t1.t.TasksModelID,
                    t1.t.EndDate,
                    t1.t.StartDate,
                    t1.t.Content,
                    t1.t.CreatedBy,
                    t1.t.CreatedOn,
                    Status = t1.t.EndDate > now && t1.t.StartDate < now ? "Open" : "Closed",

                    ttu = t1.t.TaskToUsers.Select(t => new
                    {
                        reply = t.ReplyToTasks.Where(x => x.TaskToUser.UserIdInt == user.ApplicationUserId).Select(r => new
                        {
                            answer = r.UserAnswer,
                            answerTime = r.AnswerTime,
                        }),
                        user = db.Users.Where(f => f.ApplicationUserId == user.ApplicationUserId).Select(u => new
                        {
                            fullName = u.Name + " " + u.Surname
                        })
                    })
                }).ToList();
            return Json(new { data = list }, JsonRequestBehavior.AllowGet);
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
