﻿using CeramikaAPI.Models;
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
        public IActionResult Delete(int id)
        {
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
    }
    
}
