using System;
using RealEstateAgency.BusinessLogic;
using RealEstateAgency.Models;

namespace RealEstateAgency.Presentation
{
    public class AgentMenu
    {
        private readonly PropertyService properties;
        private readonly ClientService clients;
        private readonly DealService deals;
        private readonly Agent agent;

        public AgentMenu(
            PropertyService properties,
            ClientService clients,
            DealService deals,
            Agent agent)
        {
            this.properties = properties;
            this.clients = clients;
            this.deals = deals;
            this.agent = agent;
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine($"\n===== РІЄЛТОР: {agent.Name} =====");
                Console.WriteLine("1. Додати нерухомість");
                Console.WriteLine("2. Переглянути всю нерухомість");
                Console.WriteLine("3. Переглянути заброньовану нерухомість");
                Console.WriteLine("4. Створити угоду");
                Console.WriteLine("5. Переглянути свої угоди");
                Console.WriteLine("6. Переглянути мою нерухомість");
                Console.WriteLine("7. Редагувати мою нерухомість");
                Console.WriteLine("8. Видалити мою нерухомість");
                Console.WriteLine("9. Показати кількість угод та прибуток");
                Console.WriteLine("10. Показати активних клієнтів");
                Console.WriteLine("11. Кількість угод за період");
                Console.WriteLine("12. Загальна сума продажів");
                Console.WriteLine("13. Показати ефективність рієлторів");
                Console.WriteLine("0. Вийти з акаунта");

                string choice = MenuHelpers.ReadText("Ваш вибір: ");

                try
                {
                    switch (choice)
                    {
                        case "1":
                            AddProperty();
                            break;
                        case "2":
                            MenuHelpers.PrintProperties(properties.GetAll());
                            break;
                        case "3":
                            MenuHelpers.PrintProperties(properties.GetBookedForAgent(agent.Name));
                            break;
                        case "4":
                            CreateDeal();
                            break;
                        case "5":
                            ShowDeals();
                            break;
                        case "6":
                            MenuHelpers.PrintProperties(properties.GetForAgent(agent.Name));
                            break;
                        case "7":
                            EditProperty();
                            break;
                        case "8":
                            DeleteProperty();
                            break;
                        case "9":
                            int dealsCount = deals.GetDealsCountForAgent(agent.Name);
                            double income = deals.GetIncomeForAgent(agent.Name);
                            Console.WriteLine($"Кількість угод: {dealsCount}, Прибуток: {income}");
                            break;
                        case "10":
                            deals.ShowActiveClients();
                            break;
                        case "11":
                            ShowDealsCountByPeriod();
                            break;
                        case "12":
                            ShowTotalSales();
                            break;
                        case "13":
                            ShowAgentsEfficiency();
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

        private void AddProperty()
        {
            var p = new Property
            {
                Type = MenuHelpers.ReadText("Тип нерухомості: "),
                Area = MenuHelpers.ReadDouble("Площа: "),
                Address = MenuHelpers.ReadText("Адреса: "),
                District = MenuHelpers.ReadText("Район: "),
                Rooms = MenuHelpers.ReadInt("Кількість кімнат: "),
                Price = MenuHelpers.ReadDouble("Ціна: ")
            };

            properties.Add(p, agent.Name);
            Console.WriteLine("Нерухомість додано і закріплено за вами!");
        }

        private Property? ChooseOwnProperty()
        {
            var own = properties.GetForAgent(agent.Name);
            MenuHelpers.PrintProperties(own);

            if (own.Count == 0)
                return null;

            int number = MenuHelpers.ReadInt("Номер вашого об'єкта (0 - скасувати): ");

            if (number == 0)
                return null;

            if (number < 1 || number > own.Count)
                throw new Exception("Невірний номер!");

            return own[number - 1];
        }
        private void ShowDealsCountByPeriod()
        {
            Console.WriteLine("\n===== КІЛЬКІСТЬ УГОД ЗА ПЕРІОД =====");

            Console.Write("Введіть початкову дату (дд.мм.рррр): ");
            string startInput = Console.ReadLine() ?? "";

            Console.Write("Введіть кінцеву дату (дд.мм.рррр): ");
            string endInput = Console.ReadLine() ?? "";

            DateTime startDate;
            DateTime endDate;

            if (!DateTime.TryParseExact(
                startInput,
                "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out startDate))
            {
                Console.WriteLine("Невірно введена початкова дата!");
                return;
            }

            if (!DateTime.TryParseExact(
                endInput,
                "dd.MM.yyyy",
                System.Globalization.CultureInfo.InvariantCulture,
                System.Globalization.DateTimeStyles.None,
                out endDate))
            {
                Console.WriteLine("Невірно введена кінцева дата!");
                return;
            }

            if (startDate > endDate)
            {
                Console.WriteLine("Початкова дата не може бути більшою за кінцеву!");
                return;
            }

            int count = deals.GetDealsCountByPeriod(startDate, endDate);

            Console.WriteLine();
            Console.WriteLine($"Період: {startDate:dd.MM.yyyy} - {endDate:dd.MM.yyyy}");
            Console.WriteLine($"Кількість успішних угод: {count}");
        }
        private void ShowTotalSales()
        {
            Console.WriteLine("\n===== ЗАГАЛЬНА СУМА ПРОДАЖІВ =====");

            double totalSales = deals.GetTotalSales();

            Console.WriteLine($"Загальна сума продажів: {totalSales:F2} грн");
        }
        private void ShowAgentsEfficiency()
        {
            Console.WriteLine("\n===== ЕФЕКТИВНІСТЬ РІЄЛТОРІВ =====");

            List<Agent> agents = new List<Agent>();

            List<Deal> allDeals = deals.GetAll();

            foreach (Deal deal in allDeals)
            {
                bool exists = false;

                foreach (Agent agentItem in agents)
                {
                    if (agentItem.Name == deal.AgentName)
                    {
                        exists = true;
                        break;
                    }
                }

                if (!exists)
                {
                    Agent agentItem = new Agent();
                    agentItem.Name = deal.AgentName;

                    agents.Add(agentItem);
                }
            }

            if (agents.Count == 0)
            {
                Console.WriteLine("Угод поки немає.");
                return;
            }

            foreach (Agent agentItem in agents)
            {
                int count = deals.GetDealsCountForAgent(agentItem.Name);
                double income = deals.GetIncomeForAgent(agentItem.Name);

                Console.WriteLine($"\nРієлтор: {agentItem.Name}");
                Console.WriteLine($"Кількість угод: {count}");
                Console.WriteLine($"Прибуток: {income:F2} грн");
            }
        }
        private void EditProperty()
        {
            Property? selected = ChooseOwnProperty();

            if (selected == null)
                return;

            if (selected.Status != "На продаж")
                throw new Exception("Редагувати можна лише об'єкти зі статусом 'На продаж'!");

            Console.WriteLine("Введіть нові значення (Enter - залишити поточне).");

            string type = MenuHelpers.ReadText($"Тип [{selected.Type}]: ");
            string address = MenuHelpers.ReadText($"Адреса [{selected.Address}]: ");
            string district = MenuHelpers.ReadText($"Район [{selected.District}]: ");
            string area = MenuHelpers.ReadText($"Площа [{selected.Area}]: ");
            string rooms = MenuHelpers.ReadText($"Кімнат [{selected.Rooms}]: ");
            string price = MenuHelpers.ReadText($"Ціна [{selected.Price}]: ");

            Property updated = new Property
            {
                Type = selected.Type,
                Address = selected.Address,
                District = selected.District,
                Area = selected.Area,
                Rooms = selected.Rooms,
                Price = selected.Price
            };

            if (type != "") updated.Type = type;
            if (address != "") updated.Address = address;
            if (district != "") updated.District = district;
            if (area != "") updated.Area = ParsePositiveDouble(area, "площу");
            if (rooms != "") updated.Rooms = ParsePositiveInt(rooms, "кількість кімнат");
            if (price != "") updated.Price = ParsePositiveDouble(price, "ціну");

            properties.Update(selected, updated, agent.Name);
            Console.WriteLine("Об'єкт успішно оновлено!");
        }

        private static double ParsePositiveDouble(string value, string field)
        {
            if (!double.TryParse(value, out double result) || result <= 0)
                throw new Exception($"Некоректно введено {field}!");

            return result;
        }

        private static int ParsePositiveInt(string value, string field)
        {
            if (!int.TryParse(value, out int result) || result <= 0)
                throw new Exception($"Некоректно введено {field}!");

            return result;
        }

        private void DeleteProperty()
        {
            Property? selected = ChooseOwnProperty();

            if (selected == null)
                return;

            if (selected.Status != "На продаж")
                throw new Exception("Видаляти можна лише об'єкти зі статусом 'На продаж'!");

            string answer = MenuHelpers.ReadText($"Видалити '{selected.Address}'? (так/ні): ");

            if (!answer.Equals("так", StringComparison.OrdinalIgnoreCase))
            {
                Console.WriteLine("Видалення скасовано.");
                return;
            }

            properties.Delete(selected, agent.Name);
            Console.WriteLine("Об'єкт видалено!");
        }

        private void CreateDeal()
        {
            var booked = properties.GetBookedForAgent(agent.Name);
            MenuHelpers.PrintProperties(booked);

            if (booked.Count == 0)
                return;

            int n = MenuHelpers.ReadInt("Номер заброньованого об'єкта: ");

            if (n < 1 || n > booked.Count)
                throw new Exception("Невірний номер!");

            Property p = booked[n - 1];

            Client? client = clients.FindByName(p.BookedByClientName);

            if (client == null)
                throw new Exception("Клієнта, який забронював об'єкт, не знайдено!");

            Console.WriteLine("Клієнт: " + client.Name);

            double commission = MenuHelpers.ReadDouble("Комісія (%): ");

            deals.Create(p, client, agent, commission, properties);
            Console.WriteLine("Угоду створено! Статус об'єкта: Продано.");
        }

        private void ShowDeals()
        {
            var list = deals.GetForAgent(agent.Name);

            if (list.Count == 0)
            {
                Console.WriteLine("Угод немає.");
                return;
            }

            foreach (Deal d in list)
            {
                Console.WriteLine($"{d.Date:g} | {d.PropertyAddress} | {d.ClientName} | {d.Type} | {d.Price} грн | комісія {d.CommissionPercent}% = {d.CommissionAmount} грн");
            }
        }
    }
}