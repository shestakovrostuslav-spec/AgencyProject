using System;
using System.Collections.Generic;
using RealEstateAgency.Models;
using RealEstateAgency.DataAccess;

namespace RealEstateAgency.BusinessLogic
{
    public class ClientService
    {
        private readonly JsonFileStorage<Client> storage = new JsonFileStorage<Client>("clients.json");
        private readonly List<Client> clients;

        public ClientService()
        {
            clients = storage.Load();
        }

        public List<Client> GetAll()
        {
            return clients;
        }

        public Client? FindByName(string name)
        {
            foreach (Client client in clients)
            {
                if (string.Equals(client.Name, name, StringComparison.OrdinalIgnoreCase))
                    return client;
            }

            return null;
        }

        public void Add(Client client)
        {
            if (string.IsNullOrWhiteSpace(client.Name))
                throw new Exception("Введіть ім'я клієнта!");

            if (FindByName(client.Name) != null)
                throw new Exception("Клієнт із таким ім'ям вже існує!");

            clients.Add(client);

            try
            {
                storage.Save(clients);
            }
            catch
            {
                clients.Remove(client);
                throw;
            }
        }
    }
}