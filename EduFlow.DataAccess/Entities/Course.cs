namespace EduFlow.DataAccess.Entities;

public class Course
{
    public Guid Id { get; set; }
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";

    public Guid TeacherId { get; set; }
    public ApplicationUser? Teacher { get; set; }

    public DateTimeOffset CreatedAtUtc { get; set; }
    public DateTimeOffset? ModifiedAtUtc { get; set; }
    public bool IsDeleted { get; set; }
}