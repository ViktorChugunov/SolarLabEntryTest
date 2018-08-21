using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SolarlabTest.Models
{
    public class UserTask
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public bool TaskDone { get; set; }
        public bool HighTaskPriority { get; set; }
        public DateTime DeadlineDate { get; set; }
        public string DeadlineTime { get; set; }
    }
}