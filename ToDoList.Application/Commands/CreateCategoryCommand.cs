using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDoList.Core.Interfaces;
using ToDoList.Core.Models;

namespace ToDoList.Application.Commands
{
    public record CreateCategoryCommand(string Name) : IRequest<CategoryEntity>;

    public class CreateCategoryCommandHandler(ICategoryRepository cats)
        : IRequestHandler<CreateCategoryCommand, CategoryEntity>
    {
        public async Task<CategoryEntity> Handle(CreateCategoryCommand r, CancellationToken ct)
        {
            var name = r.Name?.Trim();
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Category name is required.");

            if (await cats.ExistsByNameAsync(name, ct))
                throw new InvalidOperationException("Category already exists.");

            return await cats.AddAsync(new CategoryEntity { Id = Guid.NewGuid(), Name = name }, ct);
        }
    }

}
