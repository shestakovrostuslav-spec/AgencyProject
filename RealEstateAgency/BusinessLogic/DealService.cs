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
        public List<Deal> GetAll()
        {
            return deals;
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
        public void ShowActiveClients()
        {
            List<string> clientNames = new List<string>();
            List<int> dealCounts = new List<int>();

            foreach (Deal deal in deals)
            {
                int index = -1;

                for (int i = 0; i < clientNames.Count; i++)
                {
                    if (clientNames[i] == deal.ClientName)
                    {
                        index = i;
                        break;
                    }
                }

                if (index == -1)
                {
                    clientNames.Add(deal.ClientName);
                    dealCounts.Add(1);
                }
                else
                {
                    dealCounts[index]++;
                }
            }

            for (int i = 0; i < dealCounts.Count - 1; i++)
            {
                for (int j = 0; j < dealCounts.Count - i - 1; j++)
                {
                    if (dealCounts[j] < dealCounts[j + 1])
                    {
                        int tempCount = dealCounts[j];
                        dealCounts[j] = dealCounts[j + 1];
                        dealCounts[j + 1] = tempCount;

                        string tempName = clientNames[j];
                        clientNames[j] = clientNames[j + 1];
                        clientNames[j + 1] = tempName;
                    }
                }
            }

            Console.WriteLine("\n===== НАЙАКТИВНІШІ КЛІЄНТИ =====");

            if (clientNames.Count == 0)
            {
                Console.WriteLine("Угод поки немає.");
                return;
            }

            for (int i = 0; i < clientNames.Count; i++)
            {
                Console.WriteLine(
                    $"{i + 1}. {clientNames[i]} - {dealCounts[i]} угод"
                );
            }
        }
        public int GetDealsCountForAgent(string agentName)
        {
            int count = 0;

            foreach (Deal deal in deals)
            {
                if (deal.AgentName == agentName)
                {
                    count++;
                }
            }

            return count;
        }

        public double GetIncomeForAgent(string agentName)
        {
            double income = 0;

            foreach (Deal deal in deals)
            {
                if (deal.AgentName == agentName)
                {
                    income += deal.CommissionAmount;
                }
            }

            return income;
        }
        public int GetDealsCountByPeriod(DateTime start, DateTime end)
        {
            int count = 0;

            foreach (Deal deal in deals)
            {
                if (deal.Date.Date >= start.Date && deal.Date.Date <= end.Date)
                {
                    count++;
                }
            }

            return count;
        }
        public double GetTotalSales()
        {
            double totalSales = 0;

            foreach (Deal deal in deals)
            {
                totalSales += deal.Price;
            }

            return totalSales;
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