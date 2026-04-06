using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoApi.Data;
using TodoApi.Models;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly TodoDbContext _context;

    public TodosController(TodoDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskItem>>> GetTodos()
    {
        var todos = await _context.TaskItems.ToListAsync();
        return Ok(todos);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItem>> CreateTodo(TaskItem taskItem)
    {
        taskItem.CreatedAt = DateTime.UtcNow;

        _context.TaskItems.Add(taskItem);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetTodos), new { id = taskItem.Id }, taskItem);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, TaskItem taskItem)
    {
        if (id != taskItem.Id)
        {
            return BadRequest();
        }

        var existingTodo = await _context.TaskItems.FindAsync(id);

        if (existingTodo == null)
        {
            return NotFound();
        }

        existingTodo.Title = taskItem.Title;
        existingTodo.Description = taskItem.Description;
        existingTodo.IsCompleted = taskItem.IsCompleted;

        await _context.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        var taskItem = await _context.TaskItems.FindAsync(id);

        if (taskItem == null)
        {
            return NotFound();
        }

        _context.TaskItems.Remove(taskItem);
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
