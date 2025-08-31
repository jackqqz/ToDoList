using MediatR;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;

namespace ToDoList.Application.Queries;

public record GetToDoListWithItemsQuery(Guid Id) : IRequest<ToDoListEntity?>;

public class GetToDoListWithItemsQueryHandler(IToDoListRepository repo)
    : IRequestHandler<GetToDoListWithItemsQuery, ToDoListEntity?>
{
    public Task<ToDoListEntity?> Handle(GetToDoListWithItemsQuery request, CancellationToken ct) =>
        repo.GetWithItemsAsync(request.Id, ct);
}
