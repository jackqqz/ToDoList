using MediatR;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;

namespace ToDoList.Application.Queries;

public record GetAllCategoriesQuery() : IRequest<IReadOnlyList<CategoryEntity>>;

public class GetAllCategoriesQueryHandler(ICategoryRepository cats)
    : IRequestHandler<GetAllCategoriesQuery, IReadOnlyList<CategoryEntity>>
{
    public async Task<IReadOnlyList<CategoryEntity>> Handle(GetAllCategoriesQuery r, CancellationToken ct)
    {
        var list = await cats.GetAllCategories();
        return list.ToList();
    }
}
