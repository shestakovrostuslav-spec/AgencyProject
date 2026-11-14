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

                Console.WriteLine("13. Пошук нерухомості");
                Console.WriteLine("14. Змінити статус нерухомості");
                Console.WriteLine("15. Історія статусів об'єкта");

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

                        case "13":
                            SearchProperties();
                            break;

                        case "14":
                            ChangePropertyStatus();
                            break;

                        case "15":
                            ShowStatusHistory();
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

        // Додавання нерухомості


        private void AddProperty()
        {
            Property property = new Property();

            property.Type = ReadText("Тип нерухомості: ");

            property.Area = ReadDouble("Площа: ");

            property.Address = ReadText("Адреса: ");

            property.District = ReadText("Район: ");

            property.Rooms = ReadInt("Кількість кімнат: ");

            property.Price = ReadDouble("Ціна: ");

            property.Status = "На продаж";

            propertyService.Add(property);

            Console.WriteLine("Нерухомість успішно додана!");
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

                Console.WriteLine("\n--------------------");

                Console.WriteLine($"Номер: {i + 1}");
                Console.WriteLine($"Тип: {p.Type}");
                Console.WriteLine($"Площа: {p.Area} м2");
                Console.WriteLine($"Адреса: {p.Address}");
                Console.WriteLine($"Район: {p.District}");
                Console.WriteLine($"Кімнат: {p.Rooms}");
                Console.WriteLine($"Ціна: {p.Price}");
                Console.WriteLine($"Статус: {p.Status}");
            }
        }


        // Зміна нерухомості

        private void UpdateProperty()
        {
            ShowProperties();

            int number = ReadInt("\nВведіть номер нерухомості: ");

            Property updated = new Property();

            updated.Type = ReadText("Новий тип: ");

            updated.Area = ReadDouble("Нова площа: ");

            updated.Address = ReadText("Нова адреса: ");

            updated.District = ReadText("Новий район: ");

            updated.Rooms = ReadInt("Нова кількість кімнат: ");

            updated.Price = ReadDouble("Нова ціна: ");

            propertyService.Update(number, updated);

            Console.WriteLine("Дані нерухомості оновлено!");
        }

        private double? ReadOptionalDouble(string message)
        {
            while (true)
            {
                Console.Write(message);

                string input = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (double.TryParse(input, out double value))
                    return value;

                Console.WriteLine("Введіть коректне число!");
            }
        }

        private int? ReadOptionalInt(string message)
        {
            while (true)
            {
                Console.Write(message);

                string input = Console.ReadLine() ?? "";

                if (string.IsNullOrWhiteSpace(input))
                    return null;

                if (int.TryParse(input, out int value))
                    return value;

                Console.WriteLine("Введіть ціле число!");
            }
        }


        private void SearchProperties()
        {
            Console.WriteLine("\n===== ПОШУК НЕРУХОМОСТІ =====");

            Console.WriteLine("Якщо параметр не потрібен, натисніть Enter.");

            PropertySearchCriteria criteria =
                new PropertySearchCriteria();

            criteria.Type = ReadText("Тип нерухомості: ");

            criteria.MinPrice =
                ReadOptionalDouble("Мінімальна ціна: ");

            criteria.MaxPrice =
                ReadOptionalDouble("Максимальна ціна: ");

            criteria.MinArea =
                ReadOptionalDouble("Мінімальна площа: ");

            criteria.MaxArea =
                ReadOptionalDouble("Максимальна площа: ");

            criteria.District = ReadText("Район: ");

            criteria.Rooms =
                ReadOptionalInt("Кількість кімнат: ");

            var result = propertyService.Search(criteria);

            if (result.Count == 0)
            {
                Console.WriteLine("Нерухомість не знайдена.");
                return;
            }

            Console.WriteLine("\n===== РЕЗУЛЬТАТИ ПОШУКУ =====");

            for (int i = 0; i < result.Count; i++)
            {
                var p = result[i];

                Console.WriteLine("\n--------------------");

                Console.WriteLine($"Номер: {i + 1}");
                Console.WriteLine($"Тип: {p.Type}");
                Console.WriteLine($"Площа: {p.Area} м2");
                Console.WriteLine($"Адреса: {p.Address}");
                Console.WriteLine($"Район: {p.District}");
                Console.WriteLine($"Кімнат: {p.Rooms}");
                Console.WriteLine($"Ціна: {p.Price}");
                Console.WriteLine($"Статус: {p.Status}");
            }

            Console.WriteLine($"\nЗнайдено об'єктів: {result.Count}");
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

                Console.WriteLine("\n--------------------");

                Console.WriteLine($"Угода №{i + 1}");

                Console.WriteLine($"Нерухомість: {d.PropertyAddress}");

                Console.WriteLine($"Клієнт: {d.ClientName}");

                Console.WriteLine($"Ріелтор: {d.AgentName}");

                Console.WriteLine($"Вартість: {d.Price}");

                Console.WriteLine($"Тип: {d.Type}");

                Console.WriteLine(
                    $"Комісія: {d.CommissionPercent}%");

                Console.WriteLine(
                    $"Сума комісії: {d.CommissionAmount}");

                Console.WriteLine($"Дата: {d.Date}");
            }
        }



        private void ChangePropertyStatus()
        {
            ShowProperties();

            int number = ReadInt(
                "\nВведіть номер нерухомості: ");

            var properties = propertyService.GetAll();

            if (number < 1 || number > properties.Count)
            {
                Console.WriteLine("Неправильний номер!");
                return;
            }

            Property property = properties[number - 1];

            // Видаляємо зайві пробіли зі статусу
            string currentStatus = property.Status.Trim();

            Console.WriteLine($"\nПоточний статус: {currentStatus}");

            string newStatus = "";

            // Перевіряємо поточний статус
            if (currentStatus == "На продаж" ||
                currentStatus == "На продажу")
            {
                Console.WriteLine("1. Забронювати");
                Console.Write("Ваш вибір: ");

                if (ReadText("") == "1")
                {
                    newStatus = "Заброньовано";
                }
                else
                {
                    Console.WriteLine("Неправильний вибір!");
                    return;
                }
            }
            else if (currentStatus == "Заброньовано")
            {
                Console.WriteLine("1. Позначити як продано");
                Console.Write("Ваш вибір: ");

                if (ReadText("") == "1")
                {
                    newStatus = "Продано";
                }
                else
                {
                    Console.WriteLine("Неправильний вибір!");
                    return;
                }
            }
            else if (currentStatus == "Продано")
            {
                Console.WriteLine(
                    "Цей об'єкт вже продано. Змінити статус неможливо!");
                return;
            }
            else
            {
                Console.WriteLine(
                    $"Невідомий статус об'єкта: {currentStatus}");
                return;
            }

            // Викликаємо зміну статусу
            propertyService.ChangeStatus(number, newStatus);

            Console.WriteLine("Статус успішно змінено!");
        }



        private void ShowStatusHistory()
        {
            ShowProperties();

            int number = ReadInt(
                "\nВведіть номер нерухомості: ");

            var history = propertyService.GetStatusHistory(number);

            if (history.Count == 0)
            {
                Console.WriteLine(
                    "Історія змін статусу відсутня.");
                return;
            }

            Console.WriteLine("\n===== ІСТОРІЯ ЗМІН СТАТУСУ =====");

            foreach (var item in history)
            {
                Console.WriteLine(
                    $"{item.OldStatus} -> {item.NewStatus}");

                Console.WriteLine(
                    $"Дата: {item.ChangeDate}");

                Console.WriteLine("--------------------");
            }
        }

    }
}