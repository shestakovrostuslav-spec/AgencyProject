namespace RealEstateAgency.Models
{
    public class Client
    {
        public string Name { get; set; } = "";
        public string Phone { get; set; } = "";
        public string Email { get; set; } = "";
        public PropertySearchCriteria[] SearchHistory { get; set; }
            = new PropertySearchCriteria[10];

        public int SearchCount { get; set; } = 0;
    }
}
