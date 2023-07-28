using FreeCourse.Web.Models.Order;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IBasketService _basketService;
        private readonly İOrderService _orderService;

        public OrderController(IBasketService basketService, İOrderService orderService)
        {
            _basketService = basketService;
            _orderService = orderService;
        }

        public async Task<IActionResult> Checkout()
        {
            var basket = await _basketService.Get();
            ViewBag.basket = basket;
            return View(new Checkoutİnfoİnput());
        }
        [HttpPost]
        public async Task<IActionResult> Checkout(Checkoutİnfoİnput checkoutİnfoİnput)
        {
            //1st way: synchronous communication
            //var orderStatus=await _orderService.CreateOrder(checkoutİnfoİnput);
            
            
            //2nd way: asynchronous communication
            var orderSuspend=await _orderService.SuspendOrder(checkoutİnfoİnput);
            if(!orderSuspend.IsSuccessfull) 
            {
                var basket = await _basketService.Get();
                ViewBag.basket = basket;
                ViewBag.error = orderSuspend.Error;
                return RedirectToAction(nameof(Checkout));
            }

            //1st way: synchronous communication
            //return RedirectToAction(nameof(SuccessfulCheckout), new { orderId=orderStatus.OrderId});
            
            
            
            //2nd way: asynchronous communication
            return RedirectToAction(nameof(SuccessfulCheckout), new { orderId = new Random().Next(1,1000)});
        }

        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }

        public async Task<IActionResult> CheckoutHistory()
        {
            return View(await _orderService.GetOrders());
        }
    }
}
