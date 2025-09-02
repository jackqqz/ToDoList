using MediatR;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;

namespace ToDoList.Application.Commands;

public record AddItemToListCommand(Guid listId, ToDoItem toDoItem) : IRequest<Guid>;

public class AddItemToListCommandHandler(IToDoListRepository toDoListRepository)
    : IRequestHandler<AddItemToListCommand, Guid>
{
    public async Task<Guid> Handle(AddItemToListCommand request, CancellationToken ct)
    {
        var list = await toDoListRepository.GetAsync(request.listId, ct);
        if (list is null) throw new KeyNotFoundException("List not found");

        var item = new ToDoItem { Title = request.toDoItem.Title, Description = request.toDoItem.Description, ToDoListId = request.listId , DueDate = request.toDoItem.DueDate, CategoryId = request.toDoItem.CategoryId};
        await toDoListRepository.AddItemAsync(item, ct);
        return item.Id;
    }
}
