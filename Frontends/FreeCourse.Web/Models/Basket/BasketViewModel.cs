﻿namespace FreeCourse.Web.Models.Basket
{
    public class BasketViewModel
    {
        public string UserId { get; set; }
        //discount coupon
        public string DiscountCode { get; set; }
        public int? DiscountRate { get; set; }
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
            set { _basketItems = value; }
        }
        public decimal TotalPrice { get => _basketItems.Sum(x => x.GetCurrentPrice); }

        public bool HasDiscount
        {
            get =>!string.IsNullOrEmpty(DiscountCode);
        }
    }
}