using System.ComponentModel.DataAnnotations;

namespace Zad08_Ef.DTOs;

public class StudentDTO
{
    public int IdStudent { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public IEnumerable<GroupDTO> Groups { get; set; }
}

public class GroupDTO
{
    public int IdGroup { get; set; }
    public string Name { get; set; }
}

public class CreateStudentDTO
{
    [Required]
    public string FirstName { get; set; }
    [Required]
    public string LastName { get; set; }
}