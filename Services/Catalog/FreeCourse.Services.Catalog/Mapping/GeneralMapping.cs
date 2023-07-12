using AutoMapper;
using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Shared.DTOs;

namespace FreeCourse.Services.Catalog.Mapping
{
    public class GeneralMapping:Profile
    {
        public GeneralMapping()
        {
            CreateMap<Course, CourseDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Response<Category>, Response<CategoryDTO>>().ReverseMap();
            CreateMap<Category, CategoryCreateDTO>().ReverseMap();
            CreateMap<Feature, FeatureDTO>().ReverseMap();

            CreateMap<Course, CourseCreateDTO>().ReverseMap();
            CreateMap<Course, CourseUpdateDTO>().ReverseMap();
        }
    }
}