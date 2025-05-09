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
        public int Length { get; set; }
        public string Picture { get; set; } = null!;
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
        public IActionResult Create([FromForm] CourseForm courseForm, [FromForm]string token)
        {
            UserService userService = new();
            bool? test = userService.VerifyUser(token);
            if (test == null || test == false) { return BadRequest(); }
            CourseModel model = new CourseModel
            {
                Name = courseForm.Name,
                Description = courseForm.Description,
                Private = courseForm.Private,
                Seats = courseForm.Seats,
                When = courseForm.When,
                Length = courseForm.Length,
                Picture = courseForm.Picture,
                Teacher = userService.UserById(courseForm.IdTeacher)
            };
            CourseModel? course = courseService.AddCourse(model);
            return course == null ? BadRequest() : Ok(model);
        }

        [HttpPost("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Delete([FromForm] int id, [FromForm] string token)
        {
            UserService userService = new();
            bool? test = userService.VerifyUser(token);
            if (test == null || test == false) { return BadRequest(); }
            return courseService.DeleteCourse(id) ? Ok(true) : BadRequest();
        }


        [HttpGet("perMonth")]
        [ProducesResponseType<List<AmountPerIndexModel>>(StatusCodes.Status200OK)]
        public IActionResult PerMonth(int month, int year, bool isPrivate)
        {
            return Ok(courseService.ListOfCoursesPerDay(isPrivate, month, year));
        }

        [HttpGet("perDay")]
        [ProducesResponseType<List<DayCourseModelDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult PerDay(int day, int month, int year)
        {
            var hold = courseService.CoursesPerDay(day, month, year);
            
            return hold == null ? BadRequest() : Ok(hold.OrderBy(c => c.Hour).ToList());
        }

        [HttpGet("avaibleHours")]
        [ProducesResponseType<List<int>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult AvaibleHours(int day, int month, int year)
        {
            var hold = courseService.AvaibleHoursInDay(day, month, year);
            return hold == null ? BadRequest() : Ok(hold);
        }

        [HttpGet("details")]
        [ProducesResponseType<CourseModelDTO>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Details(int id)
        {
            var hold = courseService.GetCourseByIdDTO(id);
            return hold == null ? BadRequest() : Ok(hold);
        }

        [HttpPost("sign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Sign([FromForm]string token, [FromForm] int course)
        {
            var hold = courseService.SignForCourse(token, course);
            return hold ? Ok() : BadRequest();
        }

        [HttpPost("signPrivate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult SignPrivate([FromForm] string token, [FromForm] DateTime when, [FromForm] int length, [FromForm] int seats)
        {
            var hold = courseService.PrivateCourse(token, when, length, seats);
            return hold ? Ok() : BadRequest();
        }

        [HttpPost("listForUser")]
        [ProducesResponseType<List<CourseModelDTO>>(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult ListForUser([FromForm] string token)
        {
            var hold = courseService.UserCourses(token);
            return hold != null ? Ok(hold) : BadRequest();
        }
    }
    
}
