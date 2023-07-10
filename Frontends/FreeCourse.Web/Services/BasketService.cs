using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models.Basket;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class BasketService : IBasketService
    {
        private readonly HttpClient _httpClient;

        public BasketService(HttpClient httpClient)
        {
            _httpClient = httpClient;
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
                basket.BasketItems.Add(basketItemVIewModel);
            }
            await SaveOrUpdate(basket);
        }

        public Task<bool> ApplyDiscount(string discountCode)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CancelApplyDiscount()
        {
            throw new NotImplementedException();
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
            var response = await _httpClient.PostAsJsonAsync<BasketViewModel>("baskets",basketViewModel);
            return response.IsSuccessStatusCode;
        }
    }
}
