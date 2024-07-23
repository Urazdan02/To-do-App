using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ToDoApp.Models
{
    public class TaskModel
    {
        public int ID { get; set; }

        [DisplayName("Tytuł")]
        [Required]
        public string Subject { get; set; }
        [DisplayName("Zawartość")]
        public string Content { get; set; }

        [DisplayName("Data")]
        public DateTime Insert_Date { get; set; }
        [DisplayName("Data")]
        [Required]

        [DataType(DataType.DateTime)]
        public DateTime Task_Date { get; set; }
        [DisplayName("Lokalizacja")]
        public string Location { get; set; }
        [DisplayName("Ukończono")]
        public bool Completed { get; set; }
        [DisplayName("Użytkownik")]
        public string UserID { get; set; }

        public virtual IdentityUser User { get; set; }
        [DataType(DataType.Upload)]
        public byte[] Image { get; set; }
    }
}
