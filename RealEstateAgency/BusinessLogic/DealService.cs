
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

        // Получение всех сделок
        public List<Deal> GetAll()
        {
            return deals;
        }

        // Создание сделки
        public void Add(
            Property property,
            Client client,
            Agent agent,
            double price,
            string type,
            double commission)
        {
            if (price <= 0)
                throw new Exception("Некоректна вартість угоди!");

            if (commission < 0 || commission > 100)
                throw new Exception("Некоректний відсоток комісії!");

            // Расчет комиссии
            double commissionAmount =
                price * commission / 100;

            // Создание сделки
            Deal deal = new Deal
            {
                PropertyAddress = property.Address,
                ClientName = client.Name,
                AgentName = agent.Name,

                Price = price,

                Type = type,

                CommissionPercent = commission,

                CommissionAmount = commissionAmount,

                Date = DateTime.Now
            };

            deals.Add(deal);

            storage.Save(deals);
        }
    }
}
