namespace FreeCourse.Web.Models.Order
{
    public class OrderCreateViewModel
    {
        public int OrderId { get; set; }
        public string Error { get; set; }
        public bool IsSuccessfull { get; set; }
    }
}