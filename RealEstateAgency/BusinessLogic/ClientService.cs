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
        public PropertySearchCriteria? GetLatestRequest(Client client)
        {
            if (client.SearchHistory == null)
            {
                return null;
            }

            for (int i = client.SearchHistory.Length - 1; i >= 0; i--)
            {
                if (client.SearchHistory[i] != null)
                {
                    return client.SearchHistory[i];
                }
            }

            return null;
        }

        public void SaveSearch(Client client, PropertySearchCriteria criteria)
        {
            if (client.SearchHistory == null)
            {
                client.SearchHistory = new PropertySearchCriteria[10];
                client.SearchCount = 0;
            }

            if (client.SearchCount < 0 || client.SearchCount > client.SearchHistory.Length)
            {
                client.SearchCount = 0;
                for (int i = 0; i < client.SearchHistory.Length; i++)
                {
                    if (client.SearchHistory[i] != null)
                    {
                        client.SearchCount = i + 1;
                    }
                }
            }

            if (client.SearchCount < client.SearchHistory.Length)
            {
                client.SearchHistory[client.SearchCount] = criteria;
                client.SearchCount++;
            }
            else
            {
                for (int i = 0; i < client.SearchHistory.Length - 1; i++)
                {
                    client.SearchHistory[i] = client.SearchHistory[i + 1];
                }

                client.SearchHistory[client.SearchHistory.Length - 1] = criteria;
            }

            storage.Save(clients);
        }
    }
}