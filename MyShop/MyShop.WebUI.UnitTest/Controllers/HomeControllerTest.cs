using Microsoft.VisualStudio.TestTools.UnitTesting;
using MyShop.WebUI.Controllers;
using System;
using System.Linq;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.WebUI.UnitTest.Mocks;

namespace MyShop.WebUI.UnitTest
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void IndexPageDoesReturnProducts()
        {
            IRepo<Product> productContext = new MockContext<Product>();
            IRepo<ProductCategory> productCategoryContext = new MockContext<ProductCategory>();
            HomeController controller = new HomeController(productContext, productCategoryContext);

            productContext.Insert(new Product());

            var result = controller.Index() as ViewResult;
            var viewModel = (ProductListViewModel) result.ViewData.Model;

            Assert.AreEqual(1, viewModel.Products.Count());



            ////Arrange
            //UnitTest1 controller = new UnitTest1();

            ////Act
            //ViewResult result = new ViewResult();

            ////Assert
            ////Assert.IsTrue(1==1);
            //Assert.IsNotNull(result);
        }
    }
}
