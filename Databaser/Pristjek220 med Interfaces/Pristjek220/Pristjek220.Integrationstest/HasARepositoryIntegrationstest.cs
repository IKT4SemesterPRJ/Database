﻿using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Integrationstest
{
    [TestFixture]
    public class HasARepositoryItegrationstest
    {
        private DataContext _context;
        private HasARepository _hasARepository;
        private readonly HasA _hasA = new HasA();
        private readonly Product _prod = new Product() { ProductName = "TestProduct" };
        private readonly Store _store = new Store() { StoreName = "TestStore" };

        [SetUp]
        public void SetUp()
        {
            _context = new DataContext();
            _context.Database.Connection.ConnectionString = "Server=.\\SQLEXPRESS;Database=Pristjek220Data.DataContext; Trusted_Connection=True;";
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
            _hasARepository = new HasARepository(_context);
            _hasA.Product = _prod;
            _hasA.Store = _store;
            _hasA.Price = 10;
        }

        [TearDown]
        public void TearDown()
        {
            _context.Database.ExecuteSqlCommand("dbo.TestCleanTable");
        }

        [Test]
        public void AddHasA_AddAHasAToDb_HasAIsInDb()
        {
            _hasA.Product = _prod;
            _hasA.Store = _store;
            _context.HasARelation.Add(_hasA);
            _context.SaveChanges();

            Assert.That(_hasARepository.Get(_prod.ProductId, _store.StoreId), Is.EqualTo(_hasA));
        }

        [Test]
        public void RemoveHasA_RemoveAHasAFromDb_HasAIsRemoved()
        {
            _hasA.Product = _prod;
            _hasA.Store = _store;
            _context.HasARelation.Add(_hasA);
            _context.SaveChanges();
            _hasARepository.Remove(_hasA);
            _context.SaveChanges();

            Assert.That(_hasARepository.Get(_prod.ProductId, _store.StoreId), Is.EqualTo(null));
        }

        [Test]
        public void RemoveHasA_HasAToRemoveIsNotInDb_ReturnsNull()
        {
            _hasARepository.Remove(_hasA);
            _context.SaveChanges();

            Assert.That(_hasARepository.Get(_prod.ProductId, _store.StoreId), Is.EqualTo(null));
        }

        [Test]
        public void Get_GetHasA_HasAReturned()
        {
            _hasA.Product = _prod;
            _hasA.Store = _store;
            _context.HasARelation.Add(_hasA);
            _context.SaveChanges();

            Assert.That(_hasARepository.Get(_prod.ProductId, _store.StoreId), Is.EqualTo(_hasA));
        }

        [Test]
        public void Get_GetHasANoProductWithThatId_ReturnNull()
        {
            Assert.That(_hasARepository.Get(_prod.ProductId, _store.StoreId), Is.EqualTo(null));
        }

        [Test]
        public void FindHasA_FindHasAHasAIsFound_HasAFound()
        {
            _context.HasARelation.Add(_hasA);
            _context.SaveChanges();

            Assert.That(_hasARepository.FindHasA(_prod.ProductName, _store.StoreName), Is.EqualTo(_hasA));
        }

        [Test]
        public void FindHasA_FindHasAHasAIsNotFound_ReturnNull()
        {
            Assert.That(_hasARepository.FindHasA("Store that doesn't exist", "Product that doesn't exist"), Is.EqualTo(null));
        }

        [Test]
        public void FindCheapestHasA_HasAsWithPrices10And12And14IsSentIn_ReturnHasAWithProductThatHasPrice10()
        {
            _context.HasARelation.Add(_hasA);
            _context.HasARelation.Add(new HasA() { Price = 12, Product = _prod, Store = new Store() { StoreName = "Store with product that costs 12" } });
            _context.HasARelation.Add(new HasA() { Price = 14, Product = _prod, Store = new Store() { StoreName = "Store with product that costs 14" } });

            _context.SaveChanges();

            Assert.That(_hasARepository.FindCheapestHasA(_prod), Is.EqualTo(_hasA));
        }

        [Test]
        public void FindCheapestHasA_NoHasAForProductExists_ReturnsNull()
        {
            Assert.That(_hasARepository.FindCheapestHasA(_prod), Is.EqualTo(null));
        }
    }
}
