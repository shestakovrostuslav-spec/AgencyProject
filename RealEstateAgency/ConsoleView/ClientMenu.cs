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
                Client? client = clients.FindByName(clientName);
                if (client == null)
                {
                    Console.WriteLine("Клієнта не знайдено!");
                    return;
                }

                bool tenant = client.ClientType == "Орендар";
                Console.WriteLine($"\n===== {client.ClientType.ToUpper()}: {clientName} =====");
                Console.WriteLine(tenant ? "1. Переглянути доступні об'єкти для оренди" : "1. Переглянути об'єкти на продаж");
                Console.WriteLine("2. Знайти нерухомість за фільтрами");
                Console.WriteLine(tenant ? "3. Забронювати об'єкт для оренди" : "3. Забронювати об'єкт для купівлі");
                Console.WriteLine("4. Історія останніх 10 пошуків");
                Console.WriteLine("5. Мій поточний запит (останній фільтр)");
                Console.WriteLine("6. Рекомендації за історією пошуків");
                Console.WriteLine("0. Вийти з акаунта");

                string choice = MenuHelpers.ReadText("Ваш вибір: ");
                try
                {
                    switch (choice)
                    {
                        case "1":
                            MenuHelpers.PrintProperties(GetAvailableForClient(client));
                            break;
                        case "2":
                            Search(client);
                            break;
                        case "3":
                            Book(client);
                            break;
                        case "4":
                            ShowSearchHistory();
                            break;
                        case "5":
                            ShowLatestRequest(client);
                            break;
                        case "6":
                            MenuHelpers.PrintProperties(properties.GetRecommendations(client));
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

        private List<Property> GetAvailableForClient(Client client)
        {
            PropertySearchCriteria criteria = new PropertySearchCriteria();
            criteria.ListingType = client.ClientType == "Орендар" ? "Оренда" : "Продаж";
            return properties.Search(criteria);
        }

        private void Search(Client client)
        {
            Console.WriteLine("Натисніть Enter, щоб пропустити параметр.");
            PropertySearchCriteria criteria = new PropertySearchCriteria();
            criteria.ListingType = client.ClientType == "Орендар" ? "Оренда" : "Продаж";
            criteria.Type = MenuHelpers.ReadText("Тип нерухомості: ");
            criteria.MinPrice = MenuHelpers.ReadOptionalDouble(criteria.ListingType == "Оренда" ? "Мінімальна оренда (грн/міс.): " : "Мінімальна ціна (грн): ");
            criteria.MaxPrice = MenuHelpers.ReadOptionalDouble(criteria.ListingType == "Оренда" ? "Максимальна оренда (грн/міс.): " : "Максимальна ціна (грн): ");
            criteria.MinArea = MenuHelpers.ReadOptionalDouble("Мінімальна площа (м²): ");
            criteria.MaxArea = MenuHelpers.ReadOptionalDouble("Максимальна площа (м²): ");
            criteria.District = MenuHelpers.ReadText("Район: ");
            criteria.Rooms = MenuHelpers.ReadOptionalInt("Кількість кімнат: ");

            if (criteria.MinPrice.HasValue && criteria.MaxPrice.HasValue && criteria.MinPrice.Value > criteria.MaxPrice.Value)
            {
                throw new Exception("Мінімальна ціна не може перевищувати максимальну!");
            }
            if (criteria.MinArea.HasValue && criteria.MaxArea.HasValue && criteria.MinArea.Value > criteria.MaxArea.Value)
            {
                throw new Exception("Мінімальна площа не може перевищувати максимальну!");
            }

            clients.SaveSearch(client, criteria);
            MenuHelpers.PrintProperties(properties.Search(criteria));
        }

        private void ShowLatestRequest(Client client)
        {
            PropertySearchCriteria? criteria = clients.GetLatestRequest(client);
            if (criteria == null)
            {
                Console.WriteLine("Запиту ще немає. Спочатку виконайте пошук.");
                return;
            }

            criteria.ListingType = client.ClientType == "Орендар" ? "Оренда" : "Продаж";
            Console.WriteLine("\n===== МІЙ ПОТОЧНИЙ ЗАПИТ =====");
            PrintCriteria(criteria);
            Console.WriteLine("\nОб'єкти за поточним запитом:");
            MenuHelpers.PrintProperties(properties.Search(criteria));
        }

        private void PrintCriteria(PropertySearchCriteria criteria)
        {
            Console.WriteLine($"Пропозиція: {criteria.ListingType}");
            Console.WriteLine($"Тип: {criteria.Type}");
            Console.WriteLine($"Ціна від: {criteria.MinPrice} до: {criteria.MaxPrice}");
            Console.WriteLine($"Площа від: {criteria.MinArea} до: {criteria.MaxArea}");
            Console.WriteLine($"Район: {criteria.District}");
            Console.WriteLine($"Кімнат: {criteria.Rooms}");
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

            for (int i = 0; i < client.SearchHistory.Length; i++)
            {
                PropertySearchCriteria criteria = client.SearchHistory[i];

                if (criteria == null)
                {
                    continue;
                }

                Console.WriteLine($"\n{i + 1}. Пошук:");
                Console.WriteLine($"   Пропозиція: {client.ClientType}");

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

            if (number < 1 || number > client.SearchHistory.Length)
            {
                Console.WriteLine("Невірний номер пошуку!");
                return;
            }

            PropertySearchCriteria? selectedCriteria = client.SearchHistory[number - 1];
            if (selectedCriteria == null)
            {
                Console.WriteLine("Цей фільтр порожній!");
                return;
            }
            selectedCriteria.ListingType = client.ClientType == "Орендар" ? "Оренда" : "Продаж";

            List<Property> result = properties.Search(selectedCriteria);

            Console.WriteLine("\n===== РЕЗУЛЬТАТ ПОВТОРНОГО ПОШУКУ =====");

            MenuHelpers.PrintProperties(result);
        }

        private void Book(Client client)
        {
            List<Property> available = GetAvailableForClient(client);
            MenuHelpers.PrintProperties(available);
            if (available.Count == 0)
            {
                return;
            }

            int number = MenuHelpers.ReadInt("Номер об'єкта для бронювання (0 - скасувати): ");
            if (number == 0)
            {
                return;
            }
            if (number < 1 || number > available.Count)
            {
                throw new Exception("Невірний номер!");
            }

            properties.Book(available[number - 1], client);
            Console.WriteLine(client.ClientType == "Орендар" ? "Об'єкт заброньовано для оренди!" : "Об'єкт заброньовано для купівлі!");
        }
    }
}
