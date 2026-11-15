using System.Collections.Generic;

namespace RealEstateAgency.Models
{
    public class Property
    {
        public string Type { get; set; } = "";
        public double Area { get; set; }
        public string Address { get; set; } = "";
        public string District { get; set; } = "";
        public int Rooms { get; set; }
        public double Price { get; set; }
        public string Status { get; set; } = "На продаж";
        public string AgentName { get; set; } = "";
        public string BookedByClientName { get; set; } = "";
        public List<StatusHistory> StatusHistory { get; set; } = new List<StatusHistory>();
    }
}
