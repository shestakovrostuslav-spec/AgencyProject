using System;
using System.Text;
using RealEstateAgency.BusinessLogic;
using RealEstateAgency.Presentation;

namespace RealEstateAgency
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.InputEncoding = Encoding.Unicode;
            Console.OutputEncoding = Encoding.Unicode;
            var propertyService = new PropertyService();
            var clientService = new ClientService();
            var agentService = new AgentService();
            var dealService = new DealService();
            new ConsoleMenu(propertyService, clientService, agentService, dealService).Run();
        }
    }
}
