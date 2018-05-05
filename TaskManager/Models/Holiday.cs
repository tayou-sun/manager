using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TaskManager.Models
{
    [Table("Holiday")]
    public class Holiday
    {
        public int Id { get; set; }
        public virtual User User { get; set; }
        public DateTime PlanStartDate { get; set; }
        public DateTime PlanEndDate { get; set; }
        public DateTime ResultStartDate { get; set; }
        public DateTime ResultEndDate { get; set; }

    }
}