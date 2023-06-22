using FreeCourse.Service.Discount.Services;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Service.Discount.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountsController : CustomBaseController
    {
        private readonly IDiscountService _discountService;
        private readonly ISharedIdentityService _sharedIdentityService;

        public DiscountsController(ISharedIdentityService sharedIdentityService, IDiscountService discountService)
        {
            _sharedIdentityService = sharedIdentityService;
            _discountService = discountService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            return CreateActionResultInstance(await _discountService.GetAll());
        }

        //api/discount/4
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            return CreateActionResultInstance(await _discountService.GetById(id));
        }


        [HttpGet]
        [Route("/api/[controller]/[action]/{code}")]
        public async Task<IActionResult> GetByCode(string code)

        {
            var userId = _sharedIdentityService.GetUserId;
            var discount = await _discountService.GetByCodeAndUserId(code, userId);
            return CreateActionResultInstance(discount);
        }


        [HttpPost]
        public async Task<IActionResult> Save(Models.Discount discount)
        {
            return CreateActionResultInstance(await _discountService.Save(discount));
        }


        [HttpPut]
        public async Task<IActionResult> Update(Models.Discount discount)
        {
            return CreateActionResultInstance(await _discountService.Update(discount));
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            return CreateActionResultInstance(await _discountService.Delete(id));
        }
    }
}
