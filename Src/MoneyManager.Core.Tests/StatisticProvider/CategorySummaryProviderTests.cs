﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MoneyManager.Core.StatisticProvider;
using MoneyManager.Foundation;
using MoneyManager.Foundation.Interfaces;
using MoneyManager.Foundation.Model;
using MoneyManager.TestFoundation;
using Moq;
using Xunit;

namespace MoneyManager.Core.Tests.StatisticProvider
{
    public class CategorySummaryProviderTests
    {
        [Fact]
        public void GetValues_NullDependency_NullReferenceException()
        {
            Assert.Throws<NullReferenceException>(() => new CategorySummaryProvider(null, null).GetValues(DateTime.Today, DateTime.Today));
        }

        [Fact]
        public void GetValues_InitializedData_IgnoreTransfers()
        {
            //Setup
            var transactionRepoSetup = new Mock<ITransactionRepository>();
            transactionRepoSetup.SetupAllProperties();

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();

            var categoryRepo = categoryRepoSetup.Object;
            categoryRepo.Data = new ObservableCollection<Category>(new List<Category>
            {
                new Category {Id = 1, Name = "Ausgehen"}
            });

            var transactionRepo = transactionRepoSetup.Object;
            transactionRepo.Data = new ObservableCollection<FinancialTransaction>(new List<FinancialTransaction>
            {
                new FinancialTransaction {Id = 1, Type = (int) TransactionType.Income, Date = DateTime.Today, Amount = 60, Category = categoryRepo.Data.First(), CategoryId = 1},
                new FinancialTransaction {Id = 2, Type = (int) TransactionType.Spending, Date = DateTime.Today, Amount = 90, Category = categoryRepo.Data.First(), CategoryId = 1},
                new FinancialTransaction {Id = 3, Type = (int) TransactionType.Transfer, Date = DateTime.Today, Amount = 40, Category = categoryRepo.Data.First(), CategoryId = 1}
            });

            //Excution
            var result = new CategorySummaryProvider(transactionRepo, categoryRepo).GetValues(DateTime.Today.AddDays(-3), DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(1);
            result.First().Value.ShouldBe(-30);
        }

        [Fact]
        public void GetValues_InitializedData_CalculateIncome()
        {
            //Setup
            var transactionRepoSetup = new Mock<ITransactionRepository>();
            transactionRepoSetup.SetupAllProperties();

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();

            var categoryRepo = categoryRepoSetup.Object;
            categoryRepo.Data = new ObservableCollection<Category>(new List<Category>
            {
                new Category {Id = 1, Name = "Einkaufen"},
                new Category {Id = 2, Name = "Ausgehen"},
                new Category {Id = 3, Name = "Foo"}
            });

            var transactionRepo = transactionRepoSetup.Object;
            transactionRepo.Data = new ObservableCollection<FinancialTransaction>(new List<FinancialTransaction>
            {
                new FinancialTransaction {Id = 1, Type = (int) TransactionType.Income, Date = DateTime.Today, Amount = 60, Category = categoryRepo.Data[0], CategoryId = 1},
                new FinancialTransaction {Id = 2, Type = (int) TransactionType.Spending, Date = DateTime.Today, Amount = 90, Category = categoryRepo.Data[0], CategoryId = 1},
                new FinancialTransaction {Id = 3, Type = (int) TransactionType.Spending, Date = DateTime.Today, Amount = 40, Category = categoryRepo.Data[1], CategoryId = 2},
                new FinancialTransaction {Id = 3, Type = (int) TransactionType.Income, Date = DateTime.Today, Amount = 66, Category = categoryRepo.Data[2], CategoryId = 3}
            });

            //Excution
            var result = new CategorySummaryProvider(transactionRepo, categoryRepo).GetValues(DateTime.Today.AddDays(-3), DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(3);
            result[0].Value.ShouldBe(-40);
            result[1].Value.ShouldBe(-30);
            result[2].Value.ShouldBe(66);
        }

        [Fact]
        public void GetValues_InitializedData_HandleDateCorrectly()
        {
            //Setup
            var transactionRepoSetup = new Mock<ITransactionRepository>();
            transactionRepoSetup.SetupAllProperties();

            var categoryRepoSetup = new Mock<IRepository<Category>>();
            categoryRepoSetup.SetupAllProperties();

            var categoryRepo = categoryRepoSetup.Object;
            categoryRepo.Data = new ObservableCollection<Category>(new List<Category>
            {
                new Category {Id = 1, Name = "Einkaufen"},
                new Category {Id = 2, Name = "Ausgehen"},
                new Category {Id = 3, Name = "Bier"}
            });

            var transactionRepo = transactionRepoSetup.Object;
            transactionRepo.Data = new ObservableCollection<FinancialTransaction>(new List<FinancialTransaction>
            {
                new FinancialTransaction {Id = 1, Type = (int) TransactionType.Spending, Date = DateTime.Today.AddDays(-5), Amount = 60, Category = categoryRepo.Data[0], CategoryId = 1},
                new FinancialTransaction {Id = 2, Type = (int) TransactionType.Spending, Date = DateTime.Today, Amount = 90, Category = categoryRepo.Data[1], CategoryId = 2},
                new FinancialTransaction {Id = 3, Type = (int) TransactionType.Spending, Date = DateTime.Today.AddDays(5), Amount = 40, Category = categoryRepo.Data[2], CategoryId = 3}
            });

            //Excution
            var result = new CategorySummaryProvider(transactionRepo, categoryRepo).GetValues(DateTime.Today.AddDays(-3), DateTime.Today.AddDays(3)).ToList();

            //Assertion
            result.Count.ShouldBe(1);
            result.First().Value.ShouldBe(-90);
        }
    }
}