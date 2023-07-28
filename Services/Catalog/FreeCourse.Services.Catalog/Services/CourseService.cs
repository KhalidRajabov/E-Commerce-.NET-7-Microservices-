using AutoMapper;
using FreeCourse.Services.Catalog.DTOs;
using FreeCourse.Services.Catalog.Models;
using FreeCourse.Services.Catalog.Settings;
using FreeCourse.Shared.DTOs;
using Mass=MassTransit;
using Microsoft.AspNetCore.Http.HttpResults;
using MongoDB.Driver;
using FreeCourse.Shared.Messages;

namespace FreeCourse.Services.Catalog.Services
{
    public class CourseService:ICourseService
    {
        private readonly IMongoCollection<Course> _courseCollection;
        private readonly IMongoCollection<Category> _categoryCollection;
        private readonly IMapper _mapper;
        private readonly Mass.IPublishEndpoint _publishEndpoint;
        public CourseService(IMapper mapper, IDatabaseSettings databaseSettings, Mass.IPublishEndpoint publishEndpoint)
        {
            var client = new MongoClient(databaseSettings.ConnectionString);
            var database = client.GetDatabase(databaseSettings.DatabaseName);
            _courseCollection = database.GetCollection<Course>(databaseSettings.CourseCollectionName);
            _categoryCollection = database.GetCollection<Category>(databaseSettings.CategoryCollectionName);
            _mapper = mapper;
            _publishEndpoint = publishEndpoint;
        }


        public async Task<Response<List<CourseDTO>>> GetAllAsync()
        {
            var courses = await _courseCollection.Find(course=>true).ToListAsync();
            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category=await _categoryCollection.Find<Category>(x=>x.Id==course.CategoryId).FirstAsync();
                }
            }
            else
            {
                courses = new List<Course>();
            }
            return Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), 200);
        }

        public async Task<Response<CourseDTO>> GetByIdAsync(string id)
        {

            try
            {
                var course = await _courseCollection.Find<Course>(x => x.Id == id).FirstOrDefaultAsync();
                if (course == null)
                    return Response<CourseDTO>.Fail("Course not found", 404);
                return Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(course), 200);
            }
            catch (Exception ex)
            {
                return Response<CourseDTO>.Fail("An error occurred while fetching the course: " + ex.Message, 500);
            }
        }

        public async Task<Response<List<CourseDTO>>> GetAllByUserId(string userId)
        {
            var courses = await _courseCollection.Find<Course>(x => x.UserId == userId).ToListAsync();
            if (courses.Any())
            {
                foreach (var course in courses)
                {
                    course.Category = await _categoryCollection.Find<Category>(x => x.Id == course.CategoryId).FirstAsync();
                }
            }
            else
            {
                return Response<List<CourseDTO>>.Fail("No course found",404);
            }

            return Response<List<CourseDTO>>.Success(_mapper.Map<List<CourseDTO>>(courses), 200); 
        }

        public async Task<Response<CourseDTO>> CreateAsync(CourseCreateDTO courseCreateDTO)
        {
            try
            {
                var category = await _categoryCollection.Find<Category>(x => x.Id == courseCreateDTO.CategoryId).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                return Response<CourseDTO>.Fail("An error occurred while fetching the course: " + ex.Message, 404);
            }


            var newCourse =_mapper.Map<Course>(courseCreateDTO);
            newCourse.CreatedTime=DateTime.Now;
            await _courseCollection.InsertOneAsync(newCourse);

            return Response<CourseDTO>.Success(_mapper.Map<CourseDTO>(newCourse),200 );
        }

        public async Task<Response<NoContent>> UpdateAsync(CourseUpdateDTO courseUpdateDTO)
        {
            var updateCourse = _mapper.Map<Course>(courseUpdateDTO);
            var result = await _courseCollection.FindOneAndReplaceAsync(x => x.Id == courseUpdateDTO.Id, updateCourse);
            if (result == null)
                return Response<NoContent>.Fail("Course not found", 404);
            await _publishEndpoint.Publish<CourseNameChangedEvent>(new CourseNameChangedEvent 
            { 
                CourseId = updateCourse.Id, UpdatedName = courseUpdateDTO.Name 
            });
            return Response<NoContent>.Success(204);
        }

        public async Task<Response<NoContent>> DeleteAsync(string id)
        {
            try
            {
                var result=await _courseCollection.DeleteOneAsync(x=>x.Id == id);
                if (result.DeletedCount > 0)
                return Response<NoContent>.Success(204);
            }
            catch (Exception ex)
            {
                return Response<NoContent>.Fail("An error occurred while fetching the course: " + ex.Message, 404);
            }
            return Response<NoContent>.Success(204);
        }
    }
}
