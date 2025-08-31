using MediatR;
using Microsoft.EntityFrameworkCore;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;

namespace ToDoList.Application.Queries;

public record SuggestCategoriesQuery(string? Prefix, int Limit = 10)
    : IRequest<IReadOnlyList<CategoryEntity>>;

public class SuggestCategoriesQueryHandler(ICategoryRepository cats)
    : IRequestHandler<SuggestCategoriesQuery, IReadOnlyList<CategoryEntity>>
{
    public async Task<IReadOnlyList<CategoryEntity>> Handle(SuggestCategoriesQuery r, CancellationToken ct)
    {
        var q = cats.Query();

        if (!string.IsNullOrWhiteSpace(r.Prefix))
        {
            var p = r.Prefix.Trim();
            q = q.Where(c => EF.Functions.Like(c.Name, p + "%"));
        }

        return await q.OrderBy(c => c.Name)
                      .Take(Math.Clamp(r.Limit, 1, 10))
                      .ToListAsync(ct);
    }
}
