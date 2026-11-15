using System;
using RealEstateAgency.BusinessLogic;
using RealEstateAgency.Models;

namespace RealEstateAgency.Presentation
{
    public class ClientMenu
    {
        private readonly PropertyService properties;
        private readonly string clientName;

        public ClientMenu(PropertyService properties, string clientName)
        {
            this.properties = properties;
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

            var c = new PropertySearchCriteria
            {
                Type = MenuHelpers.ReadText("Тип: "),
                MinPrice = MenuHelpers.ReadOptionalDouble("Мінімальна ціна: "),
                MaxPrice = MenuHelpers.ReadOptionalDouble("Максимальна ціна: "),
                MinArea = MenuHelpers.ReadOptionalDouble("Мінімальна площа: "),
                MaxArea = MenuHelpers.ReadOptionalDouble("Максимальна площа: "),
                District = MenuHelpers.ReadText("Район: "),
                Rooms = MenuHelpers.ReadOptionalInt("Кількість кімнат: ")
            };

            MenuHelpers.PrintProperties(properties.Search(c));
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