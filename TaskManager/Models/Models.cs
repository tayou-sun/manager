using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TaskManager.Models
{

    public class HolidayModel
    {
        public DateTime PlanStartDate { get; set; }
        public DateTime PlanEndDate { get; set; }
        public DateTime ResultStartDate { get; set; }
        public DateTime ResultEndDate { get; set; }
    }

    public class LoginModel
    {
        public string Login { get; set; }
        public string Password { get; set; }
    }

    public class SearchModel
    {
        public string Name { get; set; }
    }

    public class TagModel
    {
        public string Name { get; set; }
    }
}