using System;
using System.Collections.Generic;
using System.Text;

using RealEstateAgency.Models;
using RealEstateAgency.DataAccess;

namespace RealEstateAgency.BusinessLogic
{
    public class ClientService
    {
        private readonly JsonFileStorage<Client> storage;
        private List<Client> clients;

        public ClientService()
        {
            storage = new JsonFileStorage<Client>("clients.json");
            clients = storage.Load();
        }

        public List<Client> GetAll()
        {
            return clients;
        }

        public void Add(Client client)
        {
            if (string.IsNullOrWhiteSpace(client.Name))
                throw new Exception("Введіть ім'я клієнта!");

            if (string.IsNullOrWhiteSpace(client.Phone))
                throw new Exception("Введіть телефон!");

            clients.Add(client);
            storage.Save(clients);
        }

        public void Update(int number, Client updated)
        {
            if (number < 1 || number > clients.Count)
                throw new Exception("Невірний номер клієнта!");

            if (string.IsNullOrWhiteSpace(updated.Name))
                throw new Exception("Введіть ім'я!");

            Client client = clients[number - 1];

            client.Name = updated.Name;
            client.Phone = updated.Phone;
            client.Email = updated.Email;
            client.Type = updated.Type;

            storage.Save(clients);
        }

        public void Delete(int number)
        {
            if (number < 1 || number > clients.Count)
                throw new Exception("Невірний номер клієнта!");

            clients.RemoveAt(number - 1);
            storage.Save(clients);
        }
    }
}