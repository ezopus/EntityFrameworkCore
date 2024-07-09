using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace P01_StudentSystem.Data.Models
{
    public class Student
    {
        public Student()
        {
            StudentsCourses = new HashSet<StudentCourse>();
            Homeworks = new HashSet<Homework>();
        }
        [Key]
        public int StudentId { get; set; }

        [MaxLength(100)]
        public string Name { get; set; } = null!;

        [MinLength(10)]
        [MaxLength(10)]
        [Unicode(false)]
        public string PhoneNumber { get; set; }

        [Required]
        public DateTime RegisteredOn { get; set; }
        public DateTime? Birthday { get; set; }

        public virtual ICollection<StudentCourse> StudentsCourses { get; set; }
        public virtual ICollection<Homework> Homeworks { get; set; }

    }
}
