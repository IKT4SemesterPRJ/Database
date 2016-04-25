﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Administration_GUI.User_Controls;
using NSubstitute;
using NUnit.Framework;
using Pristjek220Data;

namespace Pristjek220.Unit.Test
{
    [TestFixture]
    class NewProductModelUnitTest
    {
        private Administration_GUI.User_Controls.NewProductModel _newProduct;
        private Store _store;

        [SetUp]
        public void SetUp()
        {
            _store = new Store() { StoreName = "Aldi", StoreId = 22 };
            _newProduct = new NewProductModel(_store);
        }

        [TestCase(2.224, 2.22)]
        [TestCase(2.226, 2.23)]
        public void ShoppingListItemPrice_Insert2point224_IsRoundedTo2point22(double price, double result)
        {
            _newProduct.ShoppingListItemPrice = price;
            Assert.That(_newProduct.ShoppingListItemPrice, Is.EqualTo(result));
        }
    }
}