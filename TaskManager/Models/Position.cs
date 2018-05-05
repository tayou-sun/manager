using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TaskManager.Models
{
    [Table("Position")]
    public partial class Position
    {
        public int Id { get; set; }
        // public Nullable<int> User { get; set; }
        public string Name { get; set; }
    }
}