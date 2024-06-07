using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DapperProject.Models
{
    public class ProductDetails
    {
        public int? slno
        {
            get;
            set;
        }
        public string FruitsName
        {
            get;
            set;
        }
        public double Price
        {
            get;
            set;
        }
        public string Url
        {
            get;
            set;
        }
        public string Features
        {
            get;
            set;
        }
        public string made
        {
            get;
            set;
        }
        public string delivery
        {
            get;
            set;
        }

        public int? ProductID
        {
            get;
            set;
        }

        public int? Quantity
        {
            get;
            set;
        }
    }
}