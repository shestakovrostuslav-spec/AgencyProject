using System;
using System.Collections.Generic;
using RealEstateAgency.Models;

namespace RealEstateAgency.Presentation
{
    public static class MenuHelpers
    {
        public static string ReadText(string prompt)
        {
            Console.Write(prompt);
            return (Console.ReadLine() ?? "").Trim();
        }

        public static int ReadInt(string prompt)
        {
            while (true)
            {
                if (int.TryParse(ReadText(prompt), out int n))
                    return n;

                Console.WriteLine("Введіть ціле число!");
            }
        }

        public static double ReadDouble(string prompt)
        {
            while (true)
            {
                if (double.TryParse(ReadText(prompt), out double n))
                    return n;

                Console.WriteLine("Введіть коректне число!");
            }
        }

        public static double? ReadOptionalDouble(string prompt)
        {
            while (true)
            {
                string s = ReadText(prompt);

                if (s == "")
                    return null;

                if (double.TryParse(s, out double n))
                    return n;

                Console.WriteLine("Введіть коректне число!");
            }
        }

        public static int? ReadOptionalInt(string prompt)
        {
            while (true)
            {
                string s = ReadText(prompt);

                if (s == "")
                    return null;

                if (int.TryParse(s, out int n))
                    return n;

                Console.WriteLine("Введіть ціле число!");
            }
        }

        public static string ReadPassword(string prompt)
        {
            Console.Write(prompt);
            // Для простоти працює і в терміналах без підтримки прихованого вводу.
            return Console.ReadLine() ?? "";
        }

        public static void PrintProperties(List<Property> properties)
        {
            if (properties.Count == 0)
            {
                Console.WriteLine("Нерухомість не знайдено.");
                return;
            }

            for (int i = 0; i < properties.Count; i++)
            {
                Property p = properties[i];

                Console.WriteLine($"{i + 1}. {p.Type}, {p.Address}, район {p.District}, {p.Area} м², кімнат: {p.Rooms}, ціна: {p.Price}, ціна за м²: {p.PricePerSquareMeter}, статус: {p.Status}");
                Console.WriteLine($"   Рієлтор: {p.AgentName}");

                if (p.Status == "Заброньовано")
                {
                    Console.WriteLine($"   Клієнт: {p.BookedByClientName}");
                }
            }
        }
    }
}