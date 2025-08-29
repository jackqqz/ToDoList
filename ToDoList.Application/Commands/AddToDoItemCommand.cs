using MediatR;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;

namespace ToDoList.Application.Commands
{
    public record AddToDoItemCommand(ToDoItem toDoItem): IRequest<ToDoItem>;

    public class AddToDoItemCommandHandler(IToDoItemRepository toDoItemRepository) : IRequestHandler<AddToDoItemCommand, ToDoItem>
    {
        public async Task<ToDoItem> Handle(AddToDoItemCommand request, CancellationToken cancellationToken)
        {
            return await toDoItemRepository.AddToDoItem(request.toDoItem);
        }
    }

}
