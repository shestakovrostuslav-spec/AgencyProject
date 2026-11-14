
using RealEstateAgency.Models;
using RealEstateAgency.DataAccess;

namespace RealEstateAgency.BusinessLogic
{
    public class PropertyService
    {
        private readonly JsonFileStorage<Property> storage;

        private List<Property> properties;

        public PropertyService()
        {
            storage = new JsonFileStorage<Property>(
                "properties.json");

            properties = storage.Load();
        }

        // Получение всех объектов
        public List<Property> GetAll()
        {
            return properties;
        }

        // Добавление недвижимости
        public void Add(Property property)
        {
            if (property.Area <= 0 || property.Price <= 0)
                throw new Exception("Некоректна площа або ціна!");

            if (property.Rooms <= 0)
                throw new Exception("Кількість кімнат повинна бути більше нуля!");

            if (string.IsNullOrWhiteSpace(property.Address))
                throw new Exception("Введіть адресу!");

            properties.Add(property);

            storage.Save(properties);
        }

        // Изменение недвижимости
        public void Update(int number, Property updated)
        {
            if (number < 1 || number > properties.Count)
                throw new Exception("Неправильний номер об'єкта!");

            if (updated.Area <= 0 || updated.Price <= 0)
                throw new Exception("Некоректна площа або ціна!");

            if (updated.Rooms <= 0)
                throw new Exception("Некоректна кількість кімнат!");

            Property property = properties[number - 1];

            property.Type = updated.Type;
            property.Area = updated.Area;
            property.Address = updated.Address;
            property.District = updated.District;
            property.Rooms = updated.Rooms;
            property.Price = updated.Price;

            storage.Save(properties);
        }

        // Удаление недвижимости
        public void Delete(int number)
        {
            if (number < 1 || number > properties.Count)
                throw new Exception("Неправильний номер об'єкта!");

            properties.RemoveAt(number - 1);

            storage.Save(properties);
        }

        // Изменение статуса недвижимости

        public void ChangeStatus(int number, string newStatus)
        {
            if (number < 1 || number > properties.Count)
                throw new Exception("Об'єкт не знайдено!");

            Property property = properties[number - 1];

            string expectedStatus;

            if (property.Status == "На продаж")
            {
                expectedStatus = "Заброньовано";
            }
            else if (property.Status == "Заброньовано")
            {
                expectedStatus = "Продано";
            }
            else
            {
                throw new Exception(
                    "Статус об'єкта вже не можна змінити!");
            }

            if (newStatus != expectedStatus)
            {
                throw new Exception(
                    "Недозволений перехід статусу!");
            }

            // Зберігаємо історію зміни статусу
            property.StatusHistory.Add(new StatusHistory
            {
                OldStatus = property.Status,
                NewStatus = newStatus,
                ChangeDate = DateTime.Now
            });

            // Змінюємо статус
            property.Status = newStatus;

            storage.Save(properties);
        }


        // Получение истории статусов
        public List<StatusHistory> GetStatusHistory(int number)
        {
            if (number < 1 || number > properties.Count)
                throw new Exception("Об'єкт не знайдено!");

            return properties[number - 1].StatusHistory;
        }

        // Поиск и фильтрация недвижимости

        public List<Property> Search(PropertySearchCriteria criteria)
        {
            var result = properties.AsEnumerable();

            // Пошук за типом
            if (!string.IsNullOrWhiteSpace(criteria.Type))
            {
                result = result.Where(p =>
                    p.Type.Contains(
                        criteria.Type,
                        StringComparison.OrdinalIgnoreCase));
            }

            // Мінімальна ціна
            if (criteria.MinPrice.HasValue)
            {
                result = result.Where(p =>
                    p.Price >= criteria.MinPrice.Value);
            }

            // Максимальна ціна
            if (criteria.MaxPrice.HasValue)
            {
                result = result.Where(p =>
                    p.Price <= criteria.MaxPrice.Value);
            }

            // Мінімальна площа
            if (criteria.MinArea.HasValue)
            {
                result = result.Where(p =>
                    p.Area >= criteria.MinArea.Value);
            }

            // Максимальна площа
            if (criteria.MaxArea.HasValue)
            {
                result = result.Where(p =>
                    p.Area <= criteria.MaxArea.Value);
            }

            // Пошук за районом
            if (!string.IsNullOrWhiteSpace(criteria.District))
            {
                result = result.Where(p =>
                    p.District.Contains(
                        criteria.District,
                        StringComparison.OrdinalIgnoreCase));
            }

            // Кількість кімнат
            if (criteria.Rooms.HasValue)
            {
                result = result.Where(p =>
                    p.Rooms == criteria.Rooms.Value);
            }

            return result.ToList();
        }

    }
}
