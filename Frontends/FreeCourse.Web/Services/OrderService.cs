using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.FakePayments;
using FreeCourse.Web.Models.Order;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class OrderService : İOrderService
    {
        private readonly IPaymentService _paymentService;
        private readonly HttpClient _httpClient;
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;
        

        public OrderService(IPaymentService paymentService, HttpClient httpClient, IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _paymentService = paymentService;
            _httpClient = httpClient;
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<OrderCreateViewModel> CreateOrder(Checkoutİnfoİnput checkoutİnfoİnput)
        {
            var basket = await _basketService.Get();
            var paymentInfoInput = new PaymentInfoInput()
            {
                CardName = checkoutİnfoİnput.CardName,
                CardNumber = checkoutİnfoİnput.CardNumber,
                Expiration = checkoutİnfoİnput.Expiration,
                CVV = checkoutİnfoİnput.CVV,
                TotalPrice = basket.TotalPrice
            };
            
            var responsePayment = await _paymentService.ReceivePayment(paymentInfoInput);

            if (!responsePayment) return new OrderCreateViewModel() { Error="Payment unsuccessfull",IsSuccessfull=false};

            var orderCreateInput = new OrderCreateInput()
            {
                BuyerId = _sharedIdentityService.GetUserId,
                Address=new AddressCreateInput()
                {
                    District=checkoutİnfoİnput.District,
                    Province=checkoutİnfoİnput.Province,
                    Line=checkoutİnfoİnput.Line,
                    Street=checkoutİnfoİnput.Street,
                    ZipCode=checkoutİnfoİnput.Line
                },

            };
            basket.BasketItems.ForEach(x =>
            {
                var orderItem = new OrderItemCreateInput { ProductId = x.CourseId, Price = x.GetCurrentPrice, PictureUrl = "", ProductName = x.CourseName };
                orderCreateInput.OrderItems.Add(orderItem);
            });

            var response = await _httpClient.PostAsJsonAsync<OrderCreateInput>("orders", orderCreateInput);
            if(!response.IsSuccessStatusCode) return new OrderCreateViewModel() { Error = "Order could not be made", IsSuccessfull = false };
            string errorContent = await response.Content.ReadAsStringAsync();

            var orderCratedVM = await response.Content.ReadFromJsonAsync<Response<OrderCreateViewModel>>();
            orderCratedVM.Data.IsSuccessfull = true;
            await _basketService.Delete();
            return orderCratedVM.Data;
        }

        public async Task<List<OrderViewModel>> GetOrders()
        {
            var response = await _httpClient.GetFromJsonAsync < Response<List<OrderViewModel>>>("orders");
            return response.Data; 
        }

        public Task SuspendOrder(Checkoutİnfoİnput checkoutİnfoİnput)
        {
            throw new NotImplementedException();
        }
    }
}
