using NuGet.ContentModel;

namespace FreeCourse.Web.Models.Basket
{
    public class BasketViewModel
    {
        public BasketViewModel()
        {
            _basketItems = new List<BasketItemVIewModel>();
        }
        public string UserId { get; set; }
        //discount coupon
        public string? DiscountCode { get; set; } = null;
        public int? DiscountRate { get; set; } = null;
        private List<BasketItemVIewModel> _basketItems { get; set; }
        public List<BasketItemVIewModel> BasketItems {

            get
            {
                if (HasDiscount)
                {
                    _basketItems.ForEach(x =>
                    {
                        var discountPrice = x.Price * ((decimal)DiscountRate.Value / 100);
                        x.AppliedDiscount(Math.Round(x.Price - discountPrice, 2));
                    });
                }
                return _basketItems;
            }
            set
            {
                _basketItems = value;
            }
        }
        public decimal TotalPrice { get => _basketItems.Sum(x => x.GetCurrentPrice); }

        public bool HasDiscount
        {
            //checks if discount code is not empty and discount rate has value, means there is discount
            get =>!string.IsNullOrEmpty(DiscountCode)&&DiscountRate.HasValue;
        }
        public void CancelApplyDiscount()
        {
            DiscountCode = null;
            DiscountRate = null;
        }

        public void ApplyDiscount(string code, int rate)
        {
            DiscountCode = code;
            DiscountRate = rate;
        }
    }
}