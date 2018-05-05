using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace TaskManager.Models
{
    [Table("Tag")]
    public partial class Tag
    {
       public Tag()
        {
            this.Tasks = new HashSet<Task>();
        }
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Task> Tasks { get; set; }

    }
}