using System.ComponentModel.DataAnnotations;

namespace FreeCourse.Web.Models.Catalog
{
    public class CourseCreateInput
    {
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public decimal Price { get; set; }
        public string? UserId { get; set; } = String.Empty;
        public string? Picture { get; set; } = String.Empty;
        public FeatureVIewModel Feature { get; set; }
        
        public string CategoryId { get; set; }
    }
}
