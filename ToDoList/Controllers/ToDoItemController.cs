using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoList.Application.Commands;
using ToDoList.Application.Queries;
using ToDoList.Core.Models;
using ToDoList.Infrastructure.Data;

namespace ToDoList.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ToDoItemController(ISender sender) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AddToDoItemAsync([FromBody] ToDoItem item, [FromServices] AppDbContext db)
        {
            if (!ModelState.IsValid) return ValidationProblem(ModelState);

            // 1) ToDoListId must be present & valid
            if (item.ToDoListId == Guid.Empty || !await db.ToDoLists.AnyAsync(l => l.Id == item.ToDoListId))
                return BadRequest("Invalid or missing ToDoListId.");

            // 2) CategoryId optional: treat empty/unknown as null
            if (item.CategoryId.HasValue)
            {
                if (item.CategoryId.Value == Guid.Empty || !await db.ToDoCategories.AnyAsync(c => c.Id == item.CategoryId.Value))
                    item.CategoryId = null;
            }

            db.ToDoItems.Add(item);
            await db.SaveChangesAsync();
            return Ok(item);
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
