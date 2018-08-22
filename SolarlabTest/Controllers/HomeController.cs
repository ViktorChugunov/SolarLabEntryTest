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
        public ActionResult Index(string filterType)
        {
            using (UserTaskContext db = new UserTaskContext()) {
                var viewModel = GetTasksViewModelData(db, filterType);
                ViewBag.filterType = filterType;
                return View(viewModel);
            }
        }

        public TasksViewModel GetTasksViewModelData(UserTaskContext db, string filterType)
        {
            // Получение значений количества выполненных задач и общего кол-ва задач в различных категориях 
            DateTime currentDate = DateTime.Now.Date;
            DateTime endDate = currentDate.AddDays(7);
            int inboxTasksNumber = db.UserTasks.Where(c => c.DeadlineDate >= currentDate).Count();
            int doneInboxTasksNumber = db.UserTasks.Where(c => c.TaskDone == true && c.DeadlineDate >= currentDate).Count();
            int todayTasksNumber = db.UserTasks.Where(c => c.DeadlineDate == currentDate).Count();
            int doneTodayTasksNumber = db.UserTasks.Where(c => c.DeadlineDate == currentDate && c.TaskDone == true).Count();
            int weekTasksNumber = db.UserTasks.Where(c => c.DeadlineDate >= currentDate && c.DeadlineDate <= endDate).Count();
            int doneWeekTasksNumber = db.UserTasks.Where(c => c.DeadlineDate >= currentDate && c.DeadlineDate <= endDate && c.TaskDone == true).Count();
            int archiveTasksNumber = db.UserTasks.Where(c => c.DeadlineDate < currentDate).Count();
            int doneArchiveTasksNumber = db.UserTasks.Where(c => c.DeadlineDate < currentDate && c.TaskDone == true).Count();
            // Получение значений "Имя страницы", "Количества задач", "Групп задач"
            string pageName;
            int queryTasksNumber;
            IEnumerable<IGrouping<DateTime, UserTask>> groups;
            if (filterType == "Today")
            {
                groups = db.UserTasks.Where(c => c.DeadlineDate == currentDate).GroupBy(p => p.DeadlineDate).ToList();
                queryTasksNumber = db.UserTasks.Where(c => c.DeadlineDate == currentDate).Count();
                pageName = "Задачи на сегодня";
            }
            else if (filterType == "Week")
            {
                groups = db.UserTasks.Where(c => c.DeadlineDate >= currentDate && c.DeadlineDate <= endDate).GroupBy(p => p.DeadlineDate).ToList();
                queryTasksNumber = db.UserTasks.Where(c => c.DeadlineDate >= currentDate && c.DeadlineDate <= endDate).Count();
                pageName = "Задачи на cледующие 7 дней";
            }
            else if (filterType == "Archive")
            {
                groups = db.UserTasks.Where(c => c.DeadlineDate < currentDate).GroupBy(p => p.DeadlineDate).ToList();
                queryTasksNumber = db.UserTasks.Where(c => c.DeadlineDate < currentDate).Count();
                pageName = "Архив задач";
            }
            else
            {
                groups = db.UserTasks.Where(c => c.DeadlineDate >= currentDate).GroupBy(p => p.DeadlineDate).ToList();
                queryTasksNumber = db.UserTasks.Where(c => c.DeadlineDate >= currentDate).Count();
                pageName = "Входящие задачи";
            }
            // Занесение вычеслинных значений в объект viewModel
            var viewModel = new TasksViewModel()
            {
                PageName = pageName,
                InboxTasksNumber = inboxTasksNumber,
                DoneInboxTasksNumber = doneInboxTasksNumber,
                TodayTasksNumber = todayTasksNumber,
                DoneTodayTasksNumber = doneTodayTasksNumber,
                WeekTasksNumber = weekTasksNumber,
                DoneWeekTasksNumber = doneWeekTasksNumber,
                ArchiveTasksNumber = archiveTasksNumber,
                DoneArchiveTasksNumber = doneArchiveTasksNumber,
                QueryTasksNumber = queryTasksNumber,
                Groups = groups
            };
            return viewModel;
        }

        [HttpGet]
        public ActionResult DeleteTask(int id, string filterType)
        {
            using (UserTaskContext db = new UserTaskContext())
            {
                UserTask task = db.UserTasks.Where(c => c.Id == id).FirstOrDefault();
                db.UserTasks.Attach(task);
                db.UserTasks.Remove(task);
                db.SaveChanges();
            }
            if (filterType == "")
            {
                return Redirect("/Home/Index");
            }
            else
            {
                return Redirect("/Home/Index/?FilterType=" + filterType);
            }
        }

        [HttpGet]
        public ActionResult ChangeTaskState(int id, string filterType)
        {
            using (UserTaskContext db = new UserTaskContext())
            {
                UserTask task = db.UserTasks.Where(c => c.Id == id).FirstOrDefault();
                task.TaskDone = !task.TaskDone;
                db.SaveChanges();
            }
            if (filterType == "")
            {
                return Redirect("/Home/Index");
            }
            else
            {
                return Redirect("/Home/Index/?FilterType=" + filterType);
            }
        }

        [HttpPost]
        public ActionResult AddTask(string taskName, string deadlineDate, string deadlineTime, string highPriority, string filterType)
        {
            bool HighTaskPriority = (highPriority == "True") ? true : false;
            DateTime Date = Convert.ToDateTime(deadlineDate);
            using (UserTaskContext db = new UserTaskContext())
            {
                UserTask task = new UserTask { TaskName = taskName, TaskDone = false, HighTaskPriority = HighTaskPriority, DeadlineDate = Date, DeadlineTime = deadlineTime };
                db.UserTasks.Add(task);
                db.SaveChanges();
            }
            if (filterType == "")
            {
                return Redirect("/Home/Index");
            }
            else
            {
                return Redirect("/Home/Index/?FilterType=" + filterType);
            }
        }

        [HttpPost]
        public ActionResult ChangeTask(int taskId, string taskName, string deadlineDate, string deadlineTime, string highPriority, string filterType)
        {
            bool HighTaskPriority = (highPriority == "True") ? true : false;
            DateTime Date = Convert.ToDateTime(deadlineDate);

            using (UserTaskContext db = new UserTaskContext())
            {
                UserTask task = db.UserTasks.Where(c => c.Id == taskId).FirstOrDefault();
                task.TaskName = taskName;
                task.HighTaskPriority = HighTaskPriority;
                task.DeadlineDate = Date;
                task.DeadlineTime = deadlineTime;
                db.SaveChanges();
            }
            if (filterType == "")
            {
                return Redirect("/Home/Index");
            }
            else
            {
                return Redirect("/Home/Index/?FilterType=" + filterType);
            }

        }

        [HttpGet]
        public JsonResult GetTaskInfo(int id)
        {
            UserTaskContext db = new UserTaskContext();
            UserTask task = db.UserTasks.Where(c => c.Id == id).FirstOrDefault();
            return Json(task, JsonRequestBehavior.AllowGet);
        }
    }
}