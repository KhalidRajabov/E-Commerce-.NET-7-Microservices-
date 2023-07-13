namespace FreeCourse.Service.Discount.Models
{
    //the attribute below will make "discount" table on sql to be mapped 
    //into "Discount" class below
    [Dapper.Contrib.Extensions.Table("discount")]
    public class Discount
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public int Rate { get; set; }
        public string Code { get; set; }
        public DateTime CreatedTime {   get; set; }
    }
}
