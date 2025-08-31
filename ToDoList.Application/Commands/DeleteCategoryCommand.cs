using MediatR;
using ToDoList.Core.Interfaces;

namespace ToDoList.Application.Commands;

public record DeleteCategoryCommand(Guid Id) : IRequest<bool>;

public class DeleteCategoryCommandHandler(ICategoryRepository cats)
    : IRequestHandler<DeleteCategoryCommand, bool>
{
    public Task<bool> Handle(DeleteCategoryCommand r, CancellationToken ct) =>
        cats.DeleteCategory(r.Id);
}
