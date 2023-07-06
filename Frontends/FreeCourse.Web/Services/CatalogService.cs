using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Models;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace FreeCourse.Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;

        public CatalogService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            var response = await _httpClient.PostAsJsonAsync<CourseCreateInput>("course", courseCreateInput);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteCourseAsync(string courseId)
        {
            var response = await _httpClient.DeleteAsync($"course/{courseId}");

            return response.IsSuccessStatusCode;
        }

        public async Task<List<CourseViewModel>> GetAllCourseAsync()
        {
            //this will request to http://localhost:5000/services/catalog/
            //because we mentioned it in program.cs file that whenever a catalog service inherites icatalog,
            //their request will be send to http://localhost:5000/services/catalog/
            var response = await _httpClient.GetAsync("course");
            if (!response.IsSuccessStatusCode) return null;

            var successfullResponse = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            return successfullResponse.Data;
        }

        public async Task<List<CategoryViewModel>> GetAllCategoryAsync()
        {
            //this will request to http://localhost:5000/services/catalog/
            //because we mentioned it in program.cs file that whenever a catalog service inherites icatalog,
            //their request will be send to http://localhost:5000/services/catalog/
            var response = await _httpClient.GetAsync("category");
            if (!response.IsSuccessStatusCode) return null;

            var successfullResponse = await response.Content.ReadFromJsonAsync<Response<List<CategoryViewModel>>>();
            return successfullResponse.Data;
        }

        public async Task<List<CourseViewModel>> GetAllCoursesByUserIdAsync(string userId)
        {
            //this will request to http://localhost:5000/services/catalog/
            //because we mentioned it in program.cs file that whenever a catalog service inherites icatalog,
            //their request will be send to http://localhost:5000/services/catalog/


            //[controller]/ GetByUserId /{ userId}
            var response = await _httpClient.GetAsync($"course/GetByUserId/{userId}");
            if (!response.IsSuccessStatusCode) return null;

            var successfullResponse = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            return successfullResponse.Data;
        }

        public async Task<CourseViewModel> GetCourseById(string courseId)
        {

            //this will request to http://localhost:5000/services/catalog/
            //because we mentioned it in program.cs file that whenever a catalog service inherites icatalog,
            //their request will be send to http://localhost:5000/services/catalog/


            //[controller]/ GetByUserId /{ userId}
            var response = await _httpClient.GetAsync($"course/{courseId}");
            if (!response.IsSuccessStatusCode) return null;

            var successfullResponse = await response.Content.ReadFromJsonAsync<Response<CourseViewModel>>();
            return successfullResponse.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var response = await _httpClient.PutAsJsonAsync<CourseUpdateInput>("course", courseUpdateInput);

            return response.IsSuccessStatusCode;
        }
    }
}
