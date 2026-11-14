using System;
using System.Collections.Generic;
using System.Text;


namespace RealEstateAgency.Models
{
    public class StatusHistory
    {
        public string OldStatus { get; set; } = "";

        public string NewStatus { get; set; } = "";

        public DateTime ChangeDate { get; set; } = DateTime.Now;
    }
}
