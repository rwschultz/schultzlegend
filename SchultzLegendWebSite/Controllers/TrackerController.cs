using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SchultzLegendWebSite.Models;
using SchultzLegendWebSite.Utilities;
using SchultzLegendWebSite.Filters;

namespace SchultzLegendWebSite.Controllers
{
    [Authorize]
    [HttpsFilter]
    public class TrackerController : BaseController
    {
        #region Actions
        // GET: Tracker
        public ActionResult Index()
        {
            TrackerViewModel model = new TrackerViewModel();
            model.CurrentPage = new Page("Summary", "/Tracker");
            model.UserId = DBHelper.DBGetUserLogin(User.Identity.Name).Id;

            List<Task> tasks = DBHelper.DBGetTasks(model.UserId);

            model.TaskTotal = tasks.Count;
            model.TaskComplete = tasks.Where(x => x.Complete == 100).ToList().Count;

            return View(model);
        }

        // GET: Tracker
        public ActionResult Tasks()
        {
            TaskViewModel model = new TaskViewModel();
            model.UserId = DBHelper.DBGetUserLogin(User.Identity.Name).Id;
            model.Tasks = GetOrderedTasks(model.UserId);
            model.CurrentPage = new Page("Tasks", "/Tracker/Tasks");
            return View(model);
        }

        [HttpPost]
        public JsonResult TaskAction(string type, string text, int[] taskids, int parent, sbyte status)
        {
            string userid = DBHelper.DBGetUserLogin(User.Identity.Name).Id;
            if (type == "new")
            {
                DBHelper.DBAddTask(userid, text, parent);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else if(type == "change")
            {
                DBHelper.DBChangeTaskName(userid, taskids[0], text);
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else if(type == "delete")
            {
                foreach (int taskid in taskids)
                {
                    DBHelper.DBDeleteTask(userid, taskid);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            else if(type == "get")
            {
                Task task = DBHelper.DBGetTaskByName(text);
                return Json(task.TaskId, JsonRequestBehavior.AllowGet);
            }
            else if(type == "status")
            {
                foreach (int taskid in taskids)
                {
                    DBHelper.DBChangeTaskStatus(userid, taskid, status);
                }
                return Json(true, JsonRequestBehavior.AllowGet);
            }

            return Json(false, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ComingSoon(string pageName = "")
        {
            TrackerViewModel model = new TrackerViewModel();
            model.CurrentPage = new Page(pageName, "/Tracker/ComingSoon");
            return View(model);
        }

        #endregion

        #region Public Methods
        public static List<Page> GetTrackerPages()
        {
            return DBHelper.DBGetTrackerPages();
        }
        #endregion

        #region Private Methods
        private List<Task> GetOrderedTasks(string userId)
        {
            List<Task> unorderedTasks = DBHelper.DBGetTasks(userId);
           
            foreach(Task task in unorderedTasks.Where(x => x.Parent != 0).ToList())
            {
                if(task.Parent != 0)
                {
                    unorderedTasks.First(x => x.TaskId == task.Parent).Children.Add(task);
                }
            }
            unorderedTasks.RemoveAll(x => x.Parent != 0);
            return unorderedTasks;
        }
        #endregion
    }
}