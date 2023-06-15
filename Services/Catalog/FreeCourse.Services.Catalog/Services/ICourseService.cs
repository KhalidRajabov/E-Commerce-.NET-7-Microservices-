using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;

namespace FreeCourse.Services.Catalog.Services
{
    public interface ICourseService
    {
        Task<Response<List<CourseDTO>>> GetAllAsync();
        Task<Response<CourseDTO>> GetByIdAsync(string id);
        Task<Response<List<CourseDTO>>> GetAllByUserId(string userId);
        Task<Response<CourseDTO>> CreateAsync(CourseCreateDTO courseCreateDTO);
        Task<Response<NoContent>> UpdateAsync(CourseUpdateDTO courseUpdateDTO);
        Task<Response<NoContent>> DeleteAsync(string id);
    }
}
