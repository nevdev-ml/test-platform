﻿using Application.Interfaces;
using Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Web.Mvc;
using web_application_mvc.Models;

namespace web_application_mvc.Controllers
{
    //[Authorize(Roles = "Администратор")]
    public class StatisticsController : Controller
    {
        IUserService userService;
        IActivityService activityService;
        IGradeService gradeService;
        IUserTaskService userTaskService;
        ITaskService taskService;

        public StatisticsController(IUserService userService, IActivityService activityService, IGradeService gradeService,
            IUserTaskService userTaskService, ITaskService taskService)
        {
            this.userService = userService;
            this.activityService = activityService;
            this.gradeService = gradeService;
            this.userTaskService = userTaskService;
            this.taskService = taskService;
        }

        // GET: Statistics
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Report(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = userService.Get(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ReportViewModel model = new ReportViewModel()
            {
                User = user,
                Activities = activityService.GetAll().Where(x => x.UserID == id),
                Grades = gradeService.GetAll().Where(x => x.UserID == id),
                Tasks = userTaskService.GetAll().Where(x => x.UserID == id)
                    .Select(x => new ExtentionTaskViewModel
                    {
                        ID = x.ID,
                        Answer = x.Answer,
                        Comment = x.Comment,
                        Grade = x.Grade,
                        Task = taskService.Get(x.TaskID),
                        User = userService.Get(x.UserID)
                    })
            };
            return View(model);
        }

        [HttpPost]
        public ActionResult Search(string query)
        {
            ViewBag.Query = query;
            List<User> result = new List<User>();
            foreach (var item in userService.GetAll())
            {
                if(PropertiesThatContainText(item, query))
                {
                    result.Add(item);
                }
            }
            return View(result);
        }

        public static bool PropertiesThatContainText<T>(T obj, string text,
            StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            var properties = typeof(T).GetProperties(BindingFlags.Public | BindingFlags.Instance)
               .Where(p => (p.PropertyType == typeof(string) || p.PropertyType == typeof(int) ||
                    p.PropertyType == typeof(int?)) && p.CanRead);
            foreach (PropertyInfo prop in properties)
            {
                string propVal = prop.GetValue(obj, null)?.ToString();
                if (propVal != null && propVal.IndexOf(text, 0, comparison) != -1)
                {
                    return true;
                }
            }
            return false;
        }
    }
}