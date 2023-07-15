namespace FreeCourse.Web.Models.Order
{
    public class OrderItemViewModel
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string PictureURL { get; set; }
        public Decimal Price { get; set; }
    }
}
