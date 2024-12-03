﻿namespace CeramikaAPI.Models
{
    public class CourseModelDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Private { get; set; }
        public int Taken { get; set; }
        public int Seats { get; set; }
        public DateTime When { get; set; }
        public string TeacherName { get; set; }
    }
}