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
            // Создаём сервисы
            PropertyService propertyService =
                new PropertyService();

            ClientService clientService =
                new ClientService();

            AgentService agentService =
                new AgentService();

            // Создаём сервис сделок
            DealService dealService = new DealService();

            // Создаём интерфейс
            ConsoleMenu menu = new ConsoleMenu(
                propertyService,
                clientService,
                agentService,
                dealService);

            // Запускаем программу
            menu.Run();
        }
    }
}
