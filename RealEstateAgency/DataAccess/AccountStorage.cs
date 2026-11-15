using System.Collections.Generic;
using RealEstateAgency.Models;

namespace RealEstateAgency.DataAccess
{
    public class AccountStorage
    {
        private readonly JsonFileStorage<UserAccount> storage = new JsonFileStorage<UserAccount>("accounts.json");

        public List<UserAccount> Load()
        {
            return storage.Load();
        }

        public void Save(List<UserAccount> accounts)
        {
            storage.Save(accounts);
        }
    }
}
