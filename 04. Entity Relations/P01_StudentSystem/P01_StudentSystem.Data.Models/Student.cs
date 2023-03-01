namespace P01_StudentSystem.Data.Models;

using P01_StudentSystem.Data.Common;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

public class Student
{
    public Student()
    {
        Homeworks = new HashSet<Homework>();
        StudentsCourses = new HashSet<StudentCourse>();
    }

    [Key]
    public int StudentId { get; set; }

    [Required]
    [MaxLength(ValidationConstants.StudentNameMaxLength)]
    public string Name { get; set; } = null!;

    
    [MaxLength(ValidationConstants.StudentPhoneNumberMaxLength)]
    public string? PhoneNumber  { get; set; }

    [Required]
    public DateTime RegisteredOn { get; set; }

    public DateTime? Birthday { get; set; }

    public virtual ICollection<Homework> Homeworks { get; set; }

    public virtual ICollection<StudentCourse> StudentsCourses { get; set; }
}