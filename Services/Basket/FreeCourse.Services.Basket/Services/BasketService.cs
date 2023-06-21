using FreeCourse.Services.Basket.DTOs;
using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using StackExchange.Redis;
using System.Text.Json;

namespace FreeCourse.Services.Basket.Services
{
    public class BasketService : IBasketService
    {
        private readonly RedisService _redisService;

        public BasketService(RedisService redisService)
        {
            _redisService = redisService;
        }

        public Task<Response<bool>> Delete(string userId)
        {
            throw new NotImplementedException();
        }

        public async Task<Response<BasketDTO>> GetBasket(string userId)
        {
            var existBasket = _redisService.GetDB().StringGet(userId);
            if (String.IsNullOrEmpty(existBasket))
                return Response<BasketDTO>.Fail("Basket not found", 404);
            return Response<BasketDTO>.Success(JsonSerializer.Deserialize<BasketDTO>(existBasket), 200);
        }

        public Task<Response<bool>> SaveOrUpdate(BasketDTO basketDTO)
        {
            throw new NotImplementedException();
        }
    }
}
