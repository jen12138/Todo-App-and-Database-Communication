using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.DTOs;
using TodoApi.Models;
namespace TodoApi.Services;
public class TodoService : ITodoService
{
    private readonly TodoDbContext _context;

    public TodoService(TodoDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TodoDto>> GetTodosAsync()
    {
        var todos = await _context.TaskItems.ToListAsync();
        return todos.Select(MapToDto);
    }

    public async Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto)
    {
        var taskItem = new TaskItem
        {
            Title = createTodoDto.Title,
            Description = createTodoDto.Description,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();

        return MapToDto(taskItem);
    }

    public async Task<TodoDto?> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto)
    {
        var existingTodo = await _context.TaskItems.FindAsync(id);

        if (existingTodo == null)
        {
            return null;
        }

        existingTodo.Title = updateTodoDto.Title;
        existingTodo.Description = updateTodoDto.Description;
        existingTodo.IsCompleted = updateTodoDto.IsCompleted;

        await _context.SaveChangesAsync();

        return MapToDto(existingTodo);
    }

    public async Task<bool> DeleteTodoAsync(int id)
    {
        var taskItem = await _context.TaskItems.FindAsync(id);

        if (taskItem == null)
        {
            return false;
        }

        _context.TaskItems.Remove(taskItem);
        await _context.SaveChangesAsync();

        return true;
    }

    private static TodoDto MapToDto(TaskItem taskItem)
    {
        return new TodoDto
        {
            Id = taskItem.Id,
            Title = taskItem.Title,
            Description = taskItem.Description,
            IsCompleted = taskItem.IsCompleted,
            CreatedAt = taskItem.CreatedAt
        };
    }
}