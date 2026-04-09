using TodoApi.DTOs;
namespace TodoApi.Services;
public interface ITodoService
{
    Task<IEnumerable<TodoDto>> GetTodosAsync();

    Task<TodoDto> CreateTodoAsync(CreateTodoDto createTodoDto);

    Task<TodoDto?> UpdateTodoAsync(int id, UpdateTodoDto updateTodoDto);

    Task<bool> DeleteTodoAsync(int id);
}