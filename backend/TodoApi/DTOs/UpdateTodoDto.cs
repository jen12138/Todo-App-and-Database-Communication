using System.ComponentModel.DataAnnotations;

namespace TodoApi.DTOs;

public class UpdateTodoDto
{
    [Required]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string Description { get; set; } = string.Empty;

    public bool IsCompleted { get; set; }
}
