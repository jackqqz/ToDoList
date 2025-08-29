using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Commands;
using ToDoList.Application.Queries;
using ToDoList.Core.Models;

namespace ToDoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddToDoItemAsync([FromBody] ToDoItem toDoItem)
        {
            var result = await sender.Send(new AddToDoItemCommand(toDoItem));
            return Ok(result);
        }

        [HttpGet("")]
        public async Task<IActionResult> GetAllToDoItemsAsync()
        {
            var result = await sender.Send(new GetAllToDoItemsQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetToDoItemByIdAsync([FromRoute] Guid id)
        {
            var result = await sender.Send(new GetToDoItemByIdQuery(id));

            if ( result is null)
            {
                return NotFound();
            }

            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateToDoItemAsync([FromRoute] Guid id, [FromBody] ToDoItem toDoItem)
        {
            var result = await sender.Send(new UpdateToDoItemCommand(id, toDoItem));
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteToDoItemAsync([FromRoute] Guid id)
        {
            var result = await sender.Send(new DeleteToDoItemCommand(id));
            return Ok(result);
        }
    }
}
