using System;
using System.Collections.Generic;
using System.Text;
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
            storage = new JsonFileStorage<Property>("properties.json");
            properties = storage.Load();
        }

        public List<Property> GetAll()
        {
            return properties;
        }

        public void Add(Property property)
        {
            if (property.Area <= 0 || property.Price <= 0)
                throw new Exception("Некорректная площадь или цена!");

            if (string.IsNullOrWhiteSpace(property.Address))
                throw new Exception("Введите адрес!");

            properties.Add(property);
            storage.Save(properties);
        }

        public void Update(int number, Property updated)
        {
            if (number < 1 || number > properties.Count)
                throw new Exception("Неверный номер объекта!");

            if (updated.Area <= 0 || updated.Price <= 0)
                throw new Exception("Некорректная площадь или цена!");

            Property property = properties[number - 1];

            property.Type = updated.Type;
            property.Area = updated.Area;
            property.Address = updated.Address;
            property.Price = updated.Price;
            property.Status = updated.Status;

            storage.Save(properties);
        }

        public void Delete(int number)
        {
            if (number < 1 || number > properties.Count)
                throw new Exception("Неверный номер объекта!");

            properties.RemoveAt(number - 1);
            storage.Save(properties);
        }
    }
}
