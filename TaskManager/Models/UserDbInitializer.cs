using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TaskManager.Models
{
    public class UserDbInitializer : DropCreateDatabaseAlways<TaskContext>
    {

        protected override void Seed(TaskContext db)
        {
            //db.User.Add(new User { Name = "1", Password = "1"});
            //db.User.Add(new User { Name = "2", Password = "2" });
            base.Seed(db);
        }
    }
}