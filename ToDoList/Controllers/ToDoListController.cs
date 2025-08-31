using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Commands;
using ToDoList.Application.Queries;
using ToDoList.Core.Models;

namespace ToDoList.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ToDoListsController(ISender sender) : ControllerBase
{
    // POST: api/todolists
    // Body: { "title": "Work" }
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromQuery] string title)
    {
        if (string.IsNullOrWhiteSpace(title)) return BadRequest("Title is required.");
        var result = await sender.Send(new CreateToDoListCommand(title));
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetByIdAsync(Guid id)
    {
        var list = await sender.Send(new GetToDoListWithItemsQuery(id));
        return list is null ? NotFound() : Ok(list);
    }

    // POST: api/todolists/{listId}/items
    // Body: { "title": "Buy milk", "description": "2L" }
    [HttpPost("{listId:guid}/items")]
    public async Task<IActionResult> AddItemAsync([FromRoute] Guid listId, [FromBody] ToDoItem toDoItem)
    {
        if (toDoItem is null)
        {
            return BadRequest("To Do Item Not Found");
        }
        if (string.IsNullOrWhiteSpace(toDoItem.Title))
        {
            return BadRequest("Item title is required.");
        }

        // NOTE: Command sets ToDoListId from route; any ToDoListId in body is ignored
        var itemId = await sender.Send(new AddItemToListCommand(listId, toDoItem));
        return Ok(itemId);
    }
}
