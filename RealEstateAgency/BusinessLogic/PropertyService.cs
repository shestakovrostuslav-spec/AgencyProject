using System;
using System.Collections.Generic;
using RealEstateAgency.Models;
using RealEstateAgency.DataAccess;

namespace RealEstateAgency.BusinessLogic
{
    public class PropertyService
    {
        private readonly JsonFileStorage<Property> storage = new JsonFileStorage<Property>("properties.json");
        private readonly List<Property> properties;

        public PropertyService()
        {
            properties = storage.Load();

            foreach (Property property in properties)
            {
                if (property.Status == "На продажу")
                    property.Status = "На продаж";

                if (property.StatusHistory == null)
                    property.StatusHistory = new List<StatusHistory>();
            }
        }

        public List<Property> GetAll()
        {
            return properties;
        }

        public List<Property> GetAvailable()
        {
            List<Property> result = new List<Property>();

            foreach (Property property in properties)
            {
                if (property.Status == "На продаж")
                    result.Add(property);
            }

            return result;
        }

        public List<Property> GetForAgent(string agentName)
        {
            List<Property> result = new List<Property>();

            foreach (Property property in properties)
            {
                if (string.Equals(property.AgentName, agentName, StringComparison.OrdinalIgnoreCase))
                    result.Add(property);
            }

            return result;
        }

        public List<Property> GetBookedForAgent(string agentName)
        {
            List<Property> result = new List<Property>();

            foreach (Property property in properties)
            {
                if (property.Status == "Заброньовано" &&
                    string.Equals(property.AgentName, agentName, StringComparison.OrdinalIgnoreCase))
                {
                    result.Add(property);
                }
            }

            return result;
        }

        private bool AddressExists(string address, Property? ignoredProperty)
        {
            foreach (Property property in properties)
            {
                if (property != ignoredProperty &&
                    string.Equals(property.Address, address, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }

        private void CheckValues(Property property)
        {
            if (property.Area <= 0 || property.Price <= 0 || property.Rooms <= 0)
                throw new Exception("Некоректна площа, ціна або кількість кімнат!");

            if (string.IsNullOrWhiteSpace(property.Type) || string.IsNullOrWhiteSpace(property.Address))
                throw new Exception("Вкажіть тип та адресу!");
        }

        public void Add(Property property, string agentName)
        {
            CheckValues(property);

            if (string.IsNullOrWhiteSpace(agentName))
                throw new Exception("Рієлтор не визначений!");

            if (AddressExists(property.Address, null))
                throw new Exception("Об'єкт із такою адресою вже існує!");

            property.AgentName = agentName;
            property.Status = "На продаж";
            property.BookedByClientName = "";

            properties.Add(property);

            try
            {
                storage.Save(properties);
            }
            catch
            {
                properties.Remove(property);
                throw;
            }
        }

        private void CheckEditable(Property property, string agentName)
        {
            if (!properties.Contains(property))
                throw new Exception("Об'єкт не знайдено!");

            if (!string.Equals(property.AgentName, agentName, StringComparison.OrdinalIgnoreCase))
                throw new Exception("Можна змінювати лише власні об'єкти!");

            if (property.Status != "На продаж")
                throw new Exception("Заброньовану або продану нерухомість не можна змінювати чи видаляти!");
        }

        public void Update(Property property, Property changes, string agentName)
        {
            CheckEditable(property, agentName);
            CheckValues(changes);

            if (AddressExists(changes.Address, property))
                throw new Exception("Об'єкт із такою адресою вже існує!");

            Property old = new Property
            {
                Type = property.Type,
                Address = property.Address,
                District = property.District,
                Area = property.Area,
                Rooms = property.Rooms,
                Price = property.Price
            };

            property.Type = changes.Type;
            property.Address = changes.Address;
            property.District = changes.District;
            property.Area = changes.Area;
            property.Rooms = changes.Rooms;
            property.Price = changes.Price;

            try
            {
                storage.Save(properties);
            }
            catch
            {
                property.Type = old.Type;
                property.Address = old.Address;
                property.District = old.District;
                property.Area = old.Area;
                property.Rooms = old.Rooms;
                property.Price = old.Price;
                throw;
            }
        }

        public void Delete(Property property, string agentName)
        {
            CheckEditable(property, agentName);
            int index = properties.IndexOf(property);

            properties.RemoveAt(index);

            try
            {
                storage.Save(properties);
            }
            catch
            {
                properties.Insert(index, property);
                throw;
            }
        }

        public void Book(Property property, string clientName)
        {
            if (!properties.Contains(property))
                throw new Exception("Об'єкт не знайдено!");

            if (property.Status != "На продаж")
                throw new Exception("Цей об'єкт недоступний для бронювання!");

            if (string.IsNullOrWhiteSpace(clientName))
                throw new Exception("Клієнт не визначений!");

            string oldStatus = property.Status;
            property.Status = "Заброньовано";
            property.BookedByClientName = clientName;

            StatusHistory history = new StatusHistory
            {
                OldStatus = oldStatus,
                NewStatus = property.Status,
                ChangeDate = DateTime.Now
            };

            property.StatusHistory.Add(history);

            try
            {
                storage.Save(properties);
            }
            catch
            {
                property.Status = oldStatus;
                property.BookedByClientName = "";
                property.StatusHistory.Remove(history);
                throw;
            }
        }

        public void MarkSold(Property property, string agentName)
        {
            if (!properties.Contains(property))
                throw new Exception("Об'єкт не знайдено!");

            if (property.Status != "Заброньовано")
                throw new Exception("Спочатку об'єкт потрібно забронювати!");

            if (!string.Equals(property.AgentName, agentName, StringComparison.OrdinalIgnoreCase))
                throw new Exception("Угоду може оформити лише рієлтор цього об'єкта!");

            property.Status = "Продано";

            StatusHistory history = new StatusHistory
            {
                OldStatus = "Заброньовано",
                NewStatus = "Продано",
                ChangeDate = DateTime.Now
            };

            property.StatusHistory.Add(history);

            try
            {
                storage.Save(properties);
            }
            catch
            {
                property.Status = "Заброньовано";
                property.StatusHistory.Remove(history);
                throw;
            }
        }

        public void RollbackSale(Property property)
        {
            if (!properties.Contains(property) || property.Status != "Продано")
                return;

            property.Status = "Заброньовано";
            int lastIndex = property.StatusHistory.Count - 1;

            if (lastIndex >= 0 && property.StatusHistory[lastIndex].NewStatus == "Продано")
                property.StatusHistory.RemoveAt(lastIndex);

            storage.Save(properties);
        }

        public List<Property> Search(PropertySearchCriteria criteria)
        {
            List<Property> result = new List<Property>();

            foreach (Property property in properties)
            {
                if (property.Status != "На продаж")
                    continue;

                if (!string.IsNullOrWhiteSpace(criteria.Type) &&
                    !property.Type.Contains(criteria.Type, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (criteria.MinPrice.HasValue && property.Price < criteria.MinPrice.Value)
                    continue;

                if (criteria.MaxPrice.HasValue && property.Price > criteria.MaxPrice.Value)
                    continue;

                if (criteria.MinArea.HasValue && property.Area < criteria.MinArea.Value)
                    continue;

                if (criteria.MaxArea.HasValue && property.Area > criteria.MaxArea.Value)
                    continue;

                if (!string.IsNullOrWhiteSpace(criteria.District) &&
                    !property.District.Contains(criteria.District, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                if (criteria.Rooms.HasValue && property.Rooms != criteria.Rooms.Value)
                    continue;

                result.Add(property);
            }

            return result;
        }
    }
}