using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarlabTest.Models
{
    public class TasksViewModel
    {
        public string PageName { get; set; }
        public int InboxTasksNumber { get; set; }
        public int DoneInboxTasksNumber { get; set; }
        public int TodayTasksNumber { get; set; }
        public int DoneTodayTasksNumber { get; set; }
        public int WeekTasksNumber { get; set; }
        public int DoneWeekTasksNumber { get; set; }
        public int ArchiveTasksNumber { get; set; }
        public int DoneArchiveTasksNumber { get; set; }
        public int QueryTasksNumber { get; set; }
        public IEnumerable<IGrouping<DateTime, UserTask>> Groups { get; set; }
    }
}