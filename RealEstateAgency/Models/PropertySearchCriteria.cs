using System;
using System.Collections.Generic;
using System.Text;



namespace RealEstateAgency.Models
{
    public class PropertySearchCriteria
    {
        public string? Type { get; set; }

        public double? MinPrice { get; set; }

        public double? MaxPrice { get; set; }

        public double? MinArea { get; set; }

        public double? MaxArea { get; set; }

        public string? District { get; set; }

        public int? Rooms { get; set; }
    }
}

