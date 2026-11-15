using System;
using System.Collections.Generic;
using RealEstateAgency.Models;
using RealEstateAgency.DataAccess;

namespace RealEstateAgency.BusinessLogic
{
    public class AuthenticationService
    {
        private readonly AccountStorage storage = new AccountStorage();
        private readonly List<UserAccount> accounts;

        public AuthenticationService()
        {
            accounts = storage.Load();
        }

        public UserAccount Register(string name, string password, string role)
        {
            name = name.Trim();

            if (string.IsNullOrWhiteSpace(name))
                throw new Exception("Введіть ім'я!");

            if (string.IsNullOrEmpty(password))
                throw new Exception("Введіть пароль!");

            if (role != "Client" && role != "Agent")
                throw new Exception("Невідома роль!");

            foreach (UserAccount existing in accounts)
            {
                if (existing.Name == name && existing.Role == role)
                    throw new Exception("Користувач із таким ім'ям вже зареєстрований у цій ролі!");
            }

            UserAccount account = new UserAccount
            {
                Name = name,
                Password = password,
                Role = role
            };

            accounts.Add(account);

            try
            {
                storage.Save(accounts);
            }
            catch
            {
                accounts.Remove(account);
                throw;
            }

            return account;
        }

        public UserAccount? Login(string name, string password, string role)
        {
            foreach (UserAccount account in accounts)
            {
                if (account.Name == name && account.Password == password && account.Role == role)
                    return account;
            }

            return null;
        }
    }
}