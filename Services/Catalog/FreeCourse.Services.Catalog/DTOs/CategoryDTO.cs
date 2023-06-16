using System.Text.Json.Serialization;

namespace FreeCourse.Services.Catalog.DTOs
{
    public class CategoryDTO
    {
        [JsonIgnore]
        public string Id { get; set; }
        public string Name { get; set; }
    }
}
