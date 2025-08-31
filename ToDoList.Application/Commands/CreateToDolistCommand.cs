// Application/Commands/CreateToDoListCommand.cs
using MediatR;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;

public record CreateToDoListCommand(string Title) : IRequest<ToDoListEntity>;

public class CreateToDoListCommandHandler(IToDoListRepository repo)
    : IRequestHandler<CreateToDoListCommand, ToDoListEntity>
{
    public async Task<ToDoListEntity> Handle(CreateToDoListCommand request, CancellationToken ct)
    {
        var list = new ToDoListEntity { Title = request.Title, Items = new List<ToDoItem>() };
        return await repo.AddAsync(list, ct); // EF returns Id populated
    }
}
