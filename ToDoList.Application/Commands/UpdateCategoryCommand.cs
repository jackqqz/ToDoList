using MediatR;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;

namespace ToDoList.Application.Commands;

public record UpdateCategoryCommand(Guid Id, string Name) : IRequest<CategoryEntity>;

public class UpdateCategoryCommandHandler(ICategoryRepository cats)
    : IRequestHandler<UpdateCategoryCommand, CategoryEntity>
{
    public async Task<CategoryEntity> Handle(UpdateCategoryCommand r, CancellationToken ct)
    {
        var name = r.Name?.Trim();
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Category name is required.");

        // optional uniqueness check:
        var exists = await cats.ExistsByNameAsync(name, ct);
        if (exists)
        {
            // if you want to allow same Id same name, read the current and compare before throwing
            var current = await cats.GetCategoryById(r.Id);
            if (current is null || !string.Equals(current.Name, name, StringComparison.Ordinal))
                throw new InvalidOperationException("Category already exists.");
        }

        var updated = await cats.UpdateCategory(r.Id, new CategoryEntity { Id = r.Id, Name = name });
        if (updated is null) throw new KeyNotFoundException("Category not found.");
        return updated;
    }
}
