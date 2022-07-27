using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MyShop.Core.Contracts;
using MyShop.Core.Models;
using MyShop.Core.ViewModels;

namespace MyShop.Services
{
    public class OrderService : IOrderService
    {
        private IRepo<Order> OrderContext;
        public OrderService(IRepo<Order> OrderContext)
        {
            this.OrderContext = OrderContext;
        }

        public void CreateOrder(Order baseOrder, List<BasketItemViewModel> basketItems)
        {
            foreach (var item in basketItems)
            {
                baseOrder.OrderItems.Add(new OrderItem()
                {
                    ProductId = item.Id,
                    ProductName = item.ProductName,
                    Price = item.Price,
                    Image = item.Image,
                    Quantity = item.Quantity
                });
            }
            OrderContext.Insert(baseOrder);
            OrderContext.Commit();
        }
    }
}
