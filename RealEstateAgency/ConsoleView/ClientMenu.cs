using System;
using RealEstateAgency.BusinessLogic;
using RealEstateAgency.Models;
using System.Collections.Generic;

namespace RealEstateAgency.Presentation
{
    public class ClientMenu
    {
        private readonly PropertyService properties;
        private readonly ClientService clients;
        private readonly string clientName;

        public ClientMenu(
            PropertyService properties,
            ClientService clients,
            string clientName)
        {
            this.properties = properties;
            this.clients = clients;
            this.clientName = clientName;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine($"\n===== КЛІЄНТ: {clientName} =====");
                Console.WriteLine("1. Переглянути доступну нерухомість");
                Console.WriteLine("2. Знайти нерухомість");
                Console.WriteLine("3. Забронювати нерухомість");
                Console.WriteLine("4. Переглянути фільтри (10 останніх)");
                Console.WriteLine("0. Вийти з акаунта");

                string choice = MenuHelpers.ReadText("Ваш вибір: ");

                try
                {
                    switch (choice)
                    {
                        case "1":
                            MenuHelpers.PrintProperties(properties.GetAvailable());
                            break;
                        case "2":
                            Search();
                            break;
                        case "3":
                            Book();
                            break;
                        case "4":
                            ShowSearchHistory();
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

        private void Search()
        {
            Console.WriteLine("Натисніть Enter, щоб пропустити параметр.");

            PropertySearchCriteria criteria = new PropertySearchCriteria();

            criteria.Type = MenuHelpers.ReadText("Тип: ");
            criteria.MinPrice = MenuHelpers.ReadOptionalDouble("Мінімальна ціна: ");
            criteria.MaxPrice = MenuHelpers.ReadOptionalDouble("Максимальна ціна: ");
            criteria.MinArea = MenuHelpers.ReadOptionalDouble("Мінімальна площа: ");
            criteria.MaxArea = MenuHelpers.ReadOptionalDouble("Максимальна площа: ");
            criteria.District = MenuHelpers.ReadText("Район: ");
            criteria.Rooms = MenuHelpers.ReadOptionalInt("Кількість кімнат: ");

            Client? client = clients.FindByName(clientName);

            if (client == null)
            {
                Console.WriteLine("Клієнта не знайдено!");
                return;
            }

            clients.SaveSearch(client, criteria);

            List<Property> result = properties.Search(criteria);

            MenuHelpers.PrintProperties(result);
        }
        private void ShowSearchHistory()
        {
            Client? client = clients.FindByName(clientName);

            if (client == null)
            {
                Console.WriteLine("Клієнта не знайдено!");
                return;
            }

            if (client.SearchHistory == null || client.SearchCount == 0)
            {
                Console.WriteLine("Історія пошуків порожня.");
                return;
            }

            Console.WriteLine("\n========== МОЇ ПОШУКИ ==========");

            for (int i = 0; i < client.SearchCount; i++)
            {
                PropertySearchCriteria criteria = client.SearchHistory[i];

                if (criteria == null)
                {
                    continue;
                }

                Console.WriteLine($"\n{i + 1}. Пошук:");

                if (!string.IsNullOrWhiteSpace(criteria.Type))
                {
                    Console.WriteLine($"   Тип: {criteria.Type}");
                }

                if (criteria.MinPrice.HasValue)
                {
                    Console.WriteLine($"   Ціна від: {criteria.MinPrice.Value} грн");
                }

                if (criteria.MaxPrice.HasValue)
                {
                    Console.WriteLine($"   Ціна до: {criteria.MaxPrice.Value} грн");
                }

                if (criteria.MinArea.HasValue)
                {
                    Console.WriteLine($"   Площа від: {criteria.MinArea.Value} м²");
                }

                if (criteria.MaxArea.HasValue)
                {
                    Console.WriteLine($"   Площа до: {criteria.MaxArea.Value} м²");
                }

                if (!string.IsNullOrWhiteSpace(criteria.District))
                {
                    Console.WriteLine($"   Район: {criteria.District}");
                }

                if (criteria.Rooms.HasValue)
                {
                    Console.WriteLine($"   Кількість кімнат: {criteria.Rooms.Value}");
                }
            }

            Console.WriteLine("\n0. Повернутися");

            int number = MenuHelpers.ReadInt("Виберіть пошук для повторення: ");

            if (number == 0)
            {
                return;
            }

            if (number < 1 || number > client.SearchCount)
            {
                Console.WriteLine("Невірний номер пошуку!");
                return;
            }

            PropertySearchCriteria selectedCriteria = client.SearchHistory[number - 1];

            List<Property> result = properties.Search(selectedCriteria);

            Console.WriteLine("\n===== РЕЗУЛЬТАТ ПОВТОРНОГО ПОШУКУ =====");

            MenuHelpers.PrintProperties(result);
        }

        private void Book()
        {
            var available = properties.GetAvailable();
            MenuHelpers.PrintProperties(available);

            if (available.Count == 0)
                return;

            int n = MenuHelpers.ReadInt("Номер об'єкта для бронювання: ");

            if (n < 1 || n > available.Count)
                throw new Exception("Невірний номер!");

            properties.Book(available[n - 1], clientName);
            Console.WriteLine("Об'єкт успішно заброньовано!");
        }
    }
}