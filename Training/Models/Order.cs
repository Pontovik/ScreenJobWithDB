using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScreenJob.Models
{
    [PrimaryKey(nameof(OrderId))]
    public class Order
    {
        
        public int OrderId { get; set; }
        [XmlElement("no")]
        public int? OrderNumber { get; set; }

        [XmlElement("reg_date")]
        [NotMapped]
        public string? RegDate
        {
            get { return this.OrderRegDate.ToString(); }
            set { this.OrderRegDate = DateTime.Parse(value); }
        }
        public DateTime? OrderRegDate { get; set; }
        [XmlElement("sum")]
        public double? OrderSum { get; set; }
        public int? UserId { get; set; }
        [XmlElement("user")]
        public virtual User? User { get; set; }
        [XmlElement("product")]
        public virtual List<BuyProduct> BuyProducts { get; } = new List<BuyProduct>();
    }

    [XmlRoot("orders")]
    public class NewOrders
    {
        [XmlElement("order")]
        public Order[] Orders;
    }

}
