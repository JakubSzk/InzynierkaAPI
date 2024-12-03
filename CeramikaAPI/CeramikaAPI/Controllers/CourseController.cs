using CeramikaAPI.Models;
using CeramikaAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CeramikaAPI.Controllers
{
   
    public class CourseForm
    {
        public string Name { get; set; } = null!;
        public string Description { get; set; } = null!;
        public bool Private { get; set; }
        public int Seats { get; set; }
        public DateTime When { get; set; }
        public int IdTeacher { get; set; }
    }
    public class ReturnCourse : CourseForm
    {
        public int Id { get; set; }
    }


    [Route("api/courses")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseService courseService;
        public CourseController()
        {
            courseService = new CourseService();
        }

        [HttpGet]
        [ProducesResponseType<List<CourseModelDTO>>(StatusCodes.Status200OK)]
        public IActionResult Get()
        {
            return Ok(courseService.GetCourses());
        }

        [HttpPost("create")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromForm] CourseForm courseForm)
        {
            UserService userService = new();

            CourseModel model = new CourseModel
            {
                Name = courseForm.Name,
                Description = courseForm.Description,
                Private = courseForm.Private,
                Seats = courseForm.Seats,
                When = courseForm.When,
                Teacher = userService.UserById(courseForm.IdTeacher)
            };
            CourseModel? course = courseService.AddCourse(model);
            return course == null ? BadRequest() : Ok(model);
        }
    }
    
}
