﻿namespace FreeCourse.Services.Catalog.DTOs
{
    public class CourseUpdateDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public Decimal Price { get; set; }
        public string UserId { get; set; }
        public string Picture { get; set; }
        public FeatureDTO Feature { get; set; }
        public string CategoryId { get; set; }
    }
}
