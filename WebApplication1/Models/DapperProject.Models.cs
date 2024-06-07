using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DapperProject.Models
{
    public class Mobiles
    {
        public int? slno { get; set; }
        public string FruitsName { get; set; }
        public double Price { get; set; }
        public string Url { get; set; }
        public string ZoomUrl { get; set; }
        public string Description { get; set; }

        public int? ProductID { get; set; }

        public int? Quantity {  get; set; }

        public string Features { get; set; }
        public string Made { get; set; }
        public string Delivery { get; set; }
        public DateTime? Ordertime { get; set; } = DateTime.Now;
    }
}
