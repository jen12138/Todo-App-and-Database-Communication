using Microsoft.AspNetCore.Mvc;
using TodoApi.DTOs;
using TodoApi.Services;
using TodoApi.Exceptions;

namespace TodoApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TodosController : ControllerBase
{
    private readonly ITodoService _todoService;

    public TodosController(ITodoService todoService)
    {
        _todoService = todoService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TodoDto>>> GetTodos()
    {
        var todos = await _todoService.GetTodosAsync();
        return Ok(todos);
    }

    [HttpPost]
    public async Task<ActionResult<TodoDto>> CreateTodo(CreateTodoDto createTodoDto)
    {
        var createdTodo = await _todoService.CreateTodoAsync(createTodoDto);
        return CreatedAtAction(nameof(GetTodos), new { id = createdTodo.Id }, createdTodo);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateTodo(int id, UpdateTodoDto updateTodoDto)
    {   
        try
        {
            await _todoService.UpdateTodoAsync(id, updateTodoDto);
            return NoContent();
        }
        catch (TodoNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteTodo(int id)
    {
        try
        {
            await _todoService.DeleteTodoAsync(id);
            return NoContent();
        }
        catch (TodoNotFoundException)
        {
            return NotFound();
        }

    }
    
}
