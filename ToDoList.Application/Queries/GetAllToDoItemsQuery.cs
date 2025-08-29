using MediatR;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;

namespace ToDoList.Application.Queries
{
    public record GetAllToDoItemsQuery(): IRequest<IEnumerable<ToDoItem>>;

    internal class GetAllToDoItemsQueryHandler(IToDoItemRepository toDoItemRepository) : IRequestHandler<GetAllToDoItemsQuery, IEnumerable<ToDoItem>>

    {
        public async Task<IEnumerable<ToDoItem>> Handle(GetAllToDoItemsQuery request, CancellationToken cancellationToken)
        {
            return await toDoItemRepository.GetToDoItems();
        }
    }
}
