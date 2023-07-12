using FreeCourse.Shared.DTOs;
using FreeCourse.Web.Helpers;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;

namespace FreeCourse.Web.Services
{
    public class CatalogService : ICatalogService
    {
        private readonly HttpClient _httpClient;
        private readonly PhotoHelper _photoHelper;
        private readonly IPhotoStockService _photoStockService;
        public CatalogService(HttpClient httpClient, IPhotoStockService photoStockService, PhotoHelper photoHelper)
        {
            _httpClient = httpClient;
            _photoStockService = photoStockService;
            _photoHelper = photoHelper;
        }

        public async Task<bool> CreateCourseAsync(CourseCreateInput courseCreateInput)
        {
            var resultPhotoService = await _photoStockService.UploadPhoto(courseCreateInput.PhotoFormFile);
            if (resultPhotoService != null) courseCreateInput.Picture = resultPhotoService.Url;

            var response = await _httpClient.PostAsJsonAsync<CourseCreateInput>("course", courseCreateInput);
            if (!response.IsSuccessStatusCode)
            {
                //hover over the "errorContent" variable in case of error
                string errorContent = await response.Content.ReadAsStringAsync();
            }

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
            successfullResponse.Data.ForEach( x =>
            {
                x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
            });
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


            var response = await _httpClient.GetAsync($"course/GetByUserId/{userId}");

            if (!response.IsSuccessStatusCode)
            {
                return null;
            }

            var responseSuccess = await response.Content.ReadFromJsonAsync<Response<List<CourseViewModel>>>();
            responseSuccess.Data.ForEach(x =>
            {
                x.StockPictureUrl = _photoHelper.GetPhotoStockUrl(x.Picture);
            });
            return responseSuccess.Data;
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
            successfullResponse.Data.Category=await GetCategoryById(successfullResponse.Data.CategoryId);
            successfullResponse.Data.StockPictureUrl=_photoHelper.GetPhotoStockUrl(successfullResponse.Data.Picture);
            return successfullResponse.Data;
        }

        public async Task<CategoryViewModel> GetCategoryById(string categoryId)
        {

            var response = await _httpClient.GetAsync($"category/{categoryId}");
            if (!response.IsSuccessStatusCode) return null;

            var successfullResponse = await response.Content.ReadFromJsonAsync<Response<CategoryViewModel>>();
            
            return successfullResponse.Data;
        }

        public async Task<bool> UpdateCourseAsync(CourseUpdateInput courseUpdateInput)
        {
            var resultPhotoService = await _photoStockService.UploadPhoto(courseUpdateInput.PhotoFormFile);
            if (resultPhotoService != null) 
            {
                await _photoStockService.DeletePhoto(courseUpdateInput.Picture);
                courseUpdateInput.Picture = resultPhotoService.Url;
            }
            var request = new HttpRequestMessage(HttpMethod.Put, "course");
            request.Content = JsonContent.Create(courseUpdateInput);

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            return response.IsSuccessStatusCode;
        }
    }
}