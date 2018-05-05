using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using System.Web.WebPages;
using DocumentFormat.OpenXml.Drawing;
using OfficeOpenXml;
using TaskManager.Models;
using Position = TaskManager.Models.Position;

namespace TaskManager.Controllers
{
    public class Userr
    {
        public User Name;
        public double? Sum;
        public string Date;
        public int Id;

        public Userr(User Name, double? Sum, string Date, int Id)
        {
            this.Name = Name;
            this.Sum = Sum;
            this.Date = Date;
            this.Id = Id;
        }
    }


    public class UserHoliday
    {
        public UserHoliday()
        {
            Holiday = new List<Holiday>();
        }
        public User User { get; set; }
        public List<Holiday> Holiday { get; set; }
    }


    public class testJsonObject
    {
        public List<string> PlanText { get; set; }
        public List<string> PlanResult { get; set; }
        public List<string> Time { get; set; }
        public int Id { get; set; }
        public string Date { get; set; }
        public DateTime Date_ { get; set; }
    }

    public class Group
    {
        public Group()
        {
            userr = new List<Userr>();
        }

        public Position Position { get; set; }
        public double? TimeForPosition { get; set; }
        private List<Userr> userr;

        public List<Userr> Userr
        {
            get { return userr; }
        }
    }

    public class HomeController : Controller
    {
        private string UserName;
        private TaskContext db;
        private List<String> months;

        public HomeController()
        {
            UserName = "";
            db = new TaskContext();
            months = new List<String>()
            {
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            };
        }

        public ActionResult Index(string smth)
        {
            if (User.Identity.IsAuthenticated)
            {

                var user = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
                UserName = user.Name;
                var role = user.Role;
                if (role == 1)
                    return RedirectToAction("Users", "Home");
                if (smth != null)
                    return RedirectToAction("WeekSchedule", new {date = DateTime.Now.ToString("dd-MM-yyyy")});
                return RedirectToAction("TodaySchedule", new {date = DateTime.Now.ToString("dd-MM-yyyy")});
            }
            return View();
        }



        [HttpGet]
        public ActionResult DateSchedule(DateTime date)
        {
            ViewBag.Name = db.User.FirstOrDefault(x => x.Login == User.Identity.Name).Name;
            var a = new List<Task>();
            foreach (var x in db.Tasks)
            {
                var b = (DateTime) x.CreateDate;
                if (x.User.Login == User.Identity.Name && b.Day == DateTime.Now.Day &&
                    b.Month == DateTime.Now.Month &&
                    b.Year == DateTime.Now.Year)
                    a.Add(x);
            }

            ViewBag.TodayDate = "Date " + date.ToShortDateString();
            ViewBag.User = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            ViewBag.Task = a;
            ViewBag.Now = months;
            return View();
        }


        [HttpGet]
        public ActionResult Tags()
        {
            ViewBag.UserForTag = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            ViewBag.Name = db.User.FirstOrDefault(x => x.Login == User.Identity.Name).Name;
            ViewBag.Tags = db.Tag.ToList();
            ViewBag.User = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            ViewBag.TagsNum = db.Tag.Count();
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult AllUsers(int Checked)
        {
            return RedirectToAction(Checked == 1 ? "WeekScheduleForAllUSers" : "WeekSchedule");
        }




        [AcceptVerbs(HttpVerbs.Post)]
        public void People1(string PlanText, string PlanResult, string Time, string Id, string Day, string Month,
            string Year, string User, string Tag)
        {
            var userId = Int32.Parse(User);

            var m = Month.Split(',');
            if (Id != "")
            {
                var a = Int32.Parse(Id);
                var task = db.Tasks.FirstOrDefault(x => x.Id == a);
                var user = db.User.FirstOrDefault(x => x.ID == userId);

                task.PlanText = PlanText;
                var d = new DateTime(Int32.Parse(Year), months.IndexOf(m[0]) + 1, Int32.Parse(Day));
                if (user.LastCreationDate < d)
                    user.LastCreationDate = d;
                task.Tag = db.Tag.FirstOrDefault(x => x.Name == Tag);
                task.CreateDate = d;
                task.ResultText = PlanResult;
                task.Time = !Time.IsEmpty() ? Int32.Parse(Time) : 0;
                db.SaveChanges();
            }

            else
            {
                var u = new Task();
                var d = new DateTime(Int32.Parse(Year), months.IndexOf(m[0]) + 1, Int32.Parse(Day));
                u.CreateDate = d;
                u.CreateDate = d;
                var user = db.User.FirstOrDefault(x => x.ID == userId);
                if (user.LastCreationDate < d)
                    user.LastCreationDate = d;
                u.Tag = db.Tag.FirstOrDefault(x => x.Name == Tag);
                u.PlanText = PlanText;
                u.Time = !Time.IsEmpty() ? Int32.Parse(Time) : 0;
                db.User.FirstOrDefault(x => x.ID == userId).Tasks.Add(u);
                u.ResultText = PlanResult;
                db.Tasks.Add(u);
                var id = db.Tasks.Count();
                u.Id = id + 1;
                db.SaveChanges();
            }

            //return RedirectToAction("WeekSchedule", new { date = new DateTime(Int32.Parse(Year), months.IndexOf(m[0])+1, Int32.Parse(Day)).ToString("dd-MM-yyyy") }); 
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public JsonResult GetTag()
        {


            return Json(db.Tag.Select(x => x.Name).ToList(), JsonRequestBehavior.AllowGet);
            ;
        }


        public ActionResult Users(string searchText, string attr, string type)
        {
            ViewBag.UserForTag = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            ViewBag.Positions = db.Position;
            ViewBag.Tags = db.Tag;
            var result = new List<User>();
            var a = TempData["list"] as List<User>;
            if (attr == "name")
            {
                if ((TempData["typeOfSorting"] is int ? (int) TempData["typeOfSorting"] : 0) == -1)
                {
                    result.AddRange(db.User.OrderBy(x => x.Name));
                    if (a != null)
                        result.AddRange(a.OrderBy(x => x.Name));
                }
                else
                {
                    result.AddRange(db.User.OrderByDescending(x => x.Name));
                    if (a != null)
                        result.AddRange(a.OrderByDescending(x => x.Name));
                }
            }
            else
            {
                result.AddRange(db.User);
            }

            if (a == null)
            {
                var aa = new List<Userr>();
                foreach (var us in result)
                {
                    var ad = us.Tasks.Where(x => ((DateTime) x.CreateDate).Month == DateTime.Now.Month)
                        .Sum(x => x.Time);
                    aa.Add(new Userr(us, ad,
                        (us.LastCreationDate is DateTime)
                            ? ((DateTime) us.LastCreationDate).ToShortDateString()
                            : "", us.ID));
                }

                ViewBag.aa = aa;
                if (db.User.FirstOrDefault(x => x.Login == User.Identity.Name) != null)
                    ViewBag.Name = db.User.FirstOrDefault(x => x.Login == User.Identity.Name).Name;
                ViewBag.User = aa;
            }

            else
            {
                var aa = new List<Userr>();
                foreach (var us in a)
                {
                    var ad = us.Tasks.Where(x => ((DateTime) x.CreateDate).Month == DateTime.Now.Month)
                        .Sum(x => x.Time);
                    aa.Add(new Userr(us, ad,
                        (us.LastCreationDate is DateTime)
                            ? ((DateTime) us.LastCreationDate).ToShortDateString()
                            : "", us.ID));
                }
                ViewBag.aa = aa;
                IEnumerable<User> users = db.User;
                ViewBag.Name = db.User.FirstOrDefault(x => x.Login == User.Identity.Name).Name;
                ViewBag.User = aa;
                ViewBag.searchText = searchText;
                TempData["list"] = null;
            }

            return View();
        }

        [HttpPost]
        public ActionResult UserCreate(User user, string Position)
        {
            user.Position = db.Position.FirstOrDefault(x => x.Name == Position);
            user.DateStart = DateTime.Now;
            db.User.Add(user);
            db.SaveChanges();
            return RedirectToAction("Index");

        }

        [HttpPost]
        public ActionResult CreateTag()
        {
            var user = db.User.FirstOrDefault(x => x.Login == User.Identity.Name).Name;

            return RedirectToAction("");

        }

        [HttpGet]
        public ActionResult WeekScheduleForAllUSers()
        {
            if (db.User.FirstOrDefault(x => x.Login == User.Identity.Name) != null)
                ViewBag.Name = db.User.FirstOrDefault(x => x.Login == User.Identity.Name).Name;


            var user = new List<Task>();

            ViewBag.Tasks = db.Tasks.ToList();
            ViewBag.Users = db.User.ToList();
            return View();
        }

        [HttpGet]
        public ActionResult NowWeekSchedule(string date, string id)
        {
            ViewBag.UserForTag = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            Models.User u = null;
            if (id == null)
            {
                u = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
                ViewBag.User = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            }
            else
            {
                var i = Int32.Parse(id);
                u = db.User.FirstOrDefault(x => x.ID == i);
                ViewBag.User = u;
            }
            ViewBag.MainUser = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            DateTime convertedDate = DateTime.Now;
            if (date != null)
            {
                convertedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            }
            var dayOfStart = convertedDate is DateTime ? (DateTime) convertedDate : new DateTime();
            var months = new List<String>()
            {
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            };
            ViewBag.Month1 = convertedDate.Month;
            var d = dayOfStart;
            var days = 1;
            if (d.Day <= 7 && convertedDate.DayOfWeek != System.DayOfWeek.Monday &&
                convertedDate.AddDays(-1).DayOfWeek != System.DayOfWeek.Monday)
            {
                d = new DateTime(dayOfStart.Year, dayOfStart.Month, 1);
                while (d.DayOfWeek != System.DayOfWeek.Sunday)
                {
                    d = d.AddDays(1);
                }

                days = d.Day;

                ViewBag.Days = days + 2;
                ViewBag.Day = 1;

            }
            else
            {
                while (d.DayOfWeek != System.DayOfWeek.Monday)
                {
                    d = d.AddDays(-1);
                }

                var dd = d;

                while (days <= 7 && dd.Month == d.Month)
                {
                    days += 1;
                    dd = dd.AddDays(1);
                }

                ViewBag.Days = days + d.Day;
                ViewBag.Day = d.Day;

            }

            var dayNowForFowrward = convertedDate;
            var dayForForward = 0;
            while (dayNowForFowrward.DayOfWeek != System.DayOfWeek.Monday)
            {
                dayNowForFowrward = dayNowForFowrward.AddDays(-1);
                dayForForward ++;
            }

            ViewBag.StartOfWeek = d.ToShortTimeString();

            ViewBag.Year = d.Year;
            ViewBag.aa = convertedDate.ToString("dd-MM-yyyy");
            ViewBag.Back = d.AddDays(-dayForForward - 1).ToString("dd-MM-yyyy");
            ViewBag.Forward = d.AddDays(days).ToString("dd-MM-yyyy");
            ViewBag.TodayDate = convertedDate.ToShortDateString();
            //var u = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            if (db.User.FirstOrDefault(x => x.Login == User.Identity.Name) != null)
                ViewBag.Name = db.User.FirstOrDefault(x => x.Login == User.Identity.Name).Name;

            ViewBag.Month = months[convertedDate.Month - 1];
            var user = new List<Task>();

            var userr = db.Tasks.Where(x => x.User.ID == u.ID);

            double? t = 0;

            foreach (var x in userr)
            {
                var b = x.CreateDate is DateTime ? (DateTime) x.CreateDate : new DateTime();
                if (b.Month == convertedDate.Month && b.Year == convertedDate.Year && x.User.ID == u.ID)
                {
                    if (x.Time != null)
                        t += x.Time;
                    user.Add(x);
                }

            }

            ViewBag.Total = t;
            ViewBag.Task = user;
            ViewBag.NowMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            ViewBag.Now = new List<String>()
            {
                "January",
                "February",
                "March",
                "April",
                "May",
                "June",
                "July",
                "August",
                "September",
                "October",
                "November",
                "December"
            };
            return View();
        }

        // [AcceptVerbs(HttpVerbs.Post)]
        [HttpGet]
        public void GetExcel(int month, int year, int user)
        {
            var date = new DateTime(year, month, 1);
        Response.ClearContent();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", string.Format("attachment; filename={0}", "Customers.xls"));
        Response.ContentType = "application/ms-excel";
        DataTable dt = BindDatatable(date, user);
        string str = string.Empty;
        foreach (DataColumn dtcol in dt.Columns)
        {
            Response.Write(str + dtcol.ColumnName);
            str = "\t";
        }
        Response.Write("\n");
        foreach (DataRow dr in dt.Rows)
        {
            str = "";
            for (int j = 0; j < dt.Columns.Count; j++)
            {
                Response.Write(str + Convert.ToString(dr[j]));
                str = "\t";
            }
            Response.Write("\n");
        }
        Response.End();
    }
        
        protected DataTable BindDatatable(DateTime date, int user)
    {
        DataTable dt = new DataTable();
        dt.Columns.Add("Date", typeof(string));
        dt.Columns.Add("Plan", typeof(string));
        dt.Columns.Add("Result", typeof(string));
        dt.Columns.Add("Time", typeof(int));


        Encoding win1252 = Encoding.GetEncoding("windows-1251");


        Encoding win = Encoding.GetEncoding(1250);
        Encoding z = Encoding.ASCII;
       // byte[] zBytes = z.GetBytes(Nazev);
        //byte[] isoBytes = Encoding.Convert(z, win, zBytes);
        //Nazev = win.GetString(isoBytes);


        Encoding enc = new UTF8Encoding(true, true);
            var amount = DateTime.DaysInMonth(date.Year, date.Month);
            var tasks =
                db.Tasks.Where(
                    x =>
                        x.User.ID == user &&
                        x.CreateDate!=null);
            for (int i = 1; i <= amount; i++)
            {
                foreach (var u in tasks)

                {
                    var dateForUser = date is DateTime ? (DateTime) date : new DateTime();

                    if (dateForUser.Month == date.Month && dateForUser.Year == date.Year && dateForUser.Day == i)
                    {
                        byte[] srcBytes = win1252.GetBytes(u.PlanText);
                        byte[] dstBytes = Encoding.Convert(Encoding.Default, win1252, srcBytes);
                        string convertedStr = win1252.GetString(dstBytes);
                        dt.Rows.Add(new DateTime(date.Year, date.Month, i).ToShortDateString(),
                            convertedStr, u.ResultText, u.Time);
                    }
                }
            }
        
           
        return dt;
    }
        


[HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SearchResult(SearchModel Text)
        {
            if (User.Identity.IsAuthenticated)
            {
                var uss = db.User.Where(x => x.Name.Contains(Text.Name));

                TempData["list"] = uss.ToList();
                return RedirectToAction("Users", new { searchText = Text.Name });
            }
            return null;
        }
            [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TagCreate (TagModel Text)
        {
            if (User.Identity.IsAuthenticated)
            {
                var tag = new Tag();
                tag.Name = Text.Name;
                tag.Id = db.Tag.Count() + 1;
                db.Tag.Add(tag);
                db.SaveChanges();
            }
            return RedirectToAction("Tags");
        }

            [HttpGet]
            public ActionResult TagDelete(string id)
            {
                if (User.Identity.IsAuthenticated)
                {
                    var tagId = Int64.Parse(id);
                    var tag = db.Tag.FirstOrDefault(x => x.Id == tagId);

                    var tasks = db.Tasks.Where(x => x.Tag.Id == tagId);
                    foreach (var t in tasks)
                        t.Tag = null;
                    
                    db.Tag.Remove(tag);
                    db.SaveChanges();
                }
                return RedirectToAction("Tags");
            }
      
        [HttpGet]
        public ActionResult SearchResult(List<User> users1)
        {
            if (User.Identity.IsAuthenticated)
            {
                var uss = users1;
                var aa = new List<Userr>();
                foreach (var us in uss)
                {
                    var ad = us.Tasks.Where(x => ((DateTime) x.CreateDate).Month == DateTime.Now.Month)
                        .Sum(x => x.Time);
                    aa.Add(new Userr(us, ad,
                        (us.LastCreationDate is DateTime) ? ((DateTime) us.LastCreationDate).ToShortDateString() : "",us.ID));
                }
                ViewBag.aa = aa;
                IEnumerable<User> users = db.User;
                if (db.User.FirstOrDefault(x => x.Login == User.Identity.Name) != null)
                    ViewBag.Name = db.User.FirstOrDefault(x => x.Login == User.Identity.Name).Name; ViewBag.User = aa;
                return View();
            }
            return null;
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public void People(string PlanText, string PlanResult, string Time, string Id, string Date, string Tag)
        {
            var aa = Date.Split('.');

            var date = new DateTime(Int32.Parse(aa[2]), Int32.Parse(aa[1]), Int32.Parse(aa[0]));
            if (Id != "")
            {

                var user = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
                if (user.LastCreationDate == null || user.LastCreationDate < date)
                    user.LastCreationDate = date;
                var a = Int32.Parse(Id);
                var task = db.Tasks.FirstOrDefault(x => x.Id == a);
                task.PlanText = PlanText;
                task.Tag = db.Tag.FirstOrDefault(x => x.Name == Tag);
                task.ResultText = PlanResult;
                if (Time != "")
                    task.Time = Int32.Parse(Time);
                else
                    task.Time = 0;
                db.SaveChanges();
            }

            else
            {
                var user = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
                if (user.LastCreationDate == null ||  user.LastCreationDate < date)
                    user.LastCreationDate = date;
                var u = new Task();
                u.PlanText = PlanText;
                u.Tag = db.Tag.FirstOrDefault(x => x.Name == Tag);

                u.CreateDate = date;
                if (Time != "")
                    u.Time = Int32.Parse(Time);
                else
                    u.Time = 0;
                db.User.FirstOrDefault(x => x.Login == User.Identity.Name).Tasks.Add(u);
                u.ResultText = PlanResult;
                db.Tasks.Add(u);
                db.SaveChanges();
            }
            //return RedirectToAction("Index");
        }
        [HttpPost]
        public JsonResult GetLastPayReportMonthsBySupplierID(int SupplierID)
        {
            return Json(null);
        }

        [HttpGet]
        public ActionResult Sorting(string attr,string type)
        {
            return RedirectToAction("Users", new { searchText = "", attr = attr, type = type });
        }

        [HttpGet]
        public ActionResult WeekSchedules(string Id, string Date)
        {
            return RedirectToAction("WeekSchedule", new { date = Date, id = Id }); 
        }

          [HttpGet]
        public ActionResult TodaySchedules(string Id, string Date)
        {
            return RedirectToAction("TodaySchedule", new { date = Date, id = Id }); 
        }
           [HttpGet]
          public ActionResult NowWeekSchedules(string Id, string Date)
        {
            return RedirectToAction("NowWeekSchedule", new { date = Date, id = Id }); 
        }


        [HttpGet]
        public ActionResult Todaychedules()
        {
            return RedirectToAction("TodaySchedule", new { date = DateTime.Now.ToString("dd-MM-yyyy") }); 

        }
        [HttpGet]
        public ActionResult Task(int id)
        {
            

            var user = db.User.Where(x => x.ID == id).FirstOrDefault();
            ViewBag.User = user;
            ViewBag.Week = new List<string>() { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday" };
            return View();
        }

		[HttpGet]
		public ActionResult Map()
		{
			return View();
		}


		[AcceptVerbs(HttpVerbs.Get)]
        [System.Web.Services.WebMethod]
        public ActionResult TaskCreate(string Name)
        {
            return View();
        }
        //[HttpPost]
        //public ActionResult UserCreate(User user)
        //{
        //    user.DateStart = DateTime.Now;
        //    db.User.Add(user);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");

        //}

        [HttpGet]
        public ActionResult WeekSchedule(string date, string id)
        {
            ViewBag.UserForTag = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            ViewBag.MainUser = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            Models.User u = null;
            if (id == null)
            {
                u = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
                ViewBag.User = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            }
            else
            {
                var i = Int32.Parse(id);
                u = db.User.FirstOrDefault(x => x.ID == i);
                ViewBag.User = u;
            }
            
            DateTime convertedDate = DateTime.Now;
            if (date != null)
             convertedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);
            ViewBag.Back = convertedDate.AddMonths(-1).ToString("dd-MM-yyyy");
            ViewBag.Forward = convertedDate.AddMonths(+1).ToString("dd-MM-yyyy");
            ViewBag.TodayDate = convertedDate.ToShortDateString();
            ViewBag.Date = convertedDate;
            if (db.User.FirstOrDefault(x => x.Login == User.Identity.Name) != null)
                ViewBag.Name = db.User.FirstOrDefault(x => x.Login == User.Identity.Name).Name;
            var months =new List<String>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            ViewBag.Month = months[convertedDate.Month-1];
            var user = new List<Task>();

            var userr = db.Tasks.Where(x => x.User.ID == u.ID);

               double? t = 0;

            foreach (var x in userr)
            {
                var b = x.CreateDate is DateTime ? (DateTime) x.CreateDate : new DateTime();
                if (b.Month == convertedDate.Month && b.Year == convertedDate.Year && x.User.ID == u.ID)
                {
                    if (x.Time != null)
                        t += x.Time;
                    user.Add(x);
                }

            }

            ViewBag.Total = t;
            ViewBag.Users = db.User.ToList();
            ViewBag.Task = user;
            ViewBag.NowMonth = DateTime.DaysInMonth(convertedDate.Year, convertedDate.Month);
            ViewBag.Now = new List<String>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            return View();
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult WeekSchedule(string date)
        {
            if (db.User.FirstOrDefault(x => x.Login == User.Identity.Name) != null)
                ViewBag.Name = db.User.FirstOrDefault(x => x.Login == User.Identity.Name).Name; var a = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            ViewBag.User = a;
            var user = db.Tasks.Where(x => x.User.ID == a.ID);
            ViewBag.Task = user;
            ViewBag.NowMonth = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
            ViewBag.Now = new List<String>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            return View();
        }


        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult TodaySchedule(int Day, int Month, int Year)
        {
                var nowDate = new DateTime(Year,Month,Day);
                nowDate = nowDate.AddDays(-1);


                if (db.User.FirstOrDefault(x => x.Login == User.Identity.Name) != null)
                    ViewBag.Name = db.User.FirstOrDefault(x => x.Login == User.Identity.Name).Name; var a = new List<Task>();
            foreach (var x in db.Tasks)
            {
                var b =(DateTime) x.CreateDate;
                if (x.User.Login == User.Identity.Name && b.Day == DateTime.Now.Day &&
                    b.Month == DateTime.Now.Month &&
                    b.Year == DateTime.Now.Year)
                    a.Add(x);
            }

            ViewBag.TodayDate = "Date " + nowDate.ToShortDateString();
            ViewBag.User = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            ViewBag.Task = a;
            ViewBag.Now = new List<String>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            return View("DateSchedule");
        }



        [HttpGet]
        public ActionResult TodaySchedule(string date, string id)
        {
            ViewBag.UserForTag = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            ViewBag.MainUser = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);

            Models.User u = null;
            if (id == null)
            {
                u = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            }
            else
            {
                var i = Int32.Parse(id);
                u = db.User.FirstOrDefault(x => x.ID == i);

            }

            ViewBag.Tags = db.Tag.ToList();
            ViewBag.TagsNumber = db.Tag.Count();
            DateTime convertedDate = DateTime.Now;
            if(date !=null)
             convertedDate = DateTime.ParseExact(date, "dd-MM-yyyy", CultureInfo.InvariantCulture);

            ViewBag.Back = convertedDate.AddDays(-1).ToString("dd-MM-yyyy");
            ViewBag.Forward = convertedDate.AddDays(+1).ToString("dd-MM-yyyy");
            if (db.User.FirstOrDefault(x => x.Login == User.Identity.Name) != null)
                ViewBag.Name = db.User.FirstOrDefault(x => x.Login == User.Identity.Name).Name; var a = new List<Task>();
            foreach (var x in db.Tasks)
            {
                var b = (DateTime)x.CreateDate;
                if (x.User.Login == u.Login && b.Day == convertedDate.Day &&
                    b.Month == convertedDate.Month &&
                    b.Year == convertedDate.Year)
                    a.Add(x);
            }

            ViewBag.TodayDate = "Date: " + convertedDate.ToString("dd.MM.yyyy");
            ViewBag.TodayMonth = convertedDate.DayOfWeek;
            ViewBag.User = u;
            ViewBag.Task = a;
            ViewBag.Now = new List<String>() { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "November", "December" };
            return View();
        }
        [HttpGet]
        public ActionResult GetCalendar(string searchText)
        {
            var aa = new List<Userr>();
            foreach (var us in db.User)
            {
                var ad = us.Tasks.Where(x => ((DateTime)x.CreateDate).Month == DateTime.Now.Month)
                    .Sum(x => x.Time);
                aa.Add(new Userr(us, ad,
                    (us.LastCreationDate is DateTime)
                        ? ((DateTime)us.LastCreationDate).ToShortDateString()
                        : "",us.ID));
            }

            var group = new List<Group>();
            foreach (var p in db.Position)
            {
                var user = aa.Where(x => x.Name.Position != null && x.Name.Position.Id == p.Id).ToList();
                if (user.Count() != 0)
                {
                    var a = new Group();
                    a.Position = p;
                    foreach (var u in user)
                        a.Userr.Add(u);
                    a.TimeForPosition = user.Sum(x => x.Sum);
                    group.Add(a);
                }
            }
          

            ViewBag.group = group;
            IEnumerable<User> users = db.User;
            if (db.User.FirstOrDefault(x => x.Login == User.Identity.Name) != null)
                ViewBag.Name = db.User.FirstOrDefault(x => x.Login == User.Identity.Name).Name; ViewBag.User = aa;
            return View();
        }


        [HttpPost]
        public ActionResult HolidayChange(HolidayModel model)
        {
            var user = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);

            var newHoliday = new Holiday
            {
                User = user,
                PlanStartDate = model.PlanStartDate,
                PlanEndDate = model.PlanEndDate,
                ResultStartDate = model.ResultStartDate,
                ResultEndDate = model.ResultEndDate
            };
            newHoliday.Id = db.Holiday.Count() + 1;
            db.Holiday.Add(newHoliday);
            db.SaveChanges();

            return RedirectToAction("Holiday");
        }


        [HttpGet]
        public ActionResult Holiday()
        {
            var user = db.User.FirstOrDefault(x => x.Login == User.Identity.Name);
            ViewBag.UserForTag = user;
            ViewBag.User = user;
            if (user.Role == 1)
            {
                var holiday = db.Holiday.FirstOrDefault(x =>
                    x.User.ID == user.ID && x.PlanStartDate != null && x.PlanStartDate.Year == DateTime.Now.Year);
                if (holiday != null)
                {
                    ViewBag.Holiday = holiday;
                    ViewBag.Date = holiday.PlanStartDate;

                    ViewBag.FirstDate = months.IndexOf(holiday.PlanStartDate.DayOfWeek.ToString()) + 1;

                    ViewBag.Days = DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month);
                }
                else
                {
                    ViewBag.Date = DateTime.Now;
                    var date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                    ViewBag.FirstDate = months.IndexOf(date.DayOfWeek.ToString()) + 1;
                    ViewBag.Days = DateTime.DaysInMonth(date.Year, date.Month);
                }

                var holidayList = new List<UserHoliday>();
                foreach (var u in db.User)
                {
                    var holidays = db.Holiday.Where(x => x.User.ID == u.ID);
                    if (holidays.Count() != 0)
                    {
                        var userHoliday = new UserHoliday() { User = u };
                        foreach (var h in holidays)
                        {
                            userHoliday.Holiday.Add(h);
                        }
                        holidayList.Add(userHoliday);
                    }
                    else
                    {
                        holidayList.Add(new UserHoliday() { Holiday = null, User = u });
                    }
                }

                ViewBag.UserHoliday = holidayList;
            }
            else
            {
                var holidayList = new List<UserHoliday>();

                var holidays = db.Holiday.Where(x =>
                    x.User.ID == user.ID && x.PlanStartDate != null && x.PlanStartDate.Year == DateTime.Now.Year);
                var userHoliday = new UserHoliday() { User = user };
                if (holidays.Count() != 0)
                {
                    foreach (var h in holidays)
                    {
                        userHoliday.Holiday.Add(h);
                    }
                }
                holidayList.Add(userHoliday);
                ViewBag.UserHoliday = holidayList;
            }


            return View();
        }

    }
}
