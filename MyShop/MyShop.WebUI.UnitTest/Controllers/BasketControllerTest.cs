using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using System.Security.Principal;
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
            IRepo<Order> orders = new MockContext<Order>();
            IRepo<Customer> customers = new MockContext<Customer>();

            var httpContext = new MockHttpContext();

            IBasketService basketService = new BasketService(product, baskets);
            IOrderService orderService = new OrderService(orders);
            var controller = new BasketController(basketService, orderService, customers);
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
            IRepo<Order> orders = new MockContext<Order>();
            IRepo<Customer> customers = new MockContext<Customer>();


            product.Insert(new Product(){ Id = "1", Price = 10});
            product.Insert(new Product(){ Id = "2", Price = 5});

            var basket = new Basket();
            basket.BasketItems.Add(new BasketItem() {ProductId = "1", Quantity = 2});
            basket.BasketItems.Add(new BasketItem() {ProductId = "2", Quantity = 3});
            baskets.Insert(basket);

            IBasketService basketService = new BasketService(product, baskets);
            IOrderService orderService = new OrderService(orders);

            var controller = new BasketController(basketService, orderService, customers);
            var httpContext = new MockHttpContext();
            httpContext.Request.Cookies.Add(new HttpCookie("eCommerceBasket"){Value=basket.Id}); ;
            controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);

            var result = controller.BasketSummary() as PartialViewResult;
            var basketSummary = (BasketSummaryViewModel) result.ViewData.Model;

            Assert.AreEqual(5, basketSummary.BasketCount);
            Assert.AreEqual(35, basketSummary.BasketTotal);

        }

        [TestMethod]
        public void canCheckOutAndCreateOrder()
        {
            //SetUp
            IRepo<Customer> customers = new MockContext<Customer>();
            IRepo<Product> products = new MockContext<Product>();
            products.Insert(new Product() { Id = "1", Price = 10 });
            products.Insert(new Product() { Id = "2", Price = 5 });

            IRepo<Basket> baskets = new MockContext<Basket>();
            var basket = new Basket();
            basket.BasketItems.Add(new BasketItem() { ProductId = "1", Quantity = 2, BasketId = basket.Id});
            basket.BasketItems.Add(new BasketItem() { ProductId = "2", Quantity = 3, BasketId = basket.Id });
            baskets.Insert(basket);

            IBasketService basketService = new BasketService(products, baskets);
            
            IRepo<Order> orders = new MockContext<Order>();
            IOrderService orderService = new OrderService(orders);

            customers.Insert(new Customer() {Id ="1", Email = "test@test.com", ZipCode = "50266"});
            IPrincipal FakeUser = new GenericPrincipal(new GenericIdentity("test@test.com", "Froms"), null);

            var controller = new BasketController(basketService, orderService, customers);
            var httpContext = new MockHttpContext();
            httpContext.User = FakeUser;
            httpContext.Request.Cookies.Add(new HttpCookie("eCommerceBasket") {Value = basket.Id});
            controller.ControllerContext = new ControllerContext(httpContext, new RouteData(), controller);


            //Act
            Order order = new Order();
            controller.CheckOut(order);

            //assert
            Assert.AreEqual(2, order.OrderItems.Count);
            Assert.AreEqual(0, basket.BasketItems.Count);


            Order ordersInRep = orders.Find(order.Id);
            Assert.AreEqual(2, ordersInRep.OrderItems.Count);
        }
    }
}
