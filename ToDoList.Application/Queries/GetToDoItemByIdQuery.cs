using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;

namespace ToDoList.Application.Queries
{
    public record GetToDoItemByIdQuery(Guid id) : IRequest<ToDoItem>;

    internal class GetToDoItemByIdQueryHandler(IToDoItemRepository toDoItemRepository) 
        : IRequestHandler<GetToDoItemByIdQuery, ToDoItem>

    {
        public async Task<ToDoItem> Handle(GetToDoItemByIdQuery request, CancellationToken cancellationToken)
        {
            return await toDoItemRepository.GetToDoItemsById(request.id);
        }
    }
}