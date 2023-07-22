using FreeCourse.Services.FakePayment.Models;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Messages;
using MassTransit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.FakePayment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FakePaymentsController : CustomBaseController
    {
        private readonly ISendEndpointProvider _sendEndpointProvider;

        public FakePaymentsController(ISendEndpointProvider sendEndpointProvider)
        {
            _sendEndpointProvider = sendEndpointProvider;
        }

        [HttpPost]
        public async Task<IActionResult> ReceivePayment(PaymentDTO paymentDTO )
        {
            var sendEndpoint = await _sendEndpointProvider.GetSendEndpoint(new Uri("queue:create-order-service"));

            var createOrderMessageCommand = new CreateOrderMessageCommand
            {
                BuyerId=paymentDTO.Order.BuyerId,
                District=paymentDTO.Order.Address.District,
                Province= paymentDTO.Order.Address.Province,
                Street= paymentDTO.Order.Address.Street,
                Line=paymentDTO.Order.Address.Line,
                ZipCode=paymentDTO.Order.Address.ZipCode
            };

            paymentDTO.Order.OrderItems.ForEach(x =>
            {
                createOrderMessageCommand.OrderItems.Add(new OrderItem
                {
                    PictureUrl=x.PictureURL,
                    Price=x.Price,
                    ProductName=x.ProductName,
                    ProductId=x.ProductId,
                    
                });
            });
            await sendEndpoint.Send<CreateOrderMessageCommand>(createOrderMessageCommand);
                
            return CreateActionResultInstance(Shared.DTOs.Response<NoContent>.Success(200));
        }
    }
}