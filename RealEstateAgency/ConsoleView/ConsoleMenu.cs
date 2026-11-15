using RealEstateAgency.BusinessLogic;

namespace RealEstateAgency.Presentation
{
    public class ConsoleMenu
    {
        private readonly AuthenticationMenu menu;

        public ConsoleMenu(PropertyService properties, ClientService clients, AgentService agents, DealService deals)
        {
            AuthenticationService authentication = new AuthenticationService();
            menu = new AuthenticationMenu(authentication, properties, clients, agents, deals);
        }

        public void Run()
        {
            menu.Run();
        }
    }
}
