﻿using FreeCourse.Services.Order.Application.DTOs;
using FreeCourse.Services.Order.Application.Mapping;
using FreeCourse.Services.Order.Application.Queries;
using FreeCourse.Services.Order.Infrastructure;
using FreeCourse.Shared.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace FreeCourse.Services.Order.Application.Handlers
{
    public class GetOrdersByUserIdQueryHandler : IRequestHandler<GetOrdersByUserIdQuery, Response<List<OrderDTO>>>
    {

        private readonly OrderDbContext _context;

        public GetOrdersByUserIdQueryHandler(OrderDbContext context)
        {
            _context = context;
        }

        public async Task<Response<List<OrderDTO>>> Handle(GetOrdersByUserIdQuery request, CancellationToken cancellationToken)
        {
            var orders = await _context.Orders.Include(x => x.OrderItems).Where(x => x.BuyerId == request.UserId).ToListAsync();

            if (!orders.Any())
            {
                return Response<List<OrderDTO>>.Success(new List<OrderDTO>(), 200);
            }

            var ordersDto = ObjectMapper.Mapper.Map<List<OrderDTO>>(orders);

            return Response<List<OrderDTO>>.Success(ordersDto, 200);
        }
    }
}
