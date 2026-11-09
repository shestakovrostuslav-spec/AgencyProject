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

        // Главное меню
        public void Run()
        {
            bool running = true;

            while (running)
            {
                Console.WriteLine("\n===== РИЕЛТОРСКОЕ АГЕНТСТВО =====");
                Console.WriteLine("1. Добавить недвижимость");
                Console.WriteLine("2. Показать недвижимость");
                Console.WriteLine("3. Изменить недвижимость");
                Console.WriteLine("4. Удалить недвижимость");

                Console.WriteLine("\n5. Добавить клиента");
                Console.WriteLine("6. Показать клиентов");
                Console.WriteLine("7. Изменить клиента");
                Console.WriteLine("8. Удалить клиента");

                Console.WriteLine("\n9. Добавить риелтора");
                Console.WriteLine("10. Показать риелторов");

                Console.WriteLine("\n11. Создать сделку");
                Console.WriteLine("12. Показать сделки");

                Console.WriteLine("\n0. Выход");

                Console.Write("\nВыберите действие: ");

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
                            Console.WriteLine("Программа завершена.");
                            break;

                        default:
                            Console.WriteLine("Неверный пункт меню!");
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Ошибка: " + ex.Message);
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

                Console.WriteLine("Введите целое число!");
            }
        }
        // Ввод текста
        private string ReadText(string message)
        {
            Console.Write(message);
            return Console.ReadLine() ?? "";
        }

        // Ввод числа
        private double ReadDouble(string message)
        {
            while (true)
            {
                Console.Write(message);

                if (double.TryParse(Console.ReadLine(), out double value))
                    return value;

                Console.WriteLine("Введите корректное число!");
            }
        }

        // Ввод ID
        private Guid ReadId(string message)
        {
            while (true)
            {
                string value = ReadText(message);

                if (Guid.TryParse(value, out Guid id))
                    return id;

                Console.WriteLine("Некорректный ID!");
            }
        }

        // Добавление недвижимости
        private void AddProperty()
        {
            Property property = new Property();

            property.Type = ReadText("Тип недвижимости: ");
            property.Area = ReadDouble("Площадь: ");
            property.Address = ReadText("Адрес: ");
            property.Price = ReadDouble("Цена: ");
            property.Status = ReadText("Статус: ");

            propertyService.Add(property);

            Console.WriteLine("Недвижимость добавлена!");
        }

        // Просмотр недвижимости
        private void ShowProperties()
        {
            var properties = propertyService.GetAll();

            if (properties.Count == 0)
            {
                Console.WriteLine("Список недвижимости пуст.");
                return;
            }

            for (int i = 0; i < properties.Count; i++)
            {
                var p = properties[i];

                Console.WriteLine($"\nНомер: {i + 1}");
                Console.WriteLine($"Тип: {p.Type}");
                Console.WriteLine($"Площадь: {p.Area}");
                Console.WriteLine($"Адрес: {p.Address}");
                Console.WriteLine($"Цена: {p.Price}");
                Console.WriteLine($"Статус: {p.Status}");
            }
        }

        // Изменение недвижимости
        private void UpdateProperty()
        {
            ShowProperties();

            int number = ReadInt("\nНомер недвижимости: ");

            Property updated = new Property();

            updated.Type = ReadText("Новый тип: ");
            updated.Area = ReadDouble("Новая площадь: ");
            updated.Address = ReadText("Новый адрес: ");
            updated.Price = ReadDouble("Новая цена: ");
            updated.Status = ReadText("Новый статус: ");

            propertyService.Update(number, updated);

            Console.WriteLine("Недвижимость изменена!");
        }

        // Удаление недвижимости
        private void DeleteProperty()
        {
            ShowProperties();

            int number = ReadInt("\nНомер недвижимости: ");

            propertyService.Delete(number);

            Console.WriteLine("Недвижимость удалена!");
        }

        // Добавление клиента
        private void AddClient()
        {
            Client client = new Client();

            client.Name = ReadText("Имя клиента: ");
            client.Phone = ReadText("Телефон: ");
            client.Email = ReadText("Email: ");
            client.Type = ReadText("Тип (Покупатель/Владелец): ");

            clientService.Add(client);

            Console.WriteLine("Клиент добавлен!");
        }

        // Просмотр клиентов
        private void ShowClients()
        {
            var clients = clientService.GetAll();

            if (clients.Count == 0)
            {
                Console.WriteLine("Список клиентов пуст.");
                return;
            }

            for (int i = 0; i < clients.Count; i++)
            {
                var c = clients[i];

                Console.WriteLine($"\nНомер: {i + 1}");
                Console.WriteLine($"Имя: {c.Name}");
                Console.WriteLine($"Телефон: {c.Phone}");
                Console.WriteLine($"Email: {c.Email}");
                Console.WriteLine($"Тип: {c.Type}");
            }
        }

        // Изменение клиента
        private void UpdateClient()
        {
            ShowClients();

            int number = ReadInt("\nНомер клиента: ");

            Client updated = new Client();

            updated.Name = ReadText("Новое имя: ");
            updated.Phone = ReadText("Новый телефон: ");
            updated.Email = ReadText("Новый Email: ");
            updated.Type = ReadText("Новый тип: ");

            clientService.Update(number, updated);

            Console.WriteLine("Клиент изменён!");
        }

        // Удаление клиента
        private void DeleteClient()
        {
            ShowClients();

            int number = ReadInt("\nНомер клиента: ");

            clientService.Delete(number);

            Console.WriteLine("Клиент удалён!");
        }

        // Добавление риелтора
        private void AddAgent()
        {
            Agent agent = new Agent();

            agent.Name = ReadText("Имя риелтора: ");
            agent.Experience = (int)ReadDouble("Опыт работы (лет): ");

            agentService.Add(agent);

            Console.WriteLine("Риелтор добавлен!");
        }

        // Просмотр риелторов
        private void ShowAgents()
        {
            var agents = agentService.GetAll();

            if (agents.Count == 0)
            {
                Console.WriteLine("Список риелторов пуст.");
                return;
            }

            for (int i = 0; i < agents.Count; i++)
            {
                var a = agents[i];

                Console.WriteLine($"\nНомер: {i + 1}");
                Console.WriteLine($"Имя: {a.Name}");
                Console.WriteLine($"Опыт: {a.Experience} лет");
            }
        }

        // Создание сделки
        private void AddDeal()
        {
            Console.WriteLine("\n--- Недвижимость ---");
            ShowProperties();

            Console.WriteLine("\n--- Клиенты ---");
            ShowClients();

            Console.WriteLine("\n--- Риелторы ---");
            ShowAgents();

            int propertyNumber = ReadInt("\nНомер недвижимости: ");
            int clientNumber = ReadInt("Номер клиента: ");
            int agentNumber = ReadInt("Номер риелтора: ");

            var properties = propertyService.GetAll();
            var clients = clientService.GetAll();
            var agents = agentService.GetAll();

            if (propertyNumber < 1 || propertyNumber > properties.Count ||
                clientNumber < 1 || clientNumber > clients.Count ||
                agentNumber < 1 || agentNumber > agents.Count)
            {
                Console.WriteLine("Неверно выбран номер!");
                return;
            }

            double price = ReadDouble("Стоимость сделки: ");

            string type = ReadText("Тип сделки (Покупка/Аренда): ");

            double commission = ReadDouble("Комиссия (%): ");

            dealService.Add(
                properties[propertyNumber - 1],
                clients[clientNumber - 1],
                agents[agentNumber - 1],
                price,
                type,
                commission
            );

            Console.WriteLine("Сделка успешно создана!");
        }

        // Просмотр сделок
        private void ShowDeals()
        {
            var deals = dealService.GetAll();

            if (deals.Count == 0)
            {
                Console.WriteLine("Список сделок пуст.");
                return;
            }

            for (int i = 0; i < deals.Count; i++)
            {
                var d = deals[i];

                Console.WriteLine($"\nСделка №{i + 1}");
                Console.WriteLine($"Недвижимость: {d.PropertyAddress}");
                Console.WriteLine($"Клиент: {d.ClientName}");
                Console.WriteLine($"Риелтор: {d.AgentName}");
                Console.WriteLine($"Стоимость: {d.Price}");
                Console.WriteLine($"Тип: {d.Type}");
                Console.WriteLine($"Комиссия: {d.CommissionPercent}%");
                Console.WriteLine($"Дата: {d.Date}");
            }
        }
    }
}
