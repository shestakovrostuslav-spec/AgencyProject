using System;
using System.Collections.Generic;
using System.Text;


namespace RealEstateAgency.Models
{
    public class Deal
    {
        public string PropertyAddress { get; set; } = "";

        public string ClientName { get; set; } = "";

        public string AgentName { get; set; } = "";

        public double Price { get; set; }

        public string Type { get; set; } = "Покупка";

        public double CommissionPercent { get; set; }

        public double CommissionAmount { get; set; }

        public DateTime Date { get; set; } = DateTime.Now;
    }
}
