using System;
using System.Collections.Generic;
using System.Text;

using RealEstateAgency.Models;
using RealEstateAgency.DataAccess;

namespace RealEstateAgency.BusinessLogic
{
    public class DealService
    {
        private readonly JsonFileStorage<Deal> storage;
        private List<Deal> deals;

        public DealService()
        {
            storage = new JsonFileStorage<Deal>("deals.json");
            deals = storage.Load();
        }

        public List<Deal> GetAll()
        {
            return deals;
        }

        public void Add(
            Property property,
            Client client,
            Agent agent,
            double price,
            string type,
            double commission)
        {
            if (price <= 0)
                throw new Exception("Некорректная стоимость!");

            if (commission < 0 || commission > 100)
                throw new Exception("Некорректная комиссия!");

            Deal deal = new Deal
            {
                PropertyAddress = property.Address,
                ClientName = client.Name,
                AgentName = agent.Name,
                Price = price,
                Type = type,
                CommissionPercent = commission
            };

            deals.Add(deal);
            storage.Save(deals);
        }
    }
}
