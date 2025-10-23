using System.ComponentModel.DataAnnotations;

namespace EduFlow.Api.Contracts.Courses;

public class CreateCourseRequest
{
    [Required, MaxLength(32)]
    public string Code { get; set; } = "";

    [Required, MaxLength(200)]
    public string Name { get; set; } = "";

    [Required]
    public Guid TeacherId { get; set; }
}