using MediatR;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;

namespace ToDoList.Application.Queries;

public record GetCategoryByIdQuery(Guid Id) : IRequest<CategoryEntity?>;

public class GetCategoryByIdQueryHandler(ICategoryRepository cats)
    : IRequestHandler<GetCategoryByIdQuery, CategoryEntity?>
{
    public async Task<CategoryEntity?> Handle(GetCategoryByIdQuery r, CancellationToken ct)
    {
        return await cats.GetCategoryById(r.Id); // may be null if not found
    }
}
