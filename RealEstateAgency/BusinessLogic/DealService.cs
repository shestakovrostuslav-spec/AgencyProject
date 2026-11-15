using System;
using System.Collections.Generic;
using RealEstateAgency.Models;
using RealEstateAgency.DataAccess;

namespace RealEstateAgency.BusinessLogic
{
    public class DealService
    {
        private readonly JsonFileStorage<Deal> storage = new JsonFileStorage<Deal>("deals.json");
        private readonly List<Deal> deals;

        public DealService()
        {
            deals = storage.Load();
        }

        public List<Deal> GetForAgent(string agentName)
        {
            List<Deal> result = new List<Deal>();

            foreach (Deal deal in deals)
            {
                if (string.Equals(deal.AgentName, agentName, StringComparison.OrdinalIgnoreCase))
                    result.Add(deal);
            }

            return result;
        }

        public void Create(Property property, Client client, Agent agent, double commission, PropertyService propertyService)
        {
            if (property.Status != "Заброньовано")
                throw new Exception("Об'єкт має бути заброньованим!");

            if (!string.Equals(property.AgentName, agent.Name, StringComparison.OrdinalIgnoreCase))
                throw new Exception("Це не ваш об'єкт!");

            if (!string.Equals(property.BookedByClientName, client.Name, StringComparison.OrdinalIgnoreCase))
                throw new Exception("Бронювання належить іншому клієнту!");

            if (commission < 0 || commission > 100)
                throw new Exception("Комісія повинна бути від 0 до 100%!");

            foreach (Deal existing in deals)
            {
                if (string.Equals(existing.PropertyAddress, property.Address, StringComparison.OrdinalIgnoreCase))
                    throw new Exception("Для цього об'єкта вже є угода!");
            }

            Deal deal = new Deal
            {
                PropertyAddress = property.Address,
                ClientName = client.Name,
                AgentName = agent.Name,
                Price = property.Price,
                Type = "Купівля",
                CommissionPercent = commission,
                CommissionAmount = property.Price * commission / 100,
                Date = DateTime.Now
            };

            propertyService.MarkSold(property, agent.Name);
            deals.Add(deal);

            try
            {
                storage.Save(deals);
            }
            catch
            {
                deals.Remove(deal);
                propertyService.RollbackSale(property);
                throw;
            }
        }
    }
}