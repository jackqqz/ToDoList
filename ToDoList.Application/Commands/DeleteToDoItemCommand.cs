using MediatR;
using ToDoList.Core.Interfaces;

namespace ToDoList.Application.Commands
{
    public record DeleteToDoItemCommand(Guid id) : IRequest<bool>;  
    internal class DeleteToDoItemCommandHandler(IToDoItemRepository toDoItemRepository)
        : IRequestHandler<DeleteToDoItemCommand, bool>
    {
        public async Task<bool> Handle(DeleteToDoItemCommand request, CancellationToken cancellationToken)
        {
            return await toDoItemRepository.DeleteToDoItemAsync(request.id);
        }
    }
}
