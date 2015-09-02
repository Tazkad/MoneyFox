﻿using System.Linq;
using MoneyManager.Core;
using MoneyManager.Core.DataAccess;
using MoneyManager.Core.Repositories;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Model;
using MoneyManager.Windows.Core.Tests.Helper;
using SQLite.Net.Platform.WinRT;
using Xunit;

namespace MoneyManager.Windows.Core.Tests.Repositories
{
    public class AccountRepositoryIntegrationTests
    {
        [Fact]
        [Trait("Category", "Integration")]
        public void AccountRepository_Update()
        {
            var repository =
                new AccountRepository(
                    new AccountDataAccess(new DbHelper(new SQLitePlatformWinRT(), new TestDatabasePath())));

            var account = new Account
            {
                Name = "Sparkonto",
                CurrentBalance = 6034
            };

            repository.Save(account);

            repository.Data[0].ShouldBeSameAs(account);

            account.Name = "newName";

            repository.Save(account);

            repository.Data.Count().ShouldBe(1);
            repository.Data[0].Name.ShouldBe("newname");
        }
    }
}