using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.DTOs
{
    public class OrderItemDTO
    {
        public string ProductId { get; private set; }
        public string ProductName { get; private set; }
        public string PictureURL { get; private set; }
        public Decimal Price { get; private set; }

    }
}
