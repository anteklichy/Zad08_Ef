using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Zad08_Ef.Context;
using Zad08_Ef.DTOs;
using Zad08_Ef.Models;

namespace Zad08_Ef.Controllers;

[Route("api/[controller]")]
[ApiController]
public class StudentController : ControllerBase
{
    private readonly ApbdContext _context;
    public StudentController(ApbdContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetStudentsAsync()
    {
        var students = await _context.Students
            .Include(s => s.StudentGroups)
            .ThenInclude(sg => sg.IdGroupNavigation)
            .Select(s => new StudentDTO
            {
                IdStudent = s.IdStudent,
                FirstName = s.FirstName,
                LastName = s.LastName,
                Groups = s.StudentGroups.Select(sg => new GroupDTO
                {
                    IdGroup = sg.IdGroup,
                    Name = sg.IdGroupNavigation.Name
                })
            })
            .ToListAsync();
        return Ok(students);
    }
    

    [HttpPost]
    public async Task<IActionResult> AddStudentAsync(CreateStudentDTO studentDTO)
    {
        var student = new Student()
        {
            FirstName = studentDTO.FirstName,
            LastName = studentDTO.LastName
        };
        
        await _context.Students.AddAsync(student);
        await _context.SaveChangesAsync();

        return Ok(student.IdStudent);
    }
    

    [HttpPost("{idStudent}/groups/{idGroup}")]
    public async Task<IActionResult> AssignStudentToGroupAsync(int idStudent, int idGroup)
    {
        var student = await _context.Students.FindAsync(idStudent);
        if (student is null) return NotFound("Student not found");
        
        var group = await _context.Groups.FindAsync(idGroup);
        if (group is null) return NotFound("Group not found");
        
        var studentGroup = await _context.StudentGroups
            .FirstOrDefaultAsync(sg => sg.IdStudent == idStudent && sg.IdGroup == idGroup);
        if (studentGroup is not null) return Conflict("Student is already assigned to this group");
        
        var newStudentGroup = new StudentGroup()
        {
            IdStudent = idStudent,
            IdGroup = idGroup,
            RegisteredAt = DateTime.Now
        };
        
        await _context.StudentGroups.AddAsync(newStudentGroup);
        await _context.SaveChangesAsync();

        return NoContent();
    }
    

    [HttpDelete("{idStudent}")]
    public async Task<IActionResult> DeleteStudentAsync(int idStudent)
    {
        var student = await _context.Students.FindAsync(idStudent);
        if (student is null) return NotFound("Student not found");
        
        await _context.Students
            .Where(s => s.IdStudent == idStudent)
            .ExecuteDeleteAsync();
        return Ok();
    }
}