using System;
using System.Collections.Generic;
using System.Text;
using RealEstateAgency.Models;
using RealEstateAgency.BusinessLogic;

namespace RealEstateAgency.Presentation
{
    public class ConsoleMenu
    {
        private readonly PropertyService propertyService;
        private readonly ClientService clientService;
        private readonly AgentService agentService;
        private readonly DealService dealService;

        public ConsoleMenu(
            PropertyService properties,
            ClientService clients,
            AgentService agents,
            DealService deals)
        {
            propertyService = properties;
            clientService = clients;
            agentService = agents;
            dealService = deals;
        }

        // Головне меню
        public void Run()
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n===== РІЄЛТОРСЬКЕ АГЕНТСТВО =====");
                Console.WriteLine("1. Додати нерухомість");
                Console.WriteLine("2. Показати нерухомість");
                Console.WriteLine("3. Змінити нерухомість");
                Console.WriteLine("4. Видалити нерухомість");

                Console.WriteLine("\n5. Додати клієнта");
                Console.WriteLine("6. Показати клієнтів");
                Console.WriteLine("7. Змінити клієнта");
                Console.WriteLine("8. Видалити клієнта");

                Console.WriteLine("\n9. Додати рієлтора");
                Console.WriteLine("10. Показати рієлторів");

                Console.WriteLine("\n11. Створити угоду");
                Console.WriteLine("12. Показати угоди");

                Console.WriteLine("\n0. Вихід");

                Console.Write("\nОберіть дію: ");

                string choice = Console.ReadLine() ?? "";

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddProperty();
                            break;

                        case "2":
                            ShowProperties();
                            break;

                        case "3":
                            UpdateProperty();
                            break;

                        case "4":
                            DeleteProperty();
                            break;

                        case "5":
                            AddClient();
                            break;

                        case "6":
                            ShowClients();
                            break;

                        case "7":
                            UpdateClient();
                            break;

                        case "8":
                            DeleteClient();
                            break;

                        case "9":
                            AddAgent();
                            break;

                        case "10":
                            ShowAgents();
                            break;

                        case "11":
                            AddDeal();
                            break;

                        case "12":
                            ShowDeals();
                            break;

                        case "0":
                            running = false;
                            Console.WriteLine("Програму завершено.");
                            break;

                        default:
                            Console.WriteLine("Невірний пункт меню!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Помилка: " + ex.Message);
                }
            }
        }

        private int ReadInt(string message)
        {
            while (true)
            {
                Console.Write(message);

                if (int.TryParse(Console.ReadLine(), out int value))
                    return value;

                Console.WriteLine("Введіть ціле число!");
            }
        }

        // Введення тексту
        private string ReadText(string message)
        {
            Console.Write(message);
            return Console.ReadLine() ?? "";
        }

        // Введення числа
        private double ReadDouble(string message)
        {
            while (true)
            {
                Console.Write(message);

                if (double.TryParse(Console.ReadLine(), out double value))
                    return value;

                Console.WriteLine("Введіть коректне число!");
            }
        }

        // Введення ID
        private Guid ReadId(string message)
        {
            while (true)
            {
                string value = ReadText(message);

                if (Guid.TryParse(value, out Guid id))
                    return id;

                Console.WriteLine("Некоректний ID!");
            }
        }

        // Додавання нерухомості
        private void AddProperty()
        {
            Property property = new Property();

            property.Type = ReadText("Тип нерухомості: ");
            property.Area = ReadDouble("Площа: ");
            property.Address = ReadText("Адреса: ");
            property.Price = ReadDouble("Ціна: ");
            property.Status = ReadText("Статус: ");

            propertyService.Add(property);

            Console.WriteLine("Нерухомість додано!");
        }

        // Перегляд нерухомості
        private void ShowProperties()
        {
            var properties = propertyService.GetAll();

            if (properties.Count == 0)
            {
                Console.WriteLine("Список нерухомості порожній.");
                return;
            }

            for (int i = 0; i < properties.Count; i++)
            {
                var p = properties[i];

                Console.WriteLine($"\nНомер: {i + 1}");
                Console.WriteLine($"Тип: {p.Type}");
                Console.WriteLine($"Площа: {p.Area}");
                Console.WriteLine($"Адреса: {p.Address}");
                Console.WriteLine($"Ціна: {p.Price}");
                Console.WriteLine($"Статус: {p.Status}");
            }
        }

        // Зміна нерухомості
        private void UpdateProperty()
        {
            ShowProperties();

            int number = ReadInt("\nНомер нерухомості: ");

            Property updated = new Property();

            updated.Type = ReadText("Новий тип: ");
            updated.Area = ReadDouble("Нова площа: ");
            updated.Address = ReadText("Нова адреса: ");
            updated.Price = ReadDouble("Нова ціна: ");
            updated.Status = ReadText("Новий статус: ");

            propertyService.Update(number, updated);

            Console.WriteLine("Нерухомість змінено!");
        }

        // Видалення нерухомості
        private void DeleteProperty()
        {
            ShowProperties();

            int number = ReadInt("\nНомер нерухомості: ");

            propertyService.Delete(number);

            Console.WriteLine("Нерухомість видалено!");
        }

        // Додавання клієнта
        private void AddClient()
        {
            Client client = new Client();

            client.Name = ReadText("Ім'я клієнта: ");
            client.Phone = ReadText("Телефон: ");
            client.Email = ReadText("Email: ");
            client.Type = ReadText("Тип (Покупець/Власник): ");

            clientService.Add(client);

            Console.WriteLine("Клієнта додано!");
        }

        // Перегляд клієнтів
        private void ShowClients()
        {
            var clients = clientService.GetAll();

            if (clients.Count == 0)
            {
                Console.WriteLine("Список клієнтів порожній.");
                return;
            }

            for (int i = 0; i < clients.Count; i++)
            {
                var c = clients[i];

                Console.WriteLine($"\nНомер: {i + 1}");
                Console.WriteLine($"Ім'я: {c.Name}");
                Console.WriteLine($"Телефон: {c.Phone}");
                Console.WriteLine($"Email: {c.Email}");
                Console.WriteLine($"Тип: {c.Type}");
            }
        }

        // Зміна клієнта
        private void UpdateClient()
        {
            ShowClients();

            int number = ReadInt("\nНомер клієнта: ");

            Client updated = new Client();

            updated.Name = ReadText("Нове ім'я: ");
            updated.Phone = ReadText("Новий телефон: ");
            updated.Email = ReadText("Новий Email: ");
            updated.Type = ReadText("Новий тип: ");

            clientService.Update(number, updated);

            Console.WriteLine("Клієнта змінено!");
        }

        // Видалення клієнта
        private void DeleteClient()
        {
            ShowClients();

            int number = ReadInt("\nНомер клієнта: ");

            clientService.Delete(number);

            Console.WriteLine("Клієнта видалено!");
        }

        // Додавання рієлтора
        private void AddAgent()
        {
            Agent agent = new Agent();

            agent.Name = ReadText("Ім'я рієлтора: ");
            agent.Experience = (int)ReadDouble("Досвід роботи (років): ");

            agentService.Add(agent);

            Console.WriteLine("Рієлтора додано!");
        }

        // Перегляд рієлторів
        private void ShowAgents()
        {
            var agents = agentService.GetAll();

            if (agents.Count == 0)
            {
                Console.WriteLine("Список рієлторів порожній.");
                return;
            }

            for (int i = 0; i < agents.Count; i++)
            {
                var a = agents[i];

                Console.WriteLine($"\nНомер: {i + 1}");
                Console.WriteLine($"Ім'я: {a.Name}");
                Console.WriteLine($"Досвід: {a.Experience} років");
            }
        }

        // Створення угоди
        private void AddDeal()
        {
            Console.WriteLine("\n--- Нерухомість ---");
            ShowProperties();

            Console.WriteLine("\n--- Клієнти ---");
            ShowClients();

            Console.WriteLine("\n--- Рієлтори ---");
            ShowAgents();

            int propertyNumber = ReadInt("\nНомер нерухомості: ");
            int clientNumber = ReadInt("Номер клієнта: ");
            int agentNumber = ReadInt("Номер рієлтора: ");

            var properties = propertyService.GetAll();
            var clients = clientService.GetAll();
            var agents = agentService.GetAll();

            if (propertyNumber < 1 || propertyNumber > properties.Count ||
                clientNumber < 1 || clientNumber > clients.Count ||
                agentNumber < 1 || agentNumber > agents.Count)
            {
                Console.WriteLine("Невірно обраний номер!");
                return;
            }

            double price = ReadDouble("Вартість угоди: ");

            string type = ReadText("Тип угоди (Купівля/Оренда): ");

            double commission = ReadDouble("Комісія (%): ");

            dealService.Add(
                properties[propertyNumber - 1],
                clients[clientNumber - 1],
                agents[agentNumber - 1],
                price,
                type,
                commission
            );

            Console.WriteLine("Угоду успішно створено!");
        }

        // Перегляд угод
        private void ShowDeals()
        {
            var deals = dealService.GetAll();

            if (deals.Count == 0)
            {
                Console.WriteLine("Список угод порожній.");
                return;
            }

            for (int i = 0; i < deals.Count; i++)
            {
                var d = deals[i];

                Console.WriteLine($"\nУгода №{i + 1}");
                Console.WriteLine($"Нерухомість: {d.PropertyAddress}");
                Console.WriteLine($"Клієнт: {d.ClientName}");
                Console.WriteLine($"Рієлтор: {d.AgentName}");
                Console.WriteLine($"Вартість: {d.Price}");
                Console.WriteLine($"Тип: {d.Type}");
                Console.WriteLine($"Комісія: {d.CommissionPercent}%");
                Console.WriteLine($"Дата: {d.Date}");
            }
        }
    }
}