using CeramikaAPI.Context;
using CeramikaAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;


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
                Length = c.Length,
                When = c.When,
                TeacherName = c.Teacher.Name,
                Picture = c.Picture,
            }).ToList();
        }

        public CourseModel? AddCourse(CourseModel course)
        {
            context.Attach(course.Teacher);
            var change = course.When;
            course.When = new DateTime(change.Year, change.Month, change.Day, change.Hour, 0, 0);
            var test = AvaibleHoursInDay(change.Day, change.Month, change.Year);
            for (int i = change.Hour; i < (change.Hour + course.Length); i++)
            {
                if (!test.Contains(i))
                    return null;
            }
            if (course.Private) { course.Taken = course.Seats; }
            context.Courses.Add(course);
            try { context.SaveChanges(); } catch(Exception ex) { Console.WriteLine($"Error saving course: {ex.Message}"); return null; }
            return course;
        }

        private CourseModel? GetCourseById(int id)
        {
            try
            {
                return context.Courses.FirstOrDefault(x => x.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public CourseModelDTO? GetCourseByIdDTO(int id)
        {
            try
            {
                var hold = context.Courses.Include(c => c.Teacher).FirstOrDefault(x => x.Id == id);
                if (hold == null) return null;
                return new CourseModelDTO { Id = hold.Id, Picture = hold.Picture, Description = hold.Description, Length = hold.Length, Name = hold.Name, Private = hold.Private, Seats = hold.Seats, Taken = hold.Seats, When = hold.When, TeacherName = hold.Teacher.Name };
            }
            catch
            {
                return null;
            }
        }

        public bool DeleteCourse(int id)
        {
            CourseModel? forDeletion = GetCourseById(id);
            if (forDeletion == null)
            {
                return false;
            }

            try
            {
                context.Courses.Remove(forDeletion);
                context.SaveChanges();
                return true; 
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return false; 
            }
        }


        public List<AmountPerIndexModel> ListOfCoursesPerDay(bool isPrivate, int month, int year)
        {
            int days = 0;
            switch (month)
            {
                case 1 or 3 or 5 or 7 or 8 or 10 or 12:
                    days = 31; break;
                case 2:
                    if (year % 4 == 0)
                        days = 29;
                    else
                        days = 28;
                    break;
                default:
                    days = 30; break;
            }
            List<AmountPerIndexModel> returnable = new List<AmountPerIndexModel>();

            for (int i = 1; i <= days; i++)
            {
                returnable.Add(new AmountPerIndexModel
                {
                    Index = i,
                    AmountPerIndex = !isPrivate
                    ? (from course in context.Courses
                       where course.When.Month == month
                       where course.When.Year == year
                       where course.Private == isPrivate
                       where course.When.Day == i
                       select course.Id).Count()
                    : (from course in context.Courses
                       where course.When.Month == month
                       where course.When.Year == year
                       where course.When.Day == i
                       select course.Length).Sum()
                            });
            }

            return returnable;
        }

        public List<DayCourseModelDTO>? CoursesPerDay(int day, int month, int year)
        {
            List<DayCourseModelDTO> returnable = context.Courses.Where(c => c.When.Day == day && c.When.Month == month && c.When.Year == year && c.Private == false)
                .Select(c => new DayCourseModelDTO { Id = c.Id, Name = c.Name, Hour = c.When.Hour, Lasts = c.Length }).ToList();
            return returnable.Count > 0 ? returnable : null;
        }

        public List<int> AvaibleHoursInDay(int day, int month, int year)
        {
            List<int> returnable = [8, 9, 10, 11, 12, 13, 14, 15];
            var result = context.Courses.Where(c => c.When.Day == day && c.When.Month == month && c.When.Year == year && c.Private == false)
                .Select(c => new { a = c.When.Hour, b = c.Length }).ToList();
            foreach (var item in result)
            {
                for (int i = item.a; i < (item.a + item.b); i++)
                    returnable.Remove(i);
            }

            return returnable;
        }

        
    }
}
