using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SolarlabTest.Models;

namespace SolarlabTest.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index(string FilterType)
        {
            UserTaskContext db = new UserTaskContext();
            var CurrentDate = DateTime.Now.Date;
            var EndDate = CurrentDate.AddDays(7);
            IQueryable<UserTask> Groups;
            int QueryTasksNumber;
            if (FilterType == "Today")
            {
                Groups = db.UserTasks.Where(c => c.DeadlineDate == CurrentDate);
                QueryTasksNumber = db.UserTasks.Where(c => c.DeadlineDate == CurrentDate).Count();
                ViewBag.PageName = "Задачи на сегодня";
            }
            else if (FilterType == "Week")
            {
                Groups = db.UserTasks.Where(c => c.DeadlineDate >= CurrentDate && c.DeadlineDate <= EndDate);
                QueryTasksNumber = db.UserTasks.Where(c => c.DeadlineDate >= CurrentDate && c.DeadlineDate <= EndDate).Count();
                ViewBag.PageName = "Задачи на cледующие 7 дней";
            }
            else if (FilterType == "Archive")
            {
                Groups = db.UserTasks.Where(c => c.DeadlineDate < CurrentDate);
                QueryTasksNumber = db.UserTasks.Where(c => c.DeadlineDate < CurrentDate).Count();
                ViewBag.PageName = "Архив задач";
            }
            else
            {
                Groups = db.UserTasks.Where(c => c.DeadlineDate >= CurrentDate);
                QueryTasksNumber = db.UserTasks.Where(c => c.DeadlineDate >= CurrentDate).Count();
                ViewBag.PageName = "Входящие задачи";
            }
            int InboxTasksNumber = db.UserTasks.Where(c => c.DeadlineDate >= CurrentDate).Count();
            int DoneInboxTasksNumber = db.UserTasks.Where(c => c.TaskDone == true && c.DeadlineDate >= CurrentDate).Count();
            IEnumerable<UserTask> TodayTasks = db.UserTasks.Where(c => c.DeadlineDate == CurrentDate);
            IEnumerable<UserTask> DoneTodayTasks = db.UserTasks.Where(c => c.DeadlineDate == CurrentDate && c.TaskDone == true);
            IEnumerable<UserTask> WeekTasks = db.UserTasks.Where(c => c.DeadlineDate >= CurrentDate && c.DeadlineDate <= EndDate);
            IEnumerable<UserTask> DoneWeekTasks = db.UserTasks.Where(c => c.DeadlineDate >= CurrentDate && c.DeadlineDate <= EndDate && c.TaskDone == true);
            IEnumerable<UserTask> ArchiveTasks = db.UserTasks.Where(c => c.DeadlineDate < CurrentDate);
            IEnumerable<UserTask> DoneArchiveTasks = db.UserTasks.Where(c => c.DeadlineDate < CurrentDate && c.TaskDone == true);
            ViewBag.DoneInboxTasksNumber = DoneInboxTasksNumber;
            ViewBag.InboxTasksNumber = InboxTasksNumber;
            ViewBag.DoneTodayTasksNumber = DoneTodayTasks.Count();
            ViewBag.TodayTasksNumber = TodayTasks.Count();
            ViewBag.DoneWeekTasksNumber = DoneWeekTasks.Count();
            ViewBag.WeekTasksNumber = WeekTasks.Count();
            ViewBag.DoneArchiveTasksNumber = DoneArchiveTasks.Count();
            ViewBag.ArchiveTasksNumber = ArchiveTasks.Count();
            ViewBag.QueryTasksNumber = QueryTasksNumber;
            ViewBag.Groups = Groups.GroupBy(p => p.DeadlineDate);
            ViewBag.FilterType = FilterType;
            return View();
        }

        [HttpGet]
        public ActionResult DeleteTask(int Id, string FilterType)
        {
            using (UserTaskContext db = new UserTaskContext())
            {
                UserTask task = db.UserTasks.Where(c => c.Id == Id).FirstOrDefault();
                db.UserTasks.Attach(task);
                db.UserTasks.Remove(task);
                db.SaveChanges();
            }
            if (FilterType == "")
            {
                return Redirect("/Home/Index");
            }
            else
            {
                return Redirect("/Home/Index/?FilterType=" + FilterType);
            }
        }

        [HttpGet]
        public ActionResult ChangeTaskState(int Id, string FilterType)
        {
            using (UserTaskContext db = new UserTaskContext())
            {
                UserTask task = db.UserTasks.Where(c => c.Id == Id).FirstOrDefault();
                task.TaskDone = !task.TaskDone;
                db.SaveChanges();
            }
            if (FilterType == "")
            {
                return Redirect("/Home/Index");
            }
            else
            {
                return Redirect("/Home/Index/?FilterType=" + FilterType);
            }
        }

        [HttpGet]
        public ActionResult AddTask(string TaskName, string DeadlineDate, string DeadlineTime, string HighPriority, string FilterType)
        {
            bool HighTaskPriority = (HighPriority == "True") ? true : false;
            DateTime Date = Convert.ToDateTime(DeadlineDate);
            using (UserTaskContext db = new UserTaskContext())
            {
                UserTask task = new UserTask { TaskName = TaskName, TaskDone = false, HighTaskPriority = HighTaskPriority, DeadlineDate = Date, DeadlineTime = DeadlineTime };
                db.UserTasks.Add(task);
                db.SaveChanges();
            }
            if (FilterType == "")
            {
                return Redirect("/Home/Index");
            }
            else
            {
                return Redirect("/Home/Index/?FilterType=" + FilterType);
            }
        }

        [HttpGet]
        public ActionResult ChangeTask(int TaskId, string TaskName, string DeadlineDate, string DeadlineTime, string HighPriority, string FilterType)
        {
            bool HighTaskPriority = (HighPriority == "True") ? true : false;
            DateTime Date = Convert.ToDateTime(DeadlineDate);

            using (UserTaskContext db = new UserTaskContext())
            {
                UserTask task = db.UserTasks.Where(c => c.Id == TaskId).FirstOrDefault();
                task.TaskName = TaskName;
                task.HighTaskPriority = HighTaskPriority;
                task.DeadlineDate = Date;
                task.DeadlineTime = DeadlineTime;
                db.SaveChanges();
            }
            if (FilterType == "")
            {
                return Redirect("/Home/Index");
            }
            else
            {
                return Redirect("/Home/Index/?FilterType=" + FilterType);
            }

        }

        [HttpGet]
        public JsonResult GetTaskInfo(int Id)
        {
            UserTaskContext db = new UserTaskContext();
            UserTask task = db.UserTasks.Where(c => c.Id == Id).FirstOrDefault();
            return Json(task, JsonRequestBehavior.AllowGet);
        }
    }
}