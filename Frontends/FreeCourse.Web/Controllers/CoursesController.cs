using FreeCourse.Shared.Services;
using FreeCourse.Web.Models.Catalog;
using FreeCourse.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Build.ObjectModelRemoting;

namespace FreeCourse.Web.Controllers
{
    [Authorize]
    public class CoursesController : Controller
    {
        private readonly ICatalogService _catalogService;
        //service below needs to be defined in program.cs file with scoped service
        private readonly ISharedIdentityService _sharedIdentityService;

        public CoursesController(ICatalogService catalogService, ISharedIdentityService sharedIdentityService)
        {
            _catalogService = catalogService;
            _sharedIdentityService = sharedIdentityService;
        }

        public async Task<IActionResult> Index()

        {
            return View(await _catalogService.GetAllCoursesByUserIdAsync(_sharedIdentityService.GetUserId));
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(CourseCreateInput course)
        {
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name");

            if (!ModelState.IsValid)
            {
                return View();
            }

            course.UserId = _sharedIdentityService.GetUserId;


            await _catalogService.CreateCourseAsync(course);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Update(string courseId)
        {

            var course = await _catalogService.GetCourseById(courseId);
            if (course == null) return RedirectToAction(nameof(Index));
            CourseUpdateInput courseUpdateInput = new()
            {
                Id = course.Id,
                Name = course.Name,
                Price = course.Price,
                Feature = course.Feature,
                Description = course.Description,
                CategoryId = course.CategoryId,
                UserId = course.UserId,
                Picture = course.Picture,
            };
            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name");
            return View(courseUpdateInput);
        }

        [HttpPost]
        public async Task<IActionResult> Update(CourseUpdateInput courseUpdateInput)
        {

            var categories = await _catalogService.GetAllCategoryAsync();
            ViewBag.CategoryList = new SelectList(categories, "Id", "Name", courseUpdateInput.Id);
            if (courseUpdateInput.Picture ==null)
            {
                // Retrieve existing product details from the database
                var existingCourse = await _catalogService.GetCourseById(courseUpdateInput.Id);

                // Assign existing picture to the model
                courseUpdateInput.Picture = existingCourse.Picture;
            }
            if (!ModelState.IsValid) return View();
            await _catalogService.UpdateCourseAsync(courseUpdateInput);
            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(string courseId)
        {
            await _catalogService.DeleteCourseAsync(courseId);
            return RedirectToAction(nameof(Index));
        }
    }
}
