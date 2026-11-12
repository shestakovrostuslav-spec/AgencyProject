using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstateAgency.Models
{
    public class Property
    {
        public string Type { get; set; } = "";
        public double Area { get; set; }
        public string Address { get; set; } = "";
        public double Price { get; set; }
        public string Status { get; set; } = "На продаж";
    }
}