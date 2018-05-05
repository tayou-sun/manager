using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TaskManager.Models
{
    public class TaskContext :DbContext
    {
        public TaskContext()
            : base("DefaultConnection")
        {
           // Database.SetInitializer<TaskContext>(new CreateDatabaseIfNotExists<TaskContext>());
        }
        public DbSet<Task> Tasks { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<Position> Position { get; set; }
        public DbSet<Tag> Tag { get; set; }

        public DbSet<Holiday> Holiday { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);
        }
    }
}