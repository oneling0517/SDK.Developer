using System;
using System.ComponentModel.DataAnnotations;

namespace todoAPI.Models
{
	public class Courses
	{
        [Key]
        public string CourseId { get; set; }
        public string CourseName { get; set; }
        public string Teacher { get; set; }
    }
}

