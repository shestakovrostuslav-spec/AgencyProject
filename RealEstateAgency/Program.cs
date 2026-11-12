using System;
using System.Collections.Generic;
using System.Text;

using RealEstateAgency.BusinessLogic;
using RealEstateAgency.Presentation;

namespace RealEstateAgency
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            Console.InputEncoding = Encoding.UTF8;

            // Створюємо сервіси
            PropertyService propertyService =
                new PropertyService();

            ClientService clientService =
                new ClientService();

            AgentService agentService =
                new AgentService();

            // Створюємо сервіс угод
            DealService dealService = new DealService();

            // Створюємо інтерфейс
            ConsoleMenu menu = new ConsoleMenu(
                propertyService,
                clientService,
                agentService,
                dealService);

            // Запускаємо програму
            menu.Run();
        }
    }
}