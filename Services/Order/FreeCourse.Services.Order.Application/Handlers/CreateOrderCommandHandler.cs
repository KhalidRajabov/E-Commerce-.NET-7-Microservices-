using FreeCourse.Services.Order.Application.Commands;
using FreeCourse.Services.Order.Application.DTOs;
using FreeCourse.Services.Order.Domain.OrderAggregate;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FreeCourse.Services.Order.Application.Handlers
{
    public class CreateOrderCommandHandler : IRequestHandler<CreateOrderCommand, Response<CreatedOrderDTO>>
    {
        private readonly OrderDbContext _context;

        public CreateOrderCommandHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<CreatedOrderDTO>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
            var address = new Address(request.Address.Province, request.Address.Street,
                request.Address.District, request.Address.ZipCode, request.Address.Line);
            Domain.OrderAggregate.Order newOrder = new Domain.OrderAggregate.Order(request.BuyerId, address);
            request.OrderItems.ForEach(item =>
            {
                newOrder.AddOrderItem(item.ProductId, item.ProductName, item.Price, item.PictureURL);
            });

            _context.Orders.Add(newOrder);
            var result = await _context.SaveChangesAsync();
            return Response<CreatedOrderDTO>.Success(new CreatedOrderDTO { OrderId = newOrder.Id }, 200);
        }
    }
}