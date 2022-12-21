using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ScreenJob.Models
{
    [PrimaryKey(nameof(ProductId))]
    public class Product
    {
        public int ProductId { get; set; }
        [XmlElement("name")]
        public string? ProductName { get; set; }
        [XmlElement("price")]
        public double? ProductPrice { get; set; }
        [XmlElement("quantity")]
        public int? ProductAmount { get; set; }
        public bool? ProductIsDelete { get; set; }
        public virtual List<BuyProduct> BuyProducts { get; } = new List<BuyProduct>();
    }
}
