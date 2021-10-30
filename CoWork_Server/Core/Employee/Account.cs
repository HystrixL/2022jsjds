using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Globalization;
using Co_Work.Local;
using Co_Work.Network;

namespace Co_Work.Core
{
    [Table(nameof(Account))]
    public class Account
    {
        // public Account(string id, string password)
        // {
        //     Id = id;
        //     Password = password;
        // }

        [Key]public string Id { get; set; }
        [Required]public string Password { get; set; }
    }

    public static class AccountManager
    {
        public static List<Account> Accounts;

        public static Response.Login.LoginResultEnum TryLogin(string id, string password)
        {
            Response.Login.LoginResultEnum loginResult = Response.Login.LoginResultEnum.UnknownAccount;
            foreach (var account in Accounts)
            {
                if (account.Id == id && account.Password == password)
                {
                    loginResult = Response.Login.LoginResultEnum.Succeed;
                }

                if (account.Id == id && account.Password != password)
                {
                    loginResult = Response.Login.LoginResultEnum.WrongPassword;
                }
            }

            return loginResult;
        }

        public static Response.Register.RegisterResultEnum TryRegister(string id, string password, string name, int age,
            string entryTime)
        {
            var registerResult = Response.Register.RegisterResultEnum.Succeed;
            foreach (var account in Accounts)
            {
                if (account.Id == id)
                {
                    registerResult = Response.Register.RegisterResultEnum.IdAlreadyExists;
                }
            }

            if (registerResult == Response.Register.RegisterResultEnum.Succeed)
            {
                Employee employee = new Employee()
                {
                    Name = name, ID = id, Age = age,
                    EntryTime = DateTime.ParseExact(entryTime, "yyyy-MM-dd", CultureInfo.CurrentCulture)
                };
                Account account = new Account()
                {
                    Id = id, Password = password
                };
                Accounts.Add(account);
                DataBaseManager._dataContext.Add(account);
                EmployeeManager.Employees.Add(employee);
                DataBaseManager._dataContext.Add(employee);
            }

            return registerResult;
        }
    }
}