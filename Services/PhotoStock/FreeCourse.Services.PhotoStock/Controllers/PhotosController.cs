using FreeCourse.Services.PhotoStock.DTOs;
using FreeCourse.Shared.ControllerBases;
using FreeCourse.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace FreeCourse.Services.PhotoStock.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotosController : CustomBaseController
    {
        [HttpPost]
        public async Task<IActionResult> PhotoSave(IFormFile photo, CancellationToken cancellationToken)
        {

            //CANCELLATION TOKEN means if browser is quit or user somehow stopped uploading, 
            //the processes below will stop as well


            if (photo != null&&photo.Length>0) 
            {
                var path = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot/photos",photo.FileName);

                using var stream = new FileStream(path, FileMode.Create);
                await photo.CopyToAsync(stream, cancellationToken);
                var returnPath="photos/"+photo.FileName;


                PhotoDTO photoDTO = new() { URL = returnPath };
                return CreateActionResultInstance(Response<PhotoDTO>.Success(photoDTO,200));
            }
            return CreateActionResultInstance(Response<PhotoDTO>.Fail("Photo is empty", 400));
        }

        [HttpGet]
        public IActionResult PhotoDelete(string photoURL)
        {
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/photos", photoURL);
            if(!System.IO.File.Exists(path))
                return CreateActionResultInstance(Response<NoContent>.Fail("Photo not found", 404));
            
            System.IO.File.Delete(path);
            return CreateActionResultInstance(Response<NoContent>.Success(204));
            
        }
    }
}
