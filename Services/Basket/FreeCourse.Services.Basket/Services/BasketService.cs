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

        public async Task<Response<bool>> Delete(string userId)
        {
            var status = await _redisService.GetDB().KeyDeleteAsync(userId);
            return status ? Response<bool>.Success(204) : Response<bool>.Fail("Basket not found", 404);
        }

        public async Task<Response<BasketDTO>> GetBasket(string userId)
        {
            var existBasket = await _redisService.GetDB().StringGetAsync(userId);

            if (String.IsNullOrEmpty(existBasket))
            {
                return Response<BasketDTO>.Fail("Basket not found", 404);
            }

            return Response<BasketDTO>.Success(JsonSerializer.Deserialize<BasketDTO>(existBasket), 200);
        }

        public async Task<Response<bool>> SaveOrUpdate(BasketDTO basketDTO)
        {
            var status = await _redisService.GetDB().StringSetAsync(basketDTO.UserId, JsonSerializer.Serialize(basketDTO));
            return status ? Response<bool>.Success(204) : Response<bool>.Fail("Could not update or save", 500);
        }
    }
}
