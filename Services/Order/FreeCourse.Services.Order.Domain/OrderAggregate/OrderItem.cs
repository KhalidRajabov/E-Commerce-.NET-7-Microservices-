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
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureURL { get; set; }
        public Decimal Price { get;set; }

        public OrderItem()
        {
            
        }
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
