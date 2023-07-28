using FreeCourse.Services.Basket.Services;
using FreeCourse.Shared.Messages;
using FreeCourse.Shared.Services;
using MassTransit;

namespace FreeCourse.Services.Basket.Consumers
{
    public class CourseNameChangedEventConsumerBasket : IConsumer<CourseNameChangedEvent>
    {
        private readonly IBasketService _basketService;
        private readonly ISharedIdentityService _sharedIdentityService;
        

        public CourseNameChangedEventConsumerBasket(IBasketService basketService, ISharedIdentityService sharedIdentityService)
        {
            _basketService = basketService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task Consume(ConsumeContext<CourseNameChangedEvent> context)
        {
            var basket = await _basketService.GetBasket(_sharedIdentityService.GetUserId);
            var basketItem = basket.Data.BasketItems.Where(x => x.CourseId == context.Message.CourseId).FirstOrDefault();
            basketItem.CourseName = context.Message.UpdatedName;
            await _basketService.SaveOrUpdate(basket.Data);
        }
    }
}
