using EduFlow.DataAccess;
using EduFlow.DataAccess.Entities;
using EduFlow.Services.Abstractions;
using Microsoft.EntityFrameworkCore;

namespace EduFlow.Services.Modules.Courses;

public class CourseService : ICourseService
{
    private readonly EduFlowDbContext _db;
    public CourseService(EduFlowDbContext db) => _db = db;
    
    public Task<bool> PingAsync(CancellationToken ct = default) => Task.FromResult(true);
    public async Task<(IReadOnlyList<Course> Items, int Total)> GetPagedAsync(int page, int pageSize, CancellationToken ct = default)
    {
        if (page < 1) page = 1;
        if (pageSize < 1 || pageSize > 100) pageSize = 20;

        var query = _db.Courses
            .AsNoTracking()
            .Where(c => !c.IsDeleted)
            .OrderBy(c => c.Name);
        
        var total = await query.CountAsync(ct);
        var items = await query
            .Skip((page -1 ) * pageSize)
            .Take(pageSize)
            .ToListAsync(ct);
        
        return (items, total);
    }

    public async Task<Course?> GetCourseById(Guid id, CancellationToken ct = default)
    {
        var course = await _db.Courses
            .AsNoTracking()
            .FirstOrDefaultAsync(c => c.Id == id, ct);
        
        return course;
    }

    public async Task<Course> CreateCourse(string code, string name, Guid teacherId, CancellationToken ct = default)
    {
        var codeTaken = await _db.Courses.AnyAsync(c => c.Code == code && !c.IsDeleted, ct);
        if (codeTaken)
        {
            throw new InvalidOperationException($"Course code '{code}' already exists.");
        }
        
        var now = DateTimeOffset.UtcNow;
        var entity = new Course
        {
            Id = Guid.NewGuid(),
            Code = code,
            Name = name,
            TeacherId = teacherId,
            CreatedAtUtc = now,
            ModifiedAtUtc = null,
            IsDeleted = false
        };
        
        _db.Courses.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }

    public async Task<bool> UpdateCourse(Guid id, string code, string name, CancellationToken ct = default)
    {
        var entity = await _db.Courses.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);
        if (entity is null) return false;
        
        var codeTaken = await _db.Courses
            .AnyAsync(c => c.Id != id && c.Code == code && !c.IsDeleted, ct);
        if (codeTaken)
        {
            throw new InvalidOperationException($"Course code '{code}' already exists.");
        }
        
        entity.Code = code;
        entity.Name = name;
        entity.ModifiedAtUtc = DateTimeOffset.UtcNow;
        
        await _db.SaveChangesAsync(ct);
        return true;
    }

    public async Task<bool> SoftDeleteCourse(Guid id, CancellationToken ct = default)
    {
        var course = await _db.Courses.FirstOrDefaultAsync(c => c.Id == id && !c.IsDeleted, ct);
        if (course is null)
            return false;

        course.IsDeleted = true;
        course.ModifiedAtUtc = DateTimeOffset.UtcNow;

        await _db.SaveChangesAsync(ct);
        return true;
    }
}