using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScreenJob.Models
{
    public class User
    {
        public int UserId { get; set; }

        public DateTime? UserRegistrationDatetime { get; set; }

        public bool? UserIsDelete { get; set; }

        public string? UserLogin { get; set; }

        public string? UserPhone { get; set; }

        [XmlElement("fio")]
        public string? UserName { get; set; }
        [XmlElement("email")]
        public string? UserEmail { get; set; }
        public virtual List<Order> Orders { get; } = new List<Order>();
    }
}
