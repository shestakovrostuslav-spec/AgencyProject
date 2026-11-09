using System;
using System.Collections.Generic;
using System.Text;
using RealEstateAgency.Models;
using RealEstateAgency.DataAccess;

namespace RealEstateAgency.BusinessLogic
{
    public class AgentService
    {
        private readonly JsonFileStorage<Agent> storage;
        private List<Agent> agents;

        public AgentService()
        {
            storage = new JsonFileStorage<Agent>("agents.json");
            agents = storage.Load();
        }

        public List<Agent> GetAll()
        {
            return agents;
        }

        public void Add(Agent agent)
        {
            if (string.IsNullOrWhiteSpace(agent.Name))
                throw new Exception("Введите имя риелтора!");

            if (agent.Experience < 0)
                throw new Exception("Некорректный опыт работы!");

            agents.Add(agent);
            storage.Save(agents);
        }
    }
}

