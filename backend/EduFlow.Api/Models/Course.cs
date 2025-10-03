using System;

namespace EduFlow.Api.Models;

public class Course
{
    public Guid Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string TeacherId { get; set; } = "";
    public ApplicationUser? Teacher { get; set; }
}