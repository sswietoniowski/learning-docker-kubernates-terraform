using backend.api;

using Microsoft.AspNetCore.Mvc;

namespace api.Controllers;

[ApiController]
[Route("api/todo")]
public class TodoController : ControllerBase
{
    private readonly ITodoService _todosService;
    private readonly ILogger<TodoController> _logger;

    public TodoController(ITodoService todosService, ILogger<TodoController> logger)
    {
        _todosService = todosService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Todo>>> GetTodos()
    {
        _logger.LogInformation("Getting all todos");
        return Ok(await _todosService.GetTodosAsync());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Todo>> GetTodoById(int id)
    {
        _logger.LogInformation($"Getting todo with id {id}");
        var todo = await _todosService.GetTodoAsync(id);

        if (todo == null)
        {
            _logger.LogWarning($"Todo with id {id} not found");
            return NotFound();
        }

        return Ok(todo);
    }

    [HttpPost]
    public async Task<ActionResult<Todo>> CreateTodoAsync(TodoForCreationDto todoForCreation)
    {
        _logger.LogInformation($"Creating todo with title {todoForCreation.Title}");
        var todo = await _todosService.CreateTodoAsync(todoForCreation);

        return CreatedAtAction(nameof(GetTodoById), new { id = todo.Id }, todo);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Todo>> UpdateTodoAsync(int id, TodoForUpdateDto todoForUpdate)
    {
        _logger.LogInformation($"Updating todo with id {id}");
        var todo = await _todosService.UpdateTodoAsync(id, todoForUpdate);

        if (todo == null)
        {
            _logger.LogWarning($"Todo with id {id} not found");
            return NotFound();
        }

        return Ok(todo);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteTodoAsync(int id)
    {
        _logger.LogInformation($"Deleting todo with id {id}");
        var todo = await _todosService.DeleteTodoAsync(id);

        if (!todo)
        {
            _logger.LogWarning($"Todo with id {id} not found");
            return NotFound();
        }

        return NoContent();
    }
}