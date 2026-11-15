using System;
using System.Collections.Generic;
using RealEstateAgency.Models;
using RealEstateAgency.DataAccess;

namespace RealEstateAgency.BusinessLogic
{
    public class AgentService
    {
        private readonly JsonFileStorage<Agent> storage = new JsonFileStorage<Agent>("agents.json");
        private readonly List<Agent> agents;

        public AgentService()
        {
            agents = storage.Load();
        }

        public List<Agent> GetAll()
        {
            return agents;
        }

        public Agent? FindByName(string name)
        {
            foreach (Agent agent in agents)
            {
                if (string.Equals(agent.Name, name, StringComparison.OrdinalIgnoreCase))
                    return agent;
            }

            return null;
        }

        public void Add(Agent agent)
        {
            if (string.IsNullOrWhiteSpace(agent.Name))
                throw new Exception("Введіть ім'я рієлтора!");

            if (agent.Experience < 0)
                throw new Exception("Некоректний досвід роботи!");

            if (FindByName(agent.Name) != null)
                throw new Exception("Рієлтор із таким ім'ям вже існує!");

            agents.Add(agent);

            try
            {
                storage.Save(agents);
            }
            catch
            {
                agents.Remove(agent);
                throw;
            }
        }
    }
}