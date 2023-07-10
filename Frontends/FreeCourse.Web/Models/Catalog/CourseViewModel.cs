﻿namespace FreeCourse.Web.Models.Catalog
{
    public class CourseViewModel
    {
        public string? Id { get; set; }
        public string Name { get; set; }

        public string ShortDescription 
        { 
            get => Description.Length > 100 ? Description.Substring(0, 100) + "..." : Description; 
        }
        public string Description { get; set; }
        public decimal Price { get; set; }

        public string UserId { get; set; }
        public string Picture { get; set; }
        public string StockPictureUrl { get; set; }
        public FeatureVIewModel Feature { get; set; }
        public string CategoryId { get; set; }
        public CategoryViewModel Category { get; set; }
    }
}
