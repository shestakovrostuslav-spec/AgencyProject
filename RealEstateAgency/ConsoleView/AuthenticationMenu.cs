using System;
using RealEstateAgency.BusinessLogic;
using RealEstateAgency.Models;

namespace RealEstateAgency.Presentation
{
    public class AuthenticationMenu
    {
        private readonly AuthenticationService auth;
        private readonly PropertyService properties;
        private readonly ClientService clients;
        private readonly AgentService agents;
        private readonly DealService deals;

        public AuthenticationMenu(
            AuthenticationService auth,
            PropertyService properties,
            ClientService clients,
            AgentService agents,
            DealService deals)
        {
            this.auth = auth;
            this.properties = properties;
            this.clients = clients;
            this.agents = agents;
            this.deals = deals;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine("\n===== АГЕНТСТВО НЕРУХОМОСТІ =====");
                Console.WriteLine("1. Увійти як клієнт");
                Console.WriteLine("2. Увійти як рієлтор");
                Console.WriteLine("3. Зареєструвати клієнта");
                Console.WriteLine("4. Зареєструвати рієлтора");
                Console.WriteLine("0. Вийти");

                string choice = MenuHelpers.ReadText("Ваш вибір: ");

                try
                {
                    switch (choice)
                    {
                        case "1":
                            Login("Client");
                            break;
                        case "2":
                            Login("Agent");
                            break;
                        case "3":
                            RegisterClient();
                            break;
                        case "4":
                            RegisterAgent();
                            break;
                        case "0":
                            return;
                        default:
                            Console.WriteLine("Невірний пункт меню!");
                            break;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine("Помилка: " + e.Message);
                }
            }
        }

        private void Login(string role)
        {
            string name = MenuHelpers.ReadText("Ім'я: ");
            string password = MenuHelpers.ReadPassword("Пароль: ");

            UserAccount? account = auth.Login(name, password, role);

            if (account == null)
            {
                Console.WriteLine("Неправильне ім'я або пароль!");
                return;
            }

            Console.WriteLine("Вхід успішний!");

            if (role == "Client")
            {
                ClientMenu menu = new ClientMenu(properties, clients, account.Name);
                menu.Run();
            }
            else
            {
                Agent? agent = agents.FindByName(account.Name);
                if (agent == null)
                    throw new Exception("Профіль рієлтора не знайдено!");

                new AgentMenu(properties, clients, deals, agent).Run();
            }
        }

        private void RegisterClient()
        {
            string name = MenuHelpers.ReadText("Ім'я клієнта: ");
            string password = MenuHelpers.ReadPassword("Пароль: ");

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(password))
                throw new Exception("Некоректне ім'я або порожній пароль!");

            if (clients.FindByName(name) != null)
                throw new Exception("Клієнт із таким ім'ям вже існує!");

            Console.WriteLine("1. Покупець (купівля нерухомості)");
            Console.WriteLine("2. Орендар (оренда нерухомості)");
            string typeChoice = MenuHelpers.ReadText("Оберіть тип клієнта: ");
            string clientType = "";
            if (typeChoice == "1")
            {
                clientType = "Покупець";
            }
            else if (typeChoice == "2")
            {
                clientType = "Орендар";
            }
            else
            {
                throw new Exception("Оберіть 1 або 2!");
            }

            clients.Add(new Client { Name = name, ClientType = clientType });
            auth.Register(name, password, "Client");

            Console.WriteLine("Клієнта зареєстровано!");
        }

        private void RegisterAgent()
        {
            string name = MenuHelpers.ReadText("Ім'я рієлтора: ");
            string password = MenuHelpers.ReadPassword("Пароль: ");
            int experience = MenuHelpers.ReadInt("Досвід роботи (років): ");

            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrEmpty(password) || experience < 0)
                throw new Exception("Некоректні дані!");

            if (agents.FindByName(name) != null)
                throw new Exception("Рієлтор із таким ім'ям вже існує!");

            agents.Add(new Agent { Name = name, Experience = experience });
            auth.Register(name, password, "Agent");

            Console.WriteLine("Рієлтора зареєстровано!");
        }
    }
}