﻿using System;
using System.Collections.Generic;
using System.Web.Mvc;
using WebApplication1.Models;

namespace DeadLiner.Models
{
    public class TaskAnswerView
    {
        public int TaskId { get; set; }
        public string Heading { get; set; }
        public string Content { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public string Status { get; set; }

        //public  List<ReplyToTask> Replies { get; set; }

        public List<TeacherShowAnswerUsers> teacherShowAnswerUsersesView = new List<TeacherShowAnswerUsers>();
        public List<teacherReplyToTaskView> teacherReplyToTaskViews = new List<teacherReplyToTaskView>();        
    }
}