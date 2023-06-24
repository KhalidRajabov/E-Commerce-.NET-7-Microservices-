using FreeCourse.Services.Order.Domain.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Domain.OrderAggregate
{
    public class OrderItem:Entity
    {
        public string ProductId { get;private set; }
        public string ProductName { get;private set; }
        public string PictureURL { get; private set; }
        public decimal Price { get;private set; }

        public OrderItem(string productId, string productName, string pictureURL, decimal price)
        {
            ProductId = productId;
            ProductName = productName;
            PictureURL = pictureURL;
            Price = price;
        }

        public void UpdateOrderItem(string productName, string pictureUrl, decimal price)
        {
            ProductName = productName;
            PictureURL = pictureUrl;
            Price = price;
        }
    }
}
