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
            var orderStatus=await _orderService.CreateOrder(checkoutİnfoİnput);
            if(!orderStatus.IsSuccessfull) 
            {
                var basket = await _basketService.Get();
                ViewBag.basket = basket;
                ViewBag.error = orderStatus.Error;
                return RedirectToAction(nameof(Checkout));
            }

            return RedirectToAction(nameof(SuccessfulCheckout), new { orderId=orderStatus.OrderId});
        }

        public IActionResult SuccessfulCheckout(int orderId)
        {
            ViewBag.orderId = orderId;
            return View();
        }
    }
}
