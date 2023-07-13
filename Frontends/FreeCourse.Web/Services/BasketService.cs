using FreeCourse.Shared.DTOs;
using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.Basket;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;
        private readonly ISharedIdentityService _sharedIdentityService;
        private readonly IDiscountService _discountService;

        public BasketService(HttpClient httpClient, ISharedIdentityService sharedIdentityService, IDiscountService discountService)
        {
            _httpClient = httpClient;
            _sharedIdentityService = sharedIdentityService;
            _discountService = discountService;
        }

        public async Task AddBasketItem(BasketItemVIewModel basketItemVIewModel)
        {
            var basket = await Get();
            if (basket != null)
            {
                if (!basket.BasketItems.Any(x => x.CourseId == basketItemVIewModel.CourseId))
                {
                    basket.BasketItems.Add(basketItemVIewModel);
                }
            }
            else
            {
                basket = new BasketViewModel();
                basket.UserId = _sharedIdentityService.GetUserId;
                
                basket.BasketItems.Add(basketItemVIewModel);
            }
            await SaveOrUpdate(basket);
        }

        public async Task<bool> ApplyDiscount(string discountCode)
        {
            await CancelApplyDiscount();

            var basket = await Get();

            if (basket == null) return false;

            var hasDiscount= await _discountService.GetDiscount(discountCode);
            if (hasDiscount == null) return false;

            basket.ApplyDiscount(hasDiscount.Code, hasDiscount.Rate);
            await SaveOrUpdate(basket);

            return true;
        }

        public async Task<bool> CancelApplyDiscount()
        {
           var basket = await Get();
            if (basket == null ||basket.DiscountCode==null) return false;
            basket.CancelApplyDiscount();
            await SaveOrUpdate(basket);
            return true;
        }


        public async Task<bool> Delete()
        {
            var result = await _httpClient.DeleteAsync("baskets");
            return result.IsSuccessStatusCode;
        }

        public async Task<BasketViewModel> Get()
        {
            var response=await _httpClient.GetAsync("baskets");
            if(!response.IsSuccessStatusCode)
            {
                return null;
            }
            var basketViewModel=await response.Content.ReadFromJsonAsync<Response<BasketViewModel>>();
            return basketViewModel.Data;
        }

        public async Task<bool> RemoveBasketItem(string courseId)
        {
            var basket = await Get();
            if(basket == null) return false;

            var courseToDelete = basket.BasketItems.FirstOrDefault(x => x.CourseId == courseId);
            if(courseToDelete == null) return false;
            var deleteResult=basket.BasketItems.Remove(courseToDelete);

            if (!deleteResult) return false;


            //if basket is empty (all items are removed from card)
            if(!basket.BasketItems.Any()) basket.DiscountCode = null;

            return await SaveOrUpdate(basket);
        }

        public async Task<bool> SaveOrUpdate(BasketViewModel basketViewModel)
        {
            var response = await _httpClient.PostAsJsonAsync<BasketViewModel>("baskets", basketViewModel);
            if (!response.IsSuccessStatusCode)
            {
                //hover over the "errorContent" variable in case of error
                string errorContent = await response.Content.ReadAsStringAsync();
            }
            return response.IsSuccessStatusCode;
        }
    }
}
