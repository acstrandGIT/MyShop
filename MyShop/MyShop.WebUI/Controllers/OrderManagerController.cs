using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MyShop.Core.Contracts;
using MyShop.Core.Models;

namespace MyShop.WebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderManagerController : Controller
    {
        private IOrderService orderService;

        public OrderManagerController(IOrderService OrderService)
        {
            this.orderService = OrderService;
        }

        // GET: OrderManager
        public ActionResult Index()
        {
            List<Order> orders = orderService.GetOrderList();
            return View(orders);
        }

        public ActionResult UpdateOrder(string id)
        {
            ViewBag.StatusList = new List<string>()
            {
                "Order Created",
                "Payment Processed",
                "Order Shipped",
                "Order Complete"
            };

            Order order = orderService.GetOrder(id);
            return View(order);
        }

        [HttpPost]
        public ActionResult UpdateOrder(Order updatedOrder, string id)
        {
            Order orderToUpdate = orderService.GetOrder(id);
            orderToUpdate.OrderStatus = updatedOrder.OrderStatus;
            orderService.UpdateOrder(orderToUpdate);
            return RedirectToAction("Index");
        }
    }
}