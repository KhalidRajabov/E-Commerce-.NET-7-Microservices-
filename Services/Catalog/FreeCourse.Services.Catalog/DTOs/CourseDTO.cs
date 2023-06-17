using FreeCourse.Services.Catalog.Models;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace FreeCourse.Services.Catalog.DTOs
{
    public class CourseDTO
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }

        public string UserId { get; set; }
        public string Picture { get; set; }
        public FeatureDTO Feature { get; set; }
        public string CategoryId { get; set; }
        public CategoryDTO Category { get; set; }
    }
}
