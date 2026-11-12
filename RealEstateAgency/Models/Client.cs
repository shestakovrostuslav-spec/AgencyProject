using System;
using System.Collections.Generic;
using System.Text;

namespace RealEstateAgency.Models
{
    public class Client
    {
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public string Type { get; set; } = "Покупець";
    }
}