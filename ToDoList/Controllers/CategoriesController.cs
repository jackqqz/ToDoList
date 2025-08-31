using MediatR;
using Microsoft.AspNetCore.Mvc;
using ToDoList.Application.Commands;
using ToDoList.Application.Queries;

namespace ToDoList.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController(ISender sender) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAll() =>
            Ok(await sender.Send(new GetAllCategoriesQuery()));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var cat = await sender.Send(new GetCategoryByIdQuery(id));
            return cat is null ? NotFound() : Ok(cat);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromQuery] string name)
        {
            var created = await sender.Send(new CreateCategoryCommand(name));
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromQuery] string name)
        {
            var updated = await sender.Send(new UpdateCategoryCommand(id, name));
            return Ok(updated);
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var ok = await sender.Send(new DeleteCategoryCommand(id));
            return ok ? NoContent() : NotFound();
        }

        [HttpGet("suggest")]
        public async Task<IActionResult> Suggest([FromQuery] string? prefix, [FromQuery] int limit = 10) =>
            Ok(await sender.Send(new SuggestCategoriesQuery(prefix, limit)));
    }

}
