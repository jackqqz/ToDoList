using MediatR;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;

namespace ToDoList.Application.Commands
{
    public record UpdateToDoItemCommand(Guid ToDoItemId, ToDoItem toDoItem)
        :IRequest<ToDoItem>;

    public class UpdateToDoItemCommandHandler(IToDoItemRepository toDoItemRepository)
        : IRequestHandler<UpdateToDoItemCommand, ToDoItem>
    {
        public async Task<ToDoItem> Handle(UpdateToDoItemCommand request, CancellationToken cancellationToken)
        {
            return await toDoItemRepository.UpdateToDoItem(request.ToDoItemId, request.toDoItem);
        }
    }
}
