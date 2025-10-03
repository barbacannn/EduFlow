using EduFlow.Api.Contracts.Courses;
using EduFlow.Services.Abstractions;
using Microsoft.AspNetCore.Mvc;

namespace EduFlow.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CoursesController(ICourseService service) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetPaged([FromQuery] int page = 1, [FromQuery] int pageSize = 20, CancellationToken ct = default)
    {
        var (items, total) = await service.GetPagedAsync(page, pageSize, ct);
        var dto = items.Select(c => new CourseDto(
            c.Id, c.Code, c.Name, c.TeacherId, c.CreatedAtUtc, c.ModifiedAtUtc));
        return Ok(new { total, page, pageSize, items = dto });
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<CourseDto>> GetCourseById(Guid id, CancellationToken ct = default)
    {
        var course = await service.GetCourseById(id, ct);
        if (course is null) return NotFound();
        return Ok(new CourseDto(course.Id, course.Code, course.Name, course.TeacherId, course.CreatedAtUtc, course.ModifiedAtUtc));
    }

    [HttpPost]
    public async Task<IActionResult> CreateCourse([FromBody] CreateCourseRequest req, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            var course = await service.CreateCourse(req.Code, req.Name, req.TeacherId, ct);
            var dto = new CourseDto(course.Id, course.Code, course.Name, course.TeacherId, course.CreatedAtUtc, course.ModifiedAtUtc);
            return CreatedAtAction(nameof(GetCourseById), new { id = course.Id }, dto);
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(ex.ParamName ?? "input", ex.Message);
            return ValidationProblem(ModelState);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(nameof(req.Code), ex.Message);
            return ValidationProblem(ModelState);
        }
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateCourse(Guid id, [FromBody] UpdateCourseRequest req, CancellationToken ct = default)
    {
        if (!ModelState.IsValid) return ValidationProblem(ModelState);
        try
        {
            var ok = await service.UpdateCourse(id, req.Code, req.Name, ct);
            return ok ? NoContent() : NotFound();
        }
        catch (ArgumentException ex)
        {
            ModelState.AddModelError(ex.ParamName ?? "input", ex.Message);
            return ValidationProblem(ModelState);
        }
        catch (InvalidOperationException ex)
        {
            ModelState.AddModelError(nameof(req.Code), ex.Message);
            return ValidationProblem(ModelState);
        }
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteCourse(Guid id, CancellationToken ct = default)
    {
        var ok = await service.SoftDeleteCourse(id, ct);
        return ok ? NoContent() : NotFound();
    }
}
