using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using DeadLiner.Models;
using Microsoft.Ajax.Utilities;
using Microsoft.AspNet.Identity;

namespace WebApplication1.Controllers
{
    public class TasksModelsController : Controller
    {
        private readonly ApplicationDbContext db = new ApplicationDbContext();

        // GET
        public ActionResult MyTasks()
        {
            var user = db.Users.Find(User.Identity.GetUserId());

            var list = db.TasksModels
                .Join(db.TaskToUsers, a => a.TasksModelID, usr => usr.TaskToUserID, (a, usr) => new { a, usr })
                .Where(@t => @t.usr.UserIdInt == user.ApplicationUserId)
                .Select(@t => @t.a);

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
            var result = db.Users.Select(a => new
            {
                a.ApplicationUserId,
                UserName = a.Name + " " + a.Surname,
                Checked = db.TaskToUsers.Any(ab => (ab.UserIdInt == a.ApplicationUserId) & (ab.TasksModelID == id))
            });

            var MyViewModel = new TaskViewModel
            {
                TaskId = id.Value,
                Heading = tasksModel.Heading,
                Content = tasksModel.Content,
                CreatedBy = tasksModel.CreatedBy,
                CreatedOn = tasksModel.CreatedOn,
                StartDate = tasksModel.StartDate,
                EndDate = tasksModel.EndDate
            };

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
            if (User.IsInRole("Student"))
            {
                return View("DetailsUser", MyViewModel);                
            }
            return View("Details", MyViewModel);
        }

        // GET: TasksModels/Create
        public ActionResult Create()
        {
            var result = db.Users.Where(x => x.UserStatus == "Student")
                .Select(a => new
                {
                    a.ApplicationUserId,
                    a.UserName,
                    Checked = (from ab in db.TaskToUsers select ab).Any()
                });

            var MyViewModel = new TaskViewModel();

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

        // POST: TasksModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
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

            var result = db.Users.Where(x => x.UserStatus == "Student")
                .Select(a => new
                {
                    a.ApplicationUserId,
                    UserName = a.Name + " " + a.Surname,
                    Checked = db.TaskToUsers.Any(ab => (ab.UserIdInt == a.ApplicationUserId) &
                                                         (ab.TasksModelID == id))
                });

            var MyViewModel = new TaskViewModel
            {
                TaskId = id.Value,
                Heading = tasksModel.Heading,
                Content = tasksModel.Content,
                CreatedBy = tasksModel.CreatedBy,
                CreatedOn = tasksModel.CreatedOn,
                StartDate = tasksModel.StartDate,
                EndDate = tasksModel.EndDate,
                Status = tasksModel.Status
            };

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
        [ValidateInput(false)]
        public ActionResult Edit(TaskViewModel tasksModel)
        {
            bool status = false;

            if (ModelState.IsValid)
            {
                var MyTask = db.TasksModels.Find(tasksModel.TaskId);
                if (MyTask != null)
                {
                    MyTask.Heading = tasksModel.Heading;
                    MyTask.Content = tasksModel.Content;
                    MyTask.StartDate = tasksModel.StartDate;
                    MyTask.EndDate = tasksModel.EndDate;
                    MyTask.CreatedBy = tasksModel.CreatedBy;
                    MyTask.CreatedOn = tasksModel.CreatedOn;
                    MyTask.Status = tasksModel.Status;
                }

                foreach (var item in tasksModel.Users)
                {
                    foreach (var item2 in db.TaskToUsers)
                    {
                        if (item2.UserIdInt == item.Id && item2.TasksModelID == tasksModel.TaskId && !item.Checked)
                        {
                            db.Entry(item2).State = EntityState.Deleted;
                        }
                    }
                    if (item.Checked)
                    {
                        if (!db.TaskToUsers.Any(x => x.UserIdInt == item.Id & x.TasksModelID == tasksModel.TaskId))
                        {
                            db.TaskToUsers.Add(new TaskToUser() { UserIdInt = item.Id, TasksModelID = tasksModel.TaskId });
                        }
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
            var result = db.Users.Select(a => new
            {
                a.ApplicationUserId,
                UserName = a.Name + " " + a.Surname,
                Checked = (db.TaskToUsers.Where(ab => (ab.UserIdInt == a.ApplicationUserId) & (ab.TasksModelID == id))).Any()
            });

            var MyViewModel = new TaskViewModel
            {
                TaskId = id.Value,
                Heading = tasksModel.Heading,
                Content = tasksModel.Content,
                CreatedBy = tasksModel.CreatedBy,
                CreatedOn = tasksModel.CreatedOn,
                StartDate = tasksModel.StartDate,
                EndDate = tasksModel.EndDate
            };

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

        // POST: TasksModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var status = false;
            using (db)
            {
                var tasksModel = db.TasksModels.Find(id);
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
            var now = DateTime.Now;
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
            var now = DateTime.Now;

            var list = db.TasksModels
                .Join(db.TaskToUsers, t => t.TasksModelID, user => user.TasksModelID, (t, user) => new { t, user })
                .Where(@t1 => @t1.user.UserIdInt == userid.ApplicationUserId)
                .Select(@t1 => new TaskViewModel()
                {
                    EndDate = @t1.t.EndDate,
                    StartDate = @t1.t.StartDate,
                    Status = @t1.t.EndDate > now && @t1.t.StartDate < now ? "Open" : "Closed"
                }).Where(x => x.Status == "Open").ToList();

            var userID = User.Identity.GetUserId();
            var userExists = db.Users.Any(x => x.Id == userID);
            var ufls = new List<LoadFileViewModel>();
            if (userExists)
            {
                var userfiles = db.UserFileses.Where(x => x.UserId == userID).ToList();
                foreach (var userFile in userfiles)
                {
                    string type = null;
                    var index = userFile.FileType.IndexOf('/');
                    if (index > 0) { type = userFile.FileType.Substring(0, index); }
                    ufls.Add(new LoadFileViewModel() { FileName = userFile.FileName, FileType = type, Id = userFile.Id });
                }
            }
            var LoginPartialView = new _LoginPartialView
            {
                TaskCount = list.Count,
                UploadImg = ufls
            };
            return PartialView(User.IsInRole("Teacher") ? "~/Views/Shared/_LoginPartialForTeacher.cshtml" : "~/Views/Shared/_LoginPartial.cshtml", LoginPartialView);
        }

        //GET
        public ActionResult ReplyToTask(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            var userid = db.Users.Find(User.Identity.GetUserId()).ApplicationUserId;

            var taskToUser = db.TaskToUsers.FirstOrDefault(x => x.UserIdInt == userid && x.TasksModelID == id);
            if (taskToUser != null)
            {
                var taskToUseId = taskToUser.TaskToUserID;

                if (db.ReplyToTasks.FirstOrDefault(x => x.TaskToUserID == taskToUseId) != null)
                {
                    var replyToTaskDefault = db.ReplyToTasks.FirstOrDefault(x => x.TaskToUserID == taskToUseId);
                    return View(replyToTaskDefault);
                }

                var replyToTask = new ReplyToTask { TaskToUserID = taskToUseId };
                return View(replyToTask);
            }
            return HttpNotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ReplyToTask(ReplyToTask taskToUser)
        {
            var status = false;

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
            var taskAnswer = new List<TaskAnswerView>();
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

        public ActionResult IndividualUser()
        {
            ViewBag.Users = db.Users.Where(x => x.UserStatus=="Student").Select(x => new SelectListItem
            {
                Text = x.UserName,
                Value = x.Id
            });
            var taskAnswer = new List<TaskAnswerView>();
            return View(taskAnswer);
        }

        public ActionResult IndividualUserGetData(string id)
        {            
            var now = DateTime.Now;
            if (!id.IsNullOrWhiteSpace())
            {                
                var applicationUser = db.Users.Find(id);

                var userId = applicationUser.Id;
                var user = db.Users.Find(userId);
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

                        ttu = t1.t.TaskToUsers.Where(x => x.UserIdInt==applicationUser.ApplicationUserId).Select(t => new
                        {
                            reply = t.ReplyToTasks.Select(r => new
                            {
                                answer = r.UserAnswer,
                                answerTime = r.AnswerTime,
                            }),
                            user = db.Users.Select(u => new
                            {
                                fullName = u.Name + " " + u.Surname
                            })
                        })
                    }).ToList();                
                return Json(new { data = list }, JsonRequestBehavior.AllowGet);
            }
            else
            {                
                var list = db.TasksModels.Select(s => new
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

                return Json(new { data = list }, JsonRequestBehavior.AllowGet);
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
