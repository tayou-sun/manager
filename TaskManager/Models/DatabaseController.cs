using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManager.Models
{
    public class DatabaseController
    {
        private readonly TaskContext _databaseContext;

        public DatabaseController()
        {
            _databaseContext = new TaskContext();
            //Тут дергать, чтобы перестроить базу
            _databaseContext.Database.Initialize(false);
        }


        public void Save()
        {
            _databaseContext.SaveChanges();
        }
    
    }
}