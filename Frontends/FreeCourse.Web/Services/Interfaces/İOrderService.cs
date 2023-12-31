﻿using FreeCourse.Web.Models.Order;

namespace FreeCourse.Web.Services.Interfaces
{
    public interface İOrderService
    {
        // Synchronous communication. Direct request to its microservice        
        Task<OrderCreatedViewModel> CreateOrder(Checkoutİnfoİnput checkoutİnfoİnput);

        // Asynchronous communication. Request will be sent to rabbitmq
        Task<OrderSuspendViewModel> SuspendOrder(Checkoutİnfoİnput checkoutİnfoİnput);

        Task<List<OrderViewModel>> GetOrders();
    }
}
