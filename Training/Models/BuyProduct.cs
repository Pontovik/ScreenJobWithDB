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
    [PrimaryKey(nameof(BuyproductId))]
    public class BuyProduct
    {
        public int BuyproductId { get; set; }

        public int? OrderId { get; set; }

        public int? ProductId { get; set; }
        [XmlElement("quantity")]
        public int? BuyproductAmount { get; set; }
        [XmlElement("price")]
        public double? BuyproductPrice { get; set; }
        [XmlElement("name")]
        [NotMapped]
        public string? BuyproductName { get; set; }
        public virtual Order? Order { get; set; }

        public virtual Product? Product { get; set; }
    }
}
