using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;
using MyShop.WebUI.UnitTest.Mocks;
using MyShop.Services;
using MyShop.WebUI.Controllers;

namespace MyShop.WebUI.UnitTest.Controllers
{
    [TestClass]
    public class BasketControllerTest
    {
        [TestMethod]
        public void CanAddBasketItem()
        {
            //SetUp
            IRepo<Basket> baskets = new MockContext<Basket>();
            IRepo<Product> product = new MockContext<Product>();

            var httpContext = new MockHttpContext();

            IBasketService basketService = new BasketService(product, baskets);
            var controller = new BasketController(basketService);
            controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);
            
            //Act
            //basketService.AddToBasket(httpContext, "1");
            controller.AddtoBasket("1");
            Basket basket = baskets.Collection().FirstOrDefault();

            //Assert
            Assert.IsNotNull(basket);
            Assert.AreEqual(1, basket.BasketItems.Count);
            Assert.AreEqual("1", basket.BasketItems.ToList().FirstOrDefault().ProductId);
        }

        [TestMethod]
        public void CanGetSummaryViewModel()
        {
            //SetUp
            IRepo<Basket> baskets = new MockContext<Basket>();
            IRepo<Product> product = new MockContext<Product>();

            product.Insert(new Product(){ Id = "1", Price = 10});
            product.Insert(new Product(){ Id = "2", Price = 5});

            var basket = new Basket();
            basket.BasketItems.Add(new BasketItem() {ProductId = "1", Quantity = 2});
            basket.BasketItems.Add(new BasketItem() {ProductId = "2", Quantity = 3});
            baskets.Insert(basket);

            IBasketService basketService = new BasketService(product, baskets);

            var controller = new BasketController(basketService);
            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new HttpCookie("eCommerceBasket"){Value=basket.Id}); ;
            controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);

            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel) result.ViewData.Model;

            Assert.AreEqual(5, basketSummary.BasketCount);
            Assert.AreEqual(35, basketSummary.BasketTotal);

        }
    }
}
