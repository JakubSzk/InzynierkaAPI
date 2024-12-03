using CeramikaAPI.Context;
using CeramikaAPI.Models;
using Microsoft.EntityFrameworkCore;


namespace CeramikaAPI.Services
{
    public class CourseService
    {
        private CeramikaContext context;
        public CourseService() { context = new CeramikaContext(); }
        public List<CourseModelDTO> GetCourses()
        {
            return context.Courses.Select(c => new CourseModelDTO
            {
                Id = c.Id,
                Name = c.Name,
                Description = c.Description,
                Private = c.Private,
                Taken = c.Taken,
                Seats = c.Seats,
                When = c.When,
                TeacherName = c.Teacher.Name
            }).ToList();
        }

        public CourseModel? AddCourse(CourseModel course)
        {
            context.Attach(course.Teacher);
            context.Courses.Add(course);
            try { context.SaveChanges(); } catch(Exception ex) { Console.WriteLine($"Error saving course: {ex.Message}"); return null; }
            return course;
        }


    }
}
