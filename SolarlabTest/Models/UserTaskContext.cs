using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace SolarlabTest.Models
{
    public class UserTaskContext : DbContext
    {
        public UserTaskContext() : base("DbConnection")
        { }

        public DbSet<UserTask> UserTasks { get; set; }
    }
}